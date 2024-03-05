using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore.Update.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces.IRepositories
{
    public interface IUserRepository
    {
        Task UpdateInfo(Entities.UserInfo userInfo);
    }
}
