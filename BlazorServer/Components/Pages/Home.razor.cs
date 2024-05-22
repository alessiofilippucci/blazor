using BlazorServer.Infrastructure;
using Microsoft.AspNetCore.Components;

namespace BlazorServer.Components.Pages
{
    public partial class Home : ComponentBase
    {
        [Inject]
        private CustomAuthenticationStateProvider AuthState { get; set; }

        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
    }
}
