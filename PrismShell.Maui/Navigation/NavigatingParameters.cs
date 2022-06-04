using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

//New

namespace Prism.Navigation
{
    public class NavigatingParameters : INavigatingParameters
    {
        public INavigationParameters NavigationParamaters { get; set; }

        //New for canceling the navigation (used in OnNavigatedFrom)
        ////false - don't cancel;  true - cancel
        public Func<Task<bool>> NavigationDialog { get; set; }

        public string Source { get; set; }
    }
}
