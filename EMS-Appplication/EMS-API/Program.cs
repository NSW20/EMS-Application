using EMP_Infrastructure.Repositories;
using EMP_Infrastructure.SqlOperation;
using EMS_API.DTOValidations;
using EMS_API.ExceptioHandling;
using EMS_Core.Domain.Entities;
using EMS_Core.Domain.RepositoryContract;
using EMS_Core.Helpers;
using EMS_Core.ServiceContracts;
using EMS_Core.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddIdentity<AppUser, AppRole>().
    AddEntityFrameworkStores<EMSDbContext>().
    AddUserStore<UserStore<AppUser,AppRole,EMSDbContext,string>>()
    .AddRoleStore<RoleStore<AppRole,EMSDbContext,string>>().AddDefaultTokenProviders();

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDesignationRepository, DesignationRepository>();
builder.Services.AddScoped<IDesignationService, DesignationService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EMSDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration logger) =>
{
    logger.ReadFrom.Configuration(context.Configuration).ReadFrom.Services(service);
});
builder.Services.AddMemoryCache();
builder.Services.AddAutoMapper(typeof(EMSAutoMapper));
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.AddTokenBucketLimiter("rateLimiter", options =>
    {
        options.TokenLimit = 100;
        options.TokensPerPeriod = 20;
        options.ReplenishmentPeriod = TimeSpan.FromSeconds(10);
        options.QueueLimit = 10;
        options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("enableCors", options =>
    {
        options.WithOrigins("http://localhost:4200");
        options.AllowAnyMethod();
        options.AllowAnyHeader();
    });
});
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<DesignationDTOValidation>();
builder.Services.AddValidatorsFromAssemblyContaining<DesignationDTOUpdateValidation>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseGlobalExeceptionMiddleware();
app.UseSwagger();
app.UseSwaggerUI();
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
    app.UseHsts();
}
app.UseCors("enableCors");
app.UseRateLimiter();
app.UseAuthorization();
app.MapControllers();

app.Run();
