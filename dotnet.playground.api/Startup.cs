using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hellang.Middleware.ProblemDetails;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.OpenApi.Models;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace dotnet.playground.api
{
#pragma warning disable CS1591
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
            services.AddHealthChecks();

            // Hellang.Middleware.ProblemDetails package
            services.AddProblemDetails();

            // Registering Swagger Generator
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Blazor Test Service API",
                    Version = "v1",
                    Description = "Blazor Test Service api",
                    Contact = new OpenApiContact() { Name = "Maurizio Attanasi", Email = "maurizio.attanasi@gmail.com", Url = new Uri("http://maurizioattanasi.it/") }
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme{
                            Reference = new OpenApiReference{
                                Type = ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[] {}
                    }
                });

                //Determine base path for the application.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                c.IncludeXmlComments(basePath + @"/dotnet.playground.api.xml");

                c.CustomSchemaIds(i => i.FullName);
            });

            // MediatR
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks("/health");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                // specifying the Swagger JSON endpoint.
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blazor Test Service API");

                c.DocumentTitle = "Dotnet Playground Service api";
            });

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseProblemDetails();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
            });

            // using Microsoft.AspNetCore.HttpOverrides;

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseAuthentication();
        }
    }
#pragma warning restore CS1591
}
