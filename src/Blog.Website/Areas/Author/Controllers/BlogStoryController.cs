using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Core.Exceptions;
using Blog.Extensions.Helpers;
using Blog.Website.Controllers;
using Blog.Website.Core.Helpers;
using Blog.Website.Core.ViewModels.Author.BlogStories;
using Blog.Website.Models.Requests.Author;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Blog.Website.Areas.Author.Controllers
{
    [Authorize]
    [Area("author"), Route("author/stories")]
    public class BlogStoryController : BaseController
    {
        private readonly String _defaultStoryImageUrl;
        private readonly Int32 _defaultThumbMaxWidth;

        private readonly IBlogStoryManager _blogStoryManager;
        private readonly ITagManager _tagManager;
        private readonly ILogger _logger;

        public BlogStoryController(IBlogStoryManager blogStoryManager,
                                   ITagManager tagManager,
                                   IConfiguration configuration,
                                   ILoggerFactory loggerFactory) : base(configuration)
        {
            _blogStoryManager = blogStoryManager;
            _tagManager = tagManager;
            _logger = loggerFactory.GetLogger();
            _defaultStoryImageUrl = configuration.GetValue<String>("default-image-url-for-post");
            _defaultThumbMaxWidth = configuration.GetValue<Int32>("default-thumb-max-width");
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
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                model.SetImageUrlIfNotExist(_defaultStoryImageUrl, _defaultThumbMaxWidth);
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
            try
            {
                await _blogStoryManager.DeleteAsync(alias, Cancel);
                return Ok();
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

        [HttpPatch("{storyId}")]
        public async Task<IActionResult> ChangeAvailability(Guid storyId, Boolean isPublished = false)
        {
            try
            {
                var story = await _blogStoryManager.ChangeAvailabilityAsync(storyId, isPublished, Cancel);
                var redirect = isPublished 
                                   ? Url.Action("Story", "BlogStory", new {alias = story.Alias}) 
                                   : String.Empty;

                return Ok(new {redirect = redirect});
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

        [HttpPost("{storyId}/accesstoken")]
        public async Task<IActionResult> UpdateAccessToken(Guid storyId)
        {
            try
            {
                var story = await _blogStoryManager.UpdateAccessTokenAsync(storyId, Cancel);
                return Ok();
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
        
        [HttpDelete("{storyId}/accesstoken")]
        public async Task<IActionResult> RemoveAccessToken(Guid storyId)
        {
            try
            {
                await _blogStoryManager.RemoveAccessTokenAsync(storyId, Cancel);
                return Ok();
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
    }
}