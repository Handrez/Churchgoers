using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Churchgoers.Prism.ViewModels
{
    public class IndexPageViewModel : ViewModelBase
    {
        public IndexPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "INICIO CORRECTAMENTE";
        }
    }
}
