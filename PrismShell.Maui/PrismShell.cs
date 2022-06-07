using Prism.Navigation;
using System;

namespace Prism
{
    public class PrismShell : Shell
    {
        public PrismShell()
        {

        }

        /// <summary>
        /// Set up the framework.
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Initialize(IServiceProvider serviceProvider)
        {
            NavigationService.SetServiceProvider(serviceProvider);
        }

        protected override async void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);

            if (Shell.Current?.CurrentPage.BindingContext is INavigatedAware)
            {
                Console.WriteLine("*************************************************************");
                Console.WriteLine($"AppShell_Navigating {Shell.Current.CurrentPage.ToString()}");
                Console.WriteLine("*************************************************************");

                NavigatingParameters param = new NavigatingParameters();
                param.NavigationParamaters = NavigationService.GetParameter();
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

            if (Shell.Current?.CurrentPage.BindingContext == null)
            {
                NavigationService.SetViewModel(Shell.Current.CurrentPage);
            }

            if (Shell.Current?.CurrentPage.BindingContext is INavigatedAware)
            {
                Console.WriteLine("*************************************************************");
                Console.WriteLine($"AppShell_Navigated {Shell.Current.CurrentPage.ToString()}");
                Console.WriteLine("*************************************************************");

                NavigatedParameters param = new NavigatedParameters();
                param.NavigationParamaters = NavigationService.GetParameter();
                param.CurrentUrl = args.Current?.Location.OriginalString;
                param.PreviousUrl = args.Previous?.Location.OriginalString;

                ((INavigatedAware)Shell.Current.CurrentPage.BindingContext).OnNavigatedTo(param);

                //the parameter was set to viewmodels and now its time to delete it!
                NavigationService.ClearParameter();
            }
        }
    }
}
