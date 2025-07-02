namespace CRUDApp.Middleware
{
    public static class CustomMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomRequestLogging(this IApplicationBuilder app) => app.UseMiddleware<CustomMiddleware>();
    }
}
