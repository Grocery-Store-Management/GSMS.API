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
    }
}
