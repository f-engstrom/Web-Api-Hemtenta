using System;
using System.Collections.Generic;
using API_Web_API_Kurs.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace API_Web_API_Kurs
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers().AddNewtonsoftJson();

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var signingKey = Convert.FromBase64String(Configuration["Token:SigningKey"]);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(signingKey)
                    };
                });

            services.AddAuthorization(config =>
            {
                config.AddPolicy("IsAdministrator", policy =>
                    policy.RequireClaim("hasRole","admin"));
            });

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WebAPI-Hemtenta API",
                    Version = "1"
                });

                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                        Enter 'Bearer' [space] and then your token in the text input below.
                        \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();


            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI-Hemtenta API v1");
            });

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            MyIdentityDataInitializer.SeedData(userManager, roleManager);

        }



        public static class MyIdentityDataInitializer
        {
            public static void SeedData(
                UserManager<IdentityUser> userManager,
                RoleManager<IdentityRole> roleManager)
            {
                SeedRoles(roleManager);
                SeedUsers(userManager);
            }

            public static void SeedUsers(UserManager<IdentityUser> userManager)
            {
                if (userManager.FindByNameAsync("notadmin").Result == null)
                {
                    IdentityUser user = new IdentityUser
                    {
                        UserName = "notadmin",
                        Email = "notadmin@gmail.com",
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Secret#123").Result;
                }


                if (userManager.FindByNameAsync("admin").Result == null)
                {
                    IdentityUser user = new IdentityUser
                    {
                        UserName = "admin",
                        Email = "admin@gmail.com"
                    };

                    IdentityResult result = userManager.CreateAsync(user, "Secret#123").Result;

                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(user, "Administrator").Wait();
                    }
                }
            }

            public static void SeedRoles(RoleManager<IdentityRole> roleManager)
            {
                if (!roleManager.RoleExistsAsync("Administrator").Result)
                {
                    IdentityRole role = new IdentityRole
                    {
                        Name = "Administrator"
                    };

                    IdentityResult roleResult = roleManager.CreateAsync(role).Result;
                }
            }


        }


    }
}
