using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AutoMapper;
using Newtonsoft.Json.Converters;
using Data.DataAccess;
using Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using Services.Mapping;
using Services.Core;
using Confluent.Kafka;
using Hangfire;
using Hangfire.PostgreSql;
using Services.Hangfire;

namespace WMS.Extensions;

public static class StartupExtension
{
    public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDbContext<AppDbContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("Dev"),
                b => b.MigrationsAssembly(Assembly.GetExecutingAssembly().GetName().Name));
        }, ServiceLifetime.Scoped);
    }

    public static void AddAutoMapper(this IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MapperProfile());
        });
        IMapper mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
    }

    public static void AddBussinessService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUomCategoryService, UomCategoryService>();
        services.AddScoped<IUomUomService, UomUomService>();
        services.AddScoped<IProductRemovalService, ProductRemovalService>();
        services.AddScoped<IProductCategoryService, ProductCategoryService>();
        services.AddScoped<IProductAttributeService, ProductAttributeService>();
        services.AddScoped<IProductAttributeValueService, ProductAttributeValueService>();
        services.AddScoped<IProductTemplateService, ProductTemplateService>();
        services.AddScoped<IProductTemplateAttributeLineService, ProductTemplateAttributeLineService>();
        services.AddScoped<IProductProductService, ProductProductService>();
        services.AddScoped<IStockWarehouseService, StockWarehouseService>();
        services.AddScoped<IStockLocationService, StockLocationService>();

        //services.AddHostedService<HangfireJob>();
        services.AddSingleton<IProducer<Null, string>>(sp =>
            new ProducerBuilder<Null, string>(new ProducerConfig
            {
                BootstrapServers = "192.168.40.81:9092"
            }).Build());
    }

    public static void ConfigIdentityService(this IServiceCollection services)
    {
        var build = services.AddIdentityCore<User>(option =>
        {
            option.SignIn.RequireConfirmedAccount = false;
            option.User.RequireUniqueEmail = false;
            option.Password.RequireDigit = false;
            option.Password.RequiredLength = 6;
            option.Password.RequireNonAlphanumeric = false;
            option.Password.RequireUppercase = false;
            option.Password.RequireLowercase = false;

            option.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            option.User.RequireUniqueEmail = true;
            option.SignIn.RequireConfirmedAccount = false;
        });

        build.AddRoles<Role>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        build.AddSignInManager<SignInManager<User>>();
        services.AddIdentity<User, Role>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddUserManager<UserManager<User>>()
            .AddRoleManager<RoleManager<Role>>()
            .AddDefaultTokenProviders();
        services.AddAuthorization();

        //services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Serialize);

    }

    public static void AddJWTAuthentication(this IServiceCollection services, string key, string issuer)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(jwtconfig =>
            {
                jwtconfig.SaveToken = true;
                jwtconfig.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                    ValidateAudience = false,
                    ValidIssuer = issuer,
                    ValidateIssuer = true,
                    ValidateLifetime = false,
                    RequireAudience = false,
                };
                jwtconfig.Events = new JwtBearerEvents()
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.ContentType = "application/json";

                        // Ensure we always have an error and error description.
                        if (string.IsNullOrEmpty(context.Error))
                            context.Error = "invalid_token";
                        if (string.IsNullOrEmpty(context.ErrorDescription))
                            context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                        // Add some extra context for expired tokens.
                        if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                            context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
                            context.ErrorDescription = $"The token expired on {authenticationException.Expires.ToString("o")}";
                        }

                        return context.Response.WriteAsync(JsonSerializer.Serialize(new
                        {
                            error = context.Error,
                            error_description = context.ErrorDescription
                        }));
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                        return Task.CompletedTask;
                    },
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (path.StartsWithSegments("/notificationHub"))
                        {
                            // Read the token out of the query string 
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
    }
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "WarehouseManagementSystem",
                Version = "1.0",
                Description = "Warehouse Management System",
            });
            c.UseInlineDefinitionsForEnums();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Add OnlineMarketplaceSystem Bearer Token Here",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Name = "Bearer"
                    },
                    new List<string>()
                }
            });
        });

        services.AddControllersWithViews().AddNewtonsoftJson(options =>
        options.SerializerSettings.Converters.Add(new StringEnumConverter()));
        services.AddSwaggerGenNewtonsoftSupport();
    }

    public static void ConfigHangFire(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(_ =>
        {
            _.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseStorage(new PostgreSqlStorage(configuration.GetConnectionString("HangfireConnection"),
                new PostgreSqlStorageOptions
                {
                    UseNativeDatabaseTransactions = true,
                    InvisibilityTimeout = TimeSpan.FromMinutes(10),
                }
                ));
        });
        services.AddHangfireServer(_ => _.WorkerCount = 2);
        //Hangfire's default worker count is 20, which opens 20 connections simultaneously.
    }
}
