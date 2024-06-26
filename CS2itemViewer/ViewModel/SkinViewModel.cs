﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CS2itemViewer.Model;
using CS2itemViewer.Services;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;

namespace CS2itemViewer.ViewModel
{
    public partial class SkinViewModel : BaseViewModel
    {
        public ObservableCollection<Skin> Skins { get; set; } = new ObservableCollection<Skin>();

        private readonly ISkinService _skinService;
        private readonly IConnectivity _connectivity;
        private List<Skin> allSkins;

        public SkinViewModel(ISkinService skinService, IConnectivity connectivity)
        {
            Title = "CS2 item Viewer";
            _skinService = skinService;
            _connectivity = connectivity;
            allSkins = new List<Skin>();

            SteamLoginIDText = "76561198268749335"; // Zoran's Steam Id

            OpenLinkCommand = new RelayCommand<string>(OpenLink);

            OnSearchedCommand = new Command(OnSearched);

            LoadLoginCommand = new RelayCommand(UpdateSteamID);

            // Call the GetSkinsCommand command when the ViewModel is constructed
            GetSkinsCommand.Execute(null);
        }

        //-------Open the filtering menu--------
        public ICommand ShowFilterMenuCommand => new Command(ShowFilterMenu);
        private void ShowFilterMenu()
        {
            IsFilterMenuVisible = !IsFilterMenuVisible;

            if (IsLoginMenuVisible)
            {
                IsLoginMenuVisible = !IsLoginMenuVisible;
            }

        }

        private bool _isFilterMenuVisible;
        public bool IsFilterMenuVisible
        {
            get => _isFilterMenuVisible;
            set => SetProperty(ref _isFilterMenuVisible, value);
        }

        //-------Open the login menu--------
        public ICommand ShowLoginMenuCommand => new Command(ShowLoginMenu);
        private void ShowLoginMenu()
        {
            IsLoginMenuVisible = !IsLoginMenuVisible;

            if (IsFilterMenuVisible)
            {
                IsFilterMenuVisible = !IsFilterMenuVisible;
            }
        }

        private bool _isLoginMenuVisible;
        public bool IsLoginMenuVisible
        {
            get => _isLoginMenuVisible;
            set => SetProperty(ref _isLoginMenuVisible, value);
        }

        //-------Load in Steam invertory with sID--------
        [ObservableProperty]
        private string steamLoginIDText;
        public ICommand LoadLoginCommand { get; }
        private void UpdateSteamID()
        {
            IsLoginMenuVisible = !IsLoginMenuVisible;
            _skinService.UpdateSteamID(SteamLoginIDText);
            GetSkinsCommand.Execute(null);
        }

        //-------Open link to Steam ID page--------
        public ICommand OpenLinkCommand { get; }
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

        //-------checking item names for seached item--------
        [ObservableProperty]
        string searchText;

        public Command OnSearchedCommand { get; }

        [RelayCommand]
        private void OnSearched()
        {
            FilterSkins();
        }

        //-------Sorting by Price--------
        public ICommand SortByPriceAscendingDescendingCommand => new Command(SortByPriceAscendingDescending);
        private string _priceAscendingDescending = "Price Asc";
        private void SortByPriceAscendingDescending()
        {

            IsSortByPriceAscending = !IsSortByPriceAscending;
            PriceAscendingDescending = IsSortByPriceAscending ? "Price Desc" : "Price Asc";

        }
        public string PriceAscendingDescending
        {
            get => _priceAscendingDescending;
            set => SetProperty(ref _priceAscendingDescending, value);
        }

        private bool _isSortByPriceAscending = false;
        public bool IsSortByPriceAscending
        {
            get => _isSortByPriceAscending;
            set
            {
                SetProperty(ref _isSortByPriceAscending, value);
                FilterSkins();
            }
        }

        private bool _SortByPriceChecked = false;
        public bool SortByPriceChecked
        {
            get => _SortByPriceChecked;
            set
            {
                SetProperty(ref _SortByPriceChecked, value);
                FilterSkins();
            }
        }

