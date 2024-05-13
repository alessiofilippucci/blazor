using BlazorServer.Components;
using BlazorServer.Services;

namespace BlazorServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            builder.Services.AddScoped<NewsService>();
            builder.Services.AddScoped<CacheService>();
            builder.Services.AddScoped<AuthService>();
            
            builder.Services.AddMemoryCache();

            builder.Services.AddCascadingAuthenticationState();

            //builder.Services.AddAuthentication(o =>
            //{
            //    o.DefaultAuthenticateScheme = "test";
            //});

            builder.Services.AddOptions();
            builder.Services.AddAuthorizationCore();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
