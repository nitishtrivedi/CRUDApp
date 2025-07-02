namespace CRUDApp.Middleware
{
    public sealed class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomMiddleware> _logger;

        public CustomMiddleware(RequestDelegate next, ILogger<CustomMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation($"{context.Request.Method} method is executed.");
            await _next(context);
            _logger.LogInformation($"The Status code is {context.Response.StatusCode}");
        }
    }
}
