using BlazorServer.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorServer.Components.Pages
{
    public partial class Home : ComponentBase
    {
        [Inject]
        private AuthService AuthService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (AuthService != null && AuthService.CurrentUser != null)
            {
            }
        }
    }
}
