using AutoMapper;
using DataAccess;
using Domain.Config;
using Domain.Domain;
using Domain.ServiceInterfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RestApi;
using RestApi.Hubs;
using RestApi.Utilities;
using Service;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Exceptions;
using Domain.Utilities;
using Microsoft.AspNetCore.Diagnostics;
using RestApi.AuthorizationRequirements;

namespace DependencyResolver
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddApplicationPart(typeof(AccountController).Assembly);

            services.AddOptions();
            services.Configure<TokenConfig>(Configuration.GetSection("TokenConfig"));
            services.Configure<FileStorageConfig>(Configuration.GetSection("FileStorage"));


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddCors(
               options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials()
                   .WithOrigins("http://localhost:4200")
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials()));

            if (!Debugger.IsAttached)
            {
                var connection = Configuration.GetSection("ConnectionStrings").GetSection("ConnectionString").Value;
                services.AddEntityFrameworkSqlServer().AddDbContext<DndContext>
                    (options => options.UseSqlServer(connection));
            }
            else
            {
                var connection = Configuration.GetSection("ConnectionStrings").GetSection("ConnectionStringDebug").Value;
                services.AddEntityFrameworkNpgsql().AddDbContext<DndContext>
                    (options => options.UseNpgsql(connection));
            }

            var key = Configuration.GetSection("TokenConfig").GetSection("Key").Value;

            services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<DndContext>();

            services.AddAuthentication(options =>
            {
                // Identity made Cookie authentication the default.
                // However, we want JWT Bearer Auth to be the default.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(cfg =>
             {
                 cfg.TokenValidationParameters = new TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidIssuer = "Rene",
                     ValidateAudience = true,
                     ValidAudience = "Rene",
                     ValidateIssuerSigningKey = true,
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key.ToString())),
                 };

                 cfg.Events = new JwtBearerEvents
                 {
                     OnMessageReceived = context =>
                     {
                         var accessToken = context.Request.Query["access_token"];

                         // If the request is for our hub...
                         var path = context.HttpContext.Request.Path;
                         if (!string.IsNullOrEmpty(accessToken) &&
                             (path.StartsWithSegments("/api/gamehub")))
                         {
                             // Read the token out of the query string
                             context.Token = accessToken;
                         }

                         return Task.CompletedTask;
                     }
                 };
             });

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.RequireUniqueEmail = true;
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("IsGameOwner", policy => policy.Requirements.Add(new IsOwnerRequirement()));
                options.AddPolicy("IsGamePlayer", policy => policy.Requirements.Add(new IsPlayerRequirement()));
            });

            services.AddSignalR();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IGameService, GameService>();
            services.AddTransient<IJwtReader, JwtReader>();
            services.AddTransient<IChatService, ChatService>();
            services.AddTransient<IJournalService, JournalService>();
            services.AddTransient<IRepository, Repository>();
            services.AddTransient<ImageProcesser>();
            services.AddTransient<IMapService, MapService>();
            services.AddTransient<ILayerService, LayerService>();

            services.AddScoped<IAuthorizationHandler, IsOwnerRequirementHandler>();
            services.AddScoped<IAuthorizationHandler, IsPlayerRequirementHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DndContext context, UserManager<User> userManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async httpContext =>
                    {
                        var exceptionHandlerPathFeature =
                            httpContext.Features.Get<IExceptionHandlerPathFeature>();

                        if (exceptionHandlerPathFeature?.Error is NotFoundException)
                        {
                            await httpContext.Response.WriteAsync(exceptionHandlerPathFeature.Error.Message);
                            httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        }
                    });
                });
                app.UseHsts();
            }

            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            context.EnsureSeeded(userManager).Wait();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DnD Api V1");
            });

            app.UseCors("AllowAll");

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseSignalR(routes =>
            {
                routes.MapHub<GameHub>("/api/gamehub");
            });
            app.UseMvc();
        }
    }
}
