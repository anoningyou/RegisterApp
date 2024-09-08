using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using API.Extensions;
using System.Text.Json.Serialization;
using API.Data;
using Microsoft.AspNetCore.Identity;
using API.Entities;
using API.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using API.Data.Seed;
using API.Entities.Accounting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>((container) =>
    {
        container.RegisterAssemblyTypes(Assembly.GetEntryAssembly()).AsImplementedInterfaces();
        container.AddDbContext(builder.Environment.IsDevelopment(), builder.Configuration);
        container.RegisterMaps();
        container.AddApplicationServices();
        container.AddDispatchers();
    });

builder.Services.AddControllers()
    .AddJsonOptions(opts =>
        {
            JsonStringEnumConverter enumConverter = new ();
            opts.JsonSerializerOptions.Converters.Add(enumConverter);
        });

builder.Services.AddApplicationServices();

builder.Services.AddIdentityServices(builder.Configuration);

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

app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins(["http://localhost:4200"])
);

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapFallbackToController("Index", "Fallback");

using IServiceScope scope = app.Services.CreateScope();
IServiceProvider services = scope.ServiceProvider;

try
{
    DataContext context = services.GetRequiredService<DataContext>();
    UserManager<AppUser> userManager = services.GetRequiredService<UserManager<AppUser>>();
    RoleManager<AppRole> roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    IUnitOfWork<DataContext> uow = services.GetRequiredService<IUnitOfWork<DataContext>>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(userManager, roleManager);
    await Seed.SeedPlaces(uow);
}
catch (Exception ex)
{
    ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex,"An error occured during migration");
}

app.Run();
