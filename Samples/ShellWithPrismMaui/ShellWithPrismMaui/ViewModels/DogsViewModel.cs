using Prism.Navigation;
using ShellWithPrismMaui.Views;
using System.Windows.Input;

namespace ShellWithPrismMaui.ViewModels
{
    public class DogsViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;

        public DogsViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            CallPage = new Command(OnCallPage);
        }

        public ICommand CallPage { get; private set; }

        public override void OnNavigatedFrom(INavigatingParameters param)
        {
            base.OnNavigatedFrom(param);
        }

        public override void OnNavigatedTo(INavigatedParameters param)
        {
            base.OnNavigatedTo(param);
        }

        private void OnCallPage(object obj)
        {
            navigationService.GoToAsync(nameof(MyDogPage));
        }
    }
}
