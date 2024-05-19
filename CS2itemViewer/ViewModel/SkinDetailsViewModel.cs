using CommunityToolkit.Mvvm.ComponentModel;
using CS2itemViewer.Model;

namespace CS2itemViewer.ViewModel
{
    public partial class SkinDetailsViewModel : BaseViewModel
    {
        private Skin? _skin;

        public Skin? Skin
        {
            get => _skin;
            set => SetProperty(ref _skin, value);
        }

        public string MarketName => Skin?.MarketName ?? "";
        public string Image => Skin?.Image ?? "";
        public string ItemName => Skin?.ItemName ?? "";
        public double PriceLatestSell => Skin?.PriceLatestSell ?? 0;
        public string Color => Skin?.Color ?? "";
        public string DescriptionFloat => Skin?.CleanDescriptionFloat ?? "";
        public string DescriptionText => Skin?.CleanDescriptionText ?? "";

        public SkinDetailsViewModel()
        {
        }

        public SkinDetailsViewModel(Skin skin)
        {
            Skin = skin;
        }
    }
}