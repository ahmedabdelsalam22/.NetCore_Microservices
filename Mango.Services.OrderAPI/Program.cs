using Mango.Services.OrderAPI.Utility;
using Mango.Services.OrderAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Mango.Services.OrderAPI.Extensions;
using Mango.Services.OrderAPI.Services.IServices;
using Mango.Services.OrderAPI.Services;
using Mango.MessageBus;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

SD.ProductAPIUrl = builder.Configuration["ServiceUrls:ProductAPIUrl"];

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option => {

    option.AddSecurityDefinition(
        name: JwtBearerDefaults.AuthenticationScheme,
        securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Enter the bearer authoriaztion as following `Bearer token`",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme{
            Reference=new OpenApiReference{
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
               }
            },
            new string[]{ }
        }
    });

});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(connectionString: connectionString));


builder.Services.AddScoped<IProductRestService, ProductRestService>();

builder.Services.AddScoped<IMessageBus, MessageBus>();


builder.Services.AddAutoMapper(typeof(MappingConfig));
//builder.Services.AddHttpContextAccessor();
//builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();

builder.AddAppAuthintication();

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
