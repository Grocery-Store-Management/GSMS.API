using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsmsLibrary
{
    public class GsmsUtils
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



    }
}
