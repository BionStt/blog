using System;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Core.Enums.Filtering;
using Blog.Core.Enums.Sorting;
using Blog.Website.Core.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Blog.Website.Controllers
{
    public class BlogStoryController : BaseController
    {
        private readonly IBlogStoryManager _blogStoryManager;
        private readonly ITagManager _tagManager;

        private readonly String _defaultTitle;
        private readonly String _defaultDescription;
        private readonly String _defaultKeywords;
        private readonly String _defaultPageNumberTitle;

        public BlogStoryController(IBlogStoryManager blogStoryManager,
                                   ITagManager tagManager,
                                   IConfiguration configuration) : base(configuration)
        {
            _blogStoryManager = blogStoryManager;
            _tagManager = tagManager;

            _defaultTitle = configuration.GetValue<String>("default-page-title");
            _defaultDescription = configuration.GetValue<String>("default-page-description");
            _defaultKeywords = configuration.GetValue<String>("default-page-keywords");
            _defaultPageNumberTitle = configuration.GetValue<String>("default-page-number-title");
        }

        [HttpGet("")]
        public async Task<IActionResult> Index(Int32 page = 1)
        {
            var skip = GetSkip(page, PageSize);

            var stories = await _blogStoryManager.GetAsync(skip, PageSize, StorySort.PublishDate, StoryFilter.Published, Cancel);
            var storiesTotalCount = await _blogStoryManager.CountPublishedAsync(Cancel);

            var tags = await _tagManager.GetAllOrderedByUseAsync(Cancel);
            var viewModel = new MainPageViewModel(stories, tags, page, PageSize, storiesTotalCount, _defaultTitle, _defaultPageNumberTitle,
                                                  _defaultDescription, _defaultKeywords);

            return View("IndexPub", viewModel);
        }

        [HttpGet("{alias}")]
        public async Task<IActionResult> Story(String alias)
        {
            var story = await _blogStoryManager.GetWithTagsAsync(alias, Cancel);
            if (story == null ||
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
            if (String.IsNullOrWhiteSpace(alias))
                return NotFound();

            var story = await _blogStoryManager.GetAsync(alias, Cancel);
            if (story == null)
                return NotFound();

            if (story.PublishedDate.HasValue)
                return NotFound();

            if (!User.Identity.IsAuthenticated && (String.IsNullOrWhiteSpace(story.AccessToken) ||
                                                   !story.AccessToken.Equals(token, StringComparison.Ordinal)))
                return NotFound();

            var tags = await _tagManager.GetAllOrderedByUseAsync(Cancel);
            var viewModel = new FullStoryViewModel(story, tags);
            return View("Story", viewModel);
        }
    }
}