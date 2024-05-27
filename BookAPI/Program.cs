
using BookAPI.Extensions;

using DataAccessLayer.Contexts.EFCore;
using NLog;
using ServicesLayer.Contracts;
using System.Reflection.Metadata;

namespace BookAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            LoadConfigurationForNLog();
            
            builder.Services.AddControllers()
            .AddApplicationPart(typeof(PresentationLayer.AssemblyReference).Assembly);
                
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureSqlContext(builder.Configuration);

            // Add services to the container.
            builder.Services.RegisterRepositoryManager();
            builder.Services.RegisterServiceManager();
            builder.Services.RegisterLoggerService();
            builder.Services.RegisterAutoMapper();

            var app = builder.Build();

            var loggerService = app.Services.GetRequiredService<ILoggerService>();
            app.ConfigureExceptionHandler(loggerService);

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void LoadConfigurationForNLog()
        {
            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "nlog.config"));
        }
    }
}
