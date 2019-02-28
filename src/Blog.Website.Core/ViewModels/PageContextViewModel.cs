using System;

namespace Blog.Website.Core.ViewModels
{
    public abstract class PageContextViewModel
    {
        public String Title { get; set; }
        public String Description { get; set; }
        public String Keywords { get; set; }

        public Int32 PageNumber { get; set; }
        public Int32 PageSize { get; set; }
        public Int32 TotalPageCount { get; set; }

        protected PageContextViewModel(Int32 pageNumber,
                                       Int32 pageSize,
                                       Int32 totalItemsCount,
                                       String mainTitlePart,
                                       String pageTitlePart,
                                       String description,
                                       String keywords)
        {
            SetPagingPart(pageNumber, pageSize, totalItemsCount);
            SetSeoContext(mainTitlePart, null, pageTitlePart, description, keywords);
        }

        protected PageContextViewModel(Int32 pageNumber,
                                       Int32 pageSize,
                                       Int32 totalItemsCount,
                                       String mainTitlePart,
                                       String extendTitlePart,
                                       String pageTitlePart,
                                       String description,
                                       String keywords)
        {
            SetPagingPart(pageNumber, pageSize, totalItemsCount);
            SetSeoContext(mainTitlePart, extendTitlePart, pageTitlePart, description, keywords);
        }

        private void SetPagingPart(Int32 pageNumber,
                                   Int32 pageSize,
                                   Int32 totalItemsCount)
        {
            var pageCount = (Int32) Math.Ceiling((Double) totalItemsCount / pageSize);
            TotalPageCount = pageCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

        private void SetSeoContext(String mainTitlePart,
                                   String extendTitlePart,
                                   String pageTitlePart,
                                   String description,
                                   String keywords)
        {
            Title = PageNumber > 1
                ? $"{mainTitlePart}{extendTitlePart}{pageTitlePart}{PageNumber.ToString()}"
                : $"{mainTitlePart}{extendTitlePart}";

            Description = description;
            Keywords = keywords;
        }
    }
}