using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using SharedLibrary.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Core.Interfaces
{
    public interface ICourseService : IServiceGeneric<Course, CourseDto>
    {
        Task<Response<IEnumerable<CourseDto>>> GetAllCoursesAsync();
        Task<Response<IEnumerable<CourseDto>>> GetCoursesByCategory(string category);
        Task<Response<IEnumerable<CourseDto>>> SearchCourses(string searchTerm);
        Task<Response<CourseDto>> GetCourseWithDetails(int id);
        Task<Response<InstructorCoursesDto>> GetCoursesByInstructorId(string instructorId);
        new Task<Response<CourseDto>> AddAsync(CreateCourseDto createCourseDto);
        new Task<Response<NoDataDto>> UpdateAsync(UpdateCourseDto updateCourseDto, int id);
        new Task<Response<NoDataDto>> RemoveAsync(int id);
    }
}
