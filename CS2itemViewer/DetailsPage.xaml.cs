using CS2itemViewer.ViewModel;

namespace CS2itemViewer;

public partial class DetailsPage : ContentPage
{
    public DetailsPage(SkinDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

}