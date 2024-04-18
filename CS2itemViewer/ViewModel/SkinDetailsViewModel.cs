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

namespace MonkeyApp.ViewModel
{
    [QueryProperty(nameof(Skin), "Skin")]
    public partial class MonkeyDetailsViewModel : BaseViewModel  // <<<<<<<<<<<<<<<<<<<<<< LOOK HERE AGAIN
    {
        IMap map;
        public MonkeyDetailsViewModel(IMap map)
        {
            this.map = map;


        }

        [ObservableProperty]
        Skin? skin;

    }
}
