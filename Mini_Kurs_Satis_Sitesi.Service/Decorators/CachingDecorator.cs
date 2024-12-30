using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;

namespace Mini_Kurs_Satis_Sitesi.Service.Decorators
{
    public class CachingDecorator(ICourseService courseService, IMemoryCache memoryCache) : ICourseService
    {
        public async Task<List<CourseDto>> GetCourses()
        {
            var cacheKey = "Courses";
            if(memoryCache.TryGetValue(cacheKey, out List<CourseDto> coursesAsDto))
            {
                return coursesAsDto;
            }
            coursesAsDto = await courseService.GetCourses();
            memoryCache.Set(cacheKey, coursesAsDto, TimeSpan.FromMinutes(5));
            return coursesAsDto;
        }
    }
}
