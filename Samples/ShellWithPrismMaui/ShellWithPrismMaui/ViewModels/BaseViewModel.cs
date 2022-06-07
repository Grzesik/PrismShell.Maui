using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ShellWithPrismMaui.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged, IPageLifecycleAware, INavigatedAware, IPageAware
    {
        protected string pagename;

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        #region --[Interfaces]--

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }

        /// <summary>
        /// Called, when the page is going to navigate to another page or back. The navigation can be canceled
        /// In the param are parameters from the actual view model. The parameters are transfered to the next view model
        /// </summary>
        /// <param name="param"></param>
        public virtual void OnNavigatedFrom(INavigatingParameters param)
        {
            Console.WriteLine($"OnNavigatedFrom for page {pagename}");
        }

        /// <summary>
        /// Called, when the page is the current page (it was navigated to the page). 
        /// In the param are parameters from the calling view model
        /// </summary>
        /// <param name="param"></param>
        public virtual void OnNavigatedTo(INavigatedParameters param)
        {
            Console.WriteLine($"OnNavigatedTo for page {pagename}");
        }

        public virtual void SetPageName(string name)
        {
            pagename = name;
        }
        #endregion
    }
}
