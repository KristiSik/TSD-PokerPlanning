using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlanningPokerBackend.Models;

namespace PlanningPokerBackend
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDbContext<PlanningPokerDbContext>(opt => opt.UseSqlServer(@"Server=.\SQLEXPRESS;Database=PlanningPokerDb;User Id=<username>;Password=<password>;"));
            services.AddDbContext<PlanningPokerDbContext>(opt => opt.UseInMemoryDatabase("somedb"));
            services.AddMvc()
                .AddJsonOptions(options => {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService <PlanningPokerDbContext>();
                context.Database.Migrate();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute("api", "api/{controller}/{action}");
            });
        }
    }
}