using Blazored.LocalStorage;
using Blazored.SessionStorage;
using BlazorServer.Components;
using BlazorServer.Config;
using BlazorServer.Infrastructure;
using BlazorServer.Infrastructure.binder;
using BlazorServer.Installers.ServiceBuilder;
using BlazorServer.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Serilog;
using System.Security.Claims;

namespace BlazorServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var loggerConfig = LoggingServiceBuilder.GetConfig();

            Log.Logger = loggerConfig.CreateLogger();

            try
            {
                Log.Logger.Information("APPLICATION STARTED");

                var builder = WebApplication.CreateBuilder(args);

                // Recupero config da appsettings e registrazione DI
                var redirectRules = SettingsSection.Configure(builder.Services, builder.Configuration, new RedirectRules());

                // Solo recupero config da appsettings
                //var redirectRules = SettingsSection.Get<RedirectRules>(builder.Configuration);

                // Add services to the container.
                builder.Services.AddRazorComponents()
                    .AddInteractiveServerComponents();

                builder.Services.AddScoped<NewsService>();
                builder.Services.AddScoped<CacheService>();
                builder.Services.AddScoped<UserService>();

                builder.Services.AddScoped<ICookie, Cookie>();

                builder.Services.AddScoped<AuthProvider>();
                builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<AuthProvider>());

                builder.Services.AddMemoryCache();

                builder.Services.AddBlazoredSessionStorage();
                builder.Services.AddBlazoredLocalStorage();

                builder.Services.AddCascadingAuthenticationState();

                builder.Services.AddHttpContextAccessor();

                builder.Services.AddOptions();
                builder.Services.AddAuthorizationCore(options =>
                {
                    options.AddPolicy("Test", policy =>
                    {
                        policy.RequireRole(Roles.Abbonato, Roles.MarketReport);
                        policy.RequireClaim(ClaimTypes.Country, "IT");
                    });
                });

                builder.Services.ConfigureLogger();

                var app = builder.Build();

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                var optionRules = new RewriteOptions()
                    .AddRedirect("pippo", "https://google.com");

                if (redirectRules != null)
                {
                    //if (redirectRules.Rule1 != null)
                    //{
                    //    optionRules.AddRewrite(redirectRules.Rule1.Regex, redirectRules.Rule1.Replacement, redirectRules.Rule1.SkipRemainingRules);
                    //}
                    //if (redirectRules.Rule2 != null)
                    //{
                    //    optionRules.AddRewrite(redirectRules.Rule2.Regex, redirectRules.Rule2.Replacement, redirectRules.Rule2.SkipRemainingRules);
                    //}
                    //if (redirectRules.Rule3 != null)
                    //{
                    //    optionRules.AddRewrite(redirectRules.Rule3.Regex, redirectRules.Rule3.Replacement, redirectRules.Rule3.SkipRemainingRules);
                    //}
                }

                app.UseRewriter(optionRules);

                app.UseHttpsRedirection();

                app.UseStaticFiles();
                app.UseAntiforgery();

                app.MapRazorComponents<App>()
                    .AddInteractiveServerRenderMode();

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Application stopped");
            }
            finally
            {
                (Log.Logger as Serilog.Core.Logger)?.Dispose();
            }
        }
    }
}
