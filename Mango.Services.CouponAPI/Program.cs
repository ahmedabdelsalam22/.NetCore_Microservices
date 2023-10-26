using Mango.Services.CouponAPI;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Extensions;
using Mango.Services.CouponAPI.Repository;
using Mango.Services.CouponAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option => {

    option.AddSecurityDefinition(
        name: JwtBearerDefaults.AuthenticationScheme,
        securityScheme: new OpenApiSecurityScheme {
        Name= "Authorization",
        Description="Enter the bearer authoriaztion as following `Bearer token`",
        In= ParameterLocation.Header,
        Type= SecuritySchemeType.ApiKey,
        Scheme= "Bearer"
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
builder.Services.AddDbContext<ApplicationDbContext>(option=>option.UseSqlServer(connectionString: connectionString));

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<ICouponRepository,CouponRepository>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();


builder.AddAppAuthintication(); // extension method that i created to make code clean

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

Stripe.StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

//SeedingDb();

app.Run();


//void SeedingDb()
//{
//    using (var scope = app.Services.CreateScope()) 
//    {
//        var _db = scope.ServiceProvider.GetService<ApplicationDbContext>();
//        if (_db.Database.GetPendingMigrations().Count() > 0) 
//        {
//            _db.Database.Migrate();
//        }
//    }
//}
