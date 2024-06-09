
using BookAPI.Extensions;

using DataAccessLayer.Contexts.EFCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NLog;
using PresentationLayer.ActionFilters;
using ServicesLayer.Concrete;
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
            
            builder.Services.AddControllers(config => {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
            })
            .AddXmlDataContractSerializerFormatters()
            .AddCustomCsvFormatter()
            .AddApplicationPart(typeof(PresentationLayer.AssemblyReference).Assembly);
                
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureSqlContext(builder.Configuration);

            // Add services to the container.
            builder.Services.RegisterRepositoryManager();
            builder.Services.RegisterServiceManager();
            builder.Services.RegisterLoggerService();
            builder.Services.RegisterAutoMapper();
            builder.Services.RegisterActionFilter();
            builder.Services.ConfigureCors();
            builder.Services.RegisterDataShaper();
            builder.Services.AddCustomMediaType();
            builder.Services.AddScoped<IBookLinks, BookLinks>();
            builder.Services.ConfigureVersioning();

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

            app.UseCors("CorsPolicy");

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
