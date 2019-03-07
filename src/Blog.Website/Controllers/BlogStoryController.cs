﻿using System;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Website.Core.ConfigurationOptions;
using Blog.Website.Core.ViewModels.User;
using Blog.Website.Filters;
using Blog.Website.Models.Requests.Reader;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Blog.Website.Controllers
{
    public class BlogStoryController : BaseController
    {
        private readonly IBlogStoryManager _blogStoryManager;
        private readonly ITagManager _tagManager;

        public BlogStoryController(IBlogStoryManager blogStoryManager,
                                   ITagManager tagManager,
                                   IOptions<DefaultPageInfoOption> pageInfo) : base(pageInfo)
        {
            _blogStoryManager = blogStoryManager;
            _tagManager = tagManager;
        }

        [HttpGet(""), DefaultSeoContent]
        public async Task<IActionResult> Index([FromQuery] Int32 page = 1)
        {
            var storiesPage = await _blogStoryManager.GetPageWithTagsAsync(GetStoriesRequest.ToQuery(page, PageSize), Cancel);
            var topTags = await _tagManager.GetTopAsync(Cancel);

            var viewModel = new MainPageViewModel(storiesPage.Items,
                                                  topTags,
                                                  page,
                                                  PageSize,
                                                  storiesPage.TotalCount);

            return View("IndexPub", viewModel);
        }

        [HttpGet("{alias}")]
        public async Task<IActionResult> Story(String alias)
        {
            var story = await _blogStoryManager.GetWithTagsAsync(alias, Cancel);
            if(story == null ||
               !story.PublishedDate.HasValue)
            {
                return NotFound();
            }

            var tags = await _tagManager.GetAllOrderedByUseAsync(Cancel);
            var viewModel = new FullStoryViewModel(story, tags);
            return View(viewModel);
        }

        [HttpGet("drafts/{alias}")]
        public async Task<IActionResult> Preview([FromRoute] String alias,
                                                 [FromQuery] String token)
        {
            if(String.IsNullOrWhiteSpace(alias))
                return NotFound();

            var story = await _blogStoryManager.GetAsync(alias, Cancel);
            if(story == null)
                return NotFound();

            if(story.PublishedDate.HasValue)
                return NotFound();

            if(!User.Identity.IsAuthenticated &&
               (String.IsNullOrWhiteSpace(story.AccessToken) ||
                !story.AccessToken.Equals(token, StringComparison.Ordinal)))
                return NotFound();

            var tags = await _tagManager.GetAllOrderedByUseAsync(Cancel);
            var viewModel = new FullStoryViewModel(story, tags);
            return View("Story", viewModel);
        }
    }
}