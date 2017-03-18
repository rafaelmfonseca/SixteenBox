using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Transaction
{
    public enum RequestType
    {
        /// <summary>
        /// The login part
        /// </summary>
        Credentials
    }

    /// <summary>
    /// Used to indentify server uri and download strings
    /// </summary>
    public static class APIRequest
    {
        /// <summary>
        /// Return the request path
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static ServerWebUri RetrieveUri(RequestType request)
        {
            switch (request)
            {
                case RequestType.Credentials:
                    return Settings.Paths[0];
            }

            return null;
        }

        /// <summary>
        /// Acess any website on background
        /// </summary>
        /// <param name="uri">The website url</param>
        /// <param name="posts">The page posts</param>
        /// <param name="whenComplete">Called when the download is complete</param>
        public static void RequestData(string uri, string posts, Action<UploadStringCompletedEventArgs> whenComplete)
        {
            WebClient wc = new WebClient();
            string result = string.Empty;
            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
            wc.UploadStringCompleted += (s, e) => { whenComplete(e); };
            wc.UploadStringAsync(new Uri(uri), posts);
        }
    }
}
