using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using AutoMapper;
using ClassLibraryDbContext;
using ClassLibraryDbContext.Initializer;
using GoogleReCaptcha.V3;
using GoogleReCaptcha.V3.Interface;
//using LesiaWebApi.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StartOfEnding.Services;

namespace StartOfEnding
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            //тепер створимо додамо дані в нашу БД
            using (var client = new SqliteContext())
            {
                client.Database.EnsureCreated();
                SqliteContextInitializer.Initialize(client);
            }
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddMvc();
            services.AddEntityFrameworkSqlite().AddDbContext<SqliteContext>();
            
            services.AddTransient<IDataBaseContext, SqliteContext>();
            //додаємо наш сервіс відправки листів
            services.AddTransient<SendEmailService>();
            
            services.AddControllersWithViews(); //
            //services.AddAutoMapper(typeof(AutoMapperConfig)); //додаємо наш мапер для того щоб з БД не було проблем при обміні інформацією
            //для роботи з телеграм ботом
            services.AddControllers().AddNewtonsoftJson(options =>
                                                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            //для роботи з файлом конфігурації
            //services.AddSingleton
            
            services.AddHttpClient<ICaptchaValidator, GoogleReCaptchaValidator>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
