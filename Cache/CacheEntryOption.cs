using Microsoft.Extensions.Caching.Memory;

namespace MobileBasedCashFlowAPI.Cache
{
    public static class CacheEntryOption
    {
        public static MemoryCacheEntryOptions MemoryCacheEntryOption()
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromHours(3))
                .SetAbsoluteExpiration(TimeSpan.FromHours(3))
                .SetPriority(CacheItemPriority.Normal)
                .SetSize(1024);
            return cacheEntryOptions;
        }
    }
}
