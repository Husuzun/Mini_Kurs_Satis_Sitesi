﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommmitAsync();

        void Commit();
    }
}