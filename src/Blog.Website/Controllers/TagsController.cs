using System;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Website.Core.ConfigurationOptions;
using Blog.Website.Core.ViewModels.User;
using Blog.Website.Models.Requests.Reader;
using Microsoft.AspNetCore.Mvc;
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
            var tag = await _tagManager.GetAsync(alias, Cancel);
            if(tag == null)
            {
                return NotFound();
            }

            var storiesByTag = await _blogStoryManager.GetPageWithTagsAsync(GetStoriesRequest.ToQuery(tag.Id, page, PageSize));

            var topTags = await _tagManager.GetTopAsync(Cancel);

            ViewBag.Title = tag.SeoTitle;
            ViewBag.SeoDescription = tag.SeoDescription;
            ViewBag.Keywords = tag.SeoKeywords;
            ViewBag.NoFollowForTags = true;

            return View("~/Views/BlogStory/IndexPub.cshtml",
                        new MainPageViewModel(storiesByTag,
                                              topTags,
                                              page));
        }
    }
}