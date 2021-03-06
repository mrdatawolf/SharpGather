using Gather.Helpers;
using Gather.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Gather.Views
{
    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/UWP/pages/settings-codebehind.md
    public sealed partial class SettingsPage : Page, INotifyPropertyChanged
    {
        public string AccessToken;
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;
        private bool _isRunOfflineEnabled;
        public bool? IsRunOfflineEnabled
        {
            get { return _isRunOfflineEnabled; }
            set { Set(ref _isRunOfflineEnabled, (bool)value);  }
        }

        /*private async void CheckBoxChecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(IsRunOfflineEnabled), true);
        }*/

        /*private async void CheckBoxUnchecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(IsRunOfflineEnabled), false);
        }*/ 

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        public SettingsPage()
        {
            InitializeComponent();
            GatherLocalData();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            await InitializeAsync();
            IsRunOfflineEnabled = await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync<bool>(nameof(IsRunOfflineEnabled));
        }

        private async Task InitializeAsync()
        {
            VersionDescription = GetVersionDescription();
            await Task.CompletedTask;
        }

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }

        private async void ThemeChanged_CheckedAsync(object sender, RoutedEventArgs e)
        {
            var param = (sender as RadioButton)?.CommandParameter;

            if (param != null)
            {
                await ThemeSelectorService.SetThemeAsync((ElementTheme)param);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        private void GatherLocalData()
        {
            AccessToken = "No token found!";
            Windows.Storage.ApplicationDataContainer container = localSettings.CreateContainer("GatherContainer", Windows.Storage.ApplicationDataCreateDisposition.Always);
            if (localSettings.Containers.ContainsKey("GatherContainer"))
            {
                if (localSettings.Containers["GatherContainer"].Values.ContainsKey("accessToken"))
                {
                    AccessToken = localSettings.Containers["GatherContainer"].Values["accessToken"].ToString();
                }
            }
            tokenText.Text = AccessToken;
        }
    }
}
