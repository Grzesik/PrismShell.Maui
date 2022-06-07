using ShellWithPrismMaui.Models;

namespace ShellWithPrismMaui.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            //BindingContext = new NewItemViewModel();
        }
    }
}