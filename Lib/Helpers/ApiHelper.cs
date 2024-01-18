
using System.Net.Http.Headers;

namespace anotherGO.Helpers
{
    public static class ApiHelper
    {
        public static HttpClient ApiClient { get; set; }

        public static void InitializeClient()
        {
            ApiClient = new HttpClient();
            //ApiClient.BaseAddress = new Uri(""); da moze da se koristi za pristup drugim apijevim adresama
            //ApiClient.DefaultRequestHeaders.Accept.Clear();
            //ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            ApiClient.DefaultRequestHeaders.Accept.Clear();
            //ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("https://rc-vault-fap-live-1.azurewebsites.net/api/gettimeentries?code=vO17RnE8vuzXzPJo5eaLLjXjmRW07law99QTD90zat9FfOQJKKUcgQ=="));
        }
    }
}
