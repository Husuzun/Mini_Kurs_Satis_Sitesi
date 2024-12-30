using Microsoft.EntityFrameworkCore;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace UdemyAuthServer.Data.Repositories
{
    public class GenericRepository<Tentity> : IGenericRepository<Tentity> where Tentity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<Tentity> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<Tentity>();
        }

        public async Task AddAsync(Tentity entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task<IEnumerable<Tentity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Tentity> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            // EntityState.Detached yapýsýný service class'sýný anlatýrken detaylandýracaðým.
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Detached;
            }

            return entity;
        }

        public void Remove(Tentity entity)
        {
            _dbSet.Remove(entity);
        }

        public Tentity Update(Tentity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            return entity;
        }

        public IQueryable<Tentity> Where(Expression<Func<Tentity, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }
    }
}