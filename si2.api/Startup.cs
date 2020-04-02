using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using si2.bll.Helpers;
using si2.bll.Services;
using si2.dal.Context;
using si2.dal.Entities;
using si2.dal.Repositories;
using si2.dal.UnitOfWork;
using System;
using System.Text;

namespace si2.api
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
            services.AddDbContextPool<Si2DbContext>((provider, options) =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Si2ConnectionString"));
                options.UseInternalServiceProvider(provider);
            });
            
            services.AddEntityFrameworkSqlServer();
           
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<Si2DbContext>();
            

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            //services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory => 
            //{
            //    var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
            //    return new UrlHelper(actionContext);
            //});

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IDataflowRepository, DataflowRepository>();
            services.AddTransient<ISimplecohortRepository, SimplecohortRepository>();

            services.AddTransient<IServiceBase, ServiceBase>();
            services.AddTransient<IDataflowService, DataflowService>();
            services.AddTransient<ISimplecohortService, SimplecohortService>();

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddAuthentication()
                .AddCookie(cfg => cfg.SlidingExpiration = true)
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = Configuration.GetValue<string>("Si2JwtBearerConstants:Issuer"),
                        ValidAudience = Configuration.GetValue<string>("Si2JwtBearerConstants:Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetValue<string>("Si2JwtBearerConstants:Key")))
                    };
                });

            services.AddControllers()
                .AddNewtonsoftJson(setupAction => setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "V1",
                    Title = "Université Saint-Joseph de Beyrouth - SI2 Server",
                    Description = "The university Web API for handling students registrations",
                    TermsOfService = new Uri("https://www.facebook.com/usj.edu.lb/videos/890474227787534/"),
                    Contact = new OpenApiContact
                    {
                        Name = "John Smith",
                        Email = "John.Smit@email.com",
                        Url = new Uri("https://twitter.com/usjliban?lang=en"),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use under USJ-LICX",
                        Url = new Uri("https://example.com/license"),
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

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
