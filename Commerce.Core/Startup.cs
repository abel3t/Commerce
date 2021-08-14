using System;
using Commerce.Core.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;

namespace Commerce.Core
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
            
            var mongoDbOptions = new MongoDbOptions()
            {
                ConnectionString = Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING"),
                Database = Environment.GetEnvironmentVariable("MONGO_DBNAME")
            };
            
            services.AddSingleton(mongoDbOptions);
            services.AddSingleton<IMongoClient, MongoClient>(x => new MongoClient(mongoDbOptions.ConnectionString));
            services.AddScoped<IMongoDbContext, MongoDbContext>();
            services.AddScoped<IMongoUnitOfWork, MongoUnitOfWork>();  
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.WithMethods("GET", "POST", "PUT", "PATCH");
                    });
            });
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Commerce.Core", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Commerce.Core v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}