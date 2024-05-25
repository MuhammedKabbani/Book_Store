﻿using DataAccessLayer.Contexts.EFCore;
using DataAccessLayer.Contracts;
using Microsoft.EntityFrameworkCore;
namespace BookAPI.Extensions
{
    public static class ServicesExtensions
    {
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<EFRepositoryContext>(options => options.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
        }
        public static void RegisterRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
    }
}