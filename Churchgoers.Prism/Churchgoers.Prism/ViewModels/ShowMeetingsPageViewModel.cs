using Churchgoers.Common.Enums;
using Churchgoers.Common.Helpers;
using Churchgoers.Common.Responses;
using Churchgoers.Common.Services;
using Churchgoers.Prism.Helpers;
using Churchgoers.Prism.ItemViewModels;
using Churchgoers.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;

namespace Churchgoers.Prism.ViewModels
{
    public class ShowMeetingsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private string _search;
        private bool _isRunning;
        private TokenResponse _token;
        private ObservableCollection<MeetingItemViewModel> _meetings;
        private List<MeetingResponse> _myMeetings;
        private DelegateCommand _searchCommand;
        private DelegateCommand _addMeetingCommand;

        public ShowMeetingsPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            _token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            Title = Languages.ShowMeetings;
            LoadMeetingsAsync();
        }

        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowMeetings));

        public DelegateCommand AddMeetingCommand => _addMeetingCommand ?? (_addMeetingCommand = new DelegateCommand(AddMeetingCommandAsync));

        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowMeetings();
            }
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public ObservableCollection<MeetingItemViewModel> Meetings
        {
            get => _meetings;
            set => SetProperty(ref _meetings, value);
        }

        private async void LoadMeetingsAsync()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                return;
            }

            IsRunning = true;
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.GetList2Async<MeetingResponse>(url, "/api", "/Meetings", _token.Token);
            IsRunning = false;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            _myMeetings = (List<MeetingResponse>)response.Result;
            ShowMeetings();
        }

        private void ShowMeetings()
        {
            if (string.IsNullOrEmpty(Search))
            {

                Meetings = new ObservableCollection<MeetingItemViewModel>(_myMeetings.Select(m => new MeetingItemViewModel(_navigationService)
                {
                    Church = m.Church,
                    Date = m.Date,
                    Assistances = m.Assistances
                }).ToList());

            }
            else
            {
                Meetings = new ObservableCollection<MeetingItemViewModel>(_myMeetings.Select(m => new MeetingItemViewModel(_navigationService)
                {
                    Church = m.Church,
                    Date = m.Date,
                    Assistances = m.Assistances
                }).Where(p => p.Date.ToString().ToLower().Contains(Search.ToLower())).ToList());

            }
        }

        private async void AddMeetingCommandAsync()
        {
            if (_token.User.UserType == UserType.Teacher)
            {
                await _navigationService.NavigateAsync($"{nameof(AddMeetingsPage)}");
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ErrorRegisterMember, Languages.Accept);
            }
        }
    }
}
