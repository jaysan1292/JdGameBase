// Project: JdGameBase
// Filename: Particle.cs
// 
// Author: Jason Recillo

using System;

using JdGameBase.Core;

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

        public Particle(Texture2D texture, ParticleConfig config) {
            Config = config;
            Texture = texture;
            OriginalColor = config.Color;
            OriginalScale = config.Scale;
            OriginalTimeToLive = config.TimeToLive;
            Alive = true;

            if (OnParticleCreated != null) OnParticleCreated.Invoke(this, GameEventArgs.Empty);
        }

        public Rectangle BoundingBox { get; private set; }

        public override void Draw(SpriteBatch spriteBatch) {
            if (!Alive) return;
            var center = new Vector2(Texture.Width / 2f, Texture.Height / 2f);
            spriteBatch.Draw(Texture, Config.Position, null, Config.Color, Config.Rotation, center, Config.Scale, SpriteEffects.None, 0f);
        }

        public override void Update(float delta) {
            Config.TimeToLive -= delta;
            Config.Position += Config.Velocity * delta;
            Config.Rotation += Config.RotationVelocity * delta;

            if (Config.ScaleOverTime) Config.Scale = MathHelper.SmoothStep(0, OriginalScale, Config.TimeToLive / OriginalTimeToLive);

            switch (Config.Behavior) {
                case ParticleBehavior.Default:
                    break;
                case ParticleBehavior.Directional:
                    Config.Position += Config.Direction * delta;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (Config.FadeOverTime && Config.TimeToLive - Config.FadeStartTime > 0f) {
                var original = OriginalTimeToLive - Config.FadeStartTime;
                var current = MathHelper.Clamp(Config.TimeToLive - Config.FadeStartTime, 0, float.MaxValue);
                var amount = current / original;
                var alpha = (int) (MathHelper.SmoothStep(0, 255, amount));
                Config.Color = Color.FromNonPremultiplied(OriginalColor.R, OriginalColor.G, OriginalColor.B, alpha);
            }

            Alive = !(Config.TimeToLive > OriginalTimeToLive);

            if (!(Config.TimeToLive < 0)) return;
            if (OnParticleDestroyed != null) OnParticleDestroyed.Invoke(this, new GameEventArgs(delta));
            Alive = false;
        }

        public static event EventHandler<GameEventArgs> OnParticleCreated;
        public static event EventHandler<GameEventArgs> OnParticleDestroyed;
    }
}
