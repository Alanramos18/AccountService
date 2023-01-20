using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Threading.Tasks;
using System;

namespace Account.Web.Middlewares
{
    public class GetApplicationMiddleware
    {
        private readonly RequestDelegate _next;
        public GetApplicationMiddleware(RequestDelegate next) { _next = next; }

        public async Task Invoke(HttpContext context)
        {
            context.Request.Headers.TryGetValue("Application", out StringValues appCode);
            Console.WriteLine(appCode);
            await _next(context);
        }
    }
}
