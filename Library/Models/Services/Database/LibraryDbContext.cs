using Library.Models.Entities;
using Library.Models.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Library.Models.Services.Database
{
    public class LibraryDbContext : IdentityDbContext
    {
        public LibraryDbContext(DbContextOptions options)
        : base(options)
        {
        }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Chapter> Chapters { get; set; }
        public virtual DbSet<Purchase> Purchases { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Book>(entity =>
            {
                entity.ToTable("Books");
                entity.HasKey(book => book.Id);
                entity.OwnsOne(book => book.CurrentPrice, builder =>
                {
                    builder.Property(money => money.Currency)
                    .HasConversion<string>();
                    builder.Property(money => money.Amount)
                    .HasColumnType("decimal(18,2)");
                });
                entity.OwnsOne(book => book.FullPrice, builder =>
                {
                    builder.Property(money => money.Currency)
                    .HasConversion<string>();
                    builder.Property(money => money.Amount)
                    .HasColumnType("decimal(18,2)");
                });
                entity.HasMany(book => book.Chapters)
                      .WithOne(chapter => chapter.Book)
                      .HasForeignKey(chapter => chapter.BookId);
                entity.HasIndex(book => new { book.Title, book.Author }).IsUnique();
                entity.Property(book => book.Status).HasConversion<string>();
                entity.HasQueryFilter(book => book.Status != BookStatus.Deleted);
                entity.HasMany(book => book.Buyers)
                      .WithMany(user => user.PurchasedBooks)
                      .UsingEntity<Purchase>(
                            entity => entity.HasOne(purchase => purchase.User).WithMany().HasForeignKey(buyer => buyer.UserId),
                            entity => entity.HasOne(purchase => purchase.Book).WithMany().HasForeignKey(buyer => buyer.BookId),
                            entity =>
                            {
                                entity.ToTable("Purchases");
                                entity.OwnsOne(purchase => purchase.Paid, builder =>
                                {
                                    builder.Property(money => money.Currency)
                                    .HasConversion<string>();
                                    builder.Property(money => money.Amount)
                                    .HasColumnType("decimal(18,2)");
                                });
                            });
            });
            modelBuilder.Entity<Chapter>(entity =>
            {
                entity.Property(chapter => chapter.Order).HasDefaultValue(1000).ValueGeneratedNever();
            });
        }
    }
}
