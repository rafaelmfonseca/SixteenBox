using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Graphics.Animations
{
    /// <summary>
    /// Used to transform drawables
    /// </summary>
    public static class TransformDrawable
    {
        /// <summary>
        /// Select which transform will use
        /// </summary>
        public static class Factory
        {
            /// <summary>
            /// Transforms for nothing!
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <returns></returns>
            public static Transformer<T> GetTransform<T>()
            {
                return null;
            }

            /// <summary>
            /// Tansform for floats.
            /// </summary>
            public static Transformer<float> GetTransform()
            {
                return new TransformFloat();
            }
        }

        /// <summary>
        /// Creates a transform or not
        /// </summary>
        /// <typeparam name="T">Property type</typeparam>
        /// <param name="property">The property to change</param>
        /// <param name="startValue">Transformation start value</param>
        /// <param name="endValue">Transformation end value</param>
        /// <param name="endTime">When the transformation will be over</param>
        public static Transformer<T> Transformer<T>(this Transformer<T> transform, T startValue, T endValue, int endTime, GameTime gameTime, Action<Transformer<T>> changeValue)
        {
            Transformer t = (Transformer)transform ?? Factory.GetTransform();
            Transformer<T> g = transform;

            try
            {
                g = (Transformer<T>)t;
                if (g.Done) return g;
                g.StartValue = startValue;
                g.EndValue = endValue;
                g.EndTime = endTime;
                g.Update(gameTime);
                changeValue(g);
            }
            catch(Exception)
            {

            }

            return g;
        }


        /// <summary>
        /// Same as TransformDrawable but for Transform non generic
        /// </summary>
        public static Transformer<T> Transformer<T>(this Transformer transform, T startValue, T endValue, int endTime, GameTime gameTime, Action<Transformer<T>> changeValue)
        {
            return Transformer((Transformer<T>)transform, startValue, endValue, endTime, gameTime, changeValue);
        }

    }
}
