namespace Prism.Navigation
{
    public class NavigatedParameters : INavigatedParameters
    {
        public INavigationParameters NavigationParamaters { get; set; }

        public string PreviousUrl { get; set; }
        public string CurrentUrl { get; set; }
    }
}
