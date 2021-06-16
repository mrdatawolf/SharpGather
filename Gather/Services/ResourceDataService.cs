using Gather.Models;
using HttpUtils;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Gather.Services
{
    public static class ResourceDataService
    {
        public static List<string> _allResources;

        public static async Task<List<string>> GetContentGridDataAsync()
        {
            string ApiBaseUrl = "https://phase1.datawolf.online/api";
            string AccessToken = GatherAccessToken();
            RestClient restClient = new RestClient(ApiBaseUrl + "/initial_gather");
            restClient.AccessToken = AccessToken;
            string jsonReturn = restClient.MakeRequest();
            Debug.WriteLine(jsonReturn);
            if (_allResources == null)
            {
                _allResources = Newtonsoft.Json.JsonConvert.DeserializeObject<List<string>>(jsonReturn);
               //_allResources = Newtonsoft.Json.JsonConvert.DeserializeObject<Resources>(jsonReturn);
            }
            
            await Task.CompletedTask;
            return _allResources;
        }

        public static string GatherAccessToken()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            return localSettings.Containers["GatherContainer"].Values["accessToken"].ToString();
        }
    }

}
