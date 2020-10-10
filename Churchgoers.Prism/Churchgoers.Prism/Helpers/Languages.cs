using Churchgoers.Common.Helpers;
using Churchgoers.Prism.Resources;
using System.Globalization;
using Xamarin.Forms;

namespace Churchgoers.Prism.Helpers
{
    public static class Languages
    {
        static Languages()
        {
            CultureInfo ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            Culture = ci.Name;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        public static string Culture { get; set; }

        public static string Accept => Resource.Accept;

        public static string ConnectionError => Resource.ConnectionError;

        public static string Error => Resource.Error;

        public static string Login => Resource.Login;

        public static string ModifyUser => Resource.ModifyUser;

        public static string ShowMembers => Resource.ShowMembers;

        public static string ShowMeetings => Resource.ShowMeetings;

        public static string Email => Resource.Email;

        public static string EmailError => Resource.EmailError;

        public static string EmailPlaceHolder => Resource.EmailPlaceHolder;

        public static string Password => Resource.Password;

        public static string PasswordError => Resource.PasswordError;

        public static string PasswordPlaceHolder => Resource.PasswordPlaceHolder;

        public static string ForgotPassword => Resource.ForgotPassword;

        public static string Register => Resource.Register;

        public static string LoginError => Resource.LoginError;

        public static string Logout => Resource.Logout;

        public static string Loading => Resource.Loading;

        public static string SearchMember => Resource.SearchMember;

        public static string LoginFirstMessage => Resource.LoginFirstMessage;

        public static string Document => Resource.Document;

        public static string DocumentError => Resource.DocumentError;

        public static string DocumentPlaceHolder => Resource.DocumentPlaceHolder;

        public static string FirstName => Resource.FirstName;

        public static string FirstNameError => Resource.FirstNameError;

        public static string FirstNamePlaceHolder => Resource.FirstNamePlaceHolder;

        public static string LastName => Resource.LastName;

        public static string LastNameError => Resource.LastNameError;

        public static string LastNamePlaceHolder => Resource.LastNamePlaceHolder;

        public static string Address => Resource.Address;

        public static string AddressError => Resource.AddressError;

        public static string AddressPlaceHolder => Resource.AddressPlaceHolder;

        public static string Phone => Resource.Phone;

        public static string PhoneError => Resource.PhoneError;

        public static string PhonePlaceHolder => Resource.PhonePlaceHolder;

        public static string Profession => Resource.Profession;

        public static string ProfessionError => Resource.ProfessionError;

        public static string ProfessionPlaceHolder => Resource.ProfessionPlaceHolder;

        public static string Church => Resource.Church;

        public static string ChurchError => Resource.ChurchError;

        public static string ChurchPlaceHolder => Resource.ChurchPlaceHolder;

        public static string District => Resource.District;

        public static string DistrictError => Resource.DistrictError;

        public static string DistrictPlaceHolder => Resource.DistrictPlaceHolder;

        public static string Field => Resource.Field;

        public static string FieldError => Resource.FieldError;

        public static string FieldPlaceHolder => Resource.FieldPlaceHolder;

        public static string PasswordConfirm => Resource.PasswordConfirm;

        public static string PasswordConfirmError1 => Resource.PasswordConfirmError1;

        public static string PasswordConfirmError2 => Resource.PasswordConfirmError2;

        public static string PasswordConfirmPlaceHolder => Resource.PasswordConfirmPlaceHolder;

        public static string Error001 => Resource.Error001;

        public static string Error003 => Resource.Error003;

        public static string Error004 => Resource.Error004;

        public static string Error006 => Resource.Error006;

        public static string Ok => Resource.Ok;

        public static string RegisterMessge => Resource.RegisterMessge;

        public static string PictureSource => Resource.PictureSource;

        public static string Cancel => Resource.Cancel;

        public static string FromCamera => Resource.FromCamera;

        public static string FromGallery => Resource.FromGallery;

        public static string NoCameraSupported => Resource.NoCameraSupported;

        public static string NoGallerySupported => Resource.NoGallerySupported;

        public static string RecoverPassword => Resource.RecoverPassword;

        public static string RecoverPasswordMessage => Resource.RecoverPasswordMessage;

        public static string ChangePassword => Resource.ChangePassword;

        public static string ChangeUserMessage => Resource.ChangeUserMessage;
    }
}
