using DataAccessLayer.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Contexts.EFCore
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly EFRepositoryContext _context;

        public RepositoryBase(EFRepositoryContext context)
        {
            _context = context;
        }

        public void Create(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            if (trackChanges)
                return _context.Set<T>();

            return _context.Set<T>().AsNoTracking();
        }

        public IQueryable<T> FindBy(Expression<Func<T, bool>> expression, bool trackChanges)
        {
            if (trackChanges)
                return _context.Set<T>().Where(expression);

            return _context.Set<T>().Where(expression).AsNoTracking();
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
