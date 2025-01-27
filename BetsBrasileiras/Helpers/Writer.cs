using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using BetsBrasileiras.Dto;
using CrispyWaffle.Serialization;
using Newtonsoft.Json;

namespace BetsBrasileiras.Helpers;

/// <summary>
/// Class Writer.
/// </summary>
internal static class Writer
{
    public static void WriteReleaseNotes(string changeLog)
    {
        if (!Directory.Exists("result"))
        {
            Directory.CreateDirectory("result");
        }

        File.WriteAllText($"result{Path.DirectorySeparatorChar}release-notes.md", changeLog);
    }

    /// <summary>
    /// Writes the change log.
    /// </summary>
    /// <param name="changeLog">The change log.</param>
    public static void WriteChangeLog(string changeLog)
    {
        if (!Directory.Exists("result"))
        {
            Directory.CreateDirectory("result");
        }

        changeLog =
            $"### {DateTime.Now:yyyy-MM-dd} - [BetsBrasileiras](https://github.com/guibranco/BetsBrasileiras)\r\n\r\n{changeLog}";

        var changeLogFile = Reader.LoadChangeLog();
        changeLogFile = changeLogFile.Replace("## Changelog\r\n\r\n", "## Changelog\n\n");
        var result = changeLogFile.Replace("## Changelog\n\n", $"## Changelog\n\n{changeLog}\n");

        File.WriteAllText($"result{Path.DirectorySeparatorChar}CHANGELOG.md", result);
    }

    /// <summary>
    /// Saves the specified bets.
    /// </summary>
    /// <param name="bets">The bets.</param>
    public static void SaveBets(IList<Bet> bets)
    {
        if (!Directory.Exists("result"))
        {
            Directory.CreateDirectory("result");
        }

        bets = bets.OrderBy(b => $"{b.ApplicationNumber}/{b.ApplicationYear}").ToList();

        SaveCsv(bets);
        bets.GetCustomSerializer(SerializerFormat.Json)
            .Save($"result{Path.DirectorySeparatorChar}bets.json");
        SaveMarkdown(bets);
        SaveSql(bets);
        new Bets { Bet = bets.ToArray() }
            .GetCustomSerializer(SerializerFormat.Xml)
            .Save($"result{Path.DirectorySeparatorChar}bets.xml");
    }

    /// <summary>
    /// Saves the CSV.
    /// </summary>
    /// <param name="bets">The bets.</param>
    private static void SaveCsv(IEnumerable<Bet> bets)
    {
        var lines = new List<string> { string.Join(",", GetFieldsJsonPropertyNames) };

        lines.AddRange(
            bets.Select(bet =>
                $"{bet.ApplicationNumber:000},{bet.ApplicationYear:0000},{bet.Document},{bet.FiscalName.Replace(",", "")},{bet.Brand},{bet.Domain},{bet.DateRegistered:O},{bet.DateUpdated:O}"
            )
        );

        File.WriteAllLines($"result{Path.DirectorySeparatorChar}bets.csv", lines, Encoding.UTF8);
    }

    /// <summary>
    /// Saves the markdown.
    /// </summary>
    /// <param name="bets">The bets.</param>
    private static void SaveMarkdown(IEnumerable<Bet> bets)
    {
        var lines = new List<string>
        {
            "# Bets Brasileiros",
            string.Empty,
            string.Join(" | ", GetFieldsDisplayNames),
            string.Join(" | ", GetFieldsDisplayNames.Select(_ => "---")),
        };

        lines.AddRange(
            bets.Select(bet =>
                $"{bet.ApplicationNumber:000} | {bet.ApplicationYear:0000} | {bet.Document} | {bet.FiscalName.Replace(",", "")} | {bet.Brand} | {bet.Domain} | {bet.DateRegistered:O} | {bet.DateUpdated:O}"
            )
        );

        File.WriteAllLines($"result{Path.DirectorySeparatorChar}bets.md", lines, Encoding.UTF8);
    }

    /// <summary>
    /// Saves the SQL.
    /// </summary>
    /// <param name="bets">The bets.</param>
    private static void SaveSql(IEnumerable<Bet> bets)
    {
        var lines = new List<string>();

        var prefix = $"INSERT INTO Bets ({string.Join(",", GetFieldsJsonPropertyNames)}) VALUES(";
        lines.AddRange(
            bets.Select(bet =>
                $"{prefix}'{bet.ApplicationNumber:000}','{bet.ApplicationYear:0000}','{bet.Document}','{bet.FiscalName.Replace("'", "''")}',{(string.IsNullOrWhiteSpace(bet.Brand) ? "NULL" : $"'{bet.Brand}'")},{(string.IsNullOrWhiteSpace(bet.Domain) ? "NULL" : $"'{bet.Domain}'")},'{bet.DateRegistered:O}','{bet.DateUpdated:O}');"
            )
        );

        File.WriteAllLines($"result{Path.DirectorySeparatorChar}bets.sql", lines, Encoding.UTF8);
    }

    /// <summary>
    /// Gets the get fields json property names.
    /// </summary>
    /// <value>The get fields json property names.</value>
    private static string[] GetFieldsJsonPropertyNames { get; } =
        typeof(Bet)
            .GetProperties()
            .Where(pi => pi.GetCustomAttribute<JsonIgnoreAttribute>() == null)
            .Select(pi => pi.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? pi.Name)
            .ToArray();

    /// <summary>
    /// Gets the get fields display names.
    /// </summary>
    /// <value>The get fields display names.</value>
    private static string[] GetFieldsDisplayNames { get; } =
        typeof(Bet)
            .GetProperties()
            .Where(pi => pi.GetCustomAttribute<JsonIgnoreAttribute>() == null)
            .Select(pi =>
                pi.GetCustomAttribute<DisplayAttribute>()?.Name
                ?? pi.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName
                ?? pi.Name
            )
            .ToArray();
}
