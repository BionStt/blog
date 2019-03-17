﻿using System;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Website.Controllers;
using Blog.Website.Core.ConfigurationOptions;
using Blog.Website.Core.Requests;
using Blog.Website.Core.ViewModels.Author.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Blog.Website.Areas.Author.Controllers
{
    [Authorize]
    [Area("author"), Route("author/tags")]
    public class TagController : BaseReaderController
    {
        private readonly ITagManager _tagManager;

        public TagController(ITagManager tagManager,
                             IOptions<DefaultPageInfoOption> pageInfo) : base(pageInfo)
        {
            _tagManager = tagManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] Int32 page = 1)
        {
            var tags = await _tagManager.GetTopAsync(Cancel);
            var viewModel = new TagsViewModel(tags, 10, page, PageSize);
            return View(viewModel);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            var tag = await _tagManager.GetAsync(id, Cancel);
            if(tag == null)
            {
                return NotFound();
            }

            return View(new TagEditViewModel(tag));
        }

        [HttpPost(Name = "edit-tag")]
        public async Task<IActionResult> Edit(TagEditViewModel model)
        {
            var tag = await _tagManager.UpdateAsync(model.ToDomain(), Cancel);
            return View(new TagEditViewModel(tag));
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] TagCreateRequest model)
        {
            var tag = await _tagManager.CreateTagAsync(model.Name, Cancel);
            return Ok(tag);
        }
    }
}