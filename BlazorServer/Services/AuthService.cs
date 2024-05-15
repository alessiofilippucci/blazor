using BlazorServer.Models;
using RestSharp;
using System.Security.Claims;

namespace BlazorServer.Services
{
    public class AuthService
    {
        protected readonly RestClient _client;

        public event Action<ClaimsPrincipal>? UserChanged;

        private ClaimsPrincipal? currentUser;

        public ClaimsPrincipal? CurrentUser
        {
            get
            {
                return currentUser ?? new ClaimsPrincipal();
            }
            set
            {
                currentUser = value;
                UserChanged?.Invoke(currentUser);
            }
        }

        public AuthService()
        {
            _client = new RestClient("http://ws3.class.it/ce.utenti/Utenti.svc/");
        }

        public bool Login(string account, string password)
        {
            var request = new RestRequest("login-utente", Method.Post);
            request.AddBody($"account={account}&password={password}", ContentType.Plain);
            var queryResult = _client.Execute<LoginResponse>(request);

            if (queryResult.IsSuccessStatusCode && queryResult.Data != null)
            {
                if (queryResult.Data.Message?.ToLower() == "ok")
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, queryResult.Data.PersonalDataLogin.Userid),
                    };

                    var identity = new ClaimsIdentity(claims, "CustomAuthentication");
                    var user = new ClaimsPrincipal(identity);

                    CurrentUser = user;

                    return true;
                }
            }

            return false;
        }
    }
}
