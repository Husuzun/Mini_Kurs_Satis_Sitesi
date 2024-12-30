using Mini_Kurs_Satis_Sitesi.Core.DTOs;

namespace Mini_Kurs_Satis_Sitesi.Core.Interfaces
{
    public interface ICourseService
    {
        Task<List<CourseDto>> GetCourses();
    }
}
