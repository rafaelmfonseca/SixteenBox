using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Graphics.Animations
{
    /// <summary>
    /// Just to be beautiful
    /// </summary>
    public abstract class Transformer
    {
        public virtual int EndTime { get; set; }
        public virtual int CurrentTime { get; set; }
        public virtual bool Done { get; set; }
        public virtual bool Running { get; set; } // For Transformation with StartValue > EndValue
    }

    /// <summary>
    /// Represents an object that transforms a value
    /// </summary>
    public abstract class Transformer<T> : Transformer, IUpdatable
    {
        public virtual T StartValue { get; set; }
        public virtual T EndValue { get; set; }
        public virtual T CurrentValue { get; protected set; }

        public abstract void Update(GameTime gameTime);
    }
}