        //-------Sorting by Rarity--------
        public ICommand SortByRarityAscendingDescendingCommand => new Command(SortByRarityAscendingDescending);
        private string _rarityAscendingDescending = "Rarity Asc";
        private void SortByRarityAscendingDescending()
        {

            IsSortByRarityAscending = !IsSortByRarityAscending;
            RarityAscendingDescending = IsSortByRarityAscending ? "Rarity Desc" : "Rarity Asc";

        }
        public string RarityAscendingDescending
        {
            get => _rarityAscendingDescending;
            set => SetProperty(ref _rarityAscendingDescending, value);
        }

        private bool _isSortByRarityAscending = false;
        public bool IsSortByRarityAscending
        {
            get => _isSortByRarityAscending;
            set
            {
                SetProperty(ref _isSortByRarityAscending, value);
                FilterSkins();
            }
        }

        private bool _SortByRarityChecked = false;

        public bool SortByRarityChecked
        {
            get => _SortByRarityChecked;
            set
            {
                SetProperty(ref _SortByRarityChecked, value);
                FilterSkins();
            }
        }

        //-------Go to detailsPage with skin info--------
        [RelayCommand]
        async Task GoToDetails(Skin selectedSkin)
        {
            if (selectedSkin != null)
            {
                var skinJson = JsonConvert.SerializeObject(selectedSkin);
                var encodedSkinJson = HttpUtility.UrlEncode(skinJson); // Encode the JSON string
                await Shell.Current.GoToAsync($"DetailsPage?skin={encodedSkinJson}");
            }
        }

        //-------Setting price to 0.00 accurate--------
        private double totalPriceLatestSell;
        public double TotalPriceLatestSell
        {
            get => totalPriceLatestSell;
            set => SetProperty(ref totalPriceLatestSell, Math.Round(value, 2));
        }

        //-------Gettiing skins from API + error messages if unable--------
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
                IsRefreshing = true; // Start refreshing
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
                IsRefreshing = false; // Stop refreshing
            }
        }

        //-------Filtering--------
        private int GetRarityOrder(string color)
        {
            return color switch
            {
                "#b0c3d9" => 1, // Consumer Grade
                "#5e98d9" => 2, // Industrial Grade
                "#4b69ff" => 3, // High Grade
                "#8847ff" => 4, // Restricted
                "#d32ce6" => 5, // Classified
                "#eb4b4b" => 6, // Covert
                "#e4ae39" => 7, // Contraband
                _ => 8 // Any other color
            };
        }

        [RelayCommand]
        void FilterSkins()
        {
            var filteredSkins = string.IsNullOrWhiteSpace(SearchText)
                ? allSkins.Where(IsSkinVisible).ToList()
                : allSkins.Where(skin => skin.MarketName.Contains(SearchText, StringComparison.OrdinalIgnoreCase) && IsSkinVisible(skin)).ToList();

            if (SortByPriceChecked)
            {
                // Sort by price if toggled
                if (IsSortByPriceAscending)
                {
                    filteredSkins = filteredSkins.OrderBy(skin => skin.PriceLatestSell).ToList();
                }
                else
                {
                    filteredSkins = filteredSkins.OrderByDescending(skin => skin.PriceLatestSell).ToList();
                }
            }

            if (SortByRarityChecked)
            {
                // Sort by rarity if toggled
                if (IsSortByRarityAscending)
                {
                    filteredSkins = filteredSkins.OrderBy(skin => GetRarityOrder(skin.Color)).ToList();
                }
                else
                {
                    filteredSkins = filteredSkins.OrderByDescending(skin => GetRarityOrder(skin.Color)).ToList();
                }
            }

            Skins.Clear();
            foreach (var skin in filteredSkins)
            {
                Skins.Add(skin);
            }

            // Calculate the total price of visible skins and round to 2 decimal places
            TotalPriceLatestSell = Skins.Sum(skin => skin.PriceLatestSell);
        }

        //-------checking checkboxes to filter rarity--------
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

        //-------Rarity checkbox properties--------
        private bool _isConsumerGradeChecked = true;
        private bool _isIndustrialGradeChecked = true;
        private bool _isHighGradeChecked = true;
        private bool _isRestrictedChecked = true;
        private bool _isClassifiedChecked = true;
        private bool _isCovertChecked = true;
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
    }
}
