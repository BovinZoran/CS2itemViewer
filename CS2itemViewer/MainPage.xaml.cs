using CS2itemViewer.Model;
using CS2itemViewer.Services;
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
    }

}
