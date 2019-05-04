using System.Globalization;
﻿using System.IO;
using System.Text;
using FluentScheduler;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Parleo.BLL;
using Parleo.BLL.Extensions;
using Parleo.DAL;
using ParleoBackend.Configuration;
using ParleoBackend.Contracts;
using ParleoBackend.Extensions;
using ParleoBackend.Hubs;
using ParleoBackend.Services;
using ParleoBackend.Validators.Event;
using ParleoBackend.Validators.Common;
using ParleoBackend.Validators.User;
using ParleoBackend.ViewModels.Entities;
using ParleoBackend.ViewModels.Pages;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.ResponseCompression;
using System.Linq;
using System.IO.Compression;

namespace ParleoBackend
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
            services.AddSignalR().AddAzureSignalR(Configuration.GetConnectionString("SignalRConnection"));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddSwaggerDocumentation();

            IJwtSettings jwtSettings = new JwtSettings(Configuration);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.JWTKey)
                        ),
                        ValidateIssuerSigningKey = true,
                    };
                });

            // TODO: refactor this and specify origins
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetIsOriginAllowed((host) => true)
                            .AllowCredentials();
                    });
            });

            ConfigureMapperFactory(services);
            BLServices.AddServices(services);
            DalServices.AddServices(services, Configuration.GetConnectionString("DefaultConnection"));
            WebServices.AddServices(services);

            var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireClaim("IsAuthorization", "true")
                .Build();

            services
                .AddMvc(options =>
                {
                    options.Filters.Add(new AuthorizeFilter(policy));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = new[] {"image/jpg", "image/jpeg", "image/png","image/svg+xml"};
                options.EnableForHttps = true;
            });
            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerDocumentation();
            }
            else
            {
                app.UseHsts();
            }

            app.UseResponseCompression();

            IImageSettings imageSettings = new ImageSettings(Configuration);
            System.IO.Directory.CreateDirectory(imageSettings.EventDestPath);
            System.IO.Directory.CreateDirectory(imageSettings.AccountDestPath);
            app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.GetFullPath(imageSettings.AccountDestPath)
                    ),
                    RequestPath = imageSettings.AccountSourceUrl
                }
            );
            app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.GetFullPath(imageSettings.EventDestPath)
                    ),
                    RequestPath = imageSettings.EventSourceUrl
                }
            );
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.ConfigureExceptionHandler();
            app.UseAzureSignalR(route =>
            {
                route.MapHub<ChatHub>("/chathub");
            });
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Swagger XML Api Demo v1.0");
            });

            loggerFactory.AddConsole();
            JobManager.Initialize(new BackgroundWorkerRegistry(app.ApplicationServices));
        }

        private void ConfigureMapperFactory(IServiceCollection services)
        {
            var mapperFactory = new MapperFactory();

            mapperFactory.Mappers.Add(typeof(BLServices).Name, BLServices.GetMapper());
            mapperFactory.Mappers.Add(typeof(WebServices).Name, WebServices.GetMapper(Configuration));

            services.AddSingleton<IMapperFactory>(mapperFactory);
        }
    }
}
