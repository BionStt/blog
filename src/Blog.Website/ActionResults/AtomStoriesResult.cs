using System.Threading.Tasks;
using Blog.Core.Containers;
using Blog.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Website.ActionResults
{
    public class AtomStoriesResult : IActionResult
    {
        private readonly Page<BlogStory> _page;

        public AtomStoriesResult(Page<BlogStory> page)
        {
            _page = page;
        }

        public async Task ExecuteResultAsync(ActionContext context)
        {
        }
    }
}