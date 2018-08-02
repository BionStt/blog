using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Blog.Website.Controllers
{
    public class BaseController : Controller
    {
        protected CancellationToken Cancel => HttpContext.RequestAborted;
        protected readonly Int32 PageSize;

        public BaseController(IConfiguration configuration)
        {
            PageSize = configuration.GetValue<Int32>("default-page-size");
        }

        public Int32 GetSkip(Int32 page, Int32 pageSize)
        {
            if (page <= 0)
            {
                return 0;
            }

            return (page - 1) * pageSize;
        }
    }
}