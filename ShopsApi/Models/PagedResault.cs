using System;
using System.Collections.Generic;

namespace ShopsApi.Models
{
    public class PagedResault<T>
    {
        public List<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalPages { get; set; }


        public PagedResault(List<T> items, int totalCount, int pageSize, int pageNumber)
        {
            {
                Items = items;
                TotalCount = totalCount;
                ItemsFrom = pageSize * (pageNumber - 1) + 1;
                ItemsTo = ItemsFrom + pageSize - 1;
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            }
        }
    }
}
