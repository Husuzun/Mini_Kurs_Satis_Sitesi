using Microsoft.EntityFrameworkCore;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Data.Repositories
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Course>> GetCoursesByCategory(string category)
        {
            return await _context.Courses
                .Where(c => c.Category == category && c.IsActive)
                .ToListAsync();
        }

        public async Task<IEnumerable<Course>> SearchCourses(string searchTerm)
        {
            return await _context.Courses
                .Where(c => (c.Name.Contains(searchTerm) || 
                           c.Description.Contains(searchTerm)) && 
                           c.IsActive)
                .ToListAsync();
        }

        public async Task<Course> GetCourseWithDetails(int id)
        {
            return await _context.Courses
                .Include(c => c.OrderItems)
                .ThenInclude(oi => oi.Order)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsActive);
        }
    }
}
