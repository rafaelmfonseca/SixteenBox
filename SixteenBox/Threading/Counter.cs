using SixteenBox.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SixteenBox.Threading
{
    public delegate void FinishedEventHandler(object sender, EventArgs e);

    /// <summary>
    /// A class to count if seconds has passed
    /// </summary>
    public class Counter : IUpdatable
    {
        #region Fields

        public bool _finished;

        #endregion

        #region Properties

        public int Elapsed { get; set; }
        public int EndTime { get; set; }
        public bool Done { get; set; }

        #endregion

        #region Events

        public event FinishedEventHandler Finished;

        #endregion

        public void Update(GameTime gameTime)
        {
            if (Done)
            {
                if (_finished) return;

                OnFinished(EventArgs.Empty);
                _finished = true;
            }

            Elapsed += gameTime.ElapsedGameTime.Milliseconds;

            if(Elapsed >= EndTime)
            {
                Done = true;
            }
        }

        /// <summary>
        /// Resets the counter
        /// </summary>
        public void Reset()
        {
            Elapsed = 0;
            Done = false;
        }

        /// <summary>
        /// Invoke the finished event
        /// </summary>
        protected virtual void OnFinished(EventArgs e)
        {
            Finished?.Invoke(this, e);
        }
    }
}
