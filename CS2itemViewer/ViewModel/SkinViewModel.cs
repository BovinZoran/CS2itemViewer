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

        public ICommand OpenLinkCommand { get; }
        public ICommand LoadLoginCommand { get; }
        public ICommand ImageTappedCommand { get; }

        private readonly INavigation _navigation;

        public ICommand DummyCommand { get; }

        public SkinViewModel(ISkinService skinService, IConnectivity connectivity, INavigation navigation)
        {
            Title = "CS2 item Viewer";
            _skinService = skinService;
            _connectivity = connectivity;
            allSkins = new List<Skin>();

            OpenLinkCommand = new RelayCommand<string>(OpenLink);

            DummyCommand = new Command(OnDummyCommand);

            _navigation = navigation;
            ImageTappedCommand = new Command<Skin>(async (skin) => await OnImageTapped(skin));

            SteamLoginIDText = "76561198268749335"; // remove later

            // Initialize LoadLoginCommand
            LoadLoginCommand = new RelayCommand(UpdateSteamID);

            // Call the GetSkinsCommand command when the ViewModel is constructed
            GetSkinsCommand.Execute(null);
        }

        private void OnDummyCommand(object parameter)
        {
            Debug.WriteLine("Dummy command executed!");
        }

        private async Task OnImageTapped(Skin tappedSkin)
        {
            Debug.WriteLine("Image tapped!");
            if (tappedSkin != null)
            {
                try
                {
                    // Navigate to the DetailsPage and pass the skin object as a parameter
                    await _navigation.PushAsync(new DetailsPage(new SkinDetailsViewModel(tappedSkin)));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error navigating to DetailsPage: {ex.Message}");
                }
            }
        }


        private void UpdateSteamID()
        {
            IsLoginMenuVisible = !IsLoginMenuVisible;
            _skinService.UpdateSteamID(SteamLoginIDText);
            GetSkinsCommand.Execute(null);
        }

        private void OpenLink(string url)
        {
            try
            {
                // Using Microsoft.Maui.ApplicationModel.Launcher to open the URL
                Launcher.OpenAsync(new Uri(url));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unable to open link: {ex.Message}");
            }
        }

        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        string searchText;

        [ObservableProperty]
        private string steamLoginIDText;


        private double totalPriceLatestSell;
        public double TotalPriceLatestSell
        {
            get => totalPriceLatestSell;
            set => SetProperty(ref totalPriceLatestSell, Math.Round(value, 2));
        }

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
            var filteredSkins = string.IsNullOrWhiteSpace(SearchText)
                ? allSkins.Where(IsSkinVisible).ToList()
                : allSkins.Where(skin => skin.MarketName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) && IsSkinVisible(skin)).ToList();

            Skins.Clear();
            foreach (var skin in filteredSkins)
            {
                Skins.Add(skin);
            }

            // Calculate the total price of visible skins and round to 2 decimal places
            TotalPriceLatestSell = Skins.Sum(skin => skin.PriceLatestSell);
        }

        // Color filter properties
        private bool _isConsumerGradeChecked = true;
        private bool _isIndustrialGradeChecked = true;
        private bool _isHighGradeChecked = true;
        private bool _isRestrictedChecked = true;
        private bool _isClassifiedChecked = true;
        private bool _isCovertChecked = true;
        private bool _isRareSpecialChecked = true;
        private bool _isContrabandChecked = true;

        public bool IsConsumerGradeChecked
        {
            get => _isConsumerGradeChecked;
            set
            {
                SetProperty(ref _isConsumerGradeChecked, value);
                FilterSkins();
            }
        }

        public bool IsIndustrialGradeChecked
        {
            get => _isIndustrialGradeChecked;
            set
            {
                SetProperty(ref _isIndustrialGradeChecked, value);
                FilterSkins();
            }
        }

        public bool IsHighGradeChecked
        {
            get => _isHighGradeChecked;
            set
            {
                SetProperty(ref _isHighGradeChecked, value);
                FilterSkins();
            }
        }

        public bool IsRestrictedChecked
        {
            get => _isRestrictedChecked;
            set
            {
                SetProperty(ref _isRestrictedChecked, value);
                FilterSkins();
            }
        }

        public bool IsClassifiedChecked
        {
            get => _isClassifiedChecked;
            set
            {
                SetProperty(ref _isClassifiedChecked, value);
                FilterSkins();
            }
        }

        public bool IsCovertChecked
        {
            get => _isCovertChecked;
            set
            {
                SetProperty(ref _isCovertChecked, value);
                FilterSkins();
            }
        }

        public bool IsContrabandChecked
        {
            get => _isContrabandChecked;
            set
            {
                SetProperty(ref _isContrabandChecked, value);
                FilterSkins();
            }
        }



        private bool _isFilterMenuVisible;
        public bool IsFilterMenuVisible
        {
            get => _isFilterMenuVisible;
            set => SetProperty(ref _isFilterMenuVisible, value);
        }

        public ICommand ShowFilterMenuCommand => new Command(ShowFilterMenu);

        private void ShowFilterMenu()
        {
            IsFilterMenuVisible = !IsFilterMenuVisible;
            
            if(IsLoginMenuVisible) 
            {
            IsLoginMenuVisible = !IsLoginMenuVisible;
            }
            
        }

        private bool _isLoginMenuVisible;
        public bool IsLoginMenuVisible
        {
            get => _isLoginMenuVisible;
            set => SetProperty(ref _isLoginMenuVisible, value);
        }

        public ICommand ShowLoginMenuCommand => new Command(ShowLoginMenu);
        private void ShowLoginMenu()
        {
            IsLoginMenuVisible = !IsLoginMenuVisible;

            if (IsFilterMenuVisible)
            {
                IsFilterMenuVisible = !IsFilterMenuVisible;
            }
        }


        bool IsSkinVisible(Skin skin)
        {
            if ((IsConsumerGradeChecked && skin.Color == "#b0c3d9") ||
                (IsIndustrialGradeChecked && skin.Color == "#5e98d9") ||
                (IsHighGradeChecked && skin.Color == "#4b69ff") ||
                (IsRestrictedChecked && skin.Color == "#8847ff") ||
                (IsClassifiedChecked && skin.Color == "#d32ce6") ||
                (IsCovertChecked && skin.Color == "#eb4b4b") ||
                (IsContrabandChecked && skin.Color == "#e4ae39"))
            {
                return true;
            }

            return false;
        }



    }
}
