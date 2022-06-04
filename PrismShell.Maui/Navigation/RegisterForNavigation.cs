using System;
using System.Collections.Generic;

namespace Prism.Navigation
{
    public static class RegisterForNavigation
    {
        private static Dictionary<string, PageNavigationInfo> _pageRegistrationCache = new Dictionary<string, PageNavigationInfo>();

        public static void Register<TView, TViewModel>() where TView : Page where TViewModel : class
        {
            var info = new PageNavigationInfo
            {
                ViewType = typeof(TView),
                ViewModelType = typeof(TViewModel)
            };

            if (!_pageRegistrationCache.ContainsKey(typeof(TView).Name))
                _pageRegistrationCache.Add(typeof(TView).Name, info);
        }

        public static void Register<TView>() where TView : Page
        {
            var info = new PageNavigationInfo
            {
                ViewType = typeof(TView)
            };

            if (!_pageRegistrationCache.ContainsKey(typeof(TView).Name))
                _pageRegistrationCache.Add(typeof(TView).Name, info);
        }

        public static PageNavigationInfo GetPageNavigationInfo(Type pageType)
        {
            foreach (var item in _pageRegistrationCache)
            {
                if (item.Value.ViewType == pageType)
                    return item.Value;
            }

            return null;
        }

        public static void ClearRegistrationCache()
        {
            _pageRegistrationCache.Clear();
        }
    }
}
