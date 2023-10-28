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
        /// <summary>
        /// 500 InternalError
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static CustomResultViewModel InternalError(Exception ex)
        {
            return new CustomResultViewModel(null, new ErrorViewModel {
                Message=ex.Message, 
                InnerMessage=ex.InnerException?.ToString()
            });
        }
        /// <summary>
        /// 400 Invalid
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static CustomResultViewModel Invalid(Exception ex)
        {
            return new CustomResultViewModel(null, new ErrorViewModel
            {
                Message = ex.Message,
                InnerMessage = ex.InnerException?.ToString()
            });
        }
        /// <summary>
        /// 403 Forbidden
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static CustomResultViewModel Forbidden(Exception ex)
        {
            return new CustomResultViewModel(null, new ErrorViewModel
            {
                Message = ex.Message,
                InnerMessage = ex.InnerException?.ToString()
            });
        }

    }
}
