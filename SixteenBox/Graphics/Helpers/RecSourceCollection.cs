using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Graphics.Helpers
{
    public class RecSourceCollection
    {
        public List<Rectangle> Sources;

        public RecSourceCollection(IEnumerable<Rectangle> initialValues)
        {
            Sources = new List<Rectangle>(initialValues);
        }

        public RecSourceCollection()
        {
            Sources = new List<Rectangle>();
        }

        public Rectangle Get(int index) // I could use indexers, but maybe later I do some code changes here
        {
            return Sources[index];
        }
    }
}
