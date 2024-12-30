using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_Kurs_Satis_Sitesi.Controllers;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using Mini_Kurs_Satis_Sitesi.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : CustomBaseController
    {
        private readonly IServiceGeneric<Course, CourseDto> _courseService;

        public CourseController(IServiceGeneric<Course, CourseDto> courseService)
        {
            _courseService = courseService;
        }
        //[Authorize(AuthenticationSchemes = "ClientCredentialSchema")]
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            return ActionResultInstance(await _courseService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> SaveCourse(CourseDto courseDto)
        {
            return ActionResultInstance(await _courseService.AddAsync(courseDto));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse(CourseDto courseDto)
        {
            return ActionResultInstance(await _courseService.Update(courseDto, courseDto.Id));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            return ActionResultInstance(await _courseService.Remove(id));
        }
    }
}