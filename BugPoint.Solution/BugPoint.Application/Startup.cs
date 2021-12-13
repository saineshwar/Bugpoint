using BugPoint.Common;
using BugPoint.Data.EFContext;
using BugPoint.Extensions;
using DNTCaptcha.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using Amazon;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Runtime;
using Amazon.S3;
using BugPoint.Application.Filters;
using BugPoint.Application.Notification;
using BugPoint.Services.AwsHelper;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;


namespace BugPoint.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServicesCommand(Configuration);
            services.AddServicesQueries(Configuration);
            services.AddScoped<AuditFilterAttribute>();
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddElmah();
            var connection = Configuration.GetConnectionString("DatabaseConnection");
            services.AddDbContext<BugPointContext>(options => options.UseSqlServer(connection));
            services.Configure<AppSettingsProperties>(Configuration.GetSection("ApplicationSettings"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<INotificationService, NotificationService>();

            #region Elmah

            //services.AddElmah<SqlErrorLog>(options =>
            //{
            //    options.ConnectionString = Configuration.GetValue<string>("ConnectionStrings:DatabaseConnection");
            //});
            //services.AddElmah(options => options.Path = "/elmahxbug"); 

            #endregion

            #region AWS S3 Settings
            services.Configure<AwsSettings>(Configuration.GetSection("AwsSettings"));
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>(new AWSOptions
            {
                Credentials = new BasicAWSCredentials(Configuration.GetValue<string>("AwsSettings:AccessKey"), Configuration.GetValue<string>("AwsSettings:SecretKey")),
                Region = RegionEndpoint.APSouth1
            });
            services.AddSingleton<IAwsS3HelperService, AwsS3HelperService>();
            services.Configure<AwsS3BucketOptions>(Configuration.GetSection(nameof(AwsS3BucketOptions)))
                .AddSingleton(x => x.GetRequiredService<IOptions<AwsS3BucketOptions>>().Value);
            #endregion

            // For FileUpload
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
                x.ValueLengthLimit = int.MaxValue; //not recommended value
                x.MemoryBufferThreshold = Int32.MaxValue;
            });

            // For Setting Session Timeout
            services.AddSession(options =>
            {
                options.Cookie.Name = ".BugPoint.Session";
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                // Make the session cookie essential
                options.Cookie.IsEssential = true;
                //options.Cookie.SameSite = SameSiteMode.None;

            });


            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.ContractResolver =
                        new Newtonsoft.Json.Serialization.DefaultContractResolver();
                })
                .AddSessionStateTempDataProvider();

            //  Cookie
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always;
                options.Secure = CookieSecurePolicy.Always;
                options.CheckConsentNeeded = context => true; // consent required
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.ConsentCookie.IsEssential = true;
                options.OnAppendCookie = (context) =>
                {
                    context.IssueCookie = true;
                };
            });


            services.AddControllersWithViews(options => { }).AddRazorRuntimeCompilation();
            services.AddControllers();

            // using Memory Cache 
            services.AddMemoryCache();

            #region Registering AddDNTCaptcha

            //  AddDNTCaptcha
            services.AddDNTCaptcha(options =>

                options.UseCookieStorageProvider()
                    .ShowThousandsSeparators(false)
                    .WithEncryptionKey("9F3baE2KFTM7m0C^tt%^Ag")

            );
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseStatusCodePagesWithReExecute("/error/{0}");
            }
            else
            {
                //app.UseDeveloperExceptionPage();
                app.UseStatusCodePagesWithReExecute("/error/{0}");

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
           
            app.UseRouting();

            app.UseAuthorization();
            //app.UseElmah();
            app.UseCookiePolicy();
            // Enabling Session
            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Portal}/{action=Login}/{id?}");
            });
        }
    }
}
