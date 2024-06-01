using EntityLayer.LogModel;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using ServicesLayer.Contracts;

namespace PresentationLayer.ActionFilters
{
    public class LogFilterAttribute : ActionFilterAttribute
    {
        private readonly ILoggerService _logger;

        public LogFilterAttribute(ILoggerService logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInfo(CreateLog("OnActionExecuting", context.RouteData));
        }
        private string CreateLog(string modelName, RouteData routeData)
        {
            LogDetails logDetails = new LogDetails()
            {
                ModelName = modelName,
                Controller = routeData.Values["controller"],
                Action = routeData.Values["action"],
                Id = routeData.Values.ContainsKey("Id") ? routeData.Values["Id"] : null,

            };
            return logDetails.ToString();
        }
    }
}
