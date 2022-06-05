namespace Prism.Navigation
{
    public class NavigatingParameters : INavigatingParameters
    {
        public INavigationParameters NavigationParamaters { get; set; }

        //New for canceling the navigation (used in OnNavigatedFrom)
        ////false - don't cancel;  true - cancel
        public Func<Task<bool>> NavigationDialog { get; set; }

        public string CurrentUrl { get; set; }
        public string TargetUrl { get; set; }
    }
}
