// Project: JdGameBase
// Filename: ParticleConfig.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Particles {
    public class ParticleConfig {
        #region Backing Fields

        private float _fadeStartTime;

        #endregion

        public ParticleConfig() {
            Behavior = ParticleBehavior.Default;
            Color = Color.White;
            Direction = Position = Velocity = TargetVelocity = Vector2.Zero;
            TimeToLive = Scale = 1f;
            ColorInterpolate = RotationVelocityInterpolate = ScaleInterpolate = VelocityInterpolate = false;
        }

        #region General

        /// <summary>
        /// The particle's behavior mode. Defaults to ParticleBehavior.Default.
        /// </summary>
        public ParticleBehavior Behavior { get; set; }

        /// <summary>
        /// Color tint. Defaults to Color.White.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The direction the particles should travel in. Has no effect if Behavior is not ParticleBehavor.Directional.
        /// </summary>
        public Vector2 Direction { get; set; }

        /// <summary>
        /// Where the particle should spawn.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// The particle sprite's rotation.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// The particle's lifetime, in seconds.
        /// </summary>
        public float TimeToLive { get; set; }

        #endregion

        #region Color

        /// <summary>
        /// Whether or not this particle should change its color.
        /// </summary>
        public bool ColorInterpolate { get; set; }

        /// <summary>
        /// Specifies when the particle will start fading. Value will be clamped between 0 and TimeToLive. Has no effect if ColorInterpolate is false.
        /// </summary>
        // TODO: FadeStartTime does not do anything yet.
        public float FadeStartTime { get { return _fadeStartTime; } set { _fadeStartTime = MathHelper.Clamp(value, 0, TimeToLive); } }

        /// <summary>
        /// The opacity this particle should reach by the end of its lifetime. Has no effect if ColorInterpolate is false.
        /// </summary>
        public Color TargetColor { get; set; }

        #endregion

        #region Rotation

        /// <summary>
        /// How fast the particle sprite should rotate.
        /// </summary>
        public float RotationVelocity { get; set; }

        /// <summary>
        /// Whether or not this particle should change its rotation speed over time.
        /// </summary>
        public bool RotationVelocityInterpolate { get; set; }

        /// <summary>
        /// The rotation speed this particle should reach by the end of its lifetime. Has no effect if RotationVelocityInterpolate is false.
        /// </summary>
        public float TargetRotationVelocity { get; set; }

        #endregion

        #region Scale

        /// <summary>
        /// The scale at which the particle texture is drawn.
        /// </summary>
        public float Scale { get; set; }

        /// <summary>
        /// Whether or not the particle should change its scale over its lifetime.
        /// </summary>
        public bool ScaleInterpolate { get; set; }

        /// <summary>
        /// The scale this particle should reach by the end of its lifetime. Has no effect if ScaleInterpolate is false.
        /// </summary>
        public float TargetScale { get; set; }

        #endregion

        #region Velocity

        /// <summary>
        /// The velocity this particle should reach by the end of its lifetime. Has no effect if VelocityInterpolate is false.
        /// </summary>
        public Vector2 TargetVelocity { get; set; }

        /// <summary>
        /// This particle's initial velocity.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Whether or not the particle should change its velocity over time.
        /// </summary>
        public bool VelocityInterpolate;

        #endregion
    }
}
