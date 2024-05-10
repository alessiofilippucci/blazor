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

        public async Task<List<News>> GetDataAsync(int? idBlocco = 7)
        {
            var request = new RestRequest("get-news-blocco-dettaglio", Method.Get);
            request.AddQueryParameter("id_blocco", idBlocco.GetValueOrDefault());
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

        public async Task<NewsDetail?> GetDataByIdAsync(string id)
        {
            var request = new RestRequest("dettaglionews", Method.Get);
            request.AddQueryParameter("id", id);
            var queryResult = await _client.ExecuteAsync<NewsDetail>(request);

            if (queryResult.IsSuccessStatusCode)
            {
                return queryResult.Data;
            }
            else
            {
                return null;
            }
        }

    }
}
