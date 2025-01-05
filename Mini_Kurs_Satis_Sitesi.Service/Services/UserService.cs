using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mini_Kurs_Satis_Sitesi.Core.Constants;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using Mini_Kurs_Satis_Sitesi.Core.Interfaces;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using Mini_Kurs_Satis_Sitesi.Data;
using SharedLibrary.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Service.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<UserApp> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserService(UserManager<UserApp> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
        }

        public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
        {
            var user = new UserApp { 
                Email = createUserDto.Email, 
                UserName = createUserDto.UserName,
                City = createUserDto.City,
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName
            };

            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
            }

            // Ensure User role exists
            if (!await _roleManager.RoleExistsAsync(Roles.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.User));
            }

            // Assign User role
            await _userManager.AddToRoleAsync(user, Roles.User);

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }

        public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return Response<UserAppDto>.Fail("UserName cannot be null or empty", 400, true);
            }

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return Response<UserAppDto>.Fail("UserName not found", 404, true);
            }

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }

        public async Task<Response<UserAppDto>> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Response<UserAppDto>.Fail("User not found", 404, true);
            }

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }

        public async Task<Response<UserPurchasedCoursesDto>> GetUserPurchasedCoursesAsync(string userId)
        {
            // Ensure user can only access their own courses
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Response<UserPurchasedCoursesDto>.Fail("User not found", 404, true);
            }

            var purchasedCourses = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Course)
                .Where(o => o.UserId == userId)
                .SelectMany(o => o.OrderItems.Select(oi => new PurchasedCourseDto
                {
                    CourseId = oi.CourseId,
                    CourseName = oi.Course.Name,
                    Category = oi.Course.Category,
                    PurchaseDate = o.OrderDate,
                    PurchasePrice = oi.Price,
                    OrderStatus = o.Status
                }))
                .ToListAsync();

            var result = new UserPurchasedCoursesDto
            {
                UserName = user.UserName,
                Email = user.Email,
                PurchasedCourses = purchasedCourses
            };

            return Response<UserPurchasedCoursesDto>.Success(result, 200);
        }

        public async Task<Response<UserAppDto>> UpdateUserAsync(string userId, UpdateUserDto updateUserDto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Response<UserAppDto>.Fail("User not found", 404, true);
            }

            // Ad Soyad güncellemesi
            if (!string.IsNullOrEmpty(updateUserDto.FirstName))
            {
                user.FirstName = updateUserDto.FirstName;
            }

            if (!string.IsNullOrEmpty(updateUserDto.LastName))
            {
                user.LastName = updateUserDto.LastName;
            }

            // Şehir güncellemesi
            if (!string.IsNullOrEmpty(updateUserDto.City) && user.City != updateUserDto.City)
            {
                user.City = updateUserDto.City;
            }

            // Kullanıcı adı güncellemesi
            if (!string.IsNullOrEmpty(updateUserDto.UserName) && user.UserName != updateUserDto.UserName)
            {
                // Kullanıcı adının benzersiz olup olmadığını kontrol et
                var existingUser = await _userManager.FindByNameAsync(updateUserDto.UserName);
                if (existingUser != null)
                {
                    return Response<UserAppDto>.Fail(new ErrorDto("Username is already taken", true), 400);
                }
                user.UserName = updateUserDto.UserName;
            }

            // Email güncellemesi
            if (!string.IsNullOrEmpty(updateUserDto.Email) && user.Email != updateUserDto.Email)
            {
                // Email'in benzersiz olup olmadığını kontrol et
                var existingUser = await _userManager.FindByEmailAsync(updateUserDto.Email);
                if (existingUser != null)
                {
                    return Response<UserAppDto>.Fail(new ErrorDto("Email is already registered", true), 400);
                }
                user.Email = updateUserDto.Email;
                user.NormalizedEmail = updateUserDto.Email.ToUpper();
            }

            // Şifre güncellemesi
            if (!string.IsNullOrEmpty(updateUserDto.CurrentPassword) || !string.IsNullOrEmpty(updateUserDto.NewPassword))
            {
                // Her iki şifre alanı da gerekli
                if (string.IsNullOrEmpty(updateUserDto.CurrentPassword) || string.IsNullOrEmpty(updateUserDto.NewPassword))
                {
                    return Response<UserAppDto>.Fail(new ErrorDto("Both current password and new password are required for password change", true), 400);
                }

                // Mevcut şifre kontrolü
                var passwordCheck = await _userManager.CheckPasswordAsync(user, updateUserDto.CurrentPassword);
                if (!passwordCheck)
                {
                    return Response<UserAppDto>.Fail(new ErrorDto("Current password is incorrect", true), 400);
                }

                // Şifre değiştirme
                var passwordResult = await _userManager.ChangePasswordAsync(user, updateUserDto.CurrentPassword, updateUserDto.NewPassword);
                if (!passwordResult.Succeeded)
                {
                    var errors = passwordResult.Errors.Select(x => x.Description).ToList();
                    return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
                }
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description).ToList();
                return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
            }

            return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return false;

            return await _userManager.IsInRoleAsync(user, role);
        }
    }
}