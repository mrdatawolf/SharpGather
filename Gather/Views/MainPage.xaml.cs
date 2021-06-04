using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using HttpUtils;
using Windows.Storage;

namespace Gather.Views
{
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
        string apiBaseUrl = "https://phase1.datawolf.online/api";
        string email;
        string password;
        string accessToken;
        bool isLoggedIn = false;

        RestClient restClient = new RestClient();


        public MainPage()
        {
            InitializeComponent();
            GetAutomattedResources();
            loginButton.Background = new Windows.UI.Xaml.Media.SolidColorBrush(Windows.UI.Colors.CornflowerBlue);
            GatherLocalData();

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


        public void GatherLocalData()
        {
            Windows.Storage.ApplicationDataContainer container = localSettings.CreateContainer("GatherContainer", Windows.Storage.ApplicationDataCreateDisposition.Always);
            if (localSettings.Containers.ContainsKey("GatherContainer"))
            {
                if(localSettings.Containers["GatherContainer"].Values.ContainsKey("accessToken"))
                {
                    accessToken = localSettings.Containers["GatherContainer"].Values["accessToken"].ToString();
                    restClient.AccessToken = accessToken;
                    statusText.Text = "Authenticated!";
                    isLoggedIn = true;
                    loginButton.IsEnabled = false;
                    emailTextbox.IsEnabled = false;
                    passwordBox.IsEnabled = false;
                } else
                {
                    statusText.Text = "Not logged in";
                }
            }
        }
        public void DoLogin()
        {
            try
            {
                restClient.Login(apiBaseUrl: apiBaseUrl, email: email, password: password);
                localSettings.Containers["GatherContainer"].Values["accessToken"] = restClient.AccessToken;
                statusText.Text = "Logged In";
                isLoggedIn = true;
                loginButton.IsEnabled = false;
            }
            catch (Exception ex)
            {
                statusText.Text = "Login failed: " + ex.Message;
                isLoggedIn = false;
            }
        }

        public void GetAutomattedResources()
        {
            try
            {
                restClient.EndPoint = apiBaseUrl + "/automate_resources";
                restClient.Method = HttpVerb.GET;
                var result = restClient.MakeRequest();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Do get failed with " + ex.Message);
                Debug.WriteLine("PostData: " + restClient.PostData);
                Debug.WriteLine("EndPoint: " + restClient.EndPoint);
                Debug.WriteLine("ContentType: " + restClient.ContentType);
                Debug.WriteLine("AccessToken: " + restClient.AccessToken);
                Debug.WriteLine("Accept: " + restClient.Accept);
            }
        }

        private bool isPasswordValid()
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            return true;
        }

        private bool isEmailValid()
        {
            if (string.IsNullOrEmpty(email) || !email.ToLower().Contains("@"))
            {
                return false;
            }

            return true;
        }

        private void email_TextChanged(object sender, TextChangedEventArgs e)
        {
            email = emailTextbox.Text;
        }
        private void passwordBox_PasswordChanged(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            password = passwordBox.Password;
        }

        private void loginButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if(isLoggedIn)
            {
                statusText.Text = "Already logged in!";
            }
            else if (!isEmailValid())
            {
                statusText.Text = "Email issue found!";
            }
            else if (!isPasswordValid())
            {
                statusText.Text = "Password issue found!";
            } else
            {
                DoLogin();
            }
        }
    }
}
