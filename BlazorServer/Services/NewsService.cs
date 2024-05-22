using BlazorServer.Models;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;

namespace BlazorServer.Services
{
    public class NewsService
    {
        protected readonly ILogger<NewsService> _logger;

        protected readonly CacheService _cacheService;
        protected readonly RestClient _client;

        public NewsService(
            ILogger<NewsService> logger,
            CacheService cacheService)
        {
            _logger = logger;

            _cacheService = cacheService;

            _client = new RestClient("https://ws3.class.it/CE.Content/Content.svc/", configureSerialization: s => s.UseNewtonsoftJson());
        }

        public async Task<List<News>> GetDataAsync(string idBlocco = "1")
        {
            //Log.Logger.Information("TEST");

            var cacheKey = $"all_news_{idBlocco}";

            var (succeeded, value) = await _cacheService.GetAsync<List<News>>(cacheKey);

            if(!succeeded)
            {
                var request = new RestRequest("get-news-blocco-dettaglio", Method.Get);
                request.AddQueryParameter("id_blocco", idBlocco);
                var queryResult = _client.Execute<List<News>>(request);

                if (queryResult.IsSuccessStatusCode)
                {
                    value = queryResult.Data ?? [];
                }
                else
                {
                    value = [];
                }

                await _cacheService.SetAsync(cacheKey, value);
            }

            return value ?? [];
        }

        public async Task<News?> GetDataByIdAsync(string id)
        {
            var newsList = await GetDataAsync();
            return newsList.FirstOrDefault(x => x.IdNews == id);
        }
    }
}
