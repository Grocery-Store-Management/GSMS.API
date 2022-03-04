using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsmsLibrary
{
    public static class GsmsUtils
    {
        public static string CreateGuiId()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }

        public static IEnumerable<TSource> Sort<TSource, TKey>(
            IEnumerable<TSource> list,
            Func<TSource, TKey> keySelector,
            SortType sortType)
        {
            // Sort
            if (sortType == SortType.ASC)
            {
                list = list.OrderBy(keySelector);
            }
            else if (sortType == SortType.DESC)
            {
                list = list.OrderByDescending(keySelector);
            }

            return list.ToList();
        }

        public static IEnumerable<T> Paging<T>(
            IEnumerable<T> list,
            int page,
            int pageSize
            )
        {
            if (page > 0)
            {
                // Paging
                list = list
                    .Skip((page - 1) * pageSize)
                        .Take(pageSize);
            }
            else
            {
                // page <= 0
                // Get All
            }

            return list;
        }

        public static async Task SetAsync(this IDistributedCache cache,
            string key, object value)
        {
            if (value == null)
            {
                return;
            }
            string jsonValue = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
            byte[] encodedValue = Encoding.UTF8.GetBytes(jsonValue);
            var options = new DistributedCacheEntryOptions()
                //.SetAbsoluteExpiration(DateTime.Now.AddHours(2))
                //.SetSlidingExpiration(TimeSpan.FromHours(1.5));
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(15))
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));
            await cache.SetAsync(key, encodedValue, options);
        }

        public static async Task<T> GetAsync<T>(this IDistributedCache cache,
            string key)
        {
            byte[] encodedKey = await cache.GetAsync(key);
            if (encodedKey == null)
            {
                return default;
            }
            string jsonValue = Encoding.UTF8.GetString(encodedKey);
            return JsonConvert.DeserializeObject<T>(jsonValue, new JsonSerializerSettings
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
        }
        
    }
}
