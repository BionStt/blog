using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Extensions.Helpers;
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

        private readonly IOptions<StoryImageOption> _defaultStoryImage;

        public BlogStoryController(IBlogStoryManager blogStoryManager,
                                   ITagManager tagManager,
                                   IOptions<DefaultPageInfoOption> pageInfo,
                                   IOptions<StoryImageOption> defaultStoryImage)
            : base(pageInfo)
        {
            _blogStoryManager = blogStoryManager;
            _tagManager = tagManager;
            _defaultStoryImage = defaultStoryImage;
        }

        [HttpGet]
        public async Task<IActionResult> Index(GetStoriesRequest request)
        {
            var storiesPage = await _blogStoryManager.GetPageAsync(request.ToQuery(PageSize), Cancel);
            var viewModel = new AuthorStoriesPageViewModel(storiesPage, request.Page, PageSize);
            return View(viewModel);
        }

        [HttpGet("edit/{storyId:guid?}")]
        public async Task<IActionResult> Edit([FromRoute] Guid storyId)
        {
            var tags = await _tagManager.GetTopAsync(Cancel);
            var story = await _blogStoryManager.GetWithTagsAsync(storyId, Cancel);
            var viewModel = new EditBlogStoryViewModel(story, tags, Url);
            return View(viewModel);
        }

        [HttpPost("edit/{storyId:guid?}", Name = "edit-story"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] Guid storyId,
                                              EditBlogStoryViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            model.SetImageUrlIfNotExist(_defaultStoryImage.Value.Url, _defaultStoryImage.Value.Width);
            var blogStory = await _blogStoryManager.CreateOrUpdateAsync(model.ToDomain(), Cancel);

            var tagIds = model.TagsSelected?.GetGuids(',')
                              .ToList();
            await _tagManager.UpdateBlogStoryTagsAsync(tagIds, blogStory, Cancel);

            return RedirectToAction("Edit", new {storyId = blogStory.Id});
        }
    }
}