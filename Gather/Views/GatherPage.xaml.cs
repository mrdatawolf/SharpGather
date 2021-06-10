using Gather.Models;
using HttpUtils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Gather.Views
{
    public sealed partial class GatherPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<Resource> Source { get; } = new ObservableCollection<Resource>();
        public static Resources _allResources;

        public GatherPage()
        {
            InitializeComponent();
        }
        
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Source.Clear();

            string ApiBaseUrl = "https://phase1.datawolf.online/api";
            string v = GatherAccessToken();
            string AccessToken = v;
            RestClient restClient = new RestClient();
            restClient.AccessToken = AccessToken;
            for (int i = 1; i < 13; i++)
            {
                restClient.EndPoint = ApiBaseUrl + "/initial_gather/" + i;   
                string jsonReturn = restClient.MakeRequest();
                Resource testc = Newtonsoft.Json.JsonConvert.DeserializeObject<Resource>(jsonReturn);
                Source.Add(testc);
            }
        }
        public static string GatherAccessToken()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            return localSettings.Containers["GatherContainer"].Values["accessToken"].ToString();
        }
        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            //do something
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
    }
}
