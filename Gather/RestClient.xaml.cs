using System;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Text;

public enum HttpVerb
{
    GET,
    POST,
    PUT,
    DELETE
}

namespace HttpUtils
{
    public class RestClient
    {
        public string EndPoint { get; set; }
        public HttpVerb Method { get; set; }
        public string ContentType { get; set; }
        public string PostData { get; set; }

        public string AccessToken { get; set; }
        public string Accept { get; set; }

        public RestClient()
        {
            EndPoint = "";
            Method = HttpVerb.GET;
            ContentType = "application/json";
            PostData = "";
        }
        public RestClient(string endpoint)
        {
            EndPoint = endpoint;
            Method = HttpVerb.GET;
            ContentType = "application/json";
            PostData = "";
        }
        public RestClient(string endpoint, HttpVerb method)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "application/json";
            PostData = "";
        }

        public RestClient(string endpoint, HttpVerb method, string postData)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = "application/json";
            PostData = postData;
        }

        public RestClient(string endpoint, HttpVerb method, string postData, string contentType)
        {
            EndPoint = endpoint;
            Method = method;
            ContentType = contentType;
            PostData = postData;
        }


        public string MakeRequest()
        {
            return MakeRequest("");
        }

        public string Login(string apiBaseUrl, string email, string password)
        {
            string endpoint = apiBaseUrl + "/login";
            //Do the login
            dynamic dynamicJson = new ExpandoObject();
            dynamicJson.email = email.ToString();
            dynamicJson.password = password.ToString();
            EndPoint = endpoint;
            Method = HttpVerb.POST;
            ContentType = "application/json";
            Accept = "application/json";
            PostData = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicJson);
            var result = MakeRequest("");
            //Get the access token
            LoginObject thisObject = Newtonsoft.Json.JsonConvert.DeserializeObject<LoginObject>(result);
            AccessToken = "Bearer " + thisObject.access_token;

            return AccessToken;
        }

        public string MakeRequest(string parameters)
        {
            var request = (HttpWebRequest)WebRequest.Create(EndPoint + parameters);

            request.Method = Method.ToString();
            request.ContentLength = 0;
            request.ContentType = ContentType;
            request.Accept = Accept;

            if (string.IsNullOrEmpty(AccessToken) && Method != HttpVerb.POST)
            {
                return "No access token found!";
            }
            else if (!string.IsNullOrEmpty(PostData) && Method == HttpVerb.POST)
            {
                var encoding = new UTF8Encoding();
                var bytes = Encoding.GetEncoding("iso-8859-1").GetBytes(PostData);
                request.ContentLength = bytes.Length;

                using (var writeStream = request.GetRequestStream())
                {
                    writeStream.Write(bytes, 0, bytes.Length);
                }
            }
            else
            {
                request.Headers.Add("Authorization", AccessToken);
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            {
                var responseValue = string.Empty;

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    var message = String.Format("Request failed. Received HTTP {0}", response.StatusCode);
                    throw new ApplicationException(message);
                }

                // grab the response
                using (var responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                        using (var reader = new StreamReader(responseStream))
                        {
                            responseValue = reader.ReadToEnd();
                        }
                }

                return responseValue;
            }
        }

    } // class
}
