using Hangfire;
using lifeEcommerce.Data;
using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Helpers;
using LifeHangfire.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddHangfire(x => x.UseSqlServerStorage("Server=.;Database=LifeEcommerceProject;Trusted_Connection=True;TrustServerCertificate=True"));
builder.Services.AddHangfireServer();

builder.Services.AddDbContext<LifeEcommerceDbContext>(options =>
                options.UseSqlServer("Server=.;Database=LifeEcommerceProject;Trusted_Connection=True;TrustServerCertificate=True"));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var smtpConfigurations = builder.Configuration.GetSection(nameof(SmtpConfiguration)).Get<SmtpConfiguration>();
builder.Services.AddSingleton(smtpConfigurations);
builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

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

app.UseHangfireDashboard();

app.UseAuthorization();

app.MapControllers();

var scope = app.Services.CreateScope();

var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();
var lifeService = new LifeService(unitOfWork, emailSender);

RecurringJob.AddOrUpdate("NotifyAdmin", () => lifeService.SendEmail(), Cron.Minutely);

app.Run();





