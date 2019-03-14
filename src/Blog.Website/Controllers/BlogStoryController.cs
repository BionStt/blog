using System;
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

            return View("IndexPub", new MainPageViewModel(storiesPage,
                                                          topTags,
                                                          page));
        }

        [HttpGet("{alias}")]
        public async Task<IActionResult> Story(String alias)
        {
            var story = await _blogStoryManager.GetPublishedWithTagsAsync(alias, Cancel);
            if(story == null)
            {
                return NotFound();
            }

            var tags = await _tagManager.GetTopAsync(Cancel);
            
            return View(new FullStoryViewModel(story, tags));
        }

        [HttpGet("drafts/{alias}")]
        public async Task<IActionResult> Preview([FromRoute] String alias,
                                                 [FromQuery] String token)
        {
            var story = await _blogStoryManager.GetAsync(alias, Cancel);
            if(story == null)
            {
                return NotFound();
            }

            if(!User.Identity.IsAuthenticated &&
               (String.IsNullOrWhiteSpace(story.AccessToken) ||
                !story.AccessToken.Equals(token, StringComparison.Ordinal)))
                return NotFound();

            var tags = await _tagManager.GetTopAsync(Cancel);
            
            return View("Story", new FullStoryViewModel(story, tags));
        }
    }
}