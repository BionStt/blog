﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using Blog.BusinessLogic.Managers;
using Blog.Core.Contracts.Managers;
using Blog.Data.Contracts.Repositories;
using Blog.Data.EntityFramework.Context;
using Blog.Data.EntityFramework.Repository;
using Blog.Website.Core.ConfigurationOptions;
using Blog.Website.Core.Contracts;
using Blog.Website.Core.Models;
using Blog.Website.Core.ViewModels.Author.ViewComponents;
using Blog.Website.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.WebEncoders;

namespace Blog.Website
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration,
                       IHostingEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BlogContext>(options => { options.UseSqlServer(Configuration["connection-strings:blog"]); });

            services.Configure<WebEncoderOptions>(options => options.TextEncoderSettings = new TextEncoderSettings(UnicodeRanges.All));
            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddAuthentication(options =>
                     {
                         options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                         options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                     })
                    .AddGoogle(options =>
                     {
                         options.ClientId = Configuration["logins:google:client-id"];
                         options.ClientSecret = Configuration["logins:google:client-secret"];
                     })
                    .AddTwitter(options =>
                     {
                         options.ConsumerKey = Configuration["logins:twitter:client-id"];
                         options.ConsumerSecret = Configuration["logins:twitter:client-secret"];

                     })
                    .AddMicrosoftAccount(options =>
                     {
                         options.ClientId = Configuration["logins:microsoft:client-id"];
                         options.ClientSecret = Configuration["logins:microsoft:client-secret"];
                     })
                    .AddCookie(options =>
                     {
                         options.LoginPath = "/account/login";
                         options.LogoutPath = "/account/logoff";
                     });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            
            services.AddMvc(options =>
            {
                options.MaxModelValidationErrors = 5;
                if(Environment.IsDevelopment())
                {
                    options.Filters.Add(typeof(GlobalException));
                }
            });

            services.AddTransient<IBlogStoryManager>(provider => new BlogStoryManager(provider.GetService<IBlogStoryRepository>(),
                                                                                      provider.GetService<ITagManager>()));
            services.AddTransient<ITagManager, TagManager>();
            services.AddTransient<IBlogStoryRepository, BlogStoryRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IBlogStoryTagRepository, BlogStoryTagRepository>();
            services.AddSingleton<IMainMenuContainer>(provider => new MainMenuContainer(Configuration.GetSection("menus:main-nav-menu")));
            services.AddSingleton<IStoryEditMenuContainer>(provider =>
                                                               new StoryEditMenuContainer(Configuration.GetSection("menus:story-edit-top-menu")));

            var pageInfoSec = Configuration.GetSection("DefaultPageInfo");
            services.Configure<DefaultPageInfoOption>(pageInfoSec);

            var storyImgSec = Configuration.GetSection("DefaultPageInfo:StoryImage");
            services.Configure<StoryImageOption>(storyImgSec);
            
            
            
            var loginRestriction = new List<LoginRestriction>();
            var section = Configuration.GetSection("logins-restrictions");
            section.Bind(loginRestriction);

            services.AddSingleton(loginRestriction);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
                              IHostingEnvironment env)
        {
            var forwardingOptions = new ForwardedHeadersOptions()
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
                KnownProxies = { IPAddress.Parse("127.0.0.1") }
            };
            
            app.UseForwardedHeaders(forwardingOptions);
            
            var items = new List<MenuItemData>();
            Configuration.GetSection("MainMenu")
                         .Bind(items);

            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                var context = app.ApplicationServices.GetService<BlogContext>();
                context?.Database.Migrate();
                
                app.UseStatusCodePagesWithReExecute("/StatusCode/{0}");
            }

            app.UseStaticFiles();
            
            app.UseAuthentication();
            
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                                name: "areaDefault",
                                template: "{area:exists}/{controller=Author}/{action=Index}");

                routes.MapRoute(
                                name: "default",
                                template: "{controller=BlogStory}/{action=Index}/{id?}");
            });
            
        }
    }
}