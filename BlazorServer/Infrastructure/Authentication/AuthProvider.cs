using BlazorServer.Config;
using BlazorServer.Models;
using BlazorServer.Services;
using Microsoft.AspNetCore.Components.Authorization;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using System.Security.Claims;

namespace BlazorServer.Infrastructure
{
    public class AuthProvider : AuthenticationStateProvider
    {
        private readonly RedirectRules _redirectRules;

        private readonly CacheService _cacheService;

        protected readonly RestClient _client;

        public ClaimsPrincipal CurrentUserIdentity { get; private set; }
        public User? CurrentUser { get; private set; }

        public bool IsLoggedIn => CurrentUserIdentity?.Identity?.IsAuthenticated ?? false;
        public string? DisplayName => IsLoggedIn ? CurrentUserIdentity.Identity?.Name : "";
        public bool HasRole(string role) => IsLoggedIn && CurrentUserIdentity.IsInRole(role);

        public AuthProvider(CacheService cacheService, RedirectRules redirectRules)
        {
            _redirectRules = redirectRules;
            _cacheService = cacheService;

            _client = new RestClient("http://ws3.class.it/ce.utenti/Utenti.svc/", configureSerialization: s => s.UseNewtonsoftJson());

            CurrentUserIdentity = new ClaimsPrincipal();
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var anonymousUser = new ClaimsPrincipal(new ClaimsIdentity());
            return await Task.FromResult(new AuthenticationState(anonymousUser));
        }

        public async Task<bool> CheckTokenAsync(string token)
        {
            var (succeeded, value) = await _cacheService.GetAsync<User>(token);

            if (succeeded)
            {
                CurrentUser = value;
                Refresh();
            }

            return succeeded;
        }

        public async Task<(bool logged, string token)> LoginAsync(string account, string password)
        {
            var logged = false;
            var token = "";

            try
            {
                var loginRequest = new RestRequest("login-utente", Method.Post);
                loginRequest.AddBody($"account={account}&password={password}", ContentType.Plain);
                var loginResult = await _client.ExecuteAsync<LoginResponse>(loginRequest);

                if (loginResult.IsSuccessStatusCode &&
                    loginResult.Data != null &&
                    loginResult.Data.Message?.ToLower() == "ok")
                {
                    var personalDataLogin = loginResult.Data.PersonalDataLogin;

                    if (personalDataLogin != null && await GetUserInfoAsync(personalDataLogin.Userid))
                    {
                        token = personalDataLogin.Userid;
                        await _cacheService.SetAsync(token, CurrentUser);
                        logged = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }

            return (logged, token);
        }

        private async Task<bool> GetUserInfoAsync(string? userId)
        {
            if (!string.IsNullOrEmpty(userId))
            {
                try
                {
                    var userStatusRequest = new RestRequest("status-utente", Method.Get);
                    userStatusRequest.AddQueryParameter("userid", userId);
                    var userStatusResult = await _client.ExecuteAsync<User>(userStatusRequest);

                    if (userStatusResult.IsSuccessStatusCode && userStatusResult.Data != null)
                    {
                        CurrentUser = userStatusResult.Data;
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                }
            }
            return false;
        }

        public async Task RefreshUserInfo(string? token)
        {
            if (!string.IsNullOrEmpty(token) && await GetUserInfoAsync(token))
            {
                await _cacheService.SetAsync(token, CurrentUser);
                Refresh(new Claim(ClaimTypes.Role, Roles.Private));
            }
        }

        public void Refresh(params Claim[] customClaims)
        {
            CurrentUserIdentity = SetupClaimsForUser(CurrentUser, customClaims);
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(CurrentUserIdentity)));
        }

        private ClaimsPrincipal SetupClaimsForUser(User? user, params Claim[] customClaims)
        {
            var claims = new List<Claim>();

            if (user != null)
            {
                var personalData = user.PersonalData;

                claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, $"{personalData.Name} {personalData.Surname}"),
                    new Claim(ClaimTypes.GivenName, personalData.Name),
                    new Claim(ClaimTypes.Surname, personalData.Surname),
                    new Claim(ClaimTypes.Email, personalData.Email),
                    new Claim(Costants.ClaimIdentifiers.Id, personalData.Userid),
                };

                var subscriptions = user.Subscriptions.Subscription;

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

                if (personalData.FlagPrivateInvestor?.Value == "S")
                {
                    roleClaims.Add(new Claim(ClaimTypes.Role, Roles.Private));
                }

                claims.AddRange(roleClaims);
                claims.AddRange(customClaims);
            }

            var identity = new ClaimsIdentity(claims, "CustomAuthentication");
            var userPrincipal = new ClaimsPrincipal(identity);
            return userPrincipal;
        }
    }
}
