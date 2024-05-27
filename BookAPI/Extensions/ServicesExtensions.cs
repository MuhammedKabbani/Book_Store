using DataAccessLayer.Concrete;
using DataAccessLayer.Contexts.EFCore;
using DataAccessLayer.Contracts;
using Microsoft.EntityFrameworkCore;
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
    }
}
