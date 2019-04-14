using System;
using System.Text;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Website.ActionResults;
using Blog.Website.Models.Requests.Reader;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Website.Controllers
{
    public class RobotsController : BaseController
    {
        private const Int32 PageSize = 50;
        private readonly IBlogStoryManager _blogStoryManager;

        public RobotsController(IBlogStoryManager blogStoryManager)
        {
            _blogStoryManager = blogStoryManager;
        }

        [HttpGet("sitemap.xml")]
        public async Task<IActionResult> SiteMap()
        {
            var siteMap = await _blogStoryManager.GetSiteMapXmlAsync($"{Request.Scheme}://{Request.Host.ToString()}", Cancel);
            return Content(siteMap, "text/xml", Encoding.UTF8);
        }

        [HttpGet("feed/rss")]
        public async Task<IActionResult> GetRssFeed([FromQuery] Int32 page = 1)
        {
            var storiesPage = await _blogStoryManager.GetPageWithTagsAsync(GetStoriesRequest.ToPublishedQuery(page, PageSize), Cancel);
            return new RssStoriesFeedResult(storiesPage);
        }
        
        [HttpGet("feed/atom")]
        public async Task<IActionResult> GetAtomFeed([FromQuery] Int32 page = 1)
        {
            var storiesPage = await _blogStoryManager.GetPageWithTagsAsync(GetStoriesRequest.ToPublishedQuery(page, PageSize), Cancel);
            return new AtomStoriesResult(storiesPage);
        }
    }
}