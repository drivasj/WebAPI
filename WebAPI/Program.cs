using Microsoft.EntityFrameworkCore;
using WebAPI;
using WebAPI.Dates;
using WebAPI.Repository;
using WebAPI.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// SqlServer
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // SQL Server
});

//Automapper
builder.Services.AddAutoMapper(typeof(MappingConfig));

//IVillaRepository
builder.Services.AddScoped<IVillaRepository, VillaRepository>();

//INumVillaRepository
builder.Services.AddScoped<INumVillaRepository, NumVillaRepository>();


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
