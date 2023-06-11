using System;
using System.IO;
using System.Text;
using System.Reflection;
using wsIncidents.Helpers;
using AspNetCoreRateLimit;
using wsIncidents.Middleware;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace wsIncidents {

    public class Startup {

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment env { get; }

        private string version = "";

        public Startup(IConfiguration configuration,IWebHostEnvironment environment) {
            Configuration = configuration;
            env = environment;

            //Global variables
            globals.path = environment.ContentRootPath;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {

            var builder = WebApplication.CreateBuilder();

            //services.AddMemoryCache();

            //services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimitingSettings"));
            //services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            //services.AddInMemoryRateLimiting();

            /*Middleware*/
            services.addRateLimiting(Configuration);

            services.AddControllers();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(
                    options => {
                        options.TokenValidationParameters = new TokenValidationParameters() {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = false,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = builder.Configuration["JwtConfig:Issuer"],
                            ValidAudience = builder.Configuration["JwtConfig:Audience"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtConfig:Key"])),
                        };
                    }
            );

            //Api Version
            services.AddApiVersioning(options => {
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1,0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            //Swagger Configuration
            services.AddSwaggerGen(options => {

                version = builder.Configuration["ApiVersion:Version"];

                options.SwaggerDoc(version,new OpenApiInfo {
                    Title = $"{builder.Configuration["ApiVersion:Title"]} {version}",
                    Version = version,
                    Description = $"Description: {builder.Configuration["ApiVersion:Title"]} {version}",
                    Contact = new OpenApiContact {
                        Name = builder.Configuration["ApiVersion:ContactName"],
                        Email = builder.Configuration["ApiVersion:ContactEmail"],
                        Url = new Uri(builder.Configuration["ApiVersion:ContactUrl"])
                    },
                });

                string xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                string path = Path.Combine(AppContext.BaseDirectory,xmlFile);
                options.IncludeXmlComments(path);

                options.EnableAnnotations();

                options.AddSecurityDefinition("Bearer",new OpenApiSecurityScheme() {
                    Description = "Standard Authorization header using the Bearer scheme (JWT)",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type= SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });

                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            });


            services.AddSingleton<IRateLimitConfiguration,RateLimitConfiguration>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            /*Middleware*/
            app.UseRateLimiting();
            //app.UseIpRateLimiting();

            var builder = WebApplication.CreateBuilder();

            app.UseHttpsRedirection();

            version = builder.Configuration["ApiVersion:Version"];

            app.UseSwagger(options => {
                options.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(options => {
                options.SwaggerEndpoint($"/swagger/{version}/swagger.json",$"{builder.Configuration["ApiVersion:Title"]} {builder.Configuration["ApiVersion:Version"]}");
                options.DocumentTitle = $"{builder.Configuration["ApiVersion:Title"]} {builder.Configuration["ApiVersion:Version"]}";
                options.EnableFilter();                
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });

        }
    }

}
