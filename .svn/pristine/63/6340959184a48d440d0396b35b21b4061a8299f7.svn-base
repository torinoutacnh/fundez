using System;
using System.IO;
using System.IO.Compression;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Elect.Core.ConfigUtils;
using Elect.Data.IO;
using Elect.DI;
using Elect.Jaeger;
using Elect.Job.Hangfire;
using Elect.Logger.Logging;
using Elect.Mapper.AutoMapper;
using Elect.Notification.Esms;
using Elect.Notification.Esms.Models;
using Elect.Notification.OneSignal;
using Elect.Notification.OneSignal.Models;
using Elect.Web.DataTable;
using Elect.Web.DataTable.Models.Constants;
using Elect.Web.DataTable.Models.Options;
using Elect.Web.HealthCheck;
using Elect.Web.Middlewares.CorsMiddleware;
using Elect.Web.Middlewares.HttpContextMiddleware;
using Elect.Web.Middlewares.MeasureProcessingTimeMiddleware;
using Elect.Web.Middlewares.RequestRewindMiddleware;
using Elect.Web.Middlewares.ServerInfoMiddleware;
using Elect.Web.Swagger;
using FluentValidation;
using FluentValidation.AspNetCore;
using Flurl.Http;
using Flurl.Http.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TIGE.Binders;
using TIGE.Core.EmailProvider.GmailProvider;
using TIGE.Core.EmailProvider.SendGridProvider;
using TIGE.Core.JsonConverters;
using TIGE.Core.Models;
using TIGE.Core.Share.Constants;
using TIGE.Core.Share.Filters.Exception;
using TIGE.Core.Share.Filters.Validation;
using TIGE.Core.Utils;
using TIGE.Filters.Auth;
using TIGE.Filters.Exception;
using TIGE.Models;
using TIGE.Repository;
using CacheControlHeaderValue = Microsoft.Net.Http.Headers.CacheControlHeaderValue;

namespace TIGE
{
    public class Startup
    {
        public readonly IWebHostEnvironment WebHostEnvironment;

        public readonly IConfiguration Configuration;

        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            WebHostEnvironment = env;

            Configuration = configuration;
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.AddTigeDbContext();

                // System Setting
                services.AddSystemSetting(Configuration.GetSection<SystemSettingModel>("SystemSetting"));

                // add config
                services.AddElectOneSignal(Configuration.GetSection(nameof(ElectOneSignalOptions)).Get<ElectOneSignalOptions>());

                // HTTPS
                if (SystemHelper.Setting.IsEnforceHttps)
                {
                    services.AddHttpsRedirection(options =>
                    {
                        options.RedirectStatusCode = StatusCodes.Status308PermanentRedirect;
                        options.HttpsPort = 443;
                    });
                }

                // Action and Temp Data

                services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
                services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

                // Data Table
                services.AddElectDataTable(_ =>
                {
                    _.DateFormat = Formattings.DateFormat;
                    _.DateTimeFormat = Formattings.DateTimeFormat;
                    _.DateTimeTimeZone = Formattings.TimeZone;
                    _.RequestDateTimeFormatType = DateTimeFormatType.Specific;
                    _.DefaultDisplayText = new ElectDataTableDefaultDisplayTextModel
                    {
                        Yes = "Yes",
                        No = "No",
                        Filter = "Filter",
                        FilterBy = "Filter By",
                        All = "All",
                        Loading = "Loading ..."
                    };
                });

                // Validation Filters

                services.AddScoped<ApiValidationActionFilterAttribute>();

                // Exception Filters

                services.AddScoped<ApiExceptionFilterAttribute>();
                services.AddScoped<PortalExceptionFilterAttribute>();
                services.AddScoped<RootExceptionFilterAttribute>();

                // Authentication

                services.AddScopedIfNotExist<ApiAuthActionFilter>();
                services.AddScopedIfNotExist<MvcAuthActionFilter>();
                services.AddScopedIfNotExist<LoggedInUserBinderFilter>();

                // Model Binders
                services.AddDateTimeBinder();
                services.AddDateTimeOffsetBinder();

                // Logger
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Information);
                });

                services.AddElectLog(options =>
                {
                    options.IsEnableLogToConsole = true;
                    options.IsEnableLogToFile = false;
                });

                // Email Provider
                //services.AddSendGridEmailProvider(Configuration.GetSection<SendGridOptions>("SendGridEmailProvider"));
                services.AddGmailProvider(Configuration.GetSection<GmailOptions>("GmailEmailProvider"));
                //services.AddAmazonProvider(Configuration.GetSection<GmailOptions>("AmazonEmailProvider"));

                // Cache
                services.AddMemoryCache();

                // Data Protection
                services.AddDataProtection();

                // Mapper
                services.AddElectAutoMapper();

                // Http Context
                services.AddElectHttpContext();

                // Server Info
                services.AddElectServerInfo();

                // API Doc - Swagger
                var electSwaggerOptions = Elect.Web.Swagger.IServiceCollectionExtensions.GetOptions(Configuration);
                services.AddElectSwagger(electSwaggerOptions);

                // Health Check
                var electHealthCheckOptions =
                    Elect.Web.HealthCheck.IServiceCollectionExtensions.GetOptions(Configuration);
                services.AddElectHealthCheck(electHealthCheckOptions);

                // Background Job - Hangfire
                var electHangfireOptions = Elect.Job.Hangfire.IServiceCollectionExtensions.GetOptions(Configuration);
                services.AddElectHangfire(electHangfireOptions);

                var configSms = Configuration.GetSection<ElectEsmsOptions>("ElectEsmsOptions");
                services.AddElectNotificationEsms(configSms);
                // MVC

                // MVC Core
                var mvcCoreBuilder = services.AddMvcCore(options => { options.EnableEndpointRouting = false; });

                // MVC View
                mvcCoreBuilder.AddViews(options => { options.HtmlHelperOptions.ClientValidationEnabled = true; });

                mvcCoreBuilder.AddRazorViewEngine(options =>
                {
                    options.AreaViewLocationFormats.Clear();
                    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
                    options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
                });

                mvcCoreBuilder.AddCacheTagHelper();

