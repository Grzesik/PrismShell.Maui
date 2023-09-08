using Microsoft.Extensions.DependencyInjection;
using PrismShell.Maui.ResX;

namespace Prism.Navigation
{
    public class NavigationService : INavigationService
    {
        #region --[fields]--

        private static INavigationParameters navigationParametersCache;
        private static IServiceProvider serviceProvideCacher;

        private IPageLifecycleAware vmPageLifecycleAware;


		#endregion

		public NavigationService()
        {
			serviceProvideCacher = PrismShell._serviceProvider;
		}

        #region --[shell navigation]--

        public Task GoToAsync(ShellNavigationState state, INavigationParameters param = null)
        {
            navigationParametersCache = param;
            return Shell.Current.GoToAsync(state);
        }

        #endregion

        #region --[parameters in navigation]--

        public INavigationParameters GetParameter()
        {
            return navigationParametersCache;
        }

        public void SetParameter(INavigationParameters param)
        {
            navigationParametersCache = param;
        }

        public void ClearParameter()
        {
            navigationParametersCache = null;
        }

        #endregion

        #region --[ViewModel]--

        public void SetViewModel(Page page, string pagename = null)
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
                        vmPageLifecycleAware = vm as IPageLifecycleAware;

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

        private void Page_Disappearing(object sender, EventArgs e)
        {
            var page = (Page)sender;
            if (page != null && page.BindingContext is IPageLifecycleAware)
            {
                ((IPageLifecycleAware)page.BindingContext).OnDisappearing();
            }
        }

        private void Page_Appearing(object sender, EventArgs e)
        {
            var page = (Page)sender;
            if (page != null && page.BindingContext is IPageLifecycleAware)
            {
                ((IPageLifecycleAware)page.BindingContext).OnAppearing();
            }
            else if (page != null && vmPageLifecycleAware != null) //when the BindingContext is set later, it calls the OnAppearing in viewmodel
			{
				vmPageLifecycleAware.OnAppearing();
			}
		}

#endregion
    }
}
