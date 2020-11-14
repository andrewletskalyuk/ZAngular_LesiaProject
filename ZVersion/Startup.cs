using BaseProjectDataContext;
using BaseProjectDataContext.Entity;
using BaseProjectDomain.Implementations;
using BaseProjectDomain.Interfaces;
using ClassLibraryDbContext.Initializer;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using ZVersion.Helper;

namespace ZVersion
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            using (var client = new SqLiteContextUsers())
            {
                client.Database.EnsureCreated();
                BaseProjectDataContext.SqliteContextInitializer.Initialize(client);
            }
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            //для роботи з телеграм ботом
            services.AddControllers()
                .AddNewtonsoftJson(options
                        =>options.SerializerSettings.ReferenceLoopHandling 
                        = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            
            services.AddCors();
            
            //тут змінили, і в нас Identity.EntityFramework тому що працюємо з Sqlite
            services.AddIdentity<User, Microsoft.AspNetCore.Identity.IdentityRole>()
                .AddEntityFrameworkStores<SqLiteContextUsers>()
                .AddDefaultTokenProviders();

            //валідація для паролю
            services.Configure<Microsoft.AspNetCore.Identity.IdentityOptions>(opt =>
            {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireDigit = true;
                opt.Password.RequireLowercase = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireNonAlphanumeric = false;
            });

            services.AddTransient<IJWTTokenService, JWTTokenService>();
            
            var jwtTokenSecretKey = Configuration.GetValue<string>("SecretPhrase");
            var singInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenSecretKey));

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = singInKey,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    // set ClockSkew is zero
                    ClockSkew = TimeSpan.Zero
                };
            });

            //Google reCapcha
            services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();

            //додаємо базу даних з якою будемо працювати
            services.AddEntityFrameworkSqlite().AddDbContext<SqLiteContextUsers>();
           

            services.AddControllersWithViews();
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseRouting();


            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
            //якщо БД пуста тоді 
            SeederDatabase.SeedData(app.ApplicationServices, env, Configuration);
        }
    }
}
