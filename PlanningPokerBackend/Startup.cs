using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlanningPokerBackend.Models;
using System;

namespace PlanningPokerBackend
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PlanningPokerDbContext>(opt => opt.UseInMemoryDatabase(new Guid().ToString()));
            services.AddMvc()
                .AddJsonOptions(options => {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, PlanningPokerDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            DataSeeder ds = new DataSeeder(context);
            ds.SeedData();

            app.UseStaticFiles();
            app.UseMvc(routes =>
            {
                routes.MapRoute("api", "api/{controller}/{action}");
                routes.MapRoute("default", "{controller=Home}/{action=Index}");
            });
        }
    }
}