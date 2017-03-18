using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SixteenBox.Transaction
{
    public abstract class APIComponent
    {
        #region Properties

        public Thread Thread { get; set; }
        public ServerWebUri Uri { get; set; }
        public string ParsedPost { get; set; }

        #endregion

        /// <summary>
        /// Connects an user with the server!
        /// </summary>
        public APIComponent(RequestType requestType, string[] postsValues)
        {
            Uri = APIRequest.RetrieveUri(requestType);
            ParsedPost = string.Format(Uri.Posts, postsValues);
            Thread = new Thread(Run) { IsBackground = true };
            Thread.Start();
        }

        /// <summary>
        /// Request access to the web on background
        /// </summary>
        protected virtual void Run()
        {
            APIRequest.RequestData(Uri.Complete(), ParsedPost, RequestComplete);
        }

        public abstract void RequestComplete(UploadStringCompletedEventArgs e);
    }
}
