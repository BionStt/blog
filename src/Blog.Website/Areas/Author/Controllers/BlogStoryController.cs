using System;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Core.Queries;
using Blog.Website.Controllers;
using Blog.Website.Core.ConfigurationOptions;
using Blog.Website.Core.ViewModels.Author.BlogStories;
using Blog.Website.Models.Requests.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Blog.Website.Areas.Author.Controllers
{
    [Authorize]
    [Area("author"), Route("author/stories")]
    public class BlogStoryController : BaseReaderController
    {
        private readonly IBlogStoryManager _blogStoryManager;
        private readonly ITagManager _tagManager;

        public BlogStoryController(IBlogStoryManager blogStoryManager,
                                   ITagManager tagManager,
                                   IOptions<DefaultPageInfoOption> pageInfo)
            : base(pageInfo)
        {
            _blogStoryManager = blogStoryManager;
            _tagManager = tagManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(GetStoriesRequest request)
        {
            var storiesPage = await _blogStoryManager.GetPageAsync(request.ToQuery(PageSize), Cancel);
            var viewModel = new AuthorStoriesPageViewModel(storiesPage, request.Page, PageSize);
            return View(viewModel);
        }

        [HttpGet("edit/{storyId:guid?}")]
        public async Task<IActionResult> Edit(Guid storyId)
        {
            var story = await _blogStoryManager.GetWithTagsAsync(storyId, Cancel);
            var tagPage = await _tagManager.GetAsync(new TagsQuery(), Cancel);
            var viewModel = new EditBlogStoryViewModel(story, tagPage.Items, Url);
            return View(viewModel);
        }

        [HttpPost("edit/{storyId:guid?}", Name = "edit-story"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] Guid storyId,
                                              EditBlogStoryViewModel model,
                                              [FromServices] IOptions<StoryImageOption> defaultStoryImage)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            model.SetImageUrlIfNotExist(defaultStoryImage.Value.Url, defaultStoryImage.Value.Width);

            var blogStory = await _blogStoryManager.CreateOrUpdateAsync(model.ToDomain(), Cancel);
            return RedirectToAction("Edit", new {storyId = blogStory.Id});
        }
    }
}