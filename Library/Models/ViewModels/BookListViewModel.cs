using Library.Models.InputModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.ViewModels
{
    public class BookListViewModel : IPaginationInfo
    {
        public ListViewModel<BookViewModel> Books { get; set; }
        public BookListInputModel Input { get; set; }
        #region Implementation IPaginationInfo
        int IPaginationInfo.CurrentPage => Input.Page;
        int IPaginationInfo.TotalResults => Books.TotalCount;
        int IPaginationInfo.ResultsPerPage => Input.Limit;
        string IPaginationInfo.Search => Input.Search;
        string IPaginationInfo.OrderBy => Input.OrderBy;
        bool IPaginationInfo.Ascending => Input.Ascending;
        #endregion
    }
}
