using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Particles.Entities {
    public class Fireball : ParticleEntity {
        protected readonly Random Random;
        private readonly Emitter _one;
        private readonly Emitter _smokeOne;
        private readonly Emitter _smokeTwo;
        private readonly Emitter _three;
        private readonly Emitter _two;

        public float ActiveDuration;
        public FireballConfig Config;
        public Vector2 Direction;
        public Vector2 Position;
        private bool _active;

        protected Fireball(Vector2 position, Vector2 direction, FireballConfig config) {
            Position = position;
            Direction = direction;
            Config = config ?? new FireballConfig();
        }

        public Fireball(Texture2D texture, Vector2 position, Vector2 direction, FireballConfig config = null)
            : this(position, direction, config) {
            Random = new Random((int) (texture.GetHashCode() + position.GetHashCode() + direction.GetHashCode() + DateTime.Now.Ticks));
            _one = new Emitter(10f, texture);
            _two = new Emitter(10f, texture);
            _three = new Emitter(10f, texture);
            _smokeOne = new Emitter(10f, texture);
            _smokeTwo = new Emitter(10f, texture);
            _active = true;
        }

        public Fireball(Texture2D[] textures, Vector2 position, Vector2 direction, FireballConfig config = null)
            : this(position, direction, config) {
            Random = new Random((int) (textures.GetHashCode() + position.GetHashCode() + direction.GetHashCode() + DateTime.Now.Ticks));
            _one = new Emitter(textures);
            _two = new Emitter(textures);
            _three = new Emitter(textures);
            _smokeOne = new Emitter(textures);
            _smokeTwo = new Emitter(textures);
        }

        public virtual bool Active {
            get { return _active; }
            set {
                if (!value) ActiveDuration = 0f;
                _one.Enabled = value;
                _two.Enabled = value;
                _three.Enabled = value;
                _smokeOne.Enabled = value;
                _smokeTwo.Enabled = value;
                _active = value;
            }
        }

        public void SetTexture(Texture2D texture, bool overwrite = false) {
            _one.SetTexture(texture, overwrite);
            _two.SetTexture(texture, overwrite);
            _three.SetTexture(texture, overwrite);
            _smokeOne.SetTexture(texture, overwrite);
            _smokeTwo.SetTexture(texture, overwrite);
        }

        public override int TotalParticleCount() {
            return _one.ParticleCount +
                   _two.ParticleCount +
                   _three.ParticleCount +
                   _smokeOne.ParticleCount +
                   _smokeTwo.ParticleCount;
        }

        public override void Draw(SpriteBatch spriteBatch) {
            _smokeOne.Draw(spriteBatch);
            _smokeTwo.Draw(spriteBatch);
            _one.Draw(spriteBatch);
            _two.Draw(spriteBatch);
            _three.Draw(spriteBatch);
        }

        public override void Update(float delta, GameTime gameTime) {
            ActiveDuration += delta;
            const float maxSpeed = 0.25f;
            var velocity1 = Utilities.RandomVelocity(Random, maxSpeed);
            var velocity2 = Utilities.RandomVelocity(Random, maxSpeed);
            var velocity3 = Utilities.RandomVelocity(Random, maxSpeed);
            var smokeVelocity = Utilities.RandomVelocity(Random, 0.1f);

            var fireballConfig = new ParticleConfig {
                Position = Position,
                TimeToLive = 750f,
                Behavior = ParticleBehavior.Directional,
                Direction = Direction,
                ScaleInterpolate = true
            };

            const float voffset = 2.25f;
            fireballConfig.Color = Config.BaseFireballColor;
            fireballConfig.Scale = 1.5f * Config.Scale;
            fireballConfig.Velocity = velocity1 / voffset;
            _one.CreateParticle(delta, fireballConfig);

            fireballConfig.Color = Config.SecondaryFireballColor;
            fireballConfig.Scale = 1.2f * Config.Scale;
            fireballConfig.Velocity = velocity2 / voffset;
            _two.CreateParticle(delta, fireballConfig);

            fireballConfig.Color = Config.TertiaryFireballColor;
            fireballConfig.Scale = 1.1f * Config.Scale;
            fireballConfig.Velocity = velocity3 / voffset;
            _three.CreateParticle(delta, fireballConfig);

            var sizeOffset = Utilities.RandomWithinRange(0.85f, 1.15f);
            fireballConfig.Color = Config.BaseSmokeColor;
            fireballConfig.Scale = 1.4f * sizeOffset * Config.Scale * Config.SmokeSize;
            fireballConfig.Velocity = (smokeVelocity / 5f) * Config.SmokeSpeed;
            fireballConfig.TimeToLive = Config.SmokeLifetime;
            fireballConfig.ScaleInterpolate = false;
            fireballConfig.ColorInterpolate = true;
            fireballConfig.FadeStartTime = 750f;
            _smokeOne.CreateParticle(delta, fireballConfig);

            fireballConfig.Color = Config.SecondarySmokeColor;
            fireballConfig.Scale = 1.2f * sizeOffset * Config.Scale * Config.SmokeSize;
            _smokeTwo.CreateParticle(delta, fireballConfig);

            _one.Update(delta, gameTime);
            _two.Update(delta, gameTime);
            _three.Update(delta, gameTime);
            _smokeOne.Update(delta, gameTime);
            _smokeTwo.Update(delta, gameTime);
        }
    }
}
