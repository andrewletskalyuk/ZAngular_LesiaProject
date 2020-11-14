using AutoMapper;
using ClassLibraryDbContext;
using ClassLibraryDbContext.Initializer;
using LesiaWebApi.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApplicationLesia
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
            //тут закінчив
            //services.AddRecaptcha(Configuration.GetSection("RecaptchaSettings"));
            services.AddControllersWithViews();
            services.AddMvc();
            services.AddEntityFrameworkSqlite().AddDbContext<SqliteContext>();
            services.AddTransient<IDataBaseContext, SqliteContext>();
            services.AddControllersWithViews(); //
            services.AddAutoMapper(typeof(AutoMapperConfig)); //додаємо наш мапер для того щоб з БД не було проблем при обміні інформацією
            services.AddControllers().AddNewtonsoftJson(options =>
                                                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
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
        }
    }
}
