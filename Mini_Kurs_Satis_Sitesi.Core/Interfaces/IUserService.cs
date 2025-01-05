using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using SharedLibrary.DTOs;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Core.Interfaces
{
    public interface IUserService
    {
        Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
        Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
        Task<Response<UserAppDto>> GetUserByIdAsync(string userId);
        Task<Response<UserPurchasedCoursesDto>> GetUserPurchasedCoursesAsync(string userId);
        Task<Response<UserAppDto>> UpdateUserAsync(string userId, UpdateUserDto updateUserDto);
        Task<bool> IsInRoleAsync(string userId, string role);
    }
}