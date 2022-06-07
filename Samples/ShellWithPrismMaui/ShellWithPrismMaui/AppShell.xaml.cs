using ShellWithPrismMaui.Views;

namespace ShellWithPrismMaui;

public partial class AppShell : Prism.PrismShell
{
    public AppShell()
    {
        InitializeComponent();

        //Register all routs, which are not defined in the menu
        Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
        Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        Routing.RegisterRoute(nameof(MyCatPage), typeof(MyCatPage));
        Routing.RegisterRoute(nameof(MyDogPage), typeof(MyDogPage));
    }

    private async void OnMenuItemClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
