using CS2itemViewer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2itemViewer.Services
{
    public interface ISkinService
    {
        Task<List<Skin>?> GetSkins();
    }
}
