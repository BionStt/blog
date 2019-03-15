using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Core.Exceptions;
using Blog.Extensions.Helpers;
using Blog.Website.Controllers;
using Blog.Website.Core.ConfigurationOptions;
using Blog.Website.Core.Helpers;
using Blog.Website.Core.ViewModels.Author.BlogStories;
using Blog.Website.Models.Requests.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Blog.Website.Areas.Author.Controllers
{
    [Authorize]
    [Area("author"), Route("author/stories")]
    public class BlogStoryController : BaseReaderController
    {
        private readonly IBlogStoryManager _blogStoryManager;
        private readonly ITagManager _tagManager;
        private readonly ILogger _logger;

        private IOptions<StoryImageOption> _defaultStoryImage;

        public BlogStoryController(IBlogStoryManager blogStoryManager,
                                   ITagManager tagManager,
                                   IOptions<DefaultPageInfoOption> pageInfo,
                                   IOptions<StoryImageOption> defaultStoryImage,
                                   ILoggerFactory loggerFactory) : base(pageInfo)
        {
            _blogStoryManager = blogStoryManager;
            _tagManager = tagManager;
            _logger = loggerFactory.GetLogger();
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
        public async Task<IActionResult> Edit(Guid storyId)
        {
            var tags = await _tagManager.GetTopAsync(Cancel);
            var story = await _blogStoryManager.GetWithTagsAsync(storyId, Cancel);
            var viewModel = new EditBlogStoryViewModel(story, tags, Url);
            return View(viewModel);
        }

        [HttpPost("edit", Name = "edit-story"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditBlogStoryViewModel model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return View(model);
                }

                model.SetImageUrlIfNotExist(_defaultStoryImage.Value.Url, _defaultStoryImage.Value.Width);
                var blogStory = await _blogStoryManager.CreateOrUpdateAsync(model.ToDomain(), Cancel);

                var tagIds = model.TagsSelected?.GetGuids(',').ToList();
                await _tagManager.UpdateBlogStoryTagsAsync(tagIds, blogStory, Cancel);

                return RedirectToAction("Edit", new {storyId = blogStory.Id});
            }
            catch (ArgumentException exception)
            {
                _logger.Error(exception);
                return BadRequest();
            }
            catch (EntityNotFoundException exception)
            {
                _logger.Error(exception);
                return NotFound();
            }
        }

        [HttpDelete("{alias}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(String alias)
        {
            await _blogStoryManager.DeleteAsync(alias, Cancel);
            return Ok();
        }

        [HttpPatch("{storyId}")]
        public async Task<IActionResult> ChangeAvailability(Guid storyId,
                                                            Boolean isPublished = false)
        {
            var story = await _blogStoryManager.ChangeAvailabilityAsync(storyId, isPublished, Cancel);
            var redirect = isPublished
                ? Url.Action("Story", "BlogStory", new {alias = story.Alias})
                : String.Empty;

            return Ok(new {redirect = redirect});
        }

        [HttpPost("{storyId}/accesstoken")]
        public async Task<IActionResult> UpdateAccessToken(Guid storyId)
        {
            var story = await _blogStoryManager.UpdateAccessTokenAsync(storyId, Cancel);
            return Ok();
        }

        [HttpDelete("{storyId}/accesstoken")]
        public async Task<IActionResult> RemoveAccessToken(Guid storyId)
        {
            await _blogStoryManager.RemoveAccessTokenAsync(storyId, Cancel);
            return Ok();
        }
    }
}