#if DEBUG
                mvcCoreBuilder.AddRazorRuntimeCompilation();
#endif

                mvcCoreBuilder.AddDataAnnotations();

                mvcCoreBuilder.AddApiExplorer(); // API Doc - Swagger Needed

                mvcCoreBuilder.AddFormatterMappings();

                // Action Context Accessor
                services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        
                // Model Binder
                services.AddDateTimeOffsetBinder();
                services.AddDateTimeBinder();
                services.AddTimeSpanBinder();

                // MVC Json Config
                mvcCoreBuilder.AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(
                        new System.Text.Json.Serialization.JsonStringEnumConverter());
                    options.JsonSerializerOptions.Converters.Add(new DateTimeOffsetJsonConverter());
                    options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
                    options.JsonSerializerOptions.Converters.Add(new TimeSpanJsonConverter());
                    options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                });

                // Fluent Validation
                mvcCoreBuilder.AddFluentValidation(fvc =>
                {
                    fvc.RegisterValidatorsFromAssemblyContaining<IValidator>();
                    fvc.RegisterValidatorsFromAssemblyContaining<Core.Share.Validators.IValidator>();
                    fvc.RegisterValidatorsFromAssemblyContaining<Core.Validators.IValidator>();
                    fvc.ImplicitlyValidateChildProperties = true;
                });

                // Cors
                services.AddElectCors();

                // Flurl Config
                FlurlHttp.Configure(config =>
                {
                    config.JsonSerializer = new NewtonsoftJsonSerializer(Formattings.JsonSerializerSettings);
                });

                // Response Compress

                services.Configure<BrotliCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.Optimal;
                });

                services.Configure<GzipCompressionProviderOptions>(options =>
                {
                    options.Level = CompressionLevel.Optimal;
                });

                services.AddResponseCompression(options =>
                {
                    options.Providers.Add<BrotliCompressionProvider>();
                    options.Providers.Add<GzipCompressionProvider>();
                });



                // DI
                services.AddElectDI();
                services.PrintServiceAddedToConsole();
                Console.ResetColor();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
        {
            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                // Cors
                app.UseElectCors();

                // Enable HTTP Body Rewind
                app.UseElectRequestRewind();

                // Http Context
                app.UseElectHttpContext();

                // Service Measure
                app.UseElectMeasureProcessingTime();

                // Server Info
                app.UseElectServerInfo();

                // Response Compress
                app.UseResponseCompression();

                // API Doc - Swagger
                app.UseElectSwagger();

                // Health Check
                app.UseElectHealthCheck();

                // Background Job - Hangfire
                app.UseElectHangfire();

                // System Setting
                app.UseSystemSetting();

                // Response Cache

                app.UseResponseCaching();

                // MVC

                // Static Files
                // --- Root Path and GZip
                foreach (var staticFileModel in StaticFiles.ListStaticFile)
                {
                    var folderPath = Path.Combine(staticFileModel.AreaFolderName, staticFileModel.AreaName, staticFileModel.FolderRelativePath);

                    folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderPath);

                    if (!Directory.Exists(folderPath))
                    {
                        folderPath = PathHelper.GetFullPath(folderPath);
                    }

                    app.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(folderPath),

                        RequestPath = new PathString(staticFileModel.HttpRequestPath),

                        OnPrepareResponse = context =>
                        {
                            var headers = context.Context.Response.GetTypedHeaders();

                            headers.CacheControl = new CacheControlHeaderValue
                            {
                                MaxAge = staticFileModel.MaxAgeResponseHeader
                            };
                        }
                    });
                }

                app.UseStatusCodePagesWithReExecute("/Error/{0}");

                // Config Global Route
                app.UseMvc(routes =>
                {
                    routes.MapRoute("area", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                    routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
                });

                //app.Use(async (context, next) =>
                //{
                //    await next();
                //    if (context.Response.StatusCode == 404)
                //    {
                //        context.Request.Path = "~/Error/404";
                //        await next();
                //    }
                //});

                //app.UseExceptionHandler("/Error");
                //app.UseHsts();

               

                //app.Use(async (ctx, next) =>
                //{
                //    await next();

                //    if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                //    {
                //        //Re-execute the request so the user gets the error page
                //        string originalPath = ctx.Request.Path.Value;
                //        ctx.Items["originalPath"] = originalPath;
                //        ctx.Request.Path = "/error/404";
                //        await next();
                //    }
                //});
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}