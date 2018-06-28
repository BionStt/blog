using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Website.Areas.Author.Controllers
{
    [Authorize]
    [Area("author"), Route("author")]
    public class AuthorController : Controller
    {
        [HttpGet, Route("dashboard")]
        public IActionResult Dashboard()
        {
            return View("Index");
        }
    }
}