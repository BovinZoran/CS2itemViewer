using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CS2itemViewer;
using CS2itemViewer.Model;
using CS2itemViewer.Services;
using CS2itemViewer.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyApp.ViewModel
{
    public partial class SkinViewModel : BaseViewModel
    {
        public ObservableCollection<Skin> Skins { get; } = new();
        ISkinService SkinService;
        IConnectivity connectivity;
        
        

        [ObservableProperty]
        bool isRefreshing;

        public SkinViewModel(ISkinService skinService, IConnectivity connectivity)
        {

           this.SkinService = skinService;
           this.connectivity = connectivity;
        }

        [RelayCommand]

        async Task GetMonkeysAsync()
        {
            if (IsBusy)
                return;

            try
            {
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("No connectivity!",
                        $"Please check internet and try again.", "OK");
                    return;
                }

                IsBusy = true;
                var skins = await SkinService.GetSkins();

                if (skins == null)
                    return;

                if (Skins.Count != 0)
                    Skins.Clear();

                foreach (var skin in skins)
                    Skins.Add(skin);

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get monkeys: {ex.Message}");
                await Shell.Current.DisplayAlert("Error!", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }

        }
        [RelayCommand]
        async Task GoToDetails(Skin skin)
        {
            if (skin == null)
                return;

            await Shell.Current.GoToAsync(nameof(DetailsPage), true, new Dictionary<string, object>
            {
                {"Skin", skin }
            });
        }






    }
}
