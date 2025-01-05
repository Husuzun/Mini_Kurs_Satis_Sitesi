using Mini_Kurs_Satis_Sitesi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Core.Interfaces
{
    public interface ICourseRepository : IGenericRepository<Course>
    {
        Task<IEnumerable<Course>> GetCoursesByCategory(string category);
        Task<IEnumerable<Course>> SearchCourses(string searchTerm);
        Task<Course> GetCourseWithDetails(int id);
    }
}
