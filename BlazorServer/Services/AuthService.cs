using BlazorServer.Models;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System.Security.Claims;

namespace BlazorServer.Services
{
    public class AuthService
    {
        protected readonly RestClient _client;

        public event Action<ClaimsPrincipal>? UserChanged;

        private ClaimsPrincipal? currentUser;

        public bool IsLoggedIn => currentUser != null && currentUser.Identity != null && currentUser.Identity.IsAuthenticated ;

        public string DisplayName => IsLoggedIn ? currentUser.Identity.Name : "";

        public bool HasRole(string role) => IsLoggedIn && currentUser.IsInRole(role);

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
            _client = new RestClient("http://ws3.class.it/ce.utenti/Utenti.svc/", configureSerialization: s => s.UseNewtonsoftJson());

        }

        public bool Login(string account, string password)
        {
            var loginRequest = new RestRequest("login-utente", Method.Post);
            loginRequest.AddBody($"account={account}&password={password}", ContentType.Plain);
            var loginResult = _client.Execute<LoginResponse>(loginRequest);

            if (loginResult.IsSuccessStatusCode && loginResult.Data != null)
            {
                if (loginResult.Data.Message?.ToLower() == "ok")
                {
                    var personalDataLogin = loginResult.Data.PersonalDataLogin;

                    if (personalDataLogin != null)
                    {
                        var userStatusRequest = new RestRequest("status-utente", Method.Get);
                        userStatusRequest.AddQueryParameter("userid", personalDataLogin.Userid);
                        var userStatusResult = _client.Execute<UserStatus>(userStatusRequest);

                        if (userStatusResult.IsSuccessStatusCode && userStatusResult.Data != null)
                        {
                            var user = userStatusResult.Data.PersonalData;

                            var claims = new List<Claim>()
                            {
                                new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}"),
                                new Claim(ClaimTypes.GivenName, user.Name),
                                new Claim(ClaimTypes.Surname, user.Surname),
                                new Claim(ClaimTypes.Email, user.Email),
                                new Claim(Costants.ClaimIdentifiers.Id, user.Userid),
                            };

                            var subscriptions = userStatusResult.Data.Subscriptions.Subscription;

                            var roles = new List<string>()
                            {
                                Roles.User
                            };

                            foreach (var subscription in subscriptions)
                            {
                                roles.AddRange(Roles.GetRoleByCode(subscription.Code));
                            }

                            var roleClaims = new List<Claim>();

                            foreach (var role in roles.Distinct())
                            {
                                roleClaims.Add(new Claim(ClaimTypes.Role, role));
                            }

                            if (user.FlagPrivateInvestor?.Value == "S")
                            {
                                roleClaims.Add(new Claim(ClaimTypes.Role, Roles.Private));
                            }

                            claims.AddRange(roleClaims);

                            //roleClaims = subscriptions.SelectMany(x => Roles.GetRoleByCode(x.Code).Select(y => new Claim(ClaimTypes.Role, y))).ToList();

                            var identity = new ClaimsIdentity(claims, "CustomAuthentication");
                            var userPrincipal = new ClaimsPrincipal(identity);

                            CurrentUser = userPrincipal;

                            return true;
                        }
                    }
                }
            }

            return false;
        }
    }
}
