using customer.read.Consumers;
using customer.read.Data;
using customer.read.Interfaces;
using customer.read.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace customer.read
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
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<DataContext>();
            services.AddSingleton(serviceProvider => 
            {
                var mongoClient = new MongoClient($"mongodb://root:P%40ssw0rd@{Configuration["MongoDbSettings:Host"]}:{Configuration["MongoDbSettings:Port"]}");
                return mongoClient.GetDatabase(Configuration["MongoDbSettings:Collection"]);
            });
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"{Configuration["IdentityServerSettings:Scheme"]}://{Configuration["IdentityServerSettings:Host"]}:{Configuration["IdentityServerSettings:Port"]}";
                    options.Audience = Configuration["IdentityServerSettings:Audience"];
                    options.RequireHttpsMetadata = false;
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                {
                    policy.RequireRole("Admin");
                    policy.RequireClaim("scope", "customer.fullaccess" /* other claims like: , "catalog.read_access" */);
                });
            });

            services.AddMassTransit(options =>
            {
                options.AddConsumer<AddCustomerConsumer>();
                options.AddConsumer<UpdateCustomerConsumer>();
                options.AddConsumer<RemoveCustomerConsumer>();

                options.UsingRabbitMq((context, configuration) =>
                {
                    configuration.Host(Configuration["EventBusSettings:HostAddress"]);
                    configuration.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();

            services.AddControllers();

            services.AddSwaggerGen(c =>
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "customer.read", Version = "v1" }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "customer.read v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
