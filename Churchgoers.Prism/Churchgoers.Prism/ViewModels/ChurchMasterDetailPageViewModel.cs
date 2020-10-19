using Churchgoers.Common.Helpers;
using Churchgoers.Common.Models;
using Churchgoers.Common.Responses;
using Churchgoers.Prism.Helpers;
using Churchgoers.Prism.ItemViewModels;
using Churchgoers.Prism.Views;
using Newtonsoft.Json;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Churchgoers.Prism.ViewModels
{
    public class ChurchMasterDetailPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private UserResponse _user;
        private static ChurchMasterDetailPageViewModel _instance;

        public ChurchMasterDetailPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _instance = this;
            _navigationService = navigationService;
            LoadMenus();
            LoadUser();
        }

        public ObservableCollection<MenuItemViewModel> Menus { get; set; }

        public UserResponse User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public static ChurchMasterDetailPageViewModel GetInstance()
        {
            return _instance;
        }

        public void LoadUser()
        {
            if (Settings.IsLogin)
            {
                TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
                User = token.User;
            }
        }

        private void LoadMenus()
        {
            List<Menu> menus = new List<Menu>
            {
                new Menu
                {
                    Icon = "ic_people",
                    PageName = $"{nameof(ShowMembersPage)}",
                    Title = Languages.ShowMembers,
                    IsLoginRequired = true
                },
                new Menu
                {
                    Icon = "ic_list_alt",
                    PageName = $"{nameof(ShowMeetingsPage)}",
                    Title = Languages.ShowMeetings,
                    IsLoginRequired = true
                },
                new Menu
                {
                    Icon = "ic_person",
                    PageName = $"{nameof(ModifyUserPage)}",
                    Title = Languages.ModifyUser,
                    IsLoginRequired = true
                },
                new Menu
                {
                    Icon = "ic_exit_to_app",
                    PageName = $"{nameof(LoginPage)}",
                    Title = Settings.IsLogin ? Languages.Logout : Languages.Login
                }
            };

            Menus = new ObservableCollection<MenuItemViewModel>(
                menus.Select(m => new MenuItemViewModel(_navigationService)
                {
                    Icon = m.Icon,
                    PageName = m.PageName,
                    Title = m.Title,
                    IsLoginRequired = m.IsLoginRequired
                }).ToList());
        }
    }
}