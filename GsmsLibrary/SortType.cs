using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsmsLibrary
{
    public enum SortType
    {
        [Description("Ascending")]
        ASC = 1,
        [Description("Descending")]
        DESC = -1
    }

    public class SortParameter<TSource, TKey>
    {
        public SortType Type { get; set; }
        public Func<TSource, TKey> KeySelector { get; set; }

        public SortParameter(SortType sortType, Func<TSource, TKey> keySelector)
        {
            Type = sortType;
            KeySelector = keySelector;
        }
    }
}
