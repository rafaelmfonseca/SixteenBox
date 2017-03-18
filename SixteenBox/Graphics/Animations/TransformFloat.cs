using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SixteenBox.Graphics.Animations
{
    public class TransformFloat : Transformer<float>
    {
        /// <summary>
        /// Calculate the float every frame
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (Done) return;

            // Do calcs
            int milliseconds = gameTime.ElapsedGameTime.Milliseconds;
            CurrentTime += milliseconds;

            float interval = StartValue > EndValue ? StartValue - EndValue : EndValue - StartValue;
            float perFrame = (interval / EndTime) * milliseconds;

            if (Running)
            {
                // And here for the rest of the run
                //FrameValue = perFrame;
                CurrentValue = StartValue < EndValue ? CurrentValue + perFrame : CurrentValue - perFrame;
            }
            else
            {
                // Here goes the code for the first run
                //FrameValue = StartValue < EndValue ? StartValue + perFrame : StartValue - perFrame;
                CurrentValue = StartValue < EndValue ? StartValue + perFrame : StartValue - perFrame;
                Running = true;
            }

            // Check ended
            if (CurrentTime >= EndTime)
            {
                Done = true;
            }
        }
    }
}
