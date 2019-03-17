using System;
using Blog.Website.Core.ConfigurationOptions;
using Microsoft.Extensions.Options;

namespace Blog.Website.Controllers
{
    public class BaseReaderController : BaseController
    {
        protected readonly Int32 PageSize;
        
        public BaseReaderController(IOptions<DefaultPageInfoOption> pageInfo)
        {
            PageSize = pageInfo.Value.PageSize;
        }
    }
}