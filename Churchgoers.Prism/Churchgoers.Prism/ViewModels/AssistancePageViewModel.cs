using Churchgoers.Common.Enums;
using Churchgoers.Common.Helpers;
using Churchgoers.Common.Requests;
using Churchgoers.Common.Responses;
using Churchgoers.Common.Services;
using Churchgoers.Prism.Helpers;
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
    public class AssistancePageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private bool _isRunning;
        private bool _isEnabled;
        private string _search;
        private MeetingResponse _meeting;
        private List<AssistanceResponse> _myAssistances;
        private ObservableCollection<AssistanceResponse> _assistances;
        private DelegateCommand _searchCommand;
        private DelegateCommand _saveCommand;
        private DateTime _date;

        public AssistancePageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _navigationService = navigationService;
            _apiService = apiService;
            IsEnabled = true;
        }

        public DelegateCommand SearchCommand => _searchCommand ?? (_searchCommand = new DelegateCommand(ShowAssistance));

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set => SetProperty(ref _isEnabled, value);
        }

        public string Search
        {
            get => _search;
            set
            {
                SetProperty(ref _search, value);
                ShowAssistance();
            }
        }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

        public MeetingResponse Meeting
        {
            get => _meeting;
            set => SetProperty(ref _meeting, value);
        }

        public ObservableCollection<AssistanceResponse> Assistances
        {
            get => _assistances;
            set => SetProperty(ref _assistances, value);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (parameters.ContainsKey("meeting"))
            {
                Meeting = parameters.GetValue<MeetingResponse>("meeting");
                Date = Meeting.Date;
                Title = Date.ToString();
                LoadAssistanceAsync(Meeting);
            }
        }

        private async void LoadAssistanceAsync(MeetingResponse meeting)
        {
            IsRunning = true;
            IsEnabled = false;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
            }

            _myAssistances = meeting.Assistances.ToList();

            IsRunning = false;
            IsEnabled = true;
            ShowAssistance();
        }

        private void ShowAssistance()
        {
            if (string.IsNullOrEmpty(Search))
            {
                Assistances = new ObservableCollection<AssistanceResponse>(_myAssistances);
            }
            else
            {
                Assistances = new ObservableCollection<AssistanceResponse>(_myAssistances.Select(a => new AssistanceResponse
                {
                    Id = a.Id,
                    Meeting = a.Meeting,
                    User = a.User,
                    IsPresent = a.IsPresent
                }).Where(a => a.User.FullName.ToLower().Contains(Search.ToLower())));
            }
        }

        private async void SaveAsync()
        {
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);
            if (token.User.UserType == UserType.Teacher)
            {
                IsRunning = true;
                IsEnabled = false;

                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    IsRunning = false;
                    IsEnabled = true;
                    await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.ConnectionError, Languages.Accept);
                    return;
                }

                MeetingRequest request = new MeetingRequest
                {
                    Assistances = Assistances.Select(a => new AssistanceResponse
                    {
                        Id = a.Id,
                        IsPresent = a.IsPresent,
                        User = new UserResponse
                        {
                            Document = a.User.Document,
                            FirstName = a.User.FirstName,
                            LastName = a.User.LastName,
                            Address = a.User.Address,
                            Email = a.User.Email,
                            Profession = a.User.Profession,
                            Church = a.User.Church,
                        }
                    }).ToList(),
                    ChurchId = Meeting.Church.Id,
                    Date = Meeting.Date

                };

                string url = App.Current.Resources["UrlAPI"].ToString();
                Response response = await _apiService.PutAsync(url, "/api", "/Meetings", request, token.Token);

                IsRunning = false;
                IsEnabled = true;

                if (!response.IsSuccess)
                {
                    if (response.Message == "Error006")
                    {
                        await App.Current.MainPage.DisplayAlert(Languages.Error, Languages.Error001, Languages.Accept);
                    }
                    return;
                }

                await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.AssistanceMessage, Languages.Accept);
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.ErrorRegisterMember, Languages.Accept);
            }
        }
    }
}
