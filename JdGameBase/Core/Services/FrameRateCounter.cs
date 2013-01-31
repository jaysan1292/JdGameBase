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

        public void UpdateFrameCount() {
            // Will not actually draw anything, but will increment the frame counter
            FrameCounter++;
        }
    }
}
