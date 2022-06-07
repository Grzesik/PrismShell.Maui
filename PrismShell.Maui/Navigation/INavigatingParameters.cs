namespace Prism.Navigation
{
    public interface INavigatingParameters
    {
        INavigationParameters NavigationParamaters { get; }

        //New for canceling the navigation (used in OnNavigatedFrom)
        //false - don't cancel;  true - cancel
        //If not defined - don't cancel
        Func<Task<bool>> OnCancel { get; set; }

        string CurrentUrl { get; set; }
        string TargetUrl { get; set; }
    }
}
