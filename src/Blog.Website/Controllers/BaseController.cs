using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Website.Controllers
{
    public class BaseController : Controller
    {
        protected CancellationToken Cancel => HttpContext.RequestAborted;
    }
}