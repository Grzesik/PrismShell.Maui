namespace Prism.Navigation
{
    public interface INavigatedAware
    {
        /// <summary>
        /// Called when the implementer has been navigated away from.
        /// </summary>
        /// <param name="parameters">The navigation parameters.</param>
        //void OnNavigatedFrom(INavigatingParameters parameters);
        void OnNavigatedFrom(INavigatingParameters param);

        /// <summary>
        /// Called when the implementer has been navigated to.
        /// </summary>
        /// <param name="parameters">The navigation parameters.</param>
        //void OnNavigatedTo(INavigationParameters parameters);
        void OnNavigatedTo(INavigationParameters param);
    }
}
