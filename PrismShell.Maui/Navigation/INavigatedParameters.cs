namespace Prism.Navigation
{
    public interface INavigatedParameters
    {
        INavigationParameters NavigationParamaters { get; }

        string PreviousUrl { get; set; }
        string CurrentUrl { get; set; }
    }
}
