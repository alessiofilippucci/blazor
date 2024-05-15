using BlazorServer.Models;
using RestSharp;

namespace BlazorServer.Services
{
    public class NewsService
    {
        protected readonly ILogger<NewsService> _logger;

        protected readonly CacheService _cacheService;
        protected readonly RestClient _client;
        protected readonly AuthService _authService;

        public NewsService(
            ILogger<NewsService> logger,
            CacheService cacheService,
            AuthService authService)
        {
            _logger = logger;

            _cacheService = cacheService;
            _authService = authService;

            _client = new RestClient("https://ws3.class.it/CE.Content/Content.svc/");
        }

        public List<News> GetData(string idBlocco = "1")
        {
            if (_authService.HasRole(Roles.Abbonato))
            {

            }

            //Log.Logger.Information("TEST");

            var cacheKey = $"all_news_{idBlocco}";

            List<News> data = _cacheService.GetOrSetCache(cacheKey, () =>
            {
                var request = new RestRequest("get-news-blocco-dettaglio", Method.Get);
                request.AddQueryParameter("id_blocco", idBlocco);
                var queryResult = _client.Execute<List<News>>(request);

                if (queryResult.IsSuccessStatusCode)
                {
                    return queryResult.Data ?? [];
                }
                else
                {
                    return [];
                }
            });

            return data;
        }

        public News? GetDataById(string id)
        {
            var newsList = GetData();
            return newsList.FirstOrDefault(x => x.IdNews == id);
        }
    }
}
