using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using Mini_Kurs_Satis_Sitesi.Core.Constants;
using Mini_Kurs_Satis_Sitesi.Core.UnitOfWork;
using Mini_Kurs_Satis_Sitesi.Data;
using Mini_Kurs_Satis_Sitesi.Data.Repositories;
using Mini_Kurs_Satis_Sitesi.Service.Services;
using SharedLibrary.Configuration;
using SharedLibrary.DTOs;
using SharedLibrary.Extensions;
using SharedLibrary.Services;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()     // Tüm originlere izin ver (geliştirme için)
            .AllowAnyHeader()     // Tüm headerlara izin ver
            .AllowAnyMethod();    // Tüm HTTP metodlarına izin ver
    });
});

// Add services to the container.
var config = new MapperConfiguration(cfg => 
{
    cfg.AddMaps(typeof(Program).Assembly);
    cfg.AddMaps(typeof(Mini_Kurs_Satis_Sitesi.Service.Mapping.MapProfile).Assembly);
});

IMapper mapper = config.CreateMapper();
builder.Services.AddSingleton(mapper);

// DI Register
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped(typeof(IServiceGeneric<,>), typeof(ServiceGeneric<,>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), sqlOptions =>
    {
        sqlOptions.MigrationsAssembly("Mini_Kurs_Satis_Sitesi.Data");
    });
});

builder.Services.AddIdentity<UserApp, IdentityRole>(Opt =>
{
    Opt.User.RequireUniqueEmail = true;
    Opt.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOption"));
builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opts =>
{
    var tokenOptions = builder.Configuration.GetSection("TokenOption").Get<CustomTokenOption>();
    opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
    {
        ValidIssuer = tokenOptions.Issuer,
        ValidAudience = tokenOptions.Audience[0],
        IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

        ValidateIssuerSigningKey = true,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Add authorization policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("InstructorOnly", policy => policy.RequireRole(Roles.Instructor));
    options.AddPolicy("UserOnly", policy => policy.RequireRole(Roles.User));
});

builder.Services.AddControllers().AddFluentValidation(options =>
{
    options.RegisterValidatorsFromAssemblyContaining<Program>();
});

builder.Services.UseCustomValidationResponse();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Mini_Kurs_Satis_Sitesi", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Mini_Kurs_Satis_Sitesi v1"));
}

app.UseCustomException();

app.UseHttpsRedirection();

// CORS middleware'ini ekle (UseRouting'den önce)
app.UseCors();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
