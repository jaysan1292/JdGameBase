// Project: JdGameBase
// Filename: TimescaleManager.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Services {
    public class TimeScaleManager {
        private const float Buffer = 0.001f;
        public float MaxTimeScale;
        public float MinTimeScale;
        public bool Paused;
        private float _previousTimeScale;
        private float _timeScale;

        public TimeScaleManager() {
            _timeScale = 1f;
            MinTimeScale = 0f;
            MaxTimeScale = 2.5f;
        }

        public float TimeScale { get { return _timeScale; } set { _timeScale = MathHelper.Clamp(value, MinTimeScale, MaxTimeScale); } }

        public void IncreaseTimeScale(float delta) {
            if (TimeScale == 0f) TimeScale = Buffer * 2;
            TimeScale = MathHelper.Clamp(TimeScale + 1 * delta, MinTimeScale, MaxTimeScale);
        }

        public void DecreaseTimeScale(float delta) {
            TimeScale = MathHelper.Clamp(TimeScale - 1 * delta, MinTimeScale, MaxTimeScale);
        }

        public void ResetTimeScale() {
            TimeScale = 1f;
        }

        public void TogglePaused() {
            Paused = !Paused;

            if (Paused) PauseTime();
            else ResumeTime();
        }

        public void PauseTime() {
            if (Math.Abs(TimeScale - 0f) > 0.0000001f) _previousTimeScale = TimeScale;
            _timeScale = 0f;
            Paused = true;
        }

        public void ResumeTime() {
            _timeScale = _previousTimeScale;
            _previousTimeScale = 0f;
            Paused = false;
        }

        private float UpdateTimescale(float delta) {
            return delta * TimeScale;
        }

        public float UpdateTimescale(GameTime gameTime) {
            if (TimeScale < Buffer && TimeScale > -Buffer) TimeScale = 0f;
            return (float) (gameTime.ElapsedGameTime.TotalSeconds * TimeScale);
        }
    }
}
