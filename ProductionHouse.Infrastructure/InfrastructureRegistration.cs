using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Infrastructure.Data;
using ProductionHouse.Infrastructure.Repositories;
using ProductionHouse.Infrastructure.Service;
using ProductionHouse.Infrastructure.Services;
using ProductionHouse.Infrastructure.UnitOfWork;
namespace ProductionHouse.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static IServiceCollection InfrastructureConfiguration(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")));

            // Generic Repository
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Repositories
            services.AddScoped<IProjectRepository, ProjectRepository>();

            // Unit Of Work
            services.AddScoped<IUnitOfWork,UnitOfWork>();

            // Services
            services.AddScoped<IProjectService, ProjectService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IImageService, ImageService>();

            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}