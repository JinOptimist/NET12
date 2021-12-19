using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using WebMaze.Models.Enums;
using WebMaze.Services;

namespace WebMaze.Controllers.AuthAttribute
{
    public class PayForAddActionFilter : IActionFilter
    {
        private readonly TypesOfRewards _typesOfRewards;

        public PayForAddActionFilter (TypesOfRewards typesOfRewards)
        {
            _typesOfRewards = typesOfRewards;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var payForActionService = context.HttpContext.RequestServices.GetService(typeof(PayForActionService));
        }
    }
}
