using AspNetCoreRateLimit;
using DataAccessLayer.Concrete;
using DataAccessLayer.Contexts.EFCore;
using DataAccessLayer.Contracts;
using EntityLayer.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.ActionFilters;
using ServicesLayer.Concrete;
using ServicesLayer.Contracts;
namespace BookAPI.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EFRepositoryContext>(options => options.UseSqlServer(configuration.GetConnectionString("sqlConnection"), b => b.MigrationsAssembly("BookAPI")));
        }
        public static void RegisterRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
        public static void RegisterServiceManager(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
        }
        public static void RegisterLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerService, LoggerManager>();
        }
        public static void RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Program));
        }
        public static void RegisterActionFilter(this IServiceCollection services)
        {
            services.AddScoped<ValidationFilterAttribute>();
            services.AddSingleton<LogFilterAttribute>();
            services.AddScoped<ValidateMediaTypeAttribute>();
        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .WithExposedHeaders("X-Pagination");
                });
            });
        }

        public static void RegisterDataShaper(this IServiceCollection services)
        {
            services.AddScoped<IDataShaper<DTOBook>, DataShaper<DTOBook>>();
        }
        public static void AddCustomMediaType(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var newtonsoftJsonOutputFormatter = config.OutputFormatters.OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();
                if (newtonsoftJsonOutputFormatter != null)
                {
                    newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.bookstore.hateoas+json");
                    newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/vnd.bookstore.apiroot+json");

                }
                var xmlOutputFormatter = config.OutputFormatters.OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();
                if (xmlOutputFormatter != null)
                {
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.bookstore.hateoas+xml");
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/vnd.bookstore.apiroot+xml");
                }
            });
        }
        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");

                //opt.Conventions.Controller<BookController>()
                //                .HasApiVersion(new ApiVersion(1, 0));

                //opt.Conventions.Controller<BookV2Controller>()
                //                .HasApiVersion(new ApiVersion(2, 0));


            });
        }
        public static void ConfigureResponseCahing(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }
        public static void ConfigureHttpCachHeaders(this IServiceCollection services)
        {
            services.AddHttpCacheHeaders();
        }
        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule()
                {
                    Endpoint = "*",
                    Limit = 3,
                    Period = "1m"
                }
            };
            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        }
    }
}
