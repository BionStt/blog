using System;
using System.Threading.Tasks;
using Blog.Core.Contracts.Managers;
using Blog.Website.Controllers;
using Blog.Website.Core.ConfigurationOptions;
using Blog.Website.Core.ViewModels.Author.Tag;
using Blog.Website.Models.Requests.Author;
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
                             IOptions<DefaultPageInfoOption> pageInfo)
            : base(pageInfo)
        {
            _tagManager = tagManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index(GetTagsRequest request)
        {
            var tagPage = await _tagManager.GetAsync(request.ToQuery(PageSize), Cancel);
            var viewModel = new TagsViewModel(tagPage, request.Page);
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
    }
}