using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Core.Interfaces
{
    public interface ITokenService
    {
        Task<TokenDto> CreateToken(UserApp userApp);

        ClientTokenDto CreateTokenByClient(Client client);
    }
} 