using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using Mini_Kurs_Satis_Sitesi.Core.UnitOfWork;
using Mini_Kurs_Satis_Sitesi.Core.Constants;
using SharedLibrary.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Mini_Kurs_Satis_Sitesi.Service.Services
{
    public class CourseService : ServiceGeneric<Course, CourseDto>, ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUserService _userService;
        private readonly ILogger<CourseService> _logger;

        public CourseService(ICourseRepository courseRepository, IUnitOfWork unitOfWork, IMapper mapper, IUserService userService, ILogger<CourseService> logger) 
            : base(courseRepository, unitOfWork, mapper, userService)
        {
            _courseRepository = courseRepository;
            _userService = userService;
            _logger = logger;
        }

        async Task<Response<CourseDto>> ICourseService.AddAsync(CreateCourseDto createCourseDto)
        {
            _logger.LogInformation($"Received CreateCourseDto: {System.Text.Json.JsonSerializer.Serialize(createCourseDto)}");

            var course = _mapper.Map<Course>(createCourseDto);
            _logger.LogInformation($"Mapped Course: {System.Text.Json.JsonSerializer.Serialize(course)}");

            course.CreatedDate = DateTime.UtcNow;
            course.IsActive = true;

            await _courseRepository.AddAsync(course);
            await _unitOfWork.CommitAsync();

            var resultDto = _mapper.Map<CourseDto>(course);
            
            // Instructor bilgilerini ekle
            if (!string.IsNullOrEmpty(course.InstructorId))
            {
                var instructor = await _userService.GetUserByIdAsync(course.InstructorId);
                if (instructor.Data != null)
                {
                    resultDto.InstructorName = $"{instructor.Data.FirstName} {instructor.Data.LastName}";
                }
            }
            
            _logger.LogInformation($"Final CourseDto: {System.Text.Json.JsonSerializer.Serialize(resultDto)}");

            return Response<CourseDto>.Success(resultDto, 200);
        }

        public async Task<Response<IEnumerable<CourseDto>>> GetAllCoursesAsync()
        {
            try
            {
                _logger.LogInformation("Getting all courses");
                var courses = await _courseRepository.GetAllAsync();
                var courseDtos = new List<CourseDto>();
                
                foreach (var course in courses)
                {
                    var courseDto = _mapper.Map<CourseDto>(course);
                    
                    if (!string.IsNullOrEmpty(course.InstructorId))
                    {
                        var instructor = await _userService.GetUserByIdAsync(course.InstructorId);
                        if (instructor.Data != null)
                        {
                            courseDto.InstructorName = $"{instructor.Data.FirstName} {instructor.Data.LastName}";
                        }
                    }
                    
                    courseDtos.Add(courseDto);
                }
                
                _logger.LogInformation($"Found {courseDtos.Count} courses");
                return Response<IEnumerable<CourseDto>>.Success(courseDtos, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while getting all courses");
                return Response<IEnumerable<CourseDto>>.Fail("An error occurred while getting courses", 500, true);
            }
        }

        public async Task<Response<IEnumerable<CourseDto>>> GetCoursesByCategory(string category)
        {
            try
            {
                _logger.LogInformation($"Getting courses by category: {category}");
                var courses = await _courseRepository.GetCoursesByCategory(category);
                var courseDtos = new List<CourseDto>();
                
                foreach (var course in courses)
                {
                    var courseDto = _mapper.Map<CourseDto>(course);
                    
                    if (!string.IsNullOrEmpty(course.InstructorId))
                    {
                        var instructor = await _userService.GetUserByIdAsync(course.InstructorId);
                        if (instructor.Data != null)
                        {
                            courseDto.InstructorName = $"{instructor.Data.FirstName} {instructor.Data.LastName}";
                        }
                    }
                    
                    courseDtos.Add(courseDto);
                }
                
                _logger.LogInformation($"Found {courseDtos.Count} courses in category {category}");
                return Response<IEnumerable<CourseDto>>.Success(courseDtos, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while getting courses by category: {category}");
                return Response<IEnumerable<CourseDto>>.Fail("An error occurred while getting courses", 500, true);
            }
        }

        public async Task<Response<CourseDto>> GetCourseWithDetails(int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
                return Response<CourseDto>.Fail("Course not found", 404, true);

            var courseDto = _mapper.Map<CourseDto>(course);
            
            if (!string.IsNullOrEmpty(course.InstructorId))
            {
                var instructor = await _userService.GetUserByIdAsync(course.InstructorId);
                if (instructor.Data != null)
                {
                    courseDto.InstructorName = $"{instructor.Data.FirstName} {instructor.Data.LastName}";
                }
            }

            return Response<CourseDto>.Success(courseDto, 200);
        }

        public async Task<Response<IEnumerable<CourseDto>>> SearchCourses(string searchTerm)
        {
            try
            {
                _logger.LogInformation($"Searching courses with term: {searchTerm}");
                var courses = await _courseRepository.SearchCourses(searchTerm);
                var courseDtos = new List<CourseDto>();
                
                foreach (var course in courses)
                {
                    var courseDto = _mapper.Map<CourseDto>(course);
                    
                    if (!string.IsNullOrEmpty(course.InstructorId))
                    {
                        var instructor = await _userService.GetUserByIdAsync(course.InstructorId);
                        if (instructor.Data != null)
                        {
                            courseDto.InstructorName = $"{instructor.Data.FirstName} {instructor.Data.LastName}";
                        }
                    }
                    
                    courseDtos.Add(courseDto);
                }
                
                _logger.LogInformation($"Found {courseDtos.Count} courses matching search term: {searchTerm}");
                return Response<IEnumerable<CourseDto>>.Success(courseDtos, 200);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while searching courses with term: {searchTerm}");
                return Response<IEnumerable<CourseDto>>.Fail("An error occurred while searching courses", 500, true);
            }
        }

        public async Task<Response<InstructorCoursesDto>> GetCoursesByInstructorId(string instructorId)
        {
            _logger.LogInformation($"Getting courses for instructor: {instructorId}");

            var userResponse = await _userService.GetUserByIdAsync(instructorId);
            if (userResponse.StatusCode == 404)
            {
                _logger.LogWarning($"Instructor not found: {instructorId}");
                return Response<InstructorCoursesDto>.Fail("Instructor not found", 404, true);
            }

            // Kullanıcının instructor rolünde olup olmadığını kontrol et
            if (!await _userService.IsInRoleAsync(instructorId, Roles.Instructor))
            {
                _logger.LogWarning($"User {instructorId} is not an instructor");
                return Response<InstructorCoursesDto>.Fail("User is not an instructor", 403, true);
            }

            var courses = await _courseRepository.Where(c => c.InstructorId == instructorId).ToListAsync();
            _logger.LogInformation($"Found {courses.Count} courses for instructor {instructorId}");
            
            var courseDtos = new List<CourseDto>();
            foreach (var course in courses)
            {
                var courseDto = _mapper.Map<CourseDto>(course);
                courseDto.InstructorName = $"{userResponse.Data.FirstName} {userResponse.Data.LastName}";
                courseDtos.Add(courseDto);
            }
            
            var instructorCoursesDto = new InstructorCoursesDto
            {
                InstructorId = userResponse.Data.Id,
                InstructorName = $"{userResponse.Data.FirstName} {userResponse.Data.LastName}",
                Email = userResponse.Data.Email,
                City = userResponse.Data.City,
                Courses = courseDtos
            };

            return Response<InstructorCoursesDto>.Success(instructorCoursesDto, 200);
        }

        async Task<Response<NoDataDto>> ICourseService.UpdateAsync(UpdateCourseDto updateCourseDto, int id)
        {
            var course = await _courseRepository.GetByIdAsync(id);
            if (course == null)
                return Response<NoDataDto>.Fail("Course not found", 404, true);

            // Instructor yetkisi kontrolü
            var instructorId = course.InstructorId;
            if (string.IsNullOrEmpty(instructorId))
            {
                _logger.LogError($"Course {id} has no instructor assigned");
                return Response<NoDataDto>.Fail("Course has no instructor assigned", 400, true);
            }

            _mapper.Map(updateCourseDto, course);
            course.UpdatedDate = DateTime.UtcNow;
            _courseRepository.Update(course);
            await _unitOfWork.CommitAsync();
            return Response<NoDataDto>.Success(204);
        }

        async Task<Response<NoDataDto>> ICourseService.RemoveAsync(int id)
        {
            try
            {
                var course = await _courseRepository.GetCourseWithDetails(id);
                if (course == null)
                    return Response<NoDataDto>.Fail("Course not found", 404, true);

                // Bağlı siparişleri kontrol et
                if (course.OrderItems != null && course.OrderItems.Any())
                {
                    _logger.LogWarning($"Cannot delete course {id} because it has associated orders");
                    return Response<NoDataDto>.Fail("Cannot delete course because it has associated orders", 400, true);
                }

                _courseRepository.Remove(course);
                await _unitOfWork.CommitAsync();
                return Response<NoDataDto>.Success(204);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error while deleting course {id}");
                return Response<NoDataDto>.Fail("An error occurred while deleting the course", 500, true);
            }
        }
    }
}
