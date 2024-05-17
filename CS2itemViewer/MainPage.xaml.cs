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

        private void OnImageButtonClicked(object sender, EventArgs e)
        {
            // Handle the click event
            DisplayAlert("Image Button Clicked", "You clicked the image button!", "OK");

        }


    }

}
