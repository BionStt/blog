using System;
using System.Threading;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Website.Controllers
{
    public class BaseController : Controller
    {
        protected CancellationToken Cancel => HttpContext.RequestAborted;
        protected readonly Int32 PageSize;

        public BaseController(Int32 pageSize)
        {
            PageSize = pageSize;
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