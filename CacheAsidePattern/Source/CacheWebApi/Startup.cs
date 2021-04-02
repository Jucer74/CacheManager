using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CacheWebApi.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using CacheWebApi.Extensions;
using Microsoft.Extensions.Caching.Memory;
using CacheAsidePattern.CacheStore.Interfaces;
using CacheAsidePattern.CacheStore;
using CacheAsidePattern.CacheManager.Interfaces;
using CacheAsidePattern.CacheManager;

namespace CacheWebApi
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
            services.AddControllers();

            // Add the Database Context
            services.AddDbContext<AppDbContext>();

            // Enable Swagger
            services.AddSwaggerDocumentation();

            // Enable the Memory Cache
            services.AddMemoryCache();

            // Get the Expiration Configuration for the Keys from App Settings
            var children = Configuration.GetSection("Caching").GetChildren();
            Dictionary<string, TimeSpan> expirationConfiguration = children.ToDictionary(child => child.Key, child => TimeSpan.Parse(child.Value));

            // Add the Instance for the CacheStore
            services.AddSingleton<ICacheStore>(x => new CacheStore(x.GetService<IMemoryCache>(), expirationConfiguration));

            // Add the Instance for the CacheManager
            services.AddSingleton<ICacheManager>(x => new CacheManager(x.GetService<IMemoryCache>(), expirationConfiguration));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Use the Swagger Documentation
            app.UseSwaggerDocumentation();
        }
    }
}