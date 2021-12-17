using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ranna_snippets.Authorization;
using ranna_snippets.Database;
using System;
using System.Linq;

namespace ranna_snippets
{
    public class Startup
    {
        public IConfiguration Configuration;

        public Startup(IConfiguration configuration) => Configuration = configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddControllers();

            services.AddCors(cfg =>
                cfg.AddDefaultPolicy(builder =>
                    builder
                        .WithOrigins(
                            Configuration.GetValue<string>("CORS:AllowedOrigins")
                                .Split(",").ToList().Select(v => v.Trim()).ToArray())
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader()));

            services.AddApiVersioning(cfg =>
            {
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            services.AddDbContext<IContext, Context>(ctx => ctx.UseNpgsql(Configuration.GetConnectionString("postgresql")));

            services
                .AddSingleton<IHasher, Argon2Hasher>()
                .AddTransient<IMasterTokenService, MasterTokenService>()
                .AddSingleton<IApiTokenService, JwtApiTokenService>()
            ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var scope = app.ApplicationServices.CreateScope())
            using (var db = scope.ServiceProvider.GetService<IContext>()!)
            {
                db.Database.Migrate();
            }
        }
    }
}
