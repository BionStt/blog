﻿using System;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Website.Controllers;
using Blog.Website.Core.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Website.Areas.Author.Controllers
{
    [Authorize]
    [Area("author"), Route("api/v{version:int}/author/tags")]
    public class TagApiController : BaseController
    {
        private readonly ITagManager _tagManager;

        public TagApiController(ITagManager tagManager)
        {
            _tagManager = tagManager;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TagCreateRequest request)
        {
            var tag = await _tagManager.CreateTagAsync(request.Name, Cancel);
            return Ok(new {id = tag.Id, name = tag.Name});
        }
        
        [HttpPost("tagId:guid/stories/{storyId:guid}")]
        public async Task<IActionResult> AssignToBlogStory([FromRoute] Guid tagId, [FromRoute] Guid storyId)
        {
            await _tagManager.AssignTagToBlogStoryAsync(tagId, storyId, Cancel);
            return Ok();
        }

        [HttpDelete("tagId:guid/stories/{storyId:guid}")]
        public async Task<IActionResult> UnassignTagFromBlogStory([FromRoute] Guid tagId, [FromRoute] Guid storyId)
        {
            await _tagManager.UnassignTagFromBlogStoryAsync(tagId, storyId, Cancel);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _tagManager.DeleteAsync(id, Cancel);
            return Ok();
        }

        
    }
}