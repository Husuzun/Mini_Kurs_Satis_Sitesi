using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;

namespace Mini_Kurs_Satis_Sitesi.Service.Decorators
{
    public class LoggingDecorator(ICourseService courseService, ILogger<LoggingDecorator> logger) : ICourseService
    {
        public Task<List<CourseDto>> GetCourses()
        {
            var stopWatch = Stopwatch.StartNew();
            var result = courseService.GetCourses();
            stopWatch.Stop();
            logger.LogInformation($"GetCourses executed in {stopWatch.ElapsedMilliseconds}ms");

            return result;
        }
    }
}
