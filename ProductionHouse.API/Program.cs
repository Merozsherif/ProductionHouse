using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductionHouse.API.Middleware;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Core.Mappings;
using ProductionHouse.Core.Validators;
using ProductionHouse.Infrastructure.Data;
using ProductionHouse.Infrastructure.Repositories;
using ProductionHouse.Infrastructure.Service;
using ProductionHouse.Infrastructure.Services;
using ProductionHouse.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using ProductionHouse.Core.Responses;
var builder = WebApplication.CreateBuilder(args);

// ==========================================
// 1. تسجيل الخدمات (Dependency Injection)
// ==========================================

builder.Services.AddControllers();

// إعداد قاعدة البيانات (DbContext) - مرة واحدة فقط
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// تسجيل الـ Repositories والـ Services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddControllers()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssemblyContaining<AddProjectDtoValidator>();
    });
builder.Services.AddAutoMapper(typeof(MappingProfile));
// إعداد Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        var response = new ApiResponse<object>
        {
            Success = false,
            Message = "Validation failed.",
            Errors = errors
        };

        return new BadRequestObjectResult(response);
    };
});
builder.Services.AddScoped<IImageService, ImageService>();


// ==========================================
// 2. بناء التطبيق (Build)
// ==========================================
var app = builder.Build();


app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();
// ==========================================
// 3. إعداد الـ Middleware Pipeline (الترتيب هنا مهم جداً لعمل التطبيق)
// ==========================================

if (app.Environment.IsDevelopment()) // يفضل وضع Swagger في بيئة التطوير فقط
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.MapControllers();

app.Run();