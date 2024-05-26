
using BookAPI.Extensions;

using DataAccessLayer.Contexts.EFCore;
using System.Reflection.Metadata;

namespace BookAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            builder.Services.AddControllers()
            .AddApplicationPart(typeof(PresentationLayer.AssemblyReference).Assembly);
                
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.ConfigureSqlContext(builder.Configuration);

            // Add services to the container.
            builder.Services.RegisterRepositoryManager();
            builder.Services.RegisterServiceManager();


            var app = builder.Build();

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
    }
}
