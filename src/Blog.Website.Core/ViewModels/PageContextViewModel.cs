using System;

namespace Blog.Website.Core.ViewModels
{
    public abstract class PageContextViewModel
    {
        public Int32 PageNumber { get; set; }
        public Int32 PageSize { get; set; }
        public Int32 TotalPageCount { get; set; }

        protected PageContextViewModel(Int32 pageNumber,
                                       Int32 pageSize,
                                       Int32 totalItemsCount)
        {
            var pageCount = (Int32) Math.Ceiling((Double) totalItemsCount / pageSize);
            TotalPageCount = pageCount;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}