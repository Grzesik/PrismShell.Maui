using Prism.Navigation;
using System;
using System.Windows.Input;

namespace ShellWithPrismMaui.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
        }

        public ICommand OpenWebCommand { get; }


        public override void OnNavigatedFrom(INavigatingParameters param)
        {
            base.OnNavigatedFrom(param);
        }

        public override void OnNavigatedTo(INavigatedParameters param)
        {
            base.OnNavigatedTo(param);
        }
    }
}