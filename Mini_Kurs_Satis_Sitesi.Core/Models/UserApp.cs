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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? City { get; set; }

        // Navigation properties
        public ICollection<Course> Courses { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
