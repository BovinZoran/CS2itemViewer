using CommunityToolkit.Mvvm.ComponentModel;
using CS2itemViewer.Model;
using Newtonsoft.Json;
using System.Web;

namespace CS2itemViewer.ViewModel
{
    [QueryProperty(nameof(SkinJson), "skin")]
    public partial class SkinDetailsViewModel : BaseViewModel
    {
        private Skin? _skin;

        [ObservableProperty]
        private string? skinJson;

        partial void OnSkinJsonChanged(string value)
        {
            var decodedSkinJson = HttpUtility.UrlDecode(value); // Decode the JSON string
            Skin = JsonConvert.DeserializeObject<Skin>(decodedSkinJson);
            OnPropertyChanged(nameof(MarketName));
            OnPropertyChanged(nameof(Image));
            OnPropertyChanged(nameof(ItemName));
            OnPropertyChanged(nameof(PriceLatestSell));
            OnPropertyChanged(nameof(Color));
            OnPropertyChanged(nameof(DescriptionFloat));
            OnPropertyChanged(nameof(DescriptionText));
        }

        public Skin? Skin
        {
            get => _skin;
            set
            {
                _skin = value;
                OnPropertyChanged(); // Ensure property change notification
            }
        }

        public string MarketName => Skin?.MarketName ?? "";
        public string Image => Skin?.Image ?? "";
        public string ItemName => Skin?.ItemName ?? "";
        public double PriceLatestSell => Skin?.PriceLatestSell ?? 0;
        public string Color => Skin?.Color ?? "";
        public string DescriptionFloat => Skin?.CleanDescriptionFloat ?? "";
        public string DescriptionText => Skin?.CleanDescriptionText ?? "";
    }
}