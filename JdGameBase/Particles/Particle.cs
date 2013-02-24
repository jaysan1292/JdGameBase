// Project: JdGameBase
// Filename: Particle.cs
// 
// Author: Jason Recillo

using System;

using JdGameBase.Core;
using JdGameBase.Interpolators;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Particles {
    public class Particle : Entity {
        public readonly Color OriginalColor;
        public readonly float OriginalScale;
        public readonly float OriginalTimeToLive;
        public readonly Texture2D Texture;
        public bool Alive;
        public ParticleConfig Config;
        private Vector2Interpolator _velocity;
        private FloatInterpolator _rotation;
        private FloatInterpolator _scale;
        private ColorInterpolator _color;

        public Particle(Texture2D texture, ParticleConfig config) {
            Config = config;
            Texture = texture;
            OriginalColor = config.Color;
            OriginalScale = config.Scale;
            OriginalTimeToLive = config.TimeToLive;
            Alive = true;

            if (Config.VelocityInterpolate)
                (_velocity = new Vector2Interpolator()).Start(Config.Velocity, Config.TargetVelocity, Config.TimeToLive);
            if (Config.ScaleInterpolate)
                (_scale = new FloatInterpolator()).Start(Config.Scale, Config.TargetScale, Config.TimeToLive, true);
            if (Config.RotationVelocityInterpolate)
                (_rotation = new FloatInterpolator()).Start(Config.RotationVelocity, Config.TargetRotationVelocity, Config.TimeToLive);
            if (Config.ColorInterpolate)
                (_color = new ColorInterpolator()).Start(Config.Color, Config.TargetColor, Config.TimeToLive);
            if (OnParticleCreated != null) OnParticleCreated.Invoke(this, GameEventArgs.Empty);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            if (!Alive) return;
            var center = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            spriteBatch.Draw(Texture, Config.Position, null, Config.Color, Config.Rotation, center, Config.Scale, SpriteEffects.None, 0f);
        }

        public override void Update(float delta, GameTime gameTime) {
            Config.TimeToLive -= delta;

            if (Config.ColorInterpolate) Config.Color = _color.Update(delta);
            if (Config.ScaleInterpolate) Config.Scale = _scale.Update(delta);
            if (Config.VelocityInterpolate) Config.Velocity = _velocity.Update(delta);
            if (Config.RotationVelocityInterpolate) Config.RotationVelocity = _rotation.Update(delta);

            Config.Position += Config.Velocity * delta;
            Config.Rotation += Config.RotationVelocity * delta;

            if (Config.Behavior == ParticleBehavior.Directional) Config.Position += Config.Direction * delta;
            //            switch (Config.Behavior) {
            //                case ParticleBehavior.Default:
            //                    break;
            //                case ParticleBehavior.Directional:
            //                    Config.Position += Config.Direction * delta;
            //                    break;
            //                default:
            //                    throw new ArgumentOutOfRangeException();
            //            }

            //            if (Config.ColorInterpolate && Config.TimeToLive - Config.FadeStartTime > 0f) {
            //                var original = OriginalTimeToLive - Config.FadeStartTime;
            //                var current = MathHelper.Clamp(Config.TimeToLive - Config.FadeStartTime, Config.TargetColor, float.MaxValue);
            //                var amount = current / original;
            //                var alpha = (int) (MathHelper.SmoothStep(Config.TargetColor, 255, amount));
            //                Config.Color = Color.FromNonPremultiplied(OriginalColor.R, OriginalColor.G, OriginalColor.B, alpha);
            //            }

            //            Alive = !(Config.TimeToLive > OriginalTimeToLive);

            if (!(Config.TimeToLive < 0)) return;
            if (OnParticleDestroyed != null) OnParticleDestroyed.Invoke(this, new GameEventArgs(delta));
            Alive = false;
        }

        public static event EventHandler<GameEventArgs> OnParticleCreated;
        public static event EventHandler<GameEventArgs> OnParticleDestroyed;
    }
}
