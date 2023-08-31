using CleanArchitecture.Persistance.Context;
using CleanArchitecture.Application;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Persistance.Services;
using MediatR;
using CleanArchitecture.Application.Behaviors;
using FluentValidation;
using CleanArchitecture.WebApi.Middleware;
using GenericRepository;
using CleanArchitecture.Domain.Repositories;
using CleanArchitecture.Persistance.Repositories;
using CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using CleanArchitecture.WebApi.OptionsSetup;
using CleanArchitecture.Application.Abstraction;
using CleanArchitecture.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddTransient<ExceptionMiddleware>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork<AppDbContext>>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();
builder.Services.ConfigureOptions<JwtOptionsSetup>();
builder.Services.ConfigureOptions<JwtBearerOptionsSetup>();
builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();
builder.Services.AddAutoMapper(typeof(CleanArchitecture.Persistance.AssemblyReference).Assembly);


//Log.Logger = new LoggerConfiguration()
//                .MinimumLevel.Debug()
//                .Enrich.FromLogContext()
//                .WriteTo.MSSqlServer(
//                    connectionString: "Data Source=localhost;Initial Catalog=CleanArchitectureDB;User ID=sa;Password=CK97123AAQ!;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False",
//                    sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
//                    {
//                        TableName = "Logs",
//                        AutoCreateSqlTable = true
//                    })
//                .WriteTo.File("Logs/log-txt", rollingInterval: RollingInterval.Day)
//                .CreateLogger();

string connectionString = builder.Configuration.GetConnectionString("SqlServer");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllers()
    .AddApplicationPart(typeof(CleanArchitecture.Presentation.AssemblyReference).Assembly);

builder.Services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(CleanArchitecture.Application.AssemblyReference).Assembly));

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(typeof(CleanArchitecture.Application.AssemblyReference).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecuritySheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** yourt JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecuritySheme.Reference.Id, jwtSecuritySheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecuritySheme, Array.Empty<string>() }
                });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddlewareExtension();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
