using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BetsBrasileiras.Dto;
using BetsBrasileiras.Helpers;
using CrispyWaffle.Extensions;

namespace BetsBrasileiras;

internal class Worker
{
    protected Worker() { }

    /// <summary>
    /// Works this instance.
    /// </summary>
    public static void Work()
    {
        Logger.Log("Reading data files", ConsoleColor.White);

        var reader = new Reader();

        var source = Reader.LoadBase();
        var original = source.DeepClone();

        AcquireData(reader, source);

        if (ProcessData(original, ref source, out var except))
        {
            return;
        }

        ProcessChanges(source, except, original);
    }

    /// <summary>
    /// Acquires the data.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="source">The source.</param>
    private static void AcquireData(Reader reader, List<Bet> source)
    {
        var items = new Dictionary<Source, int>();
        var logBuilder = new StringBuilder("\r\n");

        logBuilder.AppendFormat("Source: {0} | ", source.Count);
        items.Add(Source.Base, source.Count);

        var spa = reader.LoadSpa();
        logBuilder.AppendFormat("SPA: {0} | ", spa.Count);
        items.Add(Source.Spa, spa.Count);

        logBuilder.AppendLine();

        Logger.Log(logBuilder.ToString(), ConsoleColor.DarkGreen);

        if (items.Any(i => i.Value == 0))
        {
            var errorItems = items.Where(i => i.Value == 0).Select(i => i.Key.ToString());
            Logger.Log(
                "Items: " + string.Join(", ", errorItems) + " are empty",
                ConsoleColor.DarkRed
            );
            Environment.Exit(3);
        }

        //new Seeder(source).Seed(spa);
    }

    private static bool ProcessData(List<Bet> original, ref List<Bet> source, out List<Bet> except)
    {
        foreach (var bet in source)
        {
            bet.DateRegistered ??= DateTimeOffset.UtcNow;
            bet.DateUpdated ??= DateTimeOffset.UtcNow;
        }

        except = source.Except(original).ToList();

        if (except.Any())
        {
            return false;
        }

        Logger.Log("No new data or updated information", ConsoleColor.DarkMagenta);
        Environment.Exit(188);
        return true;
    }

    /// <summary>
    /// Processes the changes.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="except">The except.</param>
    /// <param name="original">The original.</param>
    private static void ProcessChanges(List<Bet> source, List<Bet> except, List<Bet> original)
    {
        var added = new List<Bet>();
        var updated = new List<Bet>();

        foreach (var exc in except)
        {
            var isUpdated = original.Exists(b => b.Document == exc.Document);

            if (isUpdated)
            {
                updated.Add(exc);
            }
            else
            {
                added.Add(exc);
            }
        }

        var changeLog = new StringBuilder();
        ProcessChangesAdded(changeLog, added);
        ProcessChangesUpdated(changeLog, updated);

        Logger.Log("\r\nSaving result files", ConsoleColor.White);

        var changeLogData = changeLog.ToString();

        Writer.WriteReleaseNotes(changeLogData);
        Writer.WriteChangeLog(changeLogData);
        Writer.SaveBets(source);

        Logger.Log($"Merge done. Bets: {source.Count}", ConsoleColor.White);
    }

    /// <summary>
    /// Processes the changes added.
    /// </summary>
    /// <param name="changeLog">The change log.</param>
    /// <param name="added">The added.</param>
    private static void ProcessChangesAdded(StringBuilder changeLog, List<Bet> added)
    {
        if (added.Count == 0)
        {
            return;
        }

        changeLog.AppendLine(
            $"- Added {added.Count} bank{(added.Count == 1 ? string.Empty : "s")}"
        );

        Logger.Log($"\r\nAdded items: {added.Count}\r\n\r\n", ConsoleColor.White);

        var color = ConsoleColor.DarkGreen;

        foreach (var item in added)
        {
            changeLog.AppendLine($"  - {item.FiscalName} - {item.Document}");
            color = color == ConsoleColor.DarkGreen ? ConsoleColor.Cyan : ConsoleColor.DarkGreen;
            Logger.Log($"Added: {item}\r\n", color);
        }
    }

    private static void ProcessChangesUpdated(StringBuilder changeLog, List<Bet> updated)
    {
        if (updated.Count == 0)
        {
            return;
        }

        changeLog.AppendLine(
            $"- Updated {updated.Count} bank{(updated.Count == 1 ? string.Empty : "s")}"
        );

        Logger.Log($"\r\nUpdated items: {updated.Count}\r\n\r\n", ConsoleColor.White);

        var color = ConsoleColor.DarkBlue;

        foreach (var item in updated)
        {
            //changeLog.AppendLine($"  - {item.Name} - {item.Document}");
            //if (item.HasChanges)
            //{
            //    foreach (var change in item.GetChanges())
            //    {
            //        changeLog.AppendLine(
            //            $"    - **{change.Key}** ({change.Value.Source.GetHumanReadableValue()}): {change.Value.OldValue} **->** {change.Value.NewValue}"
            //        );
            //    }
            //}

            color = color == ConsoleColor.DarkBlue ? ConsoleColor.Blue : ConsoleColor.DarkBlue;
            Logger.Log($"Updated: {item}\r\n", color);
        }
    }
}
