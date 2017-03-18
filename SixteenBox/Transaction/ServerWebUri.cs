using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Transaction
{
    /// <summary>
    /// Represents a file in our server and his post data.
    /// </summary>
    public class ServerWebUri
    {
        public string FileName { get; set; }
        public string Posts { get; set; }

        /// <summary>
        /// Get the complete url
        /// </summary>
        /// <returns>A string with the server url plus file name</returns>
        public string Complete()
        {
            return Settings.ServerWebUri + FileName;
        }
    }
}
