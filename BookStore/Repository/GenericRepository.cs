using BookStore.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        public GenericRepository(BookStoreDBContext db)
        {
            Db = db;
        }

        public BookStoreDBContext Db { get; }

        public List<TEntity> GetAll() 
        {
            return Db.Set<TEntity>().ToList();
        }

        public TEntity GetById(int id) 
        {
            return Db.Set<TEntity>().Find(id);
        }


        public void Add(TEntity entity)
        {
            Db.Set<TEntity>().Add(entity);
            //Db.SaveChanges();

        }
        public TEntity Edit(TEntity entity)
        {
            Db.Entry(entity).State=EntityState.Modified;
            Db.SaveChanges();
            return entity;
        }

        public TEntity Delete(int id) 
        {
            TEntity entity=GetById(id);
            Db.Set<TEntity>().Remove(entity);
            Db.SaveChanges();
            return (entity);
        }

    }
}
