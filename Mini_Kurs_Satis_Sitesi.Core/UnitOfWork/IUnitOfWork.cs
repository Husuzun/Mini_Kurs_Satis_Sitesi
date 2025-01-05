using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mini_Kurs_Satis_Sitesi.Core.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task CommitAsync();

        void Commit();
    }
}