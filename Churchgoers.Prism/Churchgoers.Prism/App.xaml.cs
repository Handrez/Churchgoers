using Prism;
using Prism.Ioc;
using Churchgoers.Prism.ViewModels;
using Churchgoers.Prism.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using Churchgoers.Common.Services;
using Syncfusion.Licensing;
using Churchgoers.Prism.Helpers;
using Churchgoers.Common.Helpers;

namespace Churchgoers.Prism
{
    public partial class App
    {
        public App(IPlatformInitializer initializer)
            : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            SyncfusionLicenseProvider.RegisterLicense("MzMxOTM5QDMxMzgyZTMzMmUzMFBCQTZnUzJYM2Y2VHpuUi9hWFJNajdXYUUxbEZ4YWNYRzg1eXNXNWd1MU09");
            InitializeComponent();
            await NavigationService.NavigateAsync($"NavigationPage/{nameof(LoginPage)}");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<IRegexHelper, RegexHelper>();
            containerRegistry.Register<IFilesHelper, FilesHelper>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ChurchMasterDetailPage, ChurchMasterDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ModifyUserPage, ModifyUserPageViewModel>();
            containerRegistry.RegisterForNavigation<ShowMembersPage, ShowMembersPageViewModel>();
            containerRegistry.RegisterForNavigation<ShowMeetingsPage, ShowMeetingsPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>();
            containerRegistry.RegisterForNavigation<RecoverPasswordPage, RecoverPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<AddAssistancesPage, AddAssistancesPageViewModel>();
        }
    }
}
