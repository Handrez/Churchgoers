using Churchgoers.Prism.Helpers;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Churchgoers.Prism.ViewModels
{
    public class ShowMembersPageViewModel : ViewModelBase
    {
        public ShowMembersPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = Languages.ShowMembers;
        }
    }
}
