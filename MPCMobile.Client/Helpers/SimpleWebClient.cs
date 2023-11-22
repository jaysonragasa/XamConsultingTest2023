using Newtonsoft.Json;

using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MPCMobile.Client.Helpers
{
    public class SimpleWebClient
    {
        string _host = "https://reqres.in/";
        //string _api = "users/api";

        HttpClient _httpClient = null;

        public CancellationTokenSource CancelationToken { get; set; } = new CancellationTokenSource();

        HttpClient ConnectClient()
        {
            if (_httpClient == null)
            {
                _httpClient = new HttpClient()
                {
                    MaxResponseContentBufferSize = 1000000,
                    Timeout = TimeSpan.FromSeconds(30)
                };
            }
            return _httpClient;
        }

        public async Task<T> PostAsync<T>(string urlPath, T payload)
        {
            var client = ConnectClient();

            try
            {
                string json = JsonConvert.SerializeObject(payload);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{_host}{urlPath}", content);

                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    var jsonResp = await response.Content.ReadAsStringAsync();

                    var des = JsonConvert.DeserializeObject<T>(jsonResp);

                    return (T)Convert.ChangeType(des, typeof(T));
                }
                else
                {
                    return default(T);
                }
            }
            catch (HttpRequestException httpException)
            {
                return default(T);
            }
            catch (Exception ex)
            {
                return default(T);
            }
            return default(T);
        }

        // https://johnthiriet.com/efficient-api-calls/#
        static T DeserializeJsonFromStream<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
                return default(T);

            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var js = new JsonSerializer();
                var searchResult = js.Deserialize<T>(jtr);
                return searchResult;
            }
        }
    }
}
