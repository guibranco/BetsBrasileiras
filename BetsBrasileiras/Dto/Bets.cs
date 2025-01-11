using System.Xml.Serialization;

namespace BetsBrasileiras.Dto;

/// <summary>
/// Class Bets.
/// </summary>
[XmlRoot("bets")]
public class Bets
{
    /// <summary>
    /// Gets or sets the bank.
    /// </summary>
    /// <value>The bank.</value>
    [XmlElement("bet")]
    public Bet[] Bet { get; set; }
}
