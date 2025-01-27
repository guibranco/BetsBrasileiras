using System;
using System.Collections.Generic;
using System.Net.Http;
using BetsBrasileiras.Dto;
using CrispyWaffle.Serialization;
using UglyToad.PdfPig;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace BetsBrasileiras.Helpers;

internal class Reader
{
    /// <summary>
    /// The user agent.
    /// </summary>
    private const string UserAgent =
        "BetsBrasileiras/1.0 (+https://github.com/guibranco/betsbrasileiras)";

    /// <summary>
    /// Downloads the string.
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns>System.String.</returns>
    private static string DownloadString(string url)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
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
        client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
        return client.GetByteArrayAsync(url).Result;
    }

    /// <summary>
    /// Downloads and parse PDF.
    /// </summary>
    /// <param name="url">The URL.</param>
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
            Logger.Log($"Downloading SPA", ConsoleColor.Green);
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
    public static List<Bet> LoadBase()
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

        var previousLine = string.Empty;
        foreach (var line in lines)
        {
            var currentLine = line.Trim('\r');
            if (
                !Patterns.SpaFullLinePattern.IsMatch(currentLine)
                && !Patterns.SpaBrandDomainPattern.IsMatch(currentLine)
            )
            {
                continue;
            }

            Bet bet = null;

            if (
                !string.IsNullOrWhiteSpace(previousLine)
                && Patterns.SpaBrandDomainPattern.IsMatch(currentLine)
            )
            {
                // TODO: Implement this when able to extract the content from the PDF
                //bet = ParseLineSpa($"{previousLine} {currentLine}");
            }
            else if (Patterns.SpaFullLinePattern.IsMatch(currentLine))
            {
                bet = ParseLineSpa(currentLine);
                //if (bet != null)
                //{
                //    previousLine = currentLine
                //        .Replace(bet.Brand, "")
                //        .Replace(bet.Domain, "")
                //        .Trim();
                //}
            }

            if (bet != null)
            {
                result.Add(bet);
            }
        }

        return result;
    }

    private Bet ParseLineSpa(string line)
    {
        if (!Patterns.SpaFullLinePattern.IsMatch(line))
        {
            return null;
        }

        var match = Patterns.SpaFullLinePattern.Match(line);

        return new Bet
        {
            ApplicationNumberString = match.Groups["applicationNumber"].Value.Trim(),
            ApplicationYearString = match.Groups["applicationYear"].Value.Trim(),
            FiscalName = match.Groups["name"].Value.Trim(),
            Document = match.Groups["document"].Value.Trim(),
            Brand = match.Groups["brand"].Value.Trim(),
            Domain = match.Groups["domain"].Value.Trim(),
        };
    }
}
