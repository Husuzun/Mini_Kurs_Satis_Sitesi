using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Core.Constants;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Mini_Kurs_Satis_Sitesi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : CustomBaseController
    {
        private readonly ICourseService _courseService;
        private readonly ILogger<CoursesController> _logger;

        public CoursesController(ICourseService courseService, ILogger<CoursesController> logger)
        {
            _courseService = courseService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return ActionResultInstance(await _courseService.GetAllCoursesAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return ActionResultInstance(await _courseService.GetByIdAsync(id));
        }

        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetCourseDetails(int id)
        {
            return ActionResultInstance(await _courseService.GetCourseWithDetails(id));
        }

        [HttpPost("category")]
        public async Task<IActionResult> GetByCategory([FromBody] CategoryDto categoryDto)
        {
            if (string.IsNullOrEmpty(categoryDto?.Category))
                return BadRequest("Category cannot be empty");

            return ActionResultInstance(await _courseService.GetCoursesByCategory(categoryDto.Category));
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string term)
        {
            return ActionResultInstance(await _courseService.SearchCourses(term));
        }

        [Authorize(Roles = Roles.Instructor)]
        [HttpGet("instructor-courses")]
        public async Task<IActionResult> GetInstructorCourses()
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation($"InstructorId from token: {instructorId}");

            if (string.IsNullOrEmpty(instructorId))
            {
                _logger.LogError("InstructorId is null or empty");
                return BadRequest("Instructor ID could not be retrieved from token");
            }

            // Query string'den başka bir instructor ID gelirse kontrol et
            var requestedInstructorId = HttpContext.Request.Query["instructorId"].ToString();
            if (!string.IsNullOrEmpty(requestedInstructorId) && requestedInstructorId != instructorId)
            {
                _logger.LogWarning($"Instructor {instructorId} tried to access courses of instructor {requestedInstructorId}");
                return Forbid();
            }

            return ActionResultInstance(await _courseService.GetCoursesByInstructorId(instructorId));
        }

        [Authorize(Roles = Roles.Instructor)]
        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseDto createCourseDto)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation($"InstructorId from token: {instructorId}");

            if (string.IsNullOrEmpty(instructorId))
            {
                _logger.LogError("InstructorId is null or empty");
                return BadRequest("Instructor ID could not be retrieved from token");
            }

            createCourseDto.InstructorId = instructorId;
            _logger.LogInformation($"CreateCourseDto after setting InstructorId: {System.Text.Json.JsonSerializer.Serialize(createCourseDto)}");

            var result = await _courseService.AddAsync(createCourseDto);
            _logger.LogInformation($"Result: {System.Text.Json.JsonSerializer.Serialize(result)}");

            return ActionResultInstance(result);
        }

        [Authorize(Roles = Roles.Instructor)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(UpdateCourseDto updateCourseDto, int id)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var course = await _courseService.GetByIdAsync(id);

            if (course.Data?.InstructorId != instructorId)
            {
                _logger.LogWarning($"Instructor {instructorId} tried to update course {id} owned by {course.Data?.InstructorId}");
                return Forbid();
            }

            return ActionResultInstance(await _courseService.UpdateAsync(updateCourseDto, id));
        }

        [Authorize(Roles = Roles.Instructor)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var instructorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var course = await _courseService.GetByIdAsync(id);

            if (course.Data?.InstructorId != instructorId)
            {
                _logger.LogWarning($"Instructor {instructorId} tried to delete course {id} owned by {course.Data?.InstructorId}");
                return Forbid();
            }

            return ActionResultInstance(await _courseService.RemoveAsync(id));
        }
    }
}