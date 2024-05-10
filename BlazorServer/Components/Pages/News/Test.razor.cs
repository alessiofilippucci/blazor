using BlazorServer.Components.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace BlazorServer.Components.Pages
{
    public partial class Test : ComponentBase
    {
        public List<string> Actions { get; set; } = new List<string>();

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.AddMarkupContent(0, "<h1>Hello, world!</h1>\\r\\n\\r\\nWelcome to your new app.\\r\\n\\r\\n");
            builder.OpenComponent<Heading>(1);
            builder.AddAttribute(2, "ImportantLevel", 4);
            builder.AddAttribute(3, "Text", "CIAO");
            builder.CloseComponent();
        }
    }
}
