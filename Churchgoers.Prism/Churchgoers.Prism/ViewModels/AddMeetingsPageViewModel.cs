using Churchgoers.Common.Helpers;
using Churchgoers.Common.Responses;
using Churchgoers.Common.Services;
using Churchgoers.Prism.Helpers;
using Churchgoers.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using System;
using Xamarin.Essentials;

namespace Churchgoers.Prism.ViewModels
{
    public class AddMeetingsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IApiService _apiService;
        private DateTime _date;
        private bool _isRunning;
        private bool _isEnabled;
        private DelegateCommand _saveCommand;


        public AddMeetingsPageViewModel(INavigationService navigationService, IApiService apiService) : base(navigationService)
        {
            _apiService = apiService;
            IsEnabled = true;
            Title = Languages.NewMeeting;
        }

        public DelegateCommand SaveCommand => _saveCommand ?? (_saveCommand = new DelegateCommand(SaveAsync));

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }

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

        private async void SaveAsync()
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
            TokenResponse token = JsonConvert.DeserializeObject<TokenResponse>(Settings.Token);

            string date = Date.Month + "-" + Date.Day + "-" + Date.Year + " " + DateTime.Now.ToString("HH:mm:ss");
            string url = App.Current.Resources["UrlAPI"].ToString();
            Response response = await _apiService.PostAsync(url, "api", "/Meetings/" + date, Date, token.Token);
            IsRunning = false;
            IsEnabled = true;

            if (!response.IsSuccess)
            {
                await App.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            await App.Current.MainPage.DisplayAlert(Languages.Ok, Languages.MeetingMessage, Languages.Accept);
        }
    }
}
