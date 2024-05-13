using BlazorServer.Models;
using RestSharp;

namespace BlazorServer.Services
{
    public class AuthService
    {
        protected readonly RestClient _client;

        public AuthService()
        {
            _client = new RestClient("http://ws3.class.it/ce.utenti/Utenti.svc/");
        }

        public LoginResponse Login(string account, string password)
        {
            var request = new RestRequest("login-utente", Method.Post);
            request.AddBody($"account={account}&password={password}", ContentType.Plain);
            var queryResult = _client.Execute<LoginResponse>(request);

            if (queryResult.IsSuccessStatusCode)
            {
                return queryResult.Data;
            }
            else
            {
                return default;
            }
        }
    }
}
