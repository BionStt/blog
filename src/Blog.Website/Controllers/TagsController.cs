using System;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Core.Enums.Filtering;
using Blog.Core.Enums.Sorting;
using Blog.Core.Exceptions;
using Blog.Website.Core.ConfigurationOptions;
using Blog.Website.Core.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Blog.Website.Controllers
{
    [Route("tags")]
    public class TagsController : BaseController
    {
        private readonly IBlogStoryManager _blogStoryManager;
        private readonly ITagManager _tagManager;

        public TagsController(ITagManager tagManager,
                              IBlogStoryManager blogStoryManager,
                              IOptions<DefaultPageInfoOption> pageInfo)
            : base(pageInfo)
        {
            _tagManager = tagManager;
            _blogStoryManager = blogStoryManager;
        }

        [HttpGet("{alias}")]
        public async Task<IActionResult> Tags([FromRoute] String alias,
                                              [FromQuery] Int32 page = 1)
        {
            var skip = GetSkip(page, PageSize);
            var tagAndStories = await _blogStoryManager.GetTagStoriesByAliasAsync(alias,
                                                                                  skip,
                                                                                  PageSize,
                                                                                  StorySort.PublishDate,
                                                                                  StoryFilter.Published,
                                                                                  Cancel);

            var tag = tagAndStories.Item1;
            var tags = await _tagManager.GetAllOrderedByUseAsync(Cancel);

            var viewModel = new MainPageViewModel(tagAndStories.Item2,
                                                  tags,
                                                  page,
                                                  PageSize,
                                                  1);

            ViewBag.Title = tag.SeoTitle;
            ViewBag.SeoDescription = tag.SeoDescription;
            ViewBag.Keywords = tag.SeoKeywords;
            ViewBag.NoFollowForTags = true;
            return View("~/Views/BlogStory/IndexPub.cshtml", viewModel);
        }
    }
}