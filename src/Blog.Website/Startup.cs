﻿using System;
using System.Collections.Generic;
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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.WebEncoders;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
                         options.CallbackPath = new PathString("/account/signin-google");
                     })
                    .AddTwitter(options =>
                     {
                         options.ConsumerKey = Configuration["logins:twitter:client-id"];
                         options.ConsumerSecret = Configuration["logins:twitter:client-secret"];
                         options.CallbackPath = new PathString("/account/signin-twitter");
                     })
                    .AddMicrosoftAccount(options =>
                     {
                         options.ClientId = Configuration["logins:microsoft:client-id"];
                         options.ClientSecret = Configuration["logins:microsoft:client-secret"];
                         options.CallbackPath = new PathString("/account/signin-microsoft");
                     })
                    .AddCookie(options =>
                     {
                         options.LoginPath = "/account/login";
                         options.LogoutPath = "/account/logoff";
                     });

            services.Configure<ForwardedHeadersOptions>(options => { options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto; });

            services.AddMvc(options =>
            {
                options.MaxModelValidationErrors = 5;
                options.Filters.Add(typeof(GlobalException));
            }).AddJsonOptions(options =>
            {
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DateFormatString = "dd.MM.yyyy HH:mm:ss zzz";
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            });

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            services.AddTransient<IBlogStoryManager>(provider => new BlogStoryManager(provider.GetService<IBlogStoryRepository>(),
                                                                                      provider.GetService<ITagManager>()));
            services.AddTransient<IBlogStoryRepository, BlogStoryRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IBlogStoryTagRepository, BlogStoryTagRepository>();
            
            services.AddTransient<ITagManager>(provider =>
            {
                var tagRepository = provider.GetService<ITagRepository>();
                var memoryCache = provider.GetService<IMemoryCache>();
                var cacheRepository = new Blog.Data.EntityFramework.MemoryCache.Repositories.TagRepository(tagRepository, memoryCache);

                var blogStoriesRepository = provider.GetService<IBlogStoryRepository>();
                var storyTagRepositories = provider.GetService<IBlogStoryTagRepository>();
                
                return new TagManager(cacheRepository, blogStoriesRepository, storyTagRepositories);
            });

            services.AddSingleton<IMainMenuContainer, MainMenuContainer>();
            services.AddSingleton<IStoryEditMenuContainer, StoryEditMenuContainer>();

            var pageInfoSec = Configuration.GetSection("DefaultPageInfo");
            services.Configure<DefaultPageInfoOptions>(pageInfoSec);

            var storyImgSec = Configuration.GetSection("DefaultPageInfo:StoryImage");
            services.Configure<StoryImageOptions>(storyImgSec);

            var menus = Configuration.GetSection("Menus");
            services.Configure<MainMenuOptions>(menus);
            services.Configure<StoryEditMenuOptions>(menus);

            var feedSection = Configuration.GetSection("Feed");
            services.Configure<FeedOptions>(feedSection);

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
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            forwardingOptions.KnownNetworks.Clear();
            forwardingOptions.KnownProxies.Clear();

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