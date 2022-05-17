using Microsoft.Extensions.Caching.Memory;

namespace UrlShortener
{
    public class RedirectMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IMemoryCache cache;
        private readonly ILogger<RedirectMiddleware> logger;

        public RedirectMiddleware(RequestDelegate next, IMemoryCache cache, ILogger<RedirectMiddleware> logger)
        {
            this.next = next;
            this.cache = cache;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var id = context.Request.Path.ToString();
            if (!string.IsNullOrEmpty(id))
            {
                if (id.StartsWith("/"))
                {
                    id = id[1..];
                }

                if (cache.TryGetValue(id, out var value))
                {
                    var redirect = value as string;
                    if (redirect != null)
                    {
                        cache.Remove(id);
                        logger.LogInformation($"'{id}': Redirecting to '{redirect}'");
                        context.Response.Redirect(redirect);
                        return;
                    }
                }
            }

            // Call the next delegate/middleware in the pipeline.
            await next(context);
        }
    }
}