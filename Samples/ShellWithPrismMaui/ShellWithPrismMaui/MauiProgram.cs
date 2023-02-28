using Prism.Navigation;
using Prism.PageDialog;
using Prism.SystemEvents;
using ShellWithPrismMaui.Models;
using ShellWithPrismMaui.Services;
using ShellWithPrismMaui.ViewModels;
using ShellWithPrismMaui.Views;

namespace ShellWithPrismMaui;

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
			})
			.RegisterAppServices()
            .RegisterViewModels()
            .RegisterViews();

        var app = builder.Build();

        //Important for the framework!
        Prism.PrismShell.Initialize(app.Services);

        return app;
    }

    public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
    {
        //Add framework services
        mauiAppBuilder.Services.AddSingleton<INavigationService, NavigationService>();
        mauiAppBuilder.Services.AddSingleton<IEventAggregator, EventAggregator>();
        mauiAppBuilder.Services.AddSingleton<IPageDialogService, PageDialogService>();

        // Add app services
        mauiAppBuilder.Services.AddSingleton<IDataStore<Item>, MockDataStore>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
    {
        // Add ViewModels to IoC container
        mauiAppBuilder.Services.AddTransient<AboutViewModel>();
        mauiAppBuilder.Services.AddTransient<ItemDetailViewModel>();
        mauiAppBuilder.Services.AddTransient<ItemsViewModel>();
        mauiAppBuilder.Services.AddTransient<LoginViewModel>();
        mauiAppBuilder.Services.AddTransient<NewItemViewModel>();
        mauiAppBuilder.Services.AddTransient<CatsViewModel>();
        mauiAppBuilder.Services.AddTransient<MyCatViewModel>();
        mauiAppBuilder.Services.AddTransient<DogsViewModel>();
        mauiAppBuilder.Services.AddTransient<MyDogViewModel>();

        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        //Connect Views to ViewModels
        RegisterForNavigation.Register<AboutPage, AboutViewModel>();
        RegisterForNavigation.Register<ItemDetailPage, ItemDetailViewModel>();
        RegisterForNavigation.Register<ItemsPage, ItemsViewModel>();
        RegisterForNavigation.Register<LoginPage, LoginViewModel>();
        RegisterForNavigation.Register<NewItemPage, NewItemViewModel>();
        RegisterForNavigation.Register<CatsPage, CatsViewModel>();
        RegisterForNavigation.Register<MyCatPage, MyCatViewModel>();
        RegisterForNavigation.Register<DogsPage, DogsViewModel>();
        RegisterForNavigation.Register<MyDogPage, MyDogViewModel>();

        //Register all routs, which are not defined in the menu -> Shell routs
        Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
        Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        Routing.RegisterRoute(nameof(MyCatPage), typeof(MyCatPage));
        Routing.RegisterRoute(nameof(MyDogPage), typeof(MyDogPage));

        return mauiAppBuilder;
    }
}
