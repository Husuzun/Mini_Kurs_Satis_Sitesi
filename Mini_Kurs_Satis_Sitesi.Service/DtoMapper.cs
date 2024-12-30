using AutoMapper;
using Mini_Kurs_Satis_Sitesi.Core.DTOs;
using Mini_Kurs_Satis_Sitesi.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mini_Kurs_Satis_Sitesi.Service
{
    internal class DtoMapper : Profile
    {
        public DtoMapper()
        {
            CreateMap<CourseDto, Course>().ReverseMap();
            CreateMap<UserAppDto, UserApp>().ReverseMap();
        }
    }
}