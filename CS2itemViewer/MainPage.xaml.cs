using CS2itemViewer.Model;
using CS2itemViewer.Services;
using CS2itemViewer.ViewModel;

namespace CS2itemViewer
{
    public partial class MainPage : ContentPage
    {
        public MainPage(/*SkinViewModel viewModel*/)
        {
            InitializeComponent();
            //BindingContext = viewModel;
            var viewModel = new SkinViewModel(new SkinService(), Connectivity.Current, Navigation);
            BindingContext = viewModel;
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
