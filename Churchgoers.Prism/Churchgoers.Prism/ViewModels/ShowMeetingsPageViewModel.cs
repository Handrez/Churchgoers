using Churchgoers.Prism.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Churchgoers.Prism.ViewModels
{
    public class ShowMeetingsPageViewModel : ViewModelBase
    {
        public ShowMeetingsPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = Languages.ShowMeetings;
        }
    }
}
