﻿using System.Threading.Tasks;
using Blog.Website.Core.Contracts;
using Blog.Website.Core.ViewModels.Author.ViewComponents;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Website.ViewComponents.MainMenu
{
    public class MainMenuViewComponent : ViewComponent
    {
        private readonly IMainMenuContainer _container;

        public MainMenuViewComponent(IMainMenuContainer container)
        {
            _container = container;
        }

        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult<IViewComponentResult>(View(new MenuViewModel(_container.Items, Request.Path.Value)));
        }
    }
}