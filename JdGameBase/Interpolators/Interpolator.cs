// Project: JdGameBase
// Filename: Interpolator.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Interpolators {
    // slightly modified from: http://www.xnawiki.com/index.php/Interpolators
    /// <summary>
    /// General Interpolator Base
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Interpolator<T> {
        protected float CurrentDuration;
        public bool IsActive;
        protected bool SmoothStep;
        protected float TotalDuration;
        protected T Value1;
        public T Value2 { get; protected set; }
        public T CurrentValue { get; private set; }

        /// <summary>
        /// Starts the Interpolator.
        /// </summary>
        /// <param name="value1">Value to start interpolation from</param>
        /// <param name="value2">Value to interpolate to</param>
        /// <param name="totalDuration">Time to interpolate from value1 to value2 in seconds</param>
        public void Start(T value1, T value2, float totalDuration) {
            Start(value1, value2, totalDuration, false);
        }

        /// <summary>
        /// Starts the interpolator.
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="totalDuration"></param>
        /// <param name="smoothStep">Use SmoothStep rather than Lerp</param>
        public void Start(T value1, T value2, float totalDuration, bool smoothStep) {
            Value1 = value1;
            Value2 = value2;
            TotalDuration = totalDuration;
            SmoothStep = smoothStep;

            IsActive = true;
            CurrentDuration = 0.0f;
            CurrentValue = value1;
        }

        /// <summary>
        /// Updates the interpolator.
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public T Update(float delta) {
            CurrentDuration += delta;
            if (CurrentDuration > TotalDuration) {
                CurrentDuration = TotalDuration;
                IsActive = false;
            }

            return CurrentValue = Interpolate();
        }

        /// <summary>
        /// Peek the next interpolation value without affecting the next update.
        /// </summary>
        /// <param name="delta"></param>
        /// <returns></returns>
        public T PeekValue(float delta) {
            var saveCurrent = CurrentDuration;
            var saveActive = IsActive;

            var nextValue = Update(delta);

            CurrentDuration = saveCurrent;
            IsActive = saveActive;

            return nextValue;
        }

        /// <summary>
        /// Resets the interpolator.
        /// </summary>
        public void Reset() {
            IsActive = false;
            CurrentDuration = 0.0f;
            TotalDuration = 0.0f;
        }

        protected abstract T Interpolate();
    }
}
