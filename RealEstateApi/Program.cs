using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Real_Estate_Context.Context;
using Real_Estate_Dtos.DTO;
using Real_Estate_IServices;
using Real_Estate_Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ecommerce_real_estateContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
   b => b.MigrationsAssembly(typeof(ecommerce_real_estateContext).Assembly.FullName)));

//JWT Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration
                                                                                     .GetSection("Jwt")
                                                                                     .Get<TokenManagerDTO>().SecretKey))
    };
});

#region Injection
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IAdminRepository, AdminRepository>();
builder.Services.AddTransient<ICustomerServicesRepository, CustomerServicesRepository>();
builder.Services.AddTransient<IProjectsRepository, ProjectsRepository>();
builder.Services.AddTransient<IProjectFeatureRepository, ProjectFeatureRepository>();
builder.Services.AddTransient<IlocationsRepository, locationsRepository>();
builder.Services.AddTransient<IlocationsTypesRepository, locationsTypesRepository>();
builder.Services.AddTransient<IPaymentTypeRepository, paymentTypeRepository>();
builder.Services.AddTransient<ILocationImageRepository, LocationImageRepository>();
builder.Services.AddTransient<IVisitorRepository, VisitorRepository>();
builder.Services.AddSingleton<MemoryCacheService>();

//builder.Services.AddHttpClient();
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
