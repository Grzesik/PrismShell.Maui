using Prism.Navigation;
using ShellWithPrismMaui.Models;
using ShellWithPrismMaui.Services;
using ShellWithPrismMaui.Views;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace ShellWithPrismMaui.ViewModels
{
    public class ItemsViewModel : BaseViewModel
    {
        private Item _selectedItem;
        private readonly INavigationService navigationService;

        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Item> ItemTapped { get; }

        public ItemsViewModel(IDataStore<Item> dataStore, INavigationService navigationService)
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Item>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
            DataStore = dataStore;
            this.navigationService = navigationService;
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public override async void OnNavigatedTo(INavigatedParameters param)
        {
            base.OnNavigatedTo(param);

            await ExecuteLoadItemsCommand();
        }

        public override void OnAppearing()
        {
            IsBusy = false;
            SelectedItem = null;
        }

        public Item SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        public IDataStore<Item> DataStore { get; }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(Item item)
        {
            if (item == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack - original way (also possible)
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");

            var param = new NavigationParameters();
            param.Add("ItemId", item.Id);

            await navigationService.GoToAsync($"{nameof(ItemDetailPage)}", param);

            //it is also possible to navigate to another page with parameters this way:
            //DynamicNavigation.SetParameter(param);
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}");

        }
    }
}