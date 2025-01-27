using System;
using System.Text;
using BetsBrasileiras.Helpers;

namespace BetsBrasileiras;

/// <summary>
/// Class Program.
/// </summary>
static class Program
{
    /// <summary>
    /// Defines the entry point of the application.
    /// </summary>
    static void Main()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        Logger.Log("Bets Brasileiras", ConsoleColor.Cyan);
        Worker.Work();
    }
}
