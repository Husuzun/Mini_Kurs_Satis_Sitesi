using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_Kurs_Satis_Sitesi.Controllers;
using Mini_Kurs_Satis_Sitesi.Core.Constants;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UdemyAuthServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //api/user
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
        }

        [Authorize(Roles = $"{Roles.User},{Roles.Instructor}")]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest(new { message = "User name not found in token" });
            }
            return ActionResultInstance(await _userService.GetUserByNameAsync(userName));
        }

        [Authorize(Roles = $"{Roles.User},{Roles.Instructor}")]
        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile(UpdateUserDto updateUserDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return ActionResultInstance(await _userService.UpdateUserAsync(userId, updateUserDto));
        }

        [Authorize(Roles = $"{Roles.User},{Roles.Instructor}")]
        [HttpGet("purchased-courses")]
        public async Task<IActionResult> GetPurchasedCourses()
        {
            // Get the current user's ID from claims
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // If user is instructor and a specific user ID is provided in query string
            var requestedUserId = HttpContext.Request.Query["userId"].ToString();
            if (User.IsInRole(Roles.Instructor) && !string.IsNullOrEmpty(requestedUserId))
            {
                userId = requestedUserId;
            }

            return ActionResultInstance(await _userService.GetUserPurchasedCoursesAsync(userId));
        }
    }
}