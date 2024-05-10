using Microsoft.AspNetCore.Components;

namespace BlazorServer.Components.Pages
{
    public partial class Home : ComponentBase
    {
        public List<string> Actions { get; set; } = new List<string>();

        public override Task SetParametersAsync(ParameterView parameters)
        {
            Actions.Add("SetParametersAsync");
            return base.SetParametersAsync(parameters);
        }

        protected override void OnInitialized()
        {
            Actions.Add("OnInitialized");
        }

        protected override async Task OnInitializedAsync()
        {
            Actions.Add("OnInitializedAsync");
            StateHasChanged();
        }

        protected override void OnParametersSet()
        {
            Actions.Add("OnParametersSet");
        }

        protected override async Task OnParametersSetAsync()
        {
            Actions.Add("OnParametersSetAsync");
        }

        protected override bool ShouldRender()
        {
            Actions.Add("ShouldRender");

            return base.ShouldRender();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            Actions.Add($"OnAfterRender: {firstRender}");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            Actions.Add($"OnAfterRenderAsync: {firstRender}");
        }

        protected void Test()
        { 
        }


    }
}
