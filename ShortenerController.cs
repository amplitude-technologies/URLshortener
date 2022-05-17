using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using shortid;
using shortid.Configuration;

namespace UrlShortener
{
    /// <summary>
    /// Controller to create a ShortUrl
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ShortenerController : ControllerBase
    {
        private readonly IMemoryCache cache;
        private readonly ILogger<ShortenerController> logger;
        private readonly GenerationOptions shortIdptions;
        private readonly MemoryCacheEntryOptions cacheOptions;

        public ShortenerController(IMemoryCache cache, ILogger<ShortenerController> logger)
        {
            this.cache = cache;
            this.logger = logger;

            var variables = Environment.GetEnvironmentVariables();

            shortIdptions = new GenerationOptions(useSpecialCharacters: false);

            cacheOptions = new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) };
            _ = cacheOptions.RegisterPostEvictionCallback(OnPostEviction);
        }

        [HttpPost]
        public ActionResult<string> CreateShortUrl(CreateShortUrlRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Url))
            {
                return BadRequest();
            }

            var id = ShortId.Generate(shortIdptions);

            cache.Set(id, request.Url, cacheOptions);

            logger.LogInformation($"Generated shor url '{id}' --> '{request.Url}'");

            return id;
        }

        private void OnPostEviction(object key, object value, EvictionReason reason, object state)
        {
            logger.LogInformation($"Removed {key} from cache for reason '{reason}'");
        }
    }
}