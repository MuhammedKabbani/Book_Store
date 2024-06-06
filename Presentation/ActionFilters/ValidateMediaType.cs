using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.ActionFilters
{
    public class ValidateMediaTypeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var acceptHeaderpresent = context.HttpContext.Request.Headers.ContainsKey("Accept"); 
            if (!acceptHeaderpresent)
            {
                context.Result = new BadRequestObjectResult("Accept header is missing");
                return;
            }

            var mediaTYpe = context.HttpContext.Request.Headers["Accept"].FirstOrDefault();

            if(!MediaTypeHeaderValue.TryParse(mediaTYpe,out MediaTypeHeaderValue? mediaType))
            {
                context.Result = new BadRequestObjectResult("Media type not supported." + " Please add Accept header with required media type.");

                return;
            }

            context.HttpContext.Items.Add("AcceptHeaderMediaType", mediaType);

        }
    }
}
