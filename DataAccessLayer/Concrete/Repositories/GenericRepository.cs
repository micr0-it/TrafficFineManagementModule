using DataAccessLayer.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concrete.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task AddAsync(T t)
        {
            await _dbSet.AddAsync(t);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T t)
        {
            _dbSet.Remove(t);
            await _context.SaveChangesAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>>? filter = null)
        {
            return filter is null ? await _dbSet.ToListAsync() : await _dbSet.Where(filter).ToListAsync();
        }

        public async Task UpdateAsync(T t)
        {
            _dbSet.Update(t);
            await _context.SaveChangesAsync();
        }
    }
}
