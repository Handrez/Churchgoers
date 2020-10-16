using Churchgoers.Common.Enums;
using Churchgoers.Common.Helpers;
using Churchgoers.Common.Responses;
using Churchgoers.Common.Services;
using Churchgoers.Prism.Helpers;
using Churchgoers.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace Churchgoers.Prism.ViewModels
{
    public class ShowMembersPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private string _search;
        private List<UserResponse> _myMembers;
        private ObservableCollection<UserResponse> _members;
        private DelegateCommand _searchCommand;
        private DelegateCommand _addMemberCommand;

        public ShowMembersPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            Title = Languages.ShowMembers;
            LoadMembersAsync();
        }

        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowMembers));

        public DelegateCommand AddMemberCommand => _addMemberCommand ?? (_addMemberCommand = new DelegateCommand(AddMemberCommandAsync));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowMembers();
            }
        }

        public ObservableCollection<UserResponse> Members
        {
            get => _members;
            set => SetProperty(ref _members, value);
        }

        private async void LoadMembersAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            IsRunning = true;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetList2Async<UserResponse>(url, "/api", "/Account/Members", token.Token);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _myMembers = (List<UserResponse>)response.Result;
            ShowMembers();
        }

        private void ShowMembers()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Members = new ObservableCollection<UserResponse>(_myMembers);
            }
            else
            {
                Members = new ObservableCollection<UserResponse>(_myMembers
                    .Where(u => u.FirstName.ToString().ToLower().Contains(Search.ToLower())));
            }
        }

        private async void AddMemberCommandAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            IsRunning = true;
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            IsRunning = false;
            if ((token.User.UserType) == UserType.Member)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorRegisterMember, Languages.Accept);
                return;
            }
            else
            {
                await _navigationService.NavigateAsync($"{nameof(RegisterPage)}");
            }
        }
    }
}