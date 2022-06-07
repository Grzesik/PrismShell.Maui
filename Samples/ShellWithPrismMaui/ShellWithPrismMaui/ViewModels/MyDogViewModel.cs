using Prism.Navigation;
using System.Windows.Input;

namespace ShellWithPrismMaui.ViewModels
{
    internal class MyDogViewModel : BaseViewModel
    {
        private readonly INavigationService dynamicNavigation;

        public MyDogViewModel(INavigationService dynamicNavigation)
        {
            this.dynamicNavigation = dynamicNavigation;
            CloseCommand = new Command(OnCloseCommand);
        }

        public ICommand CloseCommand { get; private set; }

        public override void OnNavigatedTo(INavigatedParameters param)
        {
            base.OnNavigatedTo(param);
        }

        private void OnCloseCommand(object obj)
        {
            dynamicNavigation.GoToAsync("..");
        }
    }
}
