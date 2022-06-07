using Prism;
using Prism.Navigation;
using Prism.PageDialog;
using Prism.SystemEvents;
using ShellWithPrismMaui.Models;
using ShellWithPrismMaui.Services;
using ShellWithPrismMaui.ViewModels;
using ShellWithPrismMaui.Views;

namespace ShellWithPrismMaui;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; set; }

    public App()
	{
		InitializeComponent();

        SetupServices();
        SetupNavigation();

        MainPage = new AppShell();
	}

    /// <summary>
    /// Add:
    /// 1. All services
    /// 2. All classes, which should be known to the IoC like ViewModels
    /// </summary>
    void SetupServices()
    {
        var services = new ServiceCollection();

        // Add platform specific services
        //addPlatformServices?.Invoke(services);
        SetupFrameworkServices(services);

        // Add ViewModels to IoC container
        services.AddTransient<AboutViewModel>();
        services.AddTransient<ItemDetailViewModel>();
        services.AddTransient<ItemsViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<NewItemViewModel>();
        services.AddTransient<CatsViewModel>();
        services.AddTransient<MyCatViewModel>();
        services.AddTransient<DogsViewModel>();
        services.AddTransient<MyDogViewModel>();

        // Add services
        services.AddSingleton<IDataStore<Item>, MockDataStore>();

        //create the IoC container
        ServiceProvider = services.BuildServiceProvider();

        //Important for the framework!
        Prism.PrismShell.Initialize(ServiceProvider);
    }

    void SetupFrameworkServices(IServiceCollection services)
    {
        //Add framework services
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IEventAggregator, EventAggregator>();
        services.AddSingleton<IPageDialogService, PageDialogService>();
    }

    /// <summary>
    /// Connects View with ViewModel.
    /// If the connection is not defined, you must create the viewmodel manually and set it to th BindinContext of the view.
    /// </summary>
    void SetupNavigation()
    {
        RegisterForNavigation.Register<AboutPage, AboutViewModel>();
        RegisterForNavigation.Register<ItemDetailPage, ItemDetailViewModel>();
        RegisterForNavigation.Register<ItemsPage, ItemsViewModel>();
        RegisterForNavigation.Register<LoginPage, LoginViewModel>();
        RegisterForNavigation.Register<NewItemPage, NewItemViewModel>();
        RegisterForNavigation.Register<CatsPage, CatsViewModel>();
        RegisterForNavigation.Register<MyCatPage, MyCatViewModel>();
        RegisterForNavigation.Register<DogsPage, DogsViewModel>();
        RegisterForNavigation.Register<MyDogPage, MyDogViewModel>();
    }

    public static BaseViewModel GetViewModel<TViewModel>() where TViewModel : BaseViewModel
    {
        return App.ServiceProvider.GetService<TViewModel>();
    }

    protected override void OnStart()
    {
    }

    protected override void OnSleep()
    {
    }

    protected override void OnResume()
    {
    }
}
