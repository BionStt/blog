using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Website.Controllers
{
    public class StatusCodeController : Controller
    {
        private Int32[] _availableCodes = new Int32[] { 400, 404, 500 };

        [Route("/StatusCode/{statusCode}")]
        public IActionResult Index(Int32 statusCode)
        {
            if (!_availableCodes.Contains(statusCode))
                statusCode = 500;

            return View($"Error-{statusCode}");
        }
    }
}