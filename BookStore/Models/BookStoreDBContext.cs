using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BookStore.Models
{
    public class BookStoreDBContext:IdentityDbContext
    {
        public BookStoreDBContext() { }
        public BookStoreDBContext(DbContextOptions<BookStoreDBContext>options)
            :base(options)
        {
        } 
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Author { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<OrderItems> OrderItems { get; set; }
        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{

        //    base.OnModelCreating(builder);
        //    builder.Entity<OrderItems>().HasKey(oi => new { oi.OrderId, oi.BookId });
        //    //builder.Entity<OrderItems>()
        //    //  .Property(oi => oi.OrderItemId)
        //    //  .ValueGeneratedOnAdd();
        //    //builder.Entity<IdentityRole>().HasData
        //    //    (
        //    //    new IdentityRole() { Name="admin",NormalizedName="ADMIN"},
        //    //    new IdentityRole() { Name="customer",NormalizedName="Customer"}
        //    //    );
        //}






    }
}
