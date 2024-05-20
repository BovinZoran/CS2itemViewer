using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CS2itemViewer.Model;
using CS2itemViewer.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CS2itemViewer.ViewModel
{
    public partial class SkinViewModel : BaseViewModel
    {
        public ObservableCollection<Skin> Skins { get; } = new();
        private readonly ISkinService _skinService;
        private readonly IConnectivity _connectivity;
        private List<Skin> allSkins;

        public SkinViewModel(ISkinService skinService, IConnectivity connectivity)
        {
            Title = "CS2 item Viewer";
            _skinService = skinService;
            _connectivity = connectivity;
            allSkins = new List<Skin>();

            // Call the GetSkinsCommand command when the ViewModel is constructed
            GetSkinsCommand.Execute(null);
        }

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        string searchText;

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

                allSkins = skins;
                FilterSkins();
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

        [RelayCommand]
        void FilterSkins()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                Skins.Clear();
                foreach (var skin in allSkins)
                {
                    Skins.Add(skin);
                }
            }
            else
            {
                var filteredSkins = allSkins.Where(skin => skin.MarketName.Contains(SearchText, StringComparison.OrdinalIgnoreCase)).ToList();
                Skins.Clear();
                foreach (var skin in filteredSkins)
                {
                    Skins.Add(skin);
                }
            }
        }
    }
}
