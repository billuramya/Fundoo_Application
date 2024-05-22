using ManagerLayer.Interfaces;
using ManagerLayer.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using RepositoryLayer.Context;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var logpath = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
NLog.GlobalDiagnosticsContext.Set("LogDirectory", logpath);
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
builder.Host.UseNLog();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<FunduContext>(x => x.UseSqlServer(builder.Configuration["ConnectionStrings:DBConnection"]));
// ConfigurationManager configuration = builder.Configuration;
//builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = configuration["RedisCacheUrl"]; });


builder.Services.AddTransient<IUserRepo,UserRepo>();
builder.Services.AddTransient<IUserManager,UserManager>();
builder.Services.AddTransient<INotesRepo, NotesRepo>();
builder.Services.AddTransient<INotesManager, NotesManeger>();
builder.Services.AddTransient<ILableRepo, LableRepo>();
builder.Services.AddTransient<ILableManager, LableManager>();
builder.Services.AddTransient<ICollabRepo, CollabRepo>();
builder.Services.AddTransient<ICollabManager, CollabManager>();

builder.Services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddSwaggerGen(
    option =>
    {
        option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
        option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Please enter a valid token",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            BearerFormat = "JWT",
            Scheme = "Bearer"
        });
        option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer" 
                }
            },
            new string[]{}
        }
    });
    });
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>
{
    var Key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
    o.SaveToken = true;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});
builder.Services.AddDistributedMemoryCache();

builder.Services.AddMassTransit(x =>
{
    x.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
    {
        config.UseHealthCheck(provider);
        config.Host(new Uri("rabbitmq://localhost"), h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    }));
});
builder.Services.AddMassTransitHostedService();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("ApiCorsPolicy");
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllers();

app.Run();