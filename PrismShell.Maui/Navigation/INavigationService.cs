namespace Prism.Navigation
{
    public interface INavigationService
    {
        //Shell navigation
        Task GoToAsync(ShellNavigationState state, INavigationParameters param = null);


        //for internal use or if the service is overloaded
        INavigationParameters GetParameter();
        void ClearParameter();
		void SetViewModel(Page page, string pagename = null);
	}
}
