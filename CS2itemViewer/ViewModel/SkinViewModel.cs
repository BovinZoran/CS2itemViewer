using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CS2itemViewer.Model;
using CS2itemViewer.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CS2itemViewer.ViewModel
{
    public partial class SkinViewModel : BaseViewModel
    {
        public ObservableCollection<Skin> Skins { get; } = new();
        private readonly ISkinService _skinService;
        private readonly IConnectivity _connectivity;

        public SkinViewModel(ISkinService skinService, IConnectivity connectivity)
        {
            Title = "CS2 item Viewer";
            _skinService = skinService;
            _connectivity = connectivity;

            // Call the GetSkinsCommand command when the ViewModel is constructed
            GetSkinsCommand.Execute(null);
        }

        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        async Task GetSkinsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                if (_connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("No connectivity!", "Please check internet and try again.", "OK");
                    return;
                }

                IsBusy = true;
                var skins = await _skinService.GetSkins();

                if (skins == null)
                    return;

                Skins.Clear();

                foreach (var skin in skins)
                    Skins.Add(skin);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to get skins: {ex.Message}");
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
                { "Skin", skin }
            });
        }

        
    }


}
