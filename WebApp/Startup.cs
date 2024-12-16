using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Utilities;
using NLog.Extensions.Logging;
using WebApp.CustomAttribute;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;
using NLog;
using NLog.Web;

namespace WebApp
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            var logger = LogManager.Setup()
                       .LoadConfigurationFromAppSettings()
                       .GetCurrentClassLogger();
            var builder = new ConfigurationBuilder()
           .SetBasePath(env.ContentRootPath)
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
           .AddEnvironmentVariables();
           
            Configuration = builder.Build();
            BuildAppSettings(configuration);

        }
        private void BuildAppSettings(IConfiguration configuration)
        {
            
         
            ApplicationConfig.DefaultConnectionString = configuration.GetConnectionString("DefaultConnection");
            
            ApplicationConfig.AppVersion = configuration["AppVersion"];
           
            ApplicationConfig.Environment = configuration["Environment"];
            
            ApplicationConfig.SqlPassPhrase = configuration["SqlPassPhrase"];
            ApplicationConfig.APISecretKey = configuration["APISecretKey"];
           
            ApplicationConfig.ErrorEmailAddress = configuration["ErrorEmailAddress"];
         
            ApplicationConfig.BaseUrl = configuration["BaseUrl"];
            ApplicationConfig.TokenDomain = configuration["TokenDomain"];
            ApplicationConfig.SMTPConfig = new SMTPConfig
            {
                From = configuration.GetSection("Smtp:From").Value,
                //Password = configuration.GetSection("Smtp:Pass").Value,
                //Port = int.Parse(configuration.GetSection("Smtp:Port").Value),
                Server = configuration.GetSection("Smtp:Server").Value,
                //User = configuration.GetSection("Smtp:User").Value
            };
         
        }
        //public Startup(IConfiguration configuration)
        //{
        //    Configuration = configuration;
        //}

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true; //Stop automatic model validation, must use controller model.isvalid
            });
            services.AddHttpClient();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IPrincipal>(provider => provider.GetService<IHttpContextAccessor>().HttpContext.User);
            services.AddCors(options =>
            {
                options.AddPolicy("EnableCORS", builder =>
                {
                    builder.AllowAnyOrigin()
                       .AllowAnyHeader()
                       .AllowAnyMethod();
                });
            });
            services.AddAuthentication(opt => {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = "http://localhost:44365",
                    ValidAudience = "http://localhost:44365",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["APISecretKey"]))
                };
            });

            //services.AddControllers();
            services.AddControllersWithViews();

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart


            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseCors("EnableCORS");
            app.UseMiddleware<JwtMiddleware>();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
            
          
        }

    }
}
