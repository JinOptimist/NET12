using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff;
using WebMaze.EfStuff.Repositories;

namespace WebMaze
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
            var connectString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WebMaze12;Integrated Security=True;";
            services.AddDbContext<WebContext>(x => x.UseSqlServer(connectString));

            services.AddScoped<UserRepository>(diContainer =>
                {
                    var webContext = diContainer.GetService<WebContext>();
                    var reviewRepository = diContainer.GetService<ReviewRepository>();
                    var repository = new UserRepository(webContext, reviewRepository);
                    return repository;
                }
            );

            services.AddScoped<ReviewRepository>(diContainer =>
            {
                var webContext = diContainer.GetService<WebContext>();
                var repository = new ReviewRepository(webContext);
                return repository;
            });

            services.AddScoped<StuffForHeroRepository>(diContainer =>
            {
                var webContext = diContainer.GetService<WebContext>();
                var repository = new StuffForHeroRepository(webContext);
                return repository;
            });

            services.AddScoped<NewsRepository>(diContainer =>
            {
                var webContext = diContainer.GetService<WebContext>();
                var repository = new NewsRepository(webContext);
                return repository;
            }
           );
        }      
            

            services.AddControllersWithViews();
        

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
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
