using Microsoft.Xna.Framework;
using SixteenBox.Transaction.Sockets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Transaction.Sockets.Models
{
    /// <summary>
    /// Represents a message on lobby chat
    /// </summary>
    public class ChatMessage : IModel
    {
        #region Properties

        public string Text { get; set; }
        public Color Color { get; set; } = Color.White;

        #endregion
    }
}
