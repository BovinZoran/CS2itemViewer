using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2itemViewer.Model
{
    public class Skin
    {
        public string image { get; set; } = "";
        public string marketname { get; set; } = "";
        //public string name { get; set; } = "";//itemtype+itemname?
        //public string descriptionText { get; set; } = "";//descriptions, third item, value
        //public string descriptionFloat { get; set; } = "";//descriptions, first item, value
        //public double itemFloat { get; set; }
        public double pricelatestsell { get; set; }
    }
}
