using Churchgoers.Common.Helpers;
using Churchgoers.Common.Responses;
using Churchgoers.Prism.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;

namespace Churchgoers.Prism.ItemViewModels
{
    public class MeetingItemViewModel : MeetingResponse
    {
        private readonly INavigationService _navigationService;
        private DelegateCommand _selectMeetingCommand;

        public MeetingItemViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public DelegateCommand SelectMeetingCommand => _selectMeetingCommand ?? (_selectMeetingCommand = new DelegateCommand(SelectMeetingAsync));

        private async void SelectMeetingAsync()
        {
            NavigationParameters parameters = new NavigationParameters
            {
                { "meeting", this }
            };

            Settings.Meeting = JsonConvert.SerializeObject(this);
            await _navigationService.NavigateAsync(nameof(AssistancePage), parameters);
        }
    }
}
