using CS2itemViewer.Model;
using CS2itemViewer.ViewModel;

namespace CS2itemViewer
{
    public partial class MainPage : ContentPage
    {
       

        public MainPage(SkinViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;


        }


        private async void OnImageTapped(object sender, EventArgs e)
        {
            // Get the tapped skin object
            var tappedSkin = (sender as Image)?.BindingContext as Skin;

            if (tappedSkin != null)
            {
                // Navigate to the DetailsPage and pass the skin object as a parameter
                await Navigation.PushAsync(new DetailsPage(new SkinDetailsViewModel(tappedSkin)));
            }
        }

        private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            var viewModel = BindingContext as SkinViewModel;
            if (viewModel != null)
            {
                viewModel.FilterSkinsCommand.Execute(null);
            }
        }

        

        



    }

}
