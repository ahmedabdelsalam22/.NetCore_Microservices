using Mango.Services.CouponAPI;
using Mango.Services.CouponAPI.Data;
using Mango.Services.CouponAPI.Repository;
using Mango.Services.CouponAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(option=>option.UseSqlServer(connectionString: connectionString));

builder.Services.AddAutoMapper(typeof(MappingConfig));

builder.Services.AddScoped<ICouponRepository,CouponRepository>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();


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
