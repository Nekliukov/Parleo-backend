using System.Globalization;
﻿using System.IO;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Parleo.BLL;
using Parleo.BLL.Extensions;
using Parleo.DAL;
using ParleoBackend.Configuration;
using ParleoBackend.Contracts;
using ParleoBackend.Extensions;
using ParleoBackend.Validators;
using ParleoBackend.Validators.User;
using ParleoBackend.ViewModels.Entities;

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
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                    });
            });

            ConfigureMapperFactory(services);

            BLServices.AddServices(services);
            DalServices.AddServices(services, Configuration.GetConnectionString("DefaultConnection"));
            WebServices.AddServices(services);

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddTransient<IValidator<CreateOrUpdateEventViewModel>, CrateOrUpdateEventViewModelValidator>();
            services.AddTransient<IValidator<UserRegistrationViewModel>, UserRegistrationViewModelValidator>();
            services.AddTransient<IValidator<UserLoginViewModel>, UserLoginViewModelValidator>();
            services.AddTransient<IValidator<UpdateUserViewModel>, UpdateUserViewModelValidator>();
            ValidatorOptions.LanguageManager.Culture = new CultureInfo("en-GB");
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

            IAccountImageSettings imageSettings = new AccountImageSettings(Configuration);
            
            app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(
                        Path.GetFullPath(imageSettings.DestPath)
                    ),
                    RequestPath = imageSettings.SourceUrl
                }
            );
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseMvc();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger XML Api Demo v1");
            });

            loggerFactory.AddConsole();
        }

        private void ConfigureMapperFactory(IServiceCollection services)
        {
            var mapperFactory = new MapperFactory();

            mapperFactory.Mappers.Add(typeof(BLServices).Name, BLServices.GetMapper());
            mapperFactory.Mappers.Add(typeof(WebServices).Name, WebServices.GetMapper());

            services.AddSingleton<IMapperFactory>(mapperFactory);
        }
    }
}
