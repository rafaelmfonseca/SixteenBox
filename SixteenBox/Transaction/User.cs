using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Transaction
{
    /// <summary>
    /// Represents an user logged
    /// </summary>
    public class User
    {
        #region Properties

        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; protected set; }
        public string TempPassword { get; set; }
        public LoginState State { get; set; }

        #endregion

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
