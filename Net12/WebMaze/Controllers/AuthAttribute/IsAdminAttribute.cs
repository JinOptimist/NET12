using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMaze.EfStuff.DbModel;
using WebMaze.MyExceptions;
using WebMaze.Services;

namespace WebMaze.Controllers.AuthAttribute
{
    public class IsAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userService = context.HttpContext.RequestServices.GetService(typeof(UserService)) as UserService;

            var user = userService.GetCurrentUser();

            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            if (!user.Perrmissions.Any(x => x.Name == Perrmission.Admin))
            {
                context.Result = new ForbidResult();

                throw new SecretLinkException();
            }

            base.OnActionExecuting(context);
        }
    }
}
