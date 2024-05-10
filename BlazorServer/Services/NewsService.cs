using BlazorServer.Models;
using RestSharp;

namespace BlazorServer.Services
{
    public class NewsService
    {
        protected readonly RestClient _client;

        public NewsService()
        {
            _client = new RestClient("https://ws3.class.it/CE.Content/Content.svc/");
        }

        public async Task<List<News>> GetDataAsync()
        {
            var request = new RestRequest("get-news-blocco-dettaglio", Method.Get);
            request.AddQueryParameter("id_blocco", 1);
            var queryResult = await _client.ExecuteAsync<List<News>>(request);

            if (queryResult.IsSuccessStatusCode)
            {
                return queryResult.Data ?? [];
            }
            else
            {
                return [];
            }
        }

        public async Task<News?> GetDataByIdAsync(string id)
        {
            var newsList = await GetDataAsync();
            return newsList.FirstOrDefault(x => x.IdNews == id);
        }
    }
}
