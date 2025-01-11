namespace BetsBrasileiras.Helpers;

using System;

/// <summary>
/// Class Helpers.
/// </summary>
internal static class Logger
{
    /// <summary>
    /// Logs the specified message.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <param name="color">The color.</param>
    public static void Log(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ForegroundColor = ConsoleColor.White;
    }
}
