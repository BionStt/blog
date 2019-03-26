using System;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Website.Controllers;
using Blog.Website.Core.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blog.Website.Areas.Author.Controllers
{
    [Authorize]
    [Area("author"), Route("api/{version:int}/author/tags")]
    public class TagApiController : BaseController
    {
        private readonly ITagManager _tagManager;

        public TagApiController(ITagManager tagManager)
        {
            _tagManager = tagManager;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromRoute] Int32 version,
                                                [FromBody] TagCreateRequest request)
        {
            var cancel = HttpContext.RequestAborted;
            var tag = await _tagManager.CreateTagAsync(request.Name, cancel);
            return Ok(new {id = tag.Id, name = tag.Name});
        }

        [HttpPost("assign/story"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToBlogStory([FromBody] TagToBlogStoryRequest request)
        {
            var cancel = HttpContext.RequestAborted;
            var story = await _tagManager.AssignTagToBlogStoryAsync(request.TagId, request.BlogStoryId, cancel);
            return Ok();
        }

        [HttpPost("unassign/story"), ValidateAntiForgeryToken]
        public async Task<IActionResult> UnassignTagFromBlogStory([FromBody] TagToBlogStoryRequest request)
        {
            var cancel = HttpContext.RequestAborted;
            var story = await _tagManager.UnassignTagFromBlogStoryAsync(request.TagId, request.BlogStoryId, cancel);
            return Ok();
        }

        [HttpDelete("{id:guid}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var cancel = HttpContext.RequestAborted;
            await _tagManager.DeleteAsync(id, cancel);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] TagCreateRequest model)
        {
            var tag = await _tagManager.CreateTagAsync(model.Name, Cancel);
            return Ok(tag);
        }
    }
}