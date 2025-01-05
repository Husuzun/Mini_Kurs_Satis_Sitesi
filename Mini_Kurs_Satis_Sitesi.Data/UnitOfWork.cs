using Microsoft.EntityFrameworkCore;
using Mini_Kurs_Satis_Sitesi.Core.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Mini_Kurs_Satis_Sitesi.Data.Repositories;
using Mini_Kurs_Satis_Sitesi.Data;

namespace Mini_Kurs_Satis_Sitesi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(ApplicationDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}