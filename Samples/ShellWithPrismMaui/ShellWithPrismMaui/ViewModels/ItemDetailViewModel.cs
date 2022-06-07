using Prism.Navigation;
using ShellWithPrismMaui.Models;
using ShellWithPrismMaui.Services;
using System.Diagnostics;

namespace ShellWithPrismMaui.ViewModels
{
    //[QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private string text;
        private string description;

        public ItemDetailViewModel(IDataStore<Item> dataStore)
        {
            DataStore = dataStore;
        }

        public string Id { get; set; }

        public string Text
        {
            get => text;
            set => SetProperty(ref text, value);
        }

        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }

        public string ItemId
        {
            get
            {
                return itemId;
            }
            set
            {
                itemId = value;
                //LoadItemId(value);
            }
        }

        public IDataStore<Item> DataStore { get; }

        public async Task LoadItemId(string itemId)
        {
            try
            {
                var item = await DataStore.GetItemAsync(itemId);
                Id = item.Id;
                Text = item.Text;
                Description = item.Description;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }

        public override async void OnNavigatedTo(INavigatedParameters param)
        {
            base.OnNavigatedTo(param);

            if (param?.NavigationParamaters != null && param.NavigationParamaters.ContainsKey("ItemId"))
            {
                var itemId = (string)param.NavigationParamaters["ItemId"];
                await LoadItemId(itemId);
            }
        }
    }
}
