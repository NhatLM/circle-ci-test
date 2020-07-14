using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IdentityService.Admin.EntityFramework.Shared.DbContexts;
using IdentityService.Admin.EntityFramework.Shared.Entities.Identity;
using IdentityService.Identity.Configuration;
using IdentityService.Identity.Configuration.Constants;
using IdentityService.Identity.Configuration.Interfaces;
using IdentityService.Identity.Helpers;
using IdentityService.API.Validator;
using IdentityService.API.Services;
using IdentityServer4.Validation;
using IdentityServer4.AspNetIdentity;
using IdentityServer4.Services;
using IdentityService.API.Repository.Interfaces;
using IdentityService.API.Repository;
using IdentityService.API.Model;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Identity
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var rootConfiguration = CreateRootConfiguration();
            services.AddSingleton(rootConfiguration);

            // Register DbContexts for IdentityServer and Identity
            RegisterDbContexts(services);

            // Add email senders which is currently setup for SendGrid and SMTP
            services.AddEmailSenders(Configuration);

            // Add services for authentication, including Identity model and external providers
            RegisterAuthentication(services);

            // Add all dependencies for Asp.Net Core Identity in MVC - these dependencies are injected into generic Controllers
            // Including settings for MVC and Localization
            // If you want to change primary keys or use another db model for Asp.Net Core Identity:
            services.AddMvcWithLocalization<UserIdentity, string>(Configuration);

            // Add authorization policies for MVC
            RegisterAuthorization(services);
            var connectionStr = System.Environment.GetEnvironmentVariable("AccountSQLConnection");
            services.AddIdSHealthChecks<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, AdminIdentityDbContext>(Configuration);
            services.AddDbContext<AccountsContext>(optionsAction=> optionsAction.UseMySql(connectionStr));
            services.AddScoped<IResourceOwnerPasswordValidator, AutoproffPasswordRequestValidator>();
            services.AddScoped<IProfileService, ProfileService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Add custom security headers
            app.UseSecurityHeaders();

            app.UseStaticFiles();
            UseAuthentication(app);
            app.UseMvcLocalizationServices();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoint =>
            {
                endpoint.MapDefaultControllerRoute();
                endpoint.MapHealthChecks("/health", new HealthCheckOptions
                {
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }

        public virtual void RegisterDbContexts(IServiceCollection services)
        {
            services.RegisterDbContexts<AdminIdentityDbContext, IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext>(Configuration);
        }

        public virtual void RegisterAuthentication(IServiceCollection services)
        {
            services.AddAuthenticationServices<AdminIdentityDbContext, UserIdentity, UserIdentityRole>(Configuration);
            services.AddIdentityServer<IdentityServerConfigurationDbContext, IdentityServerPersistedGrantDbContext, UserIdentity>(Configuration).AddCustomUserStore();
        }
    

        public virtual void RegisterAuthorization(IServiceCollection services)
        {
            var rootConfiguration = CreateRootConfiguration();
            services.AddAuthorizationPolicies(rootConfiguration);
        }

        public virtual void UseAuthentication(IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }

        protected IRootConfiguration CreateRootConfiguration()
        {
            var rootConfiguration = new RootConfiguration();
            Configuration.GetSection(ConfigurationConsts.AdminConfigurationKey).Bind(rootConfiguration.AdminConfiguration);
            Configuration.GetSection(ConfigurationConsts.RegisterConfigurationKey).Bind(rootConfiguration.RegisterConfiguration);
            return rootConfiguration;
        }
    }
}













