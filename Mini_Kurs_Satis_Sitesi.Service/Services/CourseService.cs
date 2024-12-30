using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;

namespace Mini_Kurs_Satis_Sitesi.Service.Services
{
    public class CourseService(ICourseRepository courseRepository, ILogger<CourseService> logger, IMemoryCache memoryCache):ICourseService
    {
        public async Task<List<CourseDto>> GetCourses()
        {
            var courses = await courseRepository.GetAllCoursesAsync();

            return courses.Select(p => new CourseDto()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price*1.2m,
                Category = p.Category
            }).ToList();
        }
    }
}
