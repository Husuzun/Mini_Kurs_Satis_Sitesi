using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mini_Kurs_Satis_Sitesi.Core.UnitOfWork;

namespace Mini_Kurs_Satis_Sitesi.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _context;

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommmitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
