using Prism.Navigation;
using System.Diagnostics;

namespace Prism
{
	public class PrismShell : Shell
    {
        internal static IServiceProvider _serviceProvider;


		public PrismShell()
        {

        }

        /// <summary>
        /// Set up the framework.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
			_serviceProvider = serviceProvider;
        }

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (Shell.Current?.CurrentPage.BindingContext is INavigatedAware)
            {
                Debug.WriteLine("*************************************************************");
                Debug.WriteLine($"PrisShell Navigating from {Shell.Current.CurrentPage.ToString()}");
				Debug.WriteLine("*************************************************************");

                var navigationService = _serviceProvider?.GetService<INavigationService>();

                if(navigationService == null )
                {
					Debug.WriteLine("*************************************************************");
                    Debug.WriteLine($"PrisShell - the IServiceProvider is not initialized!");
					Debug.WriteLine("*************************************************************");
				}

				NavigatingParameters param = new NavigatingParameters();
                param.NavigationParamaters = navigationService?.GetParameter();
                param.CurrentUrl = args.Current?.Location.OriginalString;
                param.TargetUrl = args.Target?.Location.OriginalString;

                ((INavigatedAware)Shell.Current.CurrentPage.BindingContext).OnNavigatedFrom(param);

                if (param.OnCancel != null)
                {
                    ShellNavigatingDeferral token = args.GetDeferral();

                    if (args.CanCancel && await param.OnCancel() == true)
                    {
                        args.Cancel();
                    }

                    token.Complete();
                }
            }
        }

        protected override void OnNavigated(ShellNavigatedEventArgs args)
        {
            base.OnNavigated(args);

			var navigationService = _serviceProvider?.GetService<INavigationService>();

			if (Shell.Current?.CurrentPage.BindingContext == null)
            {
				navigationService?.SetViewModel(Shell.Current.CurrentPage);
            }

            if (Shell.Current?.CurrentPage.BindingContext is INavigatedAware)
            {
                Console.WriteLine("*************************************************************");
                Console.WriteLine($"PrisShell Navigated from {Shell.Current.CurrentPage.ToString()}");
                Console.WriteLine("*************************************************************");

                NavigatedParameters param = new NavigatedParameters();
                param.NavigationParamaters = navigationService?.GetParameter();
                param.CurrentUrl = args.Current?.Location.OriginalString;
                param.PreviousUrl = args.Previous?.Location.OriginalString;

                ((INavigatedAware)Shell.Current.CurrentPage.BindingContext).OnNavigatedTo(param);

				//the parameter was set to viewmodels and now its time to delete it!
				navigationService?.ClearParameter();
            }
        }
    }
}
