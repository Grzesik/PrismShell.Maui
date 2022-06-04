using Prism.Navigation;
using System;

namespace Prism
{
    public class PrismShell : Shell
    {
        public PrismShell()
        {

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
                param.NavigationParamaters = DynamicNavigation.GetParameter();
                param.Source = args.Target.Location.OriginalString;
                ((INavigatedAware)Shell.Current.CurrentPage.BindingContext).OnNavigatedFrom(param);

                if (param.NavigationDialog != null)
                {
                    ShellNavigatingDeferral token = args.GetDeferral();

                    if (args.CanCancel && await param.NavigationDialog() == true)
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
                DynamicNavigation.SetViewModel(Shell.Current.CurrentPage);
            }

            if (Shell.Current?.CurrentPage.BindingContext is INavigatedAware)
            {
                Console.WriteLine("*************************************************************");
                Console.WriteLine($"AppShell_Navigated {Shell.Current.CurrentPage.ToString()}");
                Console.WriteLine("*************************************************************");
                ((INavigatedAware)Shell.Current.CurrentPage.BindingContext).OnNavigatedTo(DynamicNavigation.GetParameter());

                //the parameter was set to viewmodels and now its time to delete it!
                DynamicNavigation.ClearParameter();
            }
        }
    }
}
