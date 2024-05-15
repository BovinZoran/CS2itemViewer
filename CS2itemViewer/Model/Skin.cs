using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

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

        public string DescriptionFloat { get; set; }

        public string DescriptionText { get; set; }



        // Inside your Skin class or ViewModel
        public string CleanDescriptionText => CleanseDescriptionText(DescriptionText);

        // Inside your Skin class or ViewModel
        private string CleanseDescriptionText(string descriptionText)
        {
            // Remove HTML tags using regex
            return Regex.Replace(descriptionText, "<.*?>", string.Empty);
        }
    }

    



    public class Description
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
