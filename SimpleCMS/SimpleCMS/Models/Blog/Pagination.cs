using SimpleCMS.Models.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCMS.Models
{
    public class Pagination
    {
        public IEnumerable<PageContent> List;
        public int PerPage { get; private set; }
        public int PageNumber { get; private set; }
        public int PageCount { get; private set; }
        public int BackIndex { get; private set; }
        public int ForwardIndex { get; private set; }
        public int From { get; private set; }
        public int To { get; private set; }

        public Pagination(IEnumerable<PageContent> _list, int _perPage, string _pageNumberString, int _paginationCount = 5)
        {
            List = _list;
            PerPage = _perPage > 0 ? _perPage : 2;
            var itemsCount = List.Count();
            var pageNumber = 1;
            if (int.TryParse(_pageNumberString, out pageNumber))
            {
                PageNumber = pageNumber;
            }
            PageNumber = PageNumber <= 0 ? 0 : PageNumber;
            var fromPage = PageNumber * PerPage;
            var pageCountMod = (itemsCount % PerPage);
            pageCountMod = pageCountMod < itemsCount ? pageCountMod : 1;
            PageCount = itemsCount / PerPage + pageCountMod;
            PageNumber = PageNumber < PageCount ? PageNumber : PageCount - 1;
            List = List.Skip(fromPage).Take(PerPage);
            BackIndex = (PageNumber > 0 ? PageNumber - 1: 0);
            ForwardIndex = (PageNumber < PageCount - 1 ? PageNumber + 1 : PageCount);
            To = (PageNumber + _paginationCount) < PageCount ? (PageNumber + _paginationCount) : PageCount;
            From = To - _paginationCount;
        }

    }
}
