using Churchgoers.Common.Helpers;
using Churchgoers.Common.Models;
using Churchgoers.Prism.Helpers;
using Churchgoers.Prism.Views;
using Prism.Commands;
using Prism.Navigation;

namespace Churchgoers.Prism.ItemViewModels
{
    public class MenuItemViewModel : Menu
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectMenuCommand;

        public MenuItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectMenuCommand => _selectMenuCommand ?? (_selectMenuCommand = new DelegateCommand(SelectMenuAsync));

        private async void SelectMenuAsync()
        {
            if (PageName == nameof(LoginPage) && Settings.IsLogin)
            {
                Settings.IsLogin = false;
                Settings.Token = null;

                await _navigationService.NavigateAsync($"/NavigationPage/{PageName}");
                return;
            }

            if (IsLoginRequired && !Settings.IsLogin)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.LoginFirstMessage, Languages.Accept);
                NavigationParameters parameters = new NavigationParameters
                {
                    { "pageReturn", PageName }
                };

                await _navigationService.NavigateAsync($"/NavigationPage/{nameof(LoginPage)}", parameters);
            }
            else
            {
                await _navigationService.NavigateAsync($"/{nameof(ChurchMasterDetailPage)}/NavigationPage/{PageName}");
            }
        }
    }
}
