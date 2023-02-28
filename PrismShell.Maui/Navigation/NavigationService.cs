using PrismShell.Maui.ResX;

namespace Prism.Navigation
{
    public class NavigationService : INavigationService
    {
        #region --[fields]--

        private static INavigationParameters navigationParametersCache;
        private static IServiceProvider serviceProvideCacher;

        #endregion

        public NavigationService()
        {
        }

        #region --[shell navigation]--

        public Task GoToAsync(ShellNavigationState state, INavigationParameters param = null)
        {
            navigationParametersCache = param;
            return Shell.Current.GoToAsync(state);
        }

        #endregion

        #region --[parameters in navigation]--

        public static INavigationParameters GetParameter()
        {
            return navigationParametersCache;
        }

        public static void SetParameter(INavigationParameters param)
        {
            navigationParametersCache = param;
        }

        public static void ClearParameter()
        {
            navigationParametersCache = null;
        }

        #endregion

        #region --[ViewModel]--

        internal static void SetServiceProvider(IServiceProvider serviceProvider)
        {
            serviceProvideCacher = serviceProvider;
        }

        public static void SetViewModel(Page page, string pagename = null)
        {
            if (page != null)
            {
                var info = RegisterForNavigation.GetPageNavigationInfo(page.GetType());
                if (info != null && info.ViewModelType != null)
                {
                    if(serviceProvideCacher == null)
                    {
                        throw new InvalidOperationException(SystemResources.ServiceProviderNotSet);
                    }

                    var vm = serviceProvideCacher.GetService(info.ViewModelType);

                    if (vm is IPageLifecycleAware)
                    {
                        page.Appearing -= Page_Appearing;
                        page.Appearing += Page_Appearing;
                        page.Disappearing -= Page_Disappearing;
                        page.Disappearing += Page_Disappearing;
                    }

                    if (vm is IPageAware)
                    {
                        ((IPageAware)vm).SetPageName(pagename ?? page.GetType().Name);
                    }

                    page.BindingContext = vm;
                }
            }
        }

        private static void Page_Disappearing(object sender, EventArgs e)
        {
            var page = (Page)sender;
            if (page != null && page.BindingContext is IPageLifecycleAware)
            {
                ((IPageLifecycleAware)page.BindingContext).OnDisappearing();
            }
        }

        private static void Page_Appearing(object sender, EventArgs e)
        {
            var page = (Page)sender;
            if (page != null && page.BindingContext is IPageLifecycleAware)
            {
                ((IPageLifecycleAware)page.BindingContext).OnAppearing();
            }
        }

#endregion
    }
}
