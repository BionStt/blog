using System;
using System.Threading;
using Blog.Website.Core.ConfigurationOptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Blog.Website.Controllers
{
    public class BaseController : Controller
    {
        protected CancellationToken Cancel => HttpContext.RequestAborted;
        protected readonly Int32 PageSize;

        public BaseController(IOptions<DefaultPageInfoOption> pageInfo)
        {
            PageSize = pageInfo.Value.PageSize;
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