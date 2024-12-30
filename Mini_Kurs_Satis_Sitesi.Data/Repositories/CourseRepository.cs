//using Microsoft.EntityFrameworkCore;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        //private readonly ApplicationDbContext _context;

        /*public CourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }*/

        public async Task<IEnumerable<Course>> GetAllCoursesAsync()
        {
            //return await _context.Courses.ToListAsync();
            return [Course.Create(1, "sql", "sql dersi", 150.99m, "db")];
        }

        public async Task<Course> GetCourseByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task AddCourseAsync(Course course)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateCourseAsync(Course course)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteCourseAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
