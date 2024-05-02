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

        private void OnCounterClicked(object sender, EventArgs e)
        {
          
        }
        private void OnButton2Clicked(object sender, EventArgs e)
        {

        }
        private void OnButton3Clicked(object sender, EventArgs e)
        {

        }
    }

}
