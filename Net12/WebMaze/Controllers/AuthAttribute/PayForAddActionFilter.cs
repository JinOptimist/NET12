using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using WebMaze.Models.Enums;
using WebMaze.Services;

namespace WebMaze.Controllers.AuthAttribute
{
    public class PayForAddActionFilter : ActionFilterAttribute
    {
        private readonly TypesOfPayment _typesOfPayment;

        public PayForAddActionFilter (TypesOfPayment typesOfPayment = TypesOfPayment.Small)
        {
            _typesOfPayment = typesOfPayment;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {            
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var payForActionService = (PayForActionService)context.HttpContext.RequestServices.GetService(typeof(PayForActionService));
            if (!payForActionService.Payment((int)_typesOfPayment))
            {
                context.ModelState.AddModelError(string.Empty, "Not enought money to add");
            }
        }
    }
}
