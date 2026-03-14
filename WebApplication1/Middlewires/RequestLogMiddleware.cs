namespace WebApplication1.Middlewires
{
    public class RequestLogMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path;
            Console.WriteLine($"Request URL: {path}");
            await _next(context); 
        }
    }
}
