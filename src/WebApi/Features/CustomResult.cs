using Core.Dtos;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.ViewModels;

namespace Infrastructure.Services
{
    public static class CustomResult
    {
        public static CustomResultViewModel Ok(object data)
        {
            return new CustomResultViewModel(data,null);
        }
        public static CustomResultViewModel NotFound()
        {
            return new CustomResultViewModel(null, null);
        }
        public static CustomResultViewModel InternalError(Exception ex)
        {
            return new CustomResultViewModel(null, new ErrorViewModel {
                Message=ex.Message, 
                InnerMessage=ex.InnerException?.ToString()
            });
        }

    }
}
