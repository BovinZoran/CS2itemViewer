using CommunityToolkit.Maui;
using CS2itemViewer.Services;
using CS2itemViewer.ViewModel;
using Microsoft.Extensions.Logging;

namespace CS2itemViewer
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            //services
            builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
            builder.Services.AddSingleton<ISkinService,SkinService>();
            //models
            builder.Services.AddSingleton<SkinViewModel>();
            builder.Services.AddTransient<SkinDetailsViewModel>();
            builder.Services.AddSingleton<BaseViewModel>();
            //pages
            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<DetailsPage>();
            //community toolkit
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
     #if DEBUG
            builder.Logging.AddDebug();
     #endif
            return builder.Build();
        }
    }
}
