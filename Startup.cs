using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vue2Spa.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Okta.Sdk;
using Okta.Sdk.Configuration;

namespace Vue2Spa
{
    public class Startup
    {
        // public Startup(IConfiguration configuration)
        // {
        //     if (env.IsDevelopment())
        //     {
        //         builder.AddUserSecrets<Startup>();
        //     }
        //     Configuration = configuration;
        // }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Simple example with dependency injection for a data provider.
            services.AddSingleton<IBlogService, OktaBlogService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.Authority = "https://dev-255380.oktapreview.com/oauth2/default";
                options.Audience = "api://default";
            });
            services.AddSingleton<IOktaClient>(new OktaClient(new OktaClientConfiguration
            {
                OrgUrl = "https://dev-255380.oktapreview.com",
                // 00JCbg0wfLC7sU1vaVUN7-jGDJPU61Z1wK0Q-DQqk1
                Token = Configuration["okta:token"]
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                // Webpack initialization with hot-reload.
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true,
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
