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
        public string Image { get; set; } = "";
        public string MarketName { get; set; } = "";
        public string Name { get; set; } = "";//itemtype+itemname?
        public string DescriptionText { get; set; } = "test";//descriptions, third item, value

        public string DescriptionFloat { get; set; } = "FN";//descriptions, first item, value

        //public double itemFloat { get; set; }
        public double PriceLatestSell { get; set; }


    }

    [JsonSerializable(typeof(List<Skin>))]
    internal sealed partial class SkinContext : JsonSerializerContext
    {

    }
}
