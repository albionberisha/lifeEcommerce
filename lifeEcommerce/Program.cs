using AutoMapper;
using lifeEcommerce.Data;
using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Helpers;
using lifeEcommerce.Services;
using lifeEcommerce.Services.IService;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var mapperConfiguration = new MapperConfiguration(
    mc => mc.AddProfile(new AutoMapperConfigurations()));

IMapper mapper = mapperConfiguration.CreateMapper();

builder.Services.AddSingleton(mapper);


builder.Services.AddDbContext<LifeEcommerceDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ICoverService, CoverService>();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

app.UseAuthorization();

app.MapControllers();

app.Run();


/*
Singleton which creates a single instance throughout the application. 
It creates the instance for the first time and reuses the same object in the all calls.

Scoped lifetime services are created once per request within the scope. 
It is equivalent to a singleton in the current scope. 

Transient lifetime services are created each time they are requested. 
This lifetime works best for lightweight, stateless services.
 */