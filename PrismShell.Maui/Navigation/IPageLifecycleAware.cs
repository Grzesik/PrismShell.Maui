namespace Prism.Navigation
{
    public interface IPageLifecycleAware
    {
        void OnAppearing();
        void OnDisappearing();
    }
}
