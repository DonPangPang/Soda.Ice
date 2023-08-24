using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Soda.Ice.Common;
using Soda.Ice.Common.Extensions;
using Soda.Ice.Common.Helpers;
using Soda.Ice.Domain;
using Soda.Ice.Shared;
using Soda.Ice.Shared.ViewModels;
using Soda.Ice.WebApi.Auth;
using Soda.Ice.WebApi.Data;
using Soda.Ice.WebApi.Options;
using Soda.Ice.WebApi.Services.CurrentUserServices;
using Soda.Ice.WebApi.UnitOfWorks;
using System.Text;
using Microsoft.OpenApi.Models;
using Soda.Ice.WebApi.Filters;

namespace Soda.Ice.WebApi.Setups;

public static class SetupExtensions
{
    public static void AddIce(this IServiceCollection services)
    {
        services.AddMemoryDb();
        services.AddIceServices();
        services.AddIceCors();
        services.AddConfigure();
        services.AddLazyResolution();
        services.AddHttpContextAccessor();
        services.AddMemoryCache();
        services.AddAuthSetup();
        services.AddAutoMapperSetup();
        services.AddSwaggerSetup();
    }

    private static void AddMemoryDb(this IServiceCollection services)
    {
        services.AddDbContext<IceDbContext>(setups =>
        {
            setups.UseInMemoryDatabase("memory");
        });
    }

    private static void AddIceServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<Session>();
    }

    private static void AddIceCors(this IServiceCollection services)
    {
        services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(policyBuilder =>
            {
                policyBuilder.AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                    .SetIsOriginAllowed(_ => true);
            });
        });
    }

    private static void AddConfigure(this IServiceCollection services)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build(); //编译成对象

        services.AddOptions().Configure<AppSettings>(config.GetSection("Appsettings"));
    }

    private static void AddLazyResolution(this IServiceCollection services)
    {
        services.AddTransient(
            typeof(Lazy<>),
            typeof(LazilyResolved<>));
    }

    private class LazilyResolved<T> : Lazy<T> where T : notnull
    {
        public LazilyResolved(IServiceProvider serviceProvider)
            : base(serviceProvider.GetRequiredService<T>)
        {
        }
    }

    /// <summary>
    /// 添加认证
    /// </summary>
    /// <param name="services"> </param>
    /// <returns> </returns>
    private static void AddAuthSetup(this IServiceCollection services)
    {
        IConfiguration? configuration = services.BuildServiceProvider().GetService<IConfiguration>();

        var para = configuration?.GetSection("TokenParameter").Get<PermissionRequirement>()!;

        services.AddAuthorization(opts =>
        {
            opts.AddPolicy(GlobalVars.Permission, policy =>
            {
                policy.Requirements.Add(new PermissionRequirement());
            });
        });

        services.AddAuthentication(opts =>
        {
            opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer("Bearer", options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = para.Audience,
                ValidIssuer = para.Issuer,
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(para.Secret))
            };

            options.SaveToken = true;

            options.Events = new JwtBearerEvents
            {
                //此处为权限验证失败后触发的事件
                OnChallenge = context =>
                {
                    //此处代码为终止.Net Core默认的返回类型和数据结果，这个很重要哦，必须
                    context.HandleResponse();
                    //自定义自己想要返回的数据结果，我这里要返回的是Json对象，通过引用Newtonsoft.Json库进行转换
                    //自定义返回的数据类型
                    context.Response.ContentType = "application/json";
                    //自定义返回状态码，默认为401 我这里改成 200
                    //context.Response.StatusCode = StatusCodes.Status200OK;
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //输出Json数据结果
                    var result = new IceResponse
                    {
                        IsSuccess = false,
                        Message = "Token校验失败"
                    }.ToJson();
                    context.Response.WriteAsync(result);
                    return Task.FromResult(0);
                }
            };
        }
        );
    }

    private static void AddAutoMapperSetup(this IServiceCollection services)
    {
        services.AddAutoMapper(setup =>
        {
            var entitys = TypeHelper.GetEntityTypes();
            var viewModels = TypeHelper.GetViewModelTypes();
            foreach (var entityType in entitys)
            {
                var viewType = viewModels.FirstOrDefault(x => new[] { $"V{entityType.Name}", $"VRegister{entityType.Name}" }.Any(t => t == x.Name));
                setup.CreateMap(entityType, viewType);
                setup.CreateMap(viewType, entityType);
            }

            setup.CreateMap<VRegisterUser, User>();
            setup.CreateMap<Blog, VBlogTiny>();
        }, AppDomain.CurrentDomain.GetAssemblies());
    }

    public static IServiceCollection AddSwaggerSetup(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Api Document", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            c.DocumentFilter<SwaggerEnumFilter>();

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });

            //var xmlFile = $"app-doc.xml";
            //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            //var sharedXml = Path.Combine(AppContext.BaseDirectory, "shared-doc.xml");

            ////... and tell Swagger to use those XML comments.
            //c.IncludeXmlComments(xmlPath, true);
            //c.IncludeXmlComments(sharedXml, true);
        });

        return services;
    }
}