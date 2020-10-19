using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace Churchgoers.Common.Helpers
{
    public static class Settings
    {
        private const string _token = "token";
        private const string _meeting = "meeting";
        private const string _meetings = "meetings";
        private const string _isLogin = "isLogin";
        private static readonly string _stringDefault = string.Empty;
        private static readonly bool _boolDefault = false;

        private static ISettings AppSettings => CrossSettings.Current;

        public static string Token
        {
            get => AppSettings.GetValueOrDefault(_token, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_token, value);
        }

        public static bool IsLogin
        {
            get => AppSettings.GetValueOrDefault(_isLogin, _boolDefault);
            set => AppSettings.AddOrUpdateValue(_isLogin, value);
        }

        public static string Meeting
        {
            get => AppSettings.GetValueOrDefault(_meeting, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_meeting, value);
        }
        public static string Meetings
        {
            get => AppSettings.GetValueOrDefault(_meetings, _stringDefault);
            set => AppSettings.AddOrUpdateValue(_meetings, value);
        }
    }
}
