using RadzenApp.Models;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace RadzenApp.Services
{
    public class NewsService
    {
        protected readonly ILogger<NewsService> _logger;

        protected readonly RestClient _client;

        public NewsService(
            ILogger<NewsService> logger)
        {
            _logger = logger;

            _client = new RestClient("https://ws3.class.it/CE.Content/Content.svc/", configureSerialization: s => s.UseNewtonsoftJson());
        }

        public async Task<List<News>> GetDataAsync(string idBlocco = "1")
        {
            var request = new RestRequest("get-news-blocco-dettaglio", Method.Get);
            request.AddQueryParameter("id_blocco", idBlocco);
            var queryResult = await _client.ExecuteAsync<List<News>>(request);

            if (queryResult.IsSuccessStatusCode)
            {
                return queryResult.Data ?? [];
            }
            return [];
        }

        public async Task<News?> GetDataByIdAsync(string id)
        {
            var newsList = await GetDataAsync();
            return newsList.FirstOrDefault(x => x.IdNews == id);
        }
    }
}
