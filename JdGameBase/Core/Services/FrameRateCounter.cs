// Project: JdGameBase
// Filename: FrameRateCounter.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Services {
    public class FrameRateCounter {
        public TimeSpan ElapsedTime;
        public int FrameCounter;
        public int FramesPerSecond;

        public void Update(GameTime gameTime) {
            ElapsedTime += gameTime.ElapsedGameTime;

            if (ElapsedTime <= TimeSpan.FromSeconds(1)) return;

            ElapsedTime -= TimeSpan.FromSeconds(1);
            FramesPerSecond = FrameCounter;
            FrameCounter = 0;
        }

        /// <summary>
        /// Updates the frame count. Must be called in your Draw() method.
        /// </summary>
        public void UpdateFrameCount() {
            // Increment the frame counter
            FrameCounter++;
        }
    }
}
