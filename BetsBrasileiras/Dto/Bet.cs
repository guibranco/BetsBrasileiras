using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Xml.Serialization;
using CrispyWaffle.I18n.PtBr;
using CrispyWaffle.Validations;
using Newtonsoft.Json;

namespace BetsBrasileiras.Dto;

/// <summary>
/// Class Bet.
/// </summary>
public class Bet
{
    /// <summary>
    /// Gets or sets the application number.
    /// </summary>
    /// <value>The application number.</value>
    [JsonIgnore]
    [XmlIgnore]
    public int ApplicationNumber { get; set; }

    /// <summary>
    /// Gets or sets the application number string.
    /// </summary>
    /// <value>The application number string.</value>
    [JsonProperty("ApplicationNumber")]
    [XmlElement("ApplicationNumber")]
    [Display(Name = "ApplicationNumber")]
    public string ApplicationNumberString
    {
        get => ApplicationNumber.ToString("0000");
        set
        {
            if (int.TryParse(value, out var parsed))
            {
                ApplicationNumber = parsed;
            }
        }
    }

    /// <summary>
    /// Gets or sets the application year.
    /// </summary>
    /// <value>The application year.</value>
    [JsonIgnore]
    [XmlIgnore]
    public int ApplicationYear { get; set; }

    /// <summary>
    /// Gets or sets the application year string.
    /// </summary>
    /// <value>The application year string.</value>
    [JsonProperty("ApplicationYear")]
    [XmlElement("ApplicationYear")]
    [Display(Name = "ApplicationYear")]
    public string ApplicationYearString
    {
        get => ApplicationYear.ToString("0000");
        set
        {
            if (int.TryParse(value, out var parsed))
            {
                ApplicationYear = parsed;
            }
        }
    }

    /// <summary>
    /// The document
    /// </summary>
    private string _document;

    /// <summary>
    /// Gets or sets the document.
    /// </summary>
    /// <value>The document.</value>
    [JsonProperty("Document")]
    [XmlElement("Document")]
    [Display(Name = "Document")]
    public string Document
    {
        get => _document;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var document = new string(value.Where(char.IsDigit).ToArray());
            if (document.Length == 8)
            {
                if (document.Equals("00000000"))
                {
                    document = "00000000000191";
                }
                else
                {
                    document += "0001";
                    document += $"{document}00".CalculateBrazilianCorporateDocument();
                }
            }

            _document = document.FormatBrazilianDocument();
        }
    }

    /// <summary>
    /// Gets or sets the fiscal name.
    /// </summary>
    /// <value>The fiscal name.</value>
    [JsonProperty("FiscalName")]
    [XmlElement("FiscalName")]
    [Display(Name = "FiscalName")]
    public string FiscalName { get; set; }

    /// <summary>
    /// Gets or sets the brand.
    /// </summary>
    /// <value>The brand.</value>
    [JsonProperty("Brand")]
    [XmlElement("Brand")]
    [Display(Name = "Brand")]
    public string Brand { get; set; }

    /// <summary>
    /// Gets or sets the domain.
    /// </summary>
    /// <value>The domain.</value>
    [JsonProperty("Domain")]
    [XmlElement("Domain")]
    [Display(Name = "Domain")]
    public string Domain { get; set; }

    /// <summary>
    /// Gets or sets the date registered.
    /// </summary>
    /// <value>The date registered.</value>
    [JsonProperty("DateRegistered")]
    [XmlElement("DateRegistered")]
    [Display(Name = "Date Registered")]
    public DateTimeOffset? DateRegistered { get; set; }

    /// <summary>
    /// Gets or sets the date updated.
    /// </summary>
    /// <value>The date updated.</value>
    [JsonProperty("DateUpdated")]
    [XmlElement("DateUpdated")]
    [Display(Name = "Date Updated")]
    public DateTimeOffset? DateUpdated { get; set; }
}
