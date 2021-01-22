using FluentValidation.AspNetCore;
using JavnaNabava.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace JavnaNabava
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<Models.RPPP06Context>(options =>
            {
                string connString = Configuration.GetConnectionString("Nabava");
                string password = Configuration["JavnaNabavaPassword"];
                connString = connString.Replace("sifra", password);
                options.UseSqlServer(connString);
            });

            services.AddControllers()
             .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
             .AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNamingPolicy = null);

            //preslikavanje s AppSettings.cs
            var appSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSection);

            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddTransient<ApiPonuditeljController>();
            services.AddTransient<ApiNaručiteljController>();
            services.AddTransient<ApiDokumentController>();
            services.AddTransient<ApiVrstaStavkiController>();
            services.AddTransient<ApiOvlaštenikController>();
            services.AddTransient<ApiKompetencijeController>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles(); //ukljucuje biblioteke
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "RPPP06 JavnaNabava WebAPI");
                c.RoutePrefix = "docs";
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("Kontakti",
                    "{controller:regex(^(KontaktPonuditelj|KontaktKonzorcij)$)}/Page{page:int}/Sort{sort:int}/ASC-{ascending:bool}", new { action = "Index" });



                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{Id?}"
                );
            });
        }
    }
}
