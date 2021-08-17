using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;

namespace Commerce.Core.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyAuthAttribute : Attribute, IAsyncActionFilter
    {

        private const string ApiKeyHeaderName = "Authorization";
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var requestApiKey))
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            
            var user = "1";
            
            if (user == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
            context.HttpContext.Items.Add("user", user);
            await next();
        }
    }
}