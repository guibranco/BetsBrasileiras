using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BetsBrasileiras.Dto;
using CrispyWaffle.Serialization;
using Microsoft.VisualBasic;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace BetsBrasileiras.Helpers;

internal class Reader
{
    /// <summary>
    /// Downloads the string.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns>System.String.</returns>
    private static string DownloadString(string url)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add(
            "User-Agent",
            "BetsBrasileiras/1.0 (+https://github.com/guibranco/bancosbrasileiros)"
        );
        using var response = client.GetAsync(url).Result;
        using var content = response.Content;
        return content.ReadAsStringAsync().Result;
    }

    /// <summary>
    /// Downloads the bytes.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns>System.Byte[].</returns>
    private static byte[] DownloadBytes(string url)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add(
            "User-Agent",
            "BetsBrasileiras/1.0 (+https://github.com/guibranco/bancosbrasileiros)"
        );
        return client.GetByteArrayAsync(url).Result;
    }

    /// <summary>
    /// Downloads and parse PDF.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="system">The system.</param>
    /// <param name="callback">The callback.</param>
    /// <returns>List&lt;Bet&gt;.</returns>
    private static List<Bet> DownloadAndParsePdf(
        string url,
        Func<string, IEnumerable<Bet>> callback
    )
    {
        var result = new List<Bet>();
        PdfDocument reader;

        try
        {
            Logger.Log($"Downloading", ConsoleColor.Green);
            var data = DownloadBytes(url);
            reader = PdfDocument.Open(data);
        }
        catch (Exception e)
        {
            Logger.Log($"Error downloading: {e.Message}", ConsoleColor.DarkRed);
            return result;
        }

        foreach (var page in reader.GetPages())
        {
            var currentText = ContentOrderTextExtractor.GetText(page);
            result.AddRange(callback(currentText));
        }

        return result;
    }

    /// <summary>
    /// Loads the change log.
    /// </summary>
    /// <returns>System.String.</returns>
    public static string LoadChangeLog() => DownloadString(Constants.ChangeLogUrl);

    /// <summary>
    /// Loads the base.
    /// </summary>
    /// <returns>List&lt;Bet&gt;.</returns>
    public List<Bet> LoadBase()
    {
        Logger.Log("Downloading base", ConsoleColor.Green);
        var data = DownloadString(Constants.BaseUrl);
        return SerializerFactory
            .GetCustomSerializer<List<Bet>>(SerializerFormat.Json)
            .Deserialize(data);
    }

    public List<Bet> LoadSpa()
    {
        return DownloadAndParsePdf(Constants.SpaUrl, ParseLinesSpa);
    }

    private IEnumerable<Bet> ParseLinesSpa(string page)
    {
        var result = new List<Bet>();
        var lines = page.Split("\n");

        var spliced = new StringBuilder();

        foreach (var line in lines)
        {
            if (!Patterns.SpaPattern.IsMatch(line))
            {
                spliced.Append($" {line}");
                continue;
            }

            Bet bet;

            if (!string.IsNullOrWhiteSpace(spliced.ToString()))
            {
                bet = ParseLineSpa(spliced.ToString().Trim());

                if (bet != null)
                {
                    result.Add(bet);
                }

                spliced.Clear();
            }

            bet = ParseLineSpa(line);

            if (bet != null)
            {
                result.Add(bet);
            }
        }

        return result;
    }

    private Bet ParseLineSpa(string line)
    {
        if (!Patterns.SpaPattern.IsMatch(line))
        {
            return null;
        }

        var match = Patterns.SpaPattern.Match(line);

        return new Bet
        {
            ApplicationNumberString = match.Groups["applicationNumber"].Value.Trim(),
            ApplicationYearString = match.Groups["applicationYear"].Value.Trim(),
            Name = match.Groups["name"].Value.Trim(),
            Document = match.Groups["document"].Value.Trim(),
            Brand = match.Groups["brand"].Value.Trim(),
            Domain = match.Groups["domain"].Value.Trim(),
        };
    }
}
