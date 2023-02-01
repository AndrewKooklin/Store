using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace Store.Web
{
    public class ExeptionFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public ExeptionFilter(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public void OnException(ExceptionContext context)
        {
            if (webHostEnvironment.IsDevelopment())
            {
                return;
            }

            if (context.Exception.TargetSite.Name == "ThrowNoElementsException")
            {
                //context.ExceptionHandled = true;

                context.Result = new ViewResult
                {
                    ViewName = "NotFound",
                    StatusCode = 404,
                };
            }
        }
    }
}
