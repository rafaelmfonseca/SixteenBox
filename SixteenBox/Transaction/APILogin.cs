using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace SixteenBox.Transaction
{

    public enum LoginState
    {
        /// <summary>
        /// The user is not playing
        /// </summary>
        Offline,
        /// <summary>
        /// The user was connected but the connection went offline suddenly
        /// </summary>
        Failing,
        /// <summary>
        /// The user was trying to connect but failed
        /// </summary>
        Failed,
        /// <summary>
        /// The user is connecting or trying to after failing
        /// </summary>
        Connecting,
        /// <summary>
        /// The user is playing and online
        /// </summary>
        Online
    }

    public class APILogin : APIComponent
    {
        #region Properties

        public User Credentials { get; set; }

        #endregion

        public APILogin(User credentials) : base(RequestType.Credentials, new string[] { credentials.Name, credentials.Password })
        {
            Credentials = credentials;
            Credentials.State = LoginState.Connecting;
        }

        /// <summary>
        /// Called when page is loaded
        /// </summary>
        /// <param name="e"></param>
        public override void RequestComplete(UploadStringCompletedEventArgs e)
        {
            try
            {
                // Get the result
                string result = e.Result;

                // Convert string to json
                var token = JObject.Parse(result);
                int count = (int)token.SelectToken("count");

                if(count > 0)
                {
                    // Change status to online
                    Credentials.State = LoginState.Online;

                    // Get user id
                    Credentials.Id = (int)token.SelectToken("user_id");

                    // Get real name
                    Credentials.Name = (string)token.SelectToken("user_name");

                    // Get temp pass
                    Credentials.TempPassword = (string)token.SelectToken("tempPass");
                }
                else
                {
                    Credentials.State = LoginState.Failed;
                }
            }
            catch (Exception) // The website was offline or something
            {
                Credentials.State = LoginState.Failed;
            }
        }

    }
}
