using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using SharedLibrary.DTOs;
namespace Mini_Kurs_Satis_Sitesi.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Response<TokenDto>> CreateTokenAsync(LoginDto loginDto);
        Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);
        Task<Response<NoDataDto>> RevokeRefreshToken(string refreshToken);
        Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
    }
} 