using Domain_Layer.Data;
using Domain_Layer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository_Layer.IRepository;
using Repository_Layer.Repository;
using Service_Layer.IService;
using Service_Layer.Service;

using DotNetEnv;
using System.Diagnostics;
using Supabase;

namespace FuelTrackerAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Env.Load();

            var builder = WebApplication.CreateBuilder(args);

            #region Environment Variables
            builder.Configuration["Jwt:SigningKey"] = Environment.GetEnvironmentVariable("SIGNING_KEY");
            builder.Configuration["Jwt:Audience"] = Environment.GetEnvironmentVariable("JWT_AUDIENCE");
            builder.Configuration["Jwt:Issuer"] = Environment.GetEnvironmentVariable("JWT_ISSUER");

            builder.Configuration["Email:Sender"] = Environment.GetEnvironmentVariable("SENDER");
            builder.Configuration["Email:Password"] = Environment.GetEnvironmentVariable("PASSWORD");

            builder.Configuration["ConnectionStrings:DefaultConnection"] = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            #endregion


            // Add database to the container
            builder.Services.AddDbContext<ApplicationDbContext>(options => 
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            #region Add services to the container.
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<ICustomService<Vehicle>, VehicleService>();
            builder.Services.AddScoped<ICustomService<Fuel>, FuelService>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
            builder.Services.AddScoped<IFuelTrackingService, FuelTrackingService>();
            #endregion

            #region Add identity to the container
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            // Add jwt bearer and authentication schema
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = 
                options.DefaultChallengeScheme =
                options.DefaultForbidScheme =
                options.DefaultScheme =
                options.DefaultSignInScheme =
                options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,

                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"])),

                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();

            #region Swagger .NET Core Web API JWT Setup
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Fuel Tracker API", Version = "v1" });
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
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // local file/image upload
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            #region Add default application roles
            try
            {
                using var scope = app.Services.CreateScope();

                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
                var roles = new string[2] { "User", "Admin" };
                var userRoleDescription = "User are are only permitted to view and update thier information.";
                var adminRoleDescription = "Administrators have a full overview of the whole application.";

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        var newRole = new ApplicationRole(role);
                        newRole.Desccription = (role == "Admin") ? adminRoleDescription : userRoleDescription;
                        newRole.IsActive = true;
                        await roleManager.CreateAsync(newRole);
                    }
                }
            } catch (Exception e)
            {
                throw new Exception($"Role seeding failed: {e.Message}");
            }
            #endregion

            await app.RunAsync();
        }
    }
}
