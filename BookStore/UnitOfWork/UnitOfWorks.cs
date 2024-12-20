using BookStore.Models;
using BookStore.Repository;

namespace BookStore.UnitOfWork
{
    public class UnitOfWorks
    {
        GenericRepository<Book> BookRepo;
        GenericRepository<Author> AuthorRepo;
        GenericRepository<Category> CategoryRepo;
        GenericRepository<Order> OrderRepo;
        GenericRepository<OrderItems> OrderItemsRepo;
        GenericRepository<Customer> CustomerRepo;

        BookStoreDBContext db;

        public UnitOfWorks(BookStoreDBContext db)
        {
            this.db= db;
        }

        public GenericRepository<Book> BookGenRepo
        { 
            get 
            {
                if (BookRepo == null) 
                
                    BookRepo = new GenericRepository<Book>(db);
                    return BookRepo;
                
            }
        }

        public GenericRepository<Author> AuthorGenRepo 
        { 
            get 
            {
                if (AuthorRepo == null)
                    AuthorRepo = new GenericRepository<Author>(db);
                    return  AuthorRepo;
            }
        }


        public GenericRepository<Order> OrderGenRepo
        {
            get
            {
                if (OrderRepo == null)

                    OrderRepo = new GenericRepository<Order>(db);

                return OrderRepo;
            }
        }


        public GenericRepository<Category> CategoryGenRepo
        {
            get
            {
                if (CategoryRepo == null)
                CategoryRepo = new GenericRepository<Category>(db);
                return CategoryRepo;

            }
        }

        public GenericRepository<OrderItems> OrderItemsGenRepo
        {
            get
            {
                if (OrderItemsRepo == null)
                OrderItemsRepo = new GenericRepository<OrderItems>(db);
                return OrderItemsRepo;

            }
        }

        public GenericRepository<Customer> CustomerGenRepo
        {
            get
            {
                if (CustomerRepo == null)

                    CustomerRepo = new GenericRepository<Customer>(db);
                return CustomerRepo;

            }
        }


        public void save()
        {
            db.SaveChanges();
        }


    }
}
