
using Api.Errors;
using Api.Extensions;
using Api.Helpers;
using Api.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using Infrastructure.Data;
using Infrastructure.Identity;
using Core.Interfaces;
using Infrastructure.Data.Repositories;
using Infrastructure.Services;
using Core.Models.Identity;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling =
Newtonsoft.Json.ReferenceLoopHandling.Ignore); 

// Add DI 

builder.Services.AddDbContext<WebDbContext>(o =>
o.UseSqlServer(builder.Configuration.GetConnectionString("con1"),
    b => b.MigrationsAssembly(typeof(WebDbContext).Assembly.FullName)));


// Add Redis Config

builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration
        .GetConnectionString("Redis"), true);
    return ConnectionMultiplexer.Connect(configuration);
});

// add identityDbContext service
builder.Services.AddDbContext<AppIdentityDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));

});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = ActionContext =>
    {
        var errors = ActionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(e => e.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

        var errorRespone = new ApiValidationErrorRespone
        {
            Errors = errors
        };

        return new BadRequestObjectResult(errorRespone);


    };
});



builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IProdcutRepository, ProductRepository>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddScoped(typeof(IGenericRepository<>),(typeof(GenericRepository<>)));

builder.Services.AddAutoMapper(typeof(MappingProfiles));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(opt =>

        opt.AddPolicy("CorsPolicy", policy =>
         {
             policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
 
         })
);

//add identity services
var config = builder.Configuration;
builder.Services.AddIdentityServices(config);


var app = builder.Build();


//seeding user Identity and products data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<WebDbContext>();
    var userManger = services.GetRequiredService<UserManager<AppUser>>();

    await StoreSeed.SeedDataAsync(context);
    await AppIdentityDbContextSeed.SeedUserAsync(userManger);

}

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
