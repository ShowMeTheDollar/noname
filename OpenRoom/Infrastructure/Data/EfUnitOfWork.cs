using ApplicationCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private bool disposedValue;
        private readonly OpenRoomContext _openRoomContext;
        private readonly Dictionary<Type, object> _repositories;

        public EfUnitOfWork(OpenRoomContext openRoomContext)
        {
            _openRoomContext = openRoomContext;
            _repositories = new Dictionary<Type, object>();
        }

        public void Commit()
        {
            _openRoomContext.Database.CommitTransaction();
        }
        public async Task CommitAsync()
        {
            await _openRoomContext.SaveChangesAsync(); // EF Core async saving changes
            await _openRoomContext.Database.CommitTransactionAsync();
        }
        public void Rollback()
        {
            _openRoomContext.Database.RollbackTransaction();
        }
        public async Task RollbackAsync()
        {
            await _openRoomContext.Database.RollbackTransactionAsync();
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _openRoomContext.Dispose();
                }
                disposedValue = true;
            }
        }
       
        public void Dispose()
        {
            // 請勿變更此程式碼。請將清除程式碼放入 'Dispose(bool disposing)' 方法
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            var typeOfEntity = typeof(T);
            if (_repositories.TryGetValue(typeOfEntity, out var existedRepository))
            {
                return (IRepository<T>)existedRepository;
            }

            var repository = new EfRepository<T>(_openRoomContext);
            _repositories.Add(typeOfEntity, repository);
            return repository;
        }

        public void Begin()
        {
            _openRoomContext.Database.BeginTransaction();
        }

        public async Task BeginAsync()
        {
            await _openRoomContext.Database.BeginTransactionAsync();
        }

        

        
    }
}
