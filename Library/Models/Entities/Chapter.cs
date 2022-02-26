using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Entities
{
    public partial class Chapter
    {
        public Chapter(string title, int bookId)
        {
            Title = title;
            ChangeTitle(title);
            BookId = bookId;
            Order = 10;
        }
        public int Id { get; private set; }
        public string Title { get; private set; }
        public int Order { get; private set; }
        public int BookId { get; private set; }
        public virtual Book Book { get; set; }
        public void ChangeTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentException("A chapter must have a title");
            }
            Title = title;
        }
        public void ChangeOrder(int order)
        {
            Order = order;
        }
    }
}
