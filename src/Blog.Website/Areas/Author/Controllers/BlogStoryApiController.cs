using System;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Website.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Website.Areas.Author.Controllers
{
    [Authorize]
    [Area("author"), Route("api/v{version:int}/author/stories")]
    public class BlogStoryApiController : BaseController
    {
        private readonly IBlogStoryManager _blogStoryManager;

        public BlogStoryApiController(IBlogStoryManager blogStoryManager)
        {
            _blogStoryManager = blogStoryManager;
        }

        [HttpDelete("{storyId:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid storyId)
        {
            await _blogStoryManager.DeleteAsync(storyId, Cancel);
            return Ok();
        }

        [HttpPatch("{storyId:guid}")]
        public async Task<IActionResult> ChangeAvailability([FromRoute] Guid storyId,
                                                            Boolean isPublished = false)
        {
            var story = await _blogStoryManager.ChangeAvailabilityAsync(storyId, isPublished, Cancel);
            var redirect = isPublished
                ? Url.Action("Story", "BlogStory", new {alias = story.Alias})
                : String.Empty;

            return Ok(new {redirect = redirect});
        }

        [HttpPut("{storyId:guid}/accesstoken")]
        public async Task<IActionResult> UpdateAccessToken([FromRoute] Guid storyId)
        {
            var story = await _blogStoryManager.UpdateAccessTokenAsync(storyId, Cancel);
            return Ok();
        }

        [HttpDelete("{storyId:guid}/accesstoken")]
        public async Task<IActionResult> RemoveAccessToken([FromRoute] Guid storyId)
        {
            await _blogStoryManager.RemoveAccessTokenAsync(storyId, Cancel);
            return Ok();
        }
    }
}