using System;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Core.Exceptions;
using Blog.Website.Core.Helpers;
using Blog.Website.Core.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Blog.Website.Areas.Author.Controllers
{
    [Authorize]
    [Area("author"), Route("api/{version:int}/author/tags")]
    public class TagApiController : Controller
    {
        private readonly Int32 _pageSize;

        private readonly ITagManager _tagManager;
        private readonly ILogger<TagApiController> _logger;

        public TagApiController(ITagManager tagManager,
                                ILogger<TagApiController> logger)
        {
            _tagManager = tagManager;
            _logger = logger;
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromRoute] Int32 version, [FromBody] TagCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var cancel = HttpContext.RequestAborted;
                var tag = await _tagManager.CreateTagAsync(request.Name, cancel);
                return Ok(new {id = tag.Id, name = tag.Name});
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

        [HttpPost("assign/story"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToBlogStory([FromBody] TagToBlogStoryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            try
            {
                var cancel = HttpContext.RequestAborted;
                var story = await _tagManager.AssignTagToBlogStoryAsync(request.TagId, request.BlogStoryId, cancel);
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

        [HttpPost("unassign/story"), ValidateAntiForgeryToken]
        public async Task<IActionResult> UnassignTagFromBlogStory([FromBody] TagToBlogStoryRequest request)
        {
            if (request == null)
                return BadRequest();

            try
            {
                var cancel = HttpContext.RequestAborted;
                var story = await _tagManager.UnassignTagFromBlogStoryAsync(request.TagId, request.BlogStoryId, cancel);
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

        [HttpDelete("{id}"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var cancel = HttpContext.RequestAborted;
                await _tagManager.DeleteAsync(id, cancel);
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