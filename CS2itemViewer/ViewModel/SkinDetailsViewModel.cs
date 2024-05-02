using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CS2itemViewer.Model;
using CS2itemViewer.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS2itemViewer.ViewModel
{
    [QueryProperty(nameof(Skin), "Skin")]
    public partial class SkinDetailsViewModel : BaseViewModel
    {
        IMap map;
        public SkinDetailsViewModel(IMap map)
        {
            this.map = map;
        }

        [ObservableProperty]
        Skin? skin;
    }
}
