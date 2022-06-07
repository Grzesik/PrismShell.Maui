using Prism.Navigation;
using Prism.PageDialog;
using Prism.SystemEvents;
using ShellWithPrismMaui.Events;
using ShellWithPrismMaui.Views;
using System.Windows.Input;

namespace ShellWithPrismMaui.ViewModels
{
    public class CatsViewModel : BaseViewModel
    {
        private readonly INavigationService navigationService;
        private readonly IEventAggregator eventAggregator;
        private readonly IPageDialogService pageDialogService;

        public CatsViewModel(INavigationService navigationService, IEventAggregator eventAggregator, IPageDialogService pageDialogService)
        {
            this.navigationService = navigationService;
            this.eventAggregator = eventAggregator;
            this.pageDialogService = pageDialogService;

            CallPage = new Command(OnCallPage);
            eventAggregator.GetEvent<NotifyEvent>().Subscribe(OnNotifyEvent);
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

        private async void OnCallPage(object obj)
        {
            await navigationService.GoToAsync(nameof(MyCatPage));
        }


        private void OnNotifyEvent(NotifyEventData data)
        {
            //event was received
            //do something ....
        }
    }
}
