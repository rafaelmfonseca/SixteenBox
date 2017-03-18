using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SixteenBox.Graphics.Helpers.Positioner;

namespace SixteenBox.Graphics.Helpers
{
    public static class Positioner
    {
        public enum CenterType
        {
            Horizontally,
            Vertically,
            Both
        }

        public enum MarginFrom
        {
            TopLeft,
            BottomRight,
            BottomLeft
        }

        public enum SideFrom
        {
            Right,
            Left,
            Top,
            Bottom
        }

        /// <summary>
        /// Represents the modifications made on the drawableobject
        /// </summary>
        public class PositionerStyle
        {
            public DrawableObject Modified { get; set; }
        }

        /// <summary>
        /// Initialize the positioner.
        /// </summary>
        /// <param name="toModify">The drawable to modify</param>
        public static PositionerStyle Init(DrawableObject toModify)
        {
            return new PositionerStyle() { Modified = toModify };
        }

        /// <summary>
        /// Used to centerize with a sprite as reference
        /// </summary>
        /// <param name="d1">Main sprite</param>
        /// <param name="obj">Sprite as Reference</param>
        /// <param name="type">How it will be centerized</param>
        /// <returns></returns>
        public static PositionerStyle CenterTo(this PositionerStyle p1, DrawableObject obj, CenterType type)
        {
            // Calculate bounds
            var d1 = p1.Modified;
            var pos1 = p1.Modified.BoundingBox;
            var pos2 = obj.BoundingBox;

            switch (type)
            {
                case CenterType.Horizontally:
                    d1.Position = new Vector2((pos2.X + (pos2.Width / 2)) - (pos1.Width / 2), pos1.Y);
                    break;
                case CenterType.Vertically:
                    d1.Position = new Vector2(pos1.X, (pos2.Y + (pos2.Height / 2)) - (pos1.Height / 2));
                    break;
                case CenterType.Both:
                    // To avoid copying code, we do this
                    p1.CenterTo(obj, CenterType.Horizontally);
                    p1.CenterTo(obj, CenterType.Vertically);
                    break;
            }
            
            return p1;
        }
        /// <summary>
        /// Used to put an object inside another with margin
        /// </summary>
        /// <param name="d1">Main sprite</param>
        /// <param name="obj">Sprite as Reference</param>
        /// <param name="margin">The margins</param>
        /// <param name="from">From where the margin starts</param>
        /// <returns></returns>
        public static PositionerStyle Margin(this PositionerStyle p1, DrawableObject obj, Vector2 margin, MarginFrom from)
        {
            // Calculate bounds
            var d1 = p1.Modified;
            var pos1 = p1.Modified.BoundingBox;
            var pos2 = obj.BoundingBox;

            switch(from)
            {
                case MarginFrom.TopLeft:
                    d1.Position = obj.Position + margin;
                    break;
                case MarginFrom.BottomRight:
                    d1.Position = obj.Position + new Vector2(pos2.Width, pos2.Height) - (new Vector2(pos1.Width, pos1.Height) + margin);
                    break;
                case MarginFrom.BottomLeft:
                    d1.Position = obj.Position + new Vector2(margin.X, pos2.Height - (pos1.Height + margin.Y));
                    break;
            }
            
            return p1;
        }

        /// <summary>
        /// Put an object side by another
        /// </summary>
        /// <param name="d1">Main sprite</param>
        /// <param name="obj">Sprite as Reference</param>
        /// <param name="side">From which side</param>
        /// <returns></returns>
        public static PositionerStyle SideBy(this PositionerStyle p1, DrawableObject obj, SideFrom side)
        {
            // Calculate bounds
            var d1 = p1.Modified;
            var pos1 = p1.Modified.BoundingBox;
            var pos2 = obj.BoundingBox;

            d1.Position = GetSideBy(pos1, pos2, side);

            return p1;
        }

        /// <summary>
        /// Same as SideBy but doesn't change the drawable position
        /// </summary>
        public static Vector2 GetSideBy(Rectangle pos1, Rectangle pos2, SideFrom side)
        {
            Vector2 result = Vector2.Zero;

            switch (side)
            {
                case SideFrom.Right:
                    result = new Vector2(pos2.X, pos2.Y) + new Vector2(pos2.Width, 0);
                    break;
                case SideFrom.Left:
                    result = new Vector2(pos2.X, pos2.Y) - new Vector2(pos2.Width, 0);
                    break;
                case SideFrom.Top:
                    result = new Vector2(pos2.X, pos2.Y) - new Vector2(0, pos1.Height);
                    break;
                case SideFrom.Bottom:
                    result = new Vector2(pos2.X, pos2.Y) + new Vector2(0, pos1.Height);
                    break;
            }

            return result;
        }

        // Order to use: CenterTo -> Margin
    }
}
