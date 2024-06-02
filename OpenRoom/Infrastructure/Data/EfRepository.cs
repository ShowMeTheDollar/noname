using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Data
{
    public class EfRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected OpenRoomContext DbContext;
        protected readonly DbSet<TEntity> DbSet;

        public EfRepository(OpenRoomContext dbContext)
        {
            DbContext = dbContext;
            DbSet = dbContext.Set<TEntity>();//點他就會有
        }

        public TEntity Add(TEntity entity)
        {
            DbSet.Add(entity);
            DbContext.SaveChanges();
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
            DbContext.SaveChanges();
            return entities;
        }

        public bool Any(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.Any(expression);
        }

        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbSet.AnyAsync(expression);
        }

        public void Delete(TEntity entity) //我們可能會寫isDelete，true or false
        {
            DbSet.Remove(entity);
            DbContext.SaveChanges();
        }

        public void DeleteRange(IEnumerable<TEntity> entities)//我們可能會寫isDelete，true or false
        {
            DbSet.RemoveRange(entities);
            DbContext.SaveChanges();
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.FirstOrDefault(expression)!;
        }

        public TEntity GetById<TId>(TId id)
        {
            return DbSet.Find(new object[] { id })!;
        }

        public async Task<TEntity> GetByIdAsync<TId>(TId id)
        {
            return (await DbSet.FindAsync(new object[] { id }))!;
        }

        public List<TEntity> List(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.Where(expression).ToList();
        }

        public List<TEntity> ListAll()
        {
            return DbSet.ToList();
        }

        public async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbSet.Where(expression).ToListAsync();
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> expression)
        {
            return DbSet.SingleOrDefault(expression)!;
        }

        public TEntity Update(TEntity entity)
        {
            DbSet.Update(entity);
            DbContext.SaveChanges();
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            DbSet.Update(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }

        public IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities)
        {
            DbSet.UpdateRange(entities);
            DbContext.SaveChanges();
            return entities;
        }
    }
}
