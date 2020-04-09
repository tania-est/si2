using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace si2.dal.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext _db;

        public virtual async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> match, CancellationToken ct)
        {
            return await _db.Set<TEntity>().Where(match).FirstAsync();
        }

        public Repository(DbContext db)
        {
            _db = db;
        }
        public IQueryable<TEntity> GetAll()
        {
            return _db.Set<TEntity>();
        }

        public virtual async Task<ICollection<TEntity>> GetAllAsync(CancellationToken ct)
        {

            return await _db.Set<TEntity>().ToListAsync(ct);
        }

        public virtual TEntity Get(Guid id)
        {
            return _db.Set<TEntity>().Find(id);
        }

        public virtual async Task<TEntity> GetAsync(Guid id, CancellationToken ct)
        {
            return await _db.Set<TEntity>().FindAsync(new object[] { id }, ct);
        }

        public virtual void Add(TEntity t)
        {
            _db.Set<TEntity>().Add(t);
        }

        public virtual async Task AddAsync(TEntity t, CancellationToken ct)
        {
            await _db.Set<TEntity>().AddAsync(t);
        }

        public virtual TEntity Find(Expression<Func<TEntity, bool>> match)
        {
            return _db.Set<TEntity>().SingleOrDefault(match);
        }

        public ICollection<TEntity> FindAll(Expression<Func<TEntity, bool>> match)
        {
            return _db.Set<TEntity>().Where(match).ToList();
        }

        public async Task<ICollection<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> match, CancellationToken ct)
        {
            return await _db.Set<TEntity>().Where(match).ToListAsync(ct);
        }

        public virtual void Delete(TEntity entity)
        {
            _db.Set<TEntity>().SingleDelete(entity);
        }

        public virtual async Task DeleteAsync(TEntity entity, CancellationToken ct)
        {
            _db.Set<TEntity>().Remove(entity);
        }

        public virtual TEntity Update(TEntity t, object key, byte[] rowVersion = null)
        {
            if (t == null)
                return null;
            TEntity exist = _db.Set<TEntity>().Find(key);
            if (exist != null)
            {
                if (rowVersion != null)
                    _db.Entry(exist).OriginalValues["RowVersion"] = rowVersion;
                _db.Entry(exist).CurrentValues.SetValues(t);
            }
            return exist;
        }

        public virtual async Task UpdateAsync(TEntity t, object key, CancellationToken ct, byte[] rowVersion = null)
        {
            if (t == null)
                return;
            TEntity exist = await _db.Set<TEntity>().FindAsync(key);
            if (exist != null)
            {
                if (rowVersion != null)
                    _db.Entry(exist).OriginalValues["RowVersion"] = rowVersion;
                _db.Entry(exist).CurrentValues.SetValues(t);
            }
        }

        public int Count()
        {
            return _db.Set<TEntity>().Count();
        }

        public async Task<int> CountAsync(CancellationToken ct)
        {
            return await _db.Set<TEntity>().CountAsync(ct);
        }

        public virtual void Save()
        {
            _db.SaveChanges();
        }

        public async virtual Task<int> SaveAsync(CancellationToken ct)
        { 
            return await _db.SaveChangesAsync(ct);
        }

        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            IQueryable<TEntity> query = _db.Set<TEntity>().Where(predicate);
            return query;
        }

        public virtual async Task<ICollection<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken ct)
        {
            return await _db.Set<TEntity>().Where(predicate).ToListAsync(ct);
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> queryable = GetAll();
            foreach (Expression<Func<TEntity, object>> includeProperty in includeProperties)
            {
                queryable = queryable.Include<TEntity, object>(includeProperty);
            }

            return queryable;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                this.disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
