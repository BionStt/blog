using System;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Core.Enums.Filtering;
using Blog.Core.Enums.Sorting;
using Blog.Core.Exceptions;
using Blog.Website.Core.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Blog.Website.Controllers
{
    [Route("tags")]
    public class TagsController : BaseController
    {
        private readonly IBlogStoryManager _blogStoryManager;
        private readonly ITagManager _tagManager;

        private readonly String _defaultTitle;
        private readonly String _defaultDescription;
        private readonly String _defaultKeywords;
        private readonly String _defaultPageTagTitle;
        private readonly String _defaultPageNumberTitle;

        public TagsController(ITagManager tagManager, IBlogStoryManager blogStoryManager, IConfiguration configuration)
            : base(configuration)
        {
            _tagManager = tagManager;
            _blogStoryManager = blogStoryManager;

            _defaultTitle = configuration.GetValue<String>("default-page-title");
            _defaultDescription = configuration.GetValue<String>("default-page-description");
            _defaultKeywords = configuration.GetValue<String>("default-page-keywords");
            _defaultPageNumberTitle = configuration.GetValue<String>("default-page-number-title");
        }

        [HttpGet("{alias}")]
        public async Task<IActionResult> Tags([FromRoute] String alias, [FromQuery] Int32 page = 1)
        {
            try
            {
                var skip = GetSkip(page, PageSize);
                var tagAndStories = await _blogStoryManager.GetTagStoriesByAliasAsync(alias, skip, PageSize, StorySort.PublishDate,
                                                                                      StoryFilter.Published, Cancel);
                var storiesTotalCount = await _blogStoryManager.CountStoriesForTagAsync(tagAndStories.Item1.Id, Cancel);

                var tag = tagAndStories.Item1;
                var tags = await _tagManager.GetAllOrderedByUseAsync(Cancel);

                var viewModel = new MainPageViewModel(tagAndStories.Item2,
                                                      tags,
                                                      page,
                                                      PageSize,
                                                      storiesTotalCount,
                                                      tag.SeoTitle,
                                                      _defaultPageNumberTitle,
                                                      tag.SeoDescription,
                                                      tag.SeoKeywords);
                ViewBag.NoFollowForTags = true;
                return View("~/Views/BlogStory/IndexPub.cshtml", viewModel);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}