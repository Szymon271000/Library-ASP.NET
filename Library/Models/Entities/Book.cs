using Library.Models.Enums;
using Library.Models.ValueObjects;
using Library.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Models.Entities
{
    public partial class Book
    {
        public Book(string title, string author)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("The book must have a title");
            }
            if (string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentException("The book must have an author");
            }

            ChangeTitle(title);
            ChangeAuthor(author);
            ChangeStatus(BookStatus.Published);
            Chapters = new HashSet<Chapter>();
            CurrentPrice = new Money(Currency.EUR, 0);
            FullPrice = new Money(Currency.EUR, 0);
            BookCover = "/images/defaultbook.png";
        }
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string BookCover { get; private set; }
        public string Author { get; private set; }
        public double Rating { get; private set; }
        public Money FullPrice { get; private set; }
        public Money CurrentPrice { get; private set; }
        public string Summary { get; private set; }
        public BookStatus Status { get; private set; }
        public virtual ICollection<Chapter> Chapters { get; private set; }
        public virtual ICollection<ApplicationUser> Buyers { get; set; }

        public void ChangeStatus(BookStatus newStatus)
        {
            Status = newStatus;
        }

        public void ChangeRating(double newRating)
        {
            Rating = newRating;
        }

        public void ChangeTitle(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
            {
                throw new ArgumentException("The book must have a title");
            }
            Title = newTitle;
        }
        public void ChangeAuthor(string newAuthor)
        {
            if (string.IsNullOrWhiteSpace(newAuthor))
            {
                throw new ArgumentException("The book must have an author");
            }
            Author = newAuthor;
        }
        public void ChangePrices(Money newFullPrice, Money newCurrentPrice)
        {
            if (newFullPrice == null || newCurrentPrice == null)
            {
                throw new ArgumentException("Prices can't be null");
            }
            if (newFullPrice.Currency != newCurrentPrice.Currency)
            {
                throw new ArgumentException("Currencies don't match");
            }
            if (newFullPrice.Amount < newCurrentPrice.Amount)
            {
                throw new ArgumentException("Full price can't be less than the current price");
            }
            FullPrice = newFullPrice;
            CurrentPrice = newCurrentPrice;
        }
        public void ChangeSummary(string newSummary)
        {
            if (newSummary != null)
            {
                if (newSummary.Length < 20)
                {
                    throw new Exception("Summary is too short");
                }
                else if (newSummary.Length > 4000)
                {
                    throw new Exception("Summary is too long");
                }
            }
            Summary = newSummary;
        }
        public void ChangeImagePath(string imagePath)
        {
            BookCover = imagePath;
        }
    }
}
