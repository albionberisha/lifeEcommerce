using AutoMapper;
using lifeEcommerce.Data;
using lifeEcommerce.Data.UnitOfWork;
using lifeEcommerce.Helpers;
using lifeEcommerce.Models.Entities;
using lifeEcommerce.Services;
using lifeEcommerce.Services.IService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Security.Cryptography.Xml;
using System.Text;
using claims = System.Security.Claims;

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

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
})
              .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
              {
                  options.Authority = "https://sso-sts.gjirafa.dev";
                  options.RequireHttpsMetadata = false;
                  options.Audience = "life_api";
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = "https://sso-sts.gjirafa.dev",
                      ValidAudience = "life_api",
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("21551351-fc9a-4d8e-8619-8c7e5acb6d47")),
                      ClockSkew = TimeSpan.Zero
                  };

                  options.Events = new JwtBearerEvents
                  {
                      OnTokenValidated = async context =>
                      {
                          context.HttpContext.User = context.Principal ?? new claims.ClaimsPrincipal();

                          var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                          var firstName = context.HttpContext.User.FindFirst(ClaimTypes.GivenName)?.Value;
                          var lastName = context.HttpContext.User.FindFirst(ClaimTypes.Surname)?.Value;
                          var email = context.HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                          var gender = context.HttpContext.User.FindFirst(ClaimTypes.Gender)?.Value;
                          var birthdate = context.HttpContext.User.FindFirst(ClaimTypes.DateOfBirth)?.Value;
                          var phone = context.HttpContext.User.FindFirst(ClaimTypes.MobilePhone)?.Value;

                          var userService = context.HttpContext.RequestServices.GetService<IUnitOfWork>();

                          if(userService.Repository<User>().GetById(x => x.Id == userId) == null)
                          {
                              var userToBeAdded = new User
                              {
                                  Id = userId,
                                  Email = email,
                                  FirsName = firstName,
                                  LastName = lastName,
                                  Gender = gender,
                                  DateOfBirth = DateTime.Parse(birthdate),
                                  PhoneNumber = phone ?? " "
                              };

                              userService.Repository<User>().Create(userToBeAdded);
                          }
                          else
                          {
                              var existingUser = userService.Repository<User>().GetById(x => x.Id == userId).FirstOrDefault();
                              existingUser.FirsName = firstName;
                              existingUser.LastName = lastName;
                              existingUser.PhoneNumber = phone ?? " ";

                              userService.Repository<User>().Update(existingUser);
                          }

                          userService.Complete();
                      }
                  };

                  // if token does not contain a dot, it is a reference token
                  options.ForwardDefaultSelector = Selector.ForwardReferenceToken("token");
              });

            

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Life Ecommerce", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri("https://sso-sts.gjirafa.dev/connect/authorize"),
                TokenUrl = new Uri("https://sso-sts.gjirafa.dev/connect/token"),
                Scopes = new Dictionary<string, string> {
                                              { "life_api", "Life Api" }
                                          }
            }
        }
    });

    c.OperationFilter<AuthorizeCheckOperationFilter>();
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DisplayRequestDuration();
        c.DefaultModelExpandDepth(0);
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LifeEcommerce");
        c.OAuthClientId("fb1b97e4-778a-431d-abb1-78bbdca9253b");
        c.OAuthClientSecret("21551351-fc9a-4d8e-8619-8c7e5acb6d47");
        c.OAuthAppName("Life Ecommerce");
        c.OAuthUsePkce();
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
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