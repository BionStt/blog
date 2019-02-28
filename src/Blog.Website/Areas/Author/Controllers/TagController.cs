﻿using System;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Website.Controllers;
using Blog.Website.Core.Requests;
using Blog.Website.Core.ViewModels.Author.Tag;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Blog.Website.Areas.Author.Controllers
{
    [Authorize]
    [Area("author"), Route("author/tags")]
    public class TagController : BaseController
    {
        private readonly Int32 _pageSize;

        private readonly ITagManager _tagManager;

        public TagController(ITagManager tagManager, IConfiguration configuration) : base(configuration)
        {
            _tagManager = tagManager;
            _pageSize = configuration.GetValue<Int32>("default-page-size");
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromRoute] Int32 page = 1)
        {
            var tags = await _tagManager.GetAllAsync(Cancel);
            var totalCount = await _tagManager.CountAsync(Cancel);
            var viewModel = new TagsViewModel(tags, totalCount, page, _pageSize);
            return View(viewModel);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Edit([FromRoute] Guid id)
        {
            var tag = await _tagManager.GetAsync(id, Cancel);
            if (tag == null)
            {
                return NotFound();
            }

            return View(new TagEditViewModel(tag));
        }

        [HttpPost(Name = "edit-tag")]
        public async Task<IActionResult> Edit(TagEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var tag = await _tagManager.UpdateAsync(model.ToDomain(), Cancel);
                return View(new TagEditViewModel(tag));
            }
            catch (Exception)
            {
                return View(model);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] TagCreateRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(model);
            }

            try
            {
                var tag = await _tagManager.CreateTagAsync(model.Name, Cancel);
                return Ok(tag);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}