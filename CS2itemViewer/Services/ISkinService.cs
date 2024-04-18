using CS2itemViewer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2itemViewerV2.Services
{
    interface ISkinService
    {
        Task<List<Skin>?> GetSkins();
    }
}
