using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GsmsLibrary
{
    public class GsmsConfiguration
    {
        #region Private Members to get Configuration
        private static IConfigurationRoot GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true);
            return builder.Build();
        }
        #endregion

        #region Public Configuration Fields

        /// <summary>
        /// Connection string for Database 
        /// </summary>
        public static string ConnectionString
            => GetConfiguration()["ConnectionString:GsmsDb"];

        #endregion
    }
}
