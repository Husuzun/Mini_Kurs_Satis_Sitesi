using System;
using System.Collections.Generic;
using System.Text;

namespace Mini_Kurs_Satis_Sitesi.Core.DTOs
{
    public class UserAppDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
    }
}