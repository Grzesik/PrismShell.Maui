namespace ShellWithPrismMaui;

public partial class AppShell
{
    public AppShell()
    {
        InitializeComponent();
    }

    private async void OnMenuItemClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
