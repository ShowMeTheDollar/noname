using ApplicationCore.Entities;
using System.Linq.Expressions;

namespace ApplicationCore.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Add(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        Task<TEntity> AddAsync(TEntity entity);
        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        TEntity GetById<TId>(TId id);//取得單一筆，透過Id;請大家不要亂刪掉IRepository原本有宣告的介面，要使用非同步請方法名稱另外加xxxAsync
        Task<TEntity> GetByIdAsync<TId>(TId id);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> expression);
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> expression);
        bool Any(Expression<Func<TEntity, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression);
        List<TEntity> List(Expression<Func<TEntity, bool>> expression);
        List<TEntity> ListAll();
        Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> expression);
        
    }
}
