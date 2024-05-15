using BlazorServer.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorServer.Infrastructure
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private AuthenticationState authenticationState;

        public CustomAuthenticationStateProvider(AuthService authService)
        {
            authenticationState = new AuthenticationState(authService.CurrentUser);

            authService.UserChanged += (user) =>
            {
                authenticationState = new AuthenticationState(user);
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
            };
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return Task.FromResult(authenticationState);
        }
    }
}
