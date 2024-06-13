using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CS2itemViewer.Model
{
    public class Skin
    {
        [JsonPropertyName("marketname")]
        public string MarketName { get; set; }

        [JsonPropertyName("image")]
        public string Image { get; set; }

        [JsonPropertyName("itemname")]
        public string ItemName { get; set; }

        [JsonPropertyName("pricelatestsell")]
        public double PriceLatestSell { get; set; }

        private string _color;
        [JsonProperty("color")]
        public string Color
        {
            get => _color.StartsWith("#") ? _color : $"#{_color}";
            set => _color = value;
        }

        public string DescriptionFloat { get; set; }

        public string DescriptionText { get; set; }

        public string CleanDescriptionFloat => CleanseDescription(DescriptionFloat);
        public string CleanDescriptionText => CleanseDescription(DescriptionText);
        private string CleanseDescription(string input)
        {
            // Remove HTML tags using regex
            return Regex.Replace(input, "<.*?>", string.Empty);
        }
    }
}
