using System;
using Blog.Website.Core.ConfigurationOptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Blog.Website.Filters
{
    public class DefaultSeoContent : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.Controller as Controller;
            if(controller == null)
            {
                base.OnActionExecuting(context);
                return;
            }

            var pageInfo = context.HttpContext.RequestServices.GetService<IOptions<DefaultPageInfoOption>>();
            if(pageInfo == null)
            {
                return;
            }

            var pageNumber = GetPageNumber(context);

            var mainTitlePart = pageInfo.Value.Title;
            var pageTitlePart = pageInfo.Value.PageNumberTitle;

            controller.ViewBag.Title = pageNumber > 1
                ? $"{mainTitlePart}{pageTitlePart}{pageNumber.ToString()}"
                : $"{mainTitlePart}";
            controller.ViewBag.Keywords = pageInfo.Value.Keywords;
            controller.ViewBag.Description = pageInfo.Value.Description;

            base.OnActionExecuting(context);
        }


        private Int32 GetPageNumber(ActionExecutingContext context)
        {
            context.ActionArguments.TryGetValue("page", out var page);

            if(page == null)
            {
                return 1;
            }

            if(Int32.TryParse(page.ToString(), out var pageNumber))
            {
                return pageNumber;
            }

            return 1;
        }
    }
}