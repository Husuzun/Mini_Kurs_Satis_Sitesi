using Microsoft.AspNetCore.Mvc;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using SharedLibrary.DTOs;

namespace Mini_Kurs_Satis_Sitesi.Controllers
{
    public class CustomBaseController : ControllerBase
    {
        public IActionResult ActionResultInstance<T>(Response<T> response) where T : class
        {
            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}