using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Core.Models
{
    public class UserApp : IdentityUser
    {
        public string City { get; set; }
    }
}
