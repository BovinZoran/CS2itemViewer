using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
    }

    public class Description
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
