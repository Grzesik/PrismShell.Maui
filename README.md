# Introduction to [PrismShell.MAUI](https://github.com/Grzesik/PrismShell.Maui)

 

PrismShell is a framework to build loosely coupled applications for Xamarin.Forms and .Net.Maui. It combines the Shell-Navigation with many convenient elements from the [PrismLibrary](https://github.com/PrismLibrary/Prism). This library doesn’t have any dependencies, therefore some files were copied and modified from the original PrismLibrary.  Most parts of the library are compatible with the PrismLibrary, but there are also some enhancements. 

PrismShell is especially interesting for developers, who want to build Xamarin.Forms or .Net.Maui application based on the Shell and use the Shell navigation. If you’ve once used the PrismLibrary, you don’t want to miss many of the convenient elements from it. Please read the description, how to use the library. You'll find an example in the project [PrismShell.Forms](https://github.com/Grzesik/PrismShell.Forms) & [PrismShell.MAUI](https://github.com/Grzesik/PrismShell.Maui)

 

# Getting started

##  

## Step 1. Create a Shell Application

Create a Shell application (Xamarin.Forms or .Net.Maui). 

### Step2. Add Nuget Packages

Add the [PrismShell.Maui](https://www.nuget.org/packages/PrismShell.Maui/) Nuget package to the main project.

### Step3. Add an IoC Container

Add an IoC Container like the Microsoft.Extensions.DependencyInjection. You can use any other IoC Container, it is only necessary to implement the IServiceProvider interface for the IoC Container.

In .Net 7 this dependency is added for you, when you create a new Maui project.

### Step4. Initialize the library

First overload the AppShell like this:

`public partial class AppShell : PrismShell`

Example Shell.

```c#
public partial class AppShell
{
  public AppShell()
  {
     InitializeComponent();
   }
}

prism:PrismShell
    x:Class="ShellWithPrismMaui.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:prism="clr-namespace:Prism;assembly=PrismShell.Maui"
    Title="ShellWithPrismMaui">           
           
```



Register all Services, Views and ViewModels  in the MauiProgram class.

```c#
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
           
```



Register all Views and ViewModels for navigation in the MauiProgramm class:

`RegisterForNavigation.Register<MyPage, MyViewModel>();`



Add all ViewModels to the IoC Container:

```
mauiAppBuilder.Services.AddTransient<MyViewModel>();
…
```



Add framework services to the IoC Container:

```
mauiAppBuilder.Services.AddSingleton<INavigationService, NavigationService>();
mauiAppBuilder.Services.AddSingleton<IEventAggregator, EventAggregator>();
mauiAppBuilder.Services.AddSingleton<IPageDialogService, PageDialogService>();
```

After the IoC Container is created, initialize the framework:

`PrismShell.Initialize(ServiceProvider);`

Example App.

```c#
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

        //Important for the framework to get the IoC!
        Prism.PrismShell.Initialize(app.Services);

        return app;
  }


  public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder)
  {
      //Add framework services
      mauiAppBuilder.Services.AddSingleton<INavigationService, NavigationService>();
      mauiAppBuilder.Services.AddSingleton<IEventAggregator, EventAggregator>();
      mauiAppBuilder.Services.AddSingleton<IPageDialogService, PageDialogService>();

      //Add app services
      mauiAppBuilder.Services.<IDataStore<Item>, MockDataStore>();
      return mauiAppBuilder;
  }
    
  public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
  {
      mauiAppBuilder.Services.AddTransient<AboutViewModel>();
      mauiAppBuilder.Services.AddTransient<LoginViewModel>();
      return mauiAppBuilder;
  }
    
  public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
  {
      //Connect Views to ViewModels
	  RegisterForNavigation.Register<AboutPage, AboutViewModel>();
      RegisterForNavigation.Register<LoginPage, LoginViewModel>();

      //Register all routs for Shell
      Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
      return mauiAppBuilder;
  }
}
```



### Page

There are no changes needed for the framework.

### ViewModels

ViewModels, like in Prism, can implement some interfaces, which will be called from the framework. Typically, you can create a base class for the ViewModels like this:

`public class BaseViewModel : INotifyPropertyChanged, IPageLifecycleAware, INavigatedAware, IPageAware`

When IPageLifecycleAware is defined, every time when the page fires the Appearing and Disappearing events, the viewmodel is notified. 

When INavigatedAware is defined, the 

`void OnNavigatedFrom(INavigatingParameters param)` 

is called from the current viewmodel, before navigated to the next one. The INavigatingParameters parameters include additionaly, in compare to the PrismLibrary, the CurrentUrl and the TargetUrl. It allows to make decisions in code, depending on the navigation url (from Shell). Additionally, it can also includes a function:

`Func<Task<bool>> OnCancel { get; set; }`

to cancel the navigation, when the OnCancel function is defined and it returns true.  It is very useful, when you need to save data etc.

Example.

```
public override void OnNavigatedFrom(INavigatingParameters param)
{
   base.OnNavigatedFrom(param);

   //Call dialog, before navigate back
   if (param.TargetUrl.Contains(".."))
   {
      param.OnCancel = CancelDialog;
   }
}

private async Task<bool> CancelDialog()
{
   var ret = await pageDialogService.DisplayAlertAsync("Navigation Dialog", "Do you want to cancel the navigation?", "Yes", "No");

  return ret;
}
```

When the IPageAware is defined, the name of the page for the viewmodel is known in the viewmodel.

Example of BaseViewModel.

```
public class BaseViewModel : INotifyPropertyChanged, IPageLifecycleAware, INavigatedAware, IPageAware
{
   protected string pagename;

   bool isBusy = false;
   public bool IsBusy
   {
      get { return isBusy; }
      set { SetProperty(ref isBusy, value); }
   }

   string title = string.Empty;
   public string Title
   {
      get { return title; }
      set { SetProperty(ref title, value); }
   }

   #region --[Interfaces]--

    protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null)
   {
      if (EqualityComparer<T>.Default.Equals(backingStore, value))
        return false;

      backingStore = value;
      onChanged?.Invoke();
      OnPropertyChanged(propertyName);
      return true;
   }

   public event PropertyChangedEventHandler PropertyChanged;

   protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
   {
      var changed = PropertyChanged;
      if (changed == null)
        return;

      changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
   }

   public virtual void OnAppearing()
   {
   }

   public virtual void OnDisappearing()
   {
   }

   /// <summary>
   /// Called, when the page is going to navigate to another page or back. 
   /// The navigation can be canceled
   /// In the param are parameters from the actual view model. 
   /// The parameters are transfered to the next view model
   /// </summary>
   /// <param name="param"></param>
   public virtual void OnNavigatedFrom(INavigatingParameters param)
   {
      Console.WriteLine($"OnNavigatedFrom for page {pagename}");
   }

   /// <summary>
   /// Called, when the page is the current page (it was navigated to the page). 
   /// In the param are parameters from the calling view model
   /// </summary>
   /// <param name="param"></param>
   public virtual void OnNavigatedTo(INavigatedParameters param)
   {
      Console.WriteLine($"OnNavigatedTo for page {pagename}");
   }

   public virtual void SetPageName(string name)
   {
      pagename = name;
   }

   #endregion
}
```



### Navigation

The navigation is the same like in the Shell. It is the biggest change in compared to the PrismLibrary. There are two possibilities to navigate to another ViewModel. The first one is:

`await navigationService.GoToAsync(nameof(NextPage));`

The Url to the next Page/ViewModel is the same like in the Shell. 

It is possible, like in Prism, to transfer parameters to the next viewmodel.

Example.

```
public MyViewModel(INavigationService navigtionService)
{
…

async void OnItemSelected(Item item)
{
   var param = new NavigationParameters();
   param.Add("ItemId", item.Id);

   await navigationService.GoToAsync($"{nameof(ItemDetailPage)}", param);
}
```

When navigating, first the 

`void OnNavigatedFrom(INavigatingParameters param)` 

function in the active ViewMadel is called. The param contains the parameters from the navigation call and additionally the url for the current and target navigation. It is also possible to cancel the navigation, when the OnCancel function is defined and it returns true. 

Example.

```
public override void OnNavigatedFrom(INavigatingParameters param)
{
  base.OnNavigatedFrom(param);

  //Call a dialog, before navigate back
  if (param.TargetUrl.Contains(".."))
  {
     param.OnCancel = CancelDialog;
  }
}

private async Task<bool> CancelDialog()
{
  var ret = await pageDialogService.DisplayAlertAsync("Navigation Dialog", "Do you want to cancel the navigation?", "Yes", "No");

  return ret;
} 
```

After the OnNavigatedFrom was called, the void OnNavigatedTo(INavigatedParameters param) from the next ViewModel is called. It contains the parameters from the navigation call and the urls for the previous and current navigation.

Example.

```
public override async void OnNavigatedTo(INavigatedParameters param)
{
   base.OnNavigatedTo(param);

   if(param?.NavigationParamaters != null && param.NavigationParamaters.ContainsKey("ItemId"))
   {
      var itemId = (string)param.NavigationParamaters["ItemId"];
      await LoadItemId(itemId);
   }
}
```

There is also another possibility to navigate to another ViewModel. It is possible to use just the Shell for the navigation directly.

`await Shell.Current.GoToAsync($"{nameof(MyPage)}");`

It is also possible to transmit parameters to the next viewmodel. Use the function: NavigationService.SetParameter(param)

Example.

```
async void OnItemSelected(Item item)
{
   var param = new NavigationParameters();
   param.Add("ItemId", item.Id);

   navigationService.SetParameter(param);
   await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}");
}
```

The navigation in this case has the same behavior, like using the NavigationService. Using the NavigationService is a preferred way to navigate with parameters.

### Page Dialog Service

PageDialogService is used to display alerts or to make a choice. It is the same as in the PrismLibrary.

Please read it for a detailed description: [Using the Page Dialog Service | Prism (prismlibrary.com)](https://prismlibrary.com/docs/xamarin-forms/dialogs/page-dialog-service.html)

 

### Event Aggregator

EventAggregator is a loosely coupled communication between two or one to many components. It is the same as in the PrismLibrary.

Please read it for a detailed description: [Event Aggregator | Prism (prismlibrary.com)](https://prismlibrary.com/docs/event-aggregator.html)

