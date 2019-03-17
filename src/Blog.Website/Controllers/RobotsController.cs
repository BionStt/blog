using System.Text;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Website.Core.ConfigurationOptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Blog.Website.Controllers
{
    public class RobotsController : BaseController
    {
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
    }
}