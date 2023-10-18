using EasyGroceries.Order.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Order.Application.Filters
{
    public class CustomExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            ResponseDto<Exception> response = new ResponseDto<Exception>()
            {
                IsSuccess = false,
                Message = context.Exception.Message,
                Result = context.Exception,
                Status = (int)HttpStatusCode.InternalServerError
            };

            context.Result = new JsonResult(response);
        }
    }
}
