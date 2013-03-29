using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Particles.Entities {
    public class Explosion : Fireball {
        private readonly Emitter _debris;
        private float _duration;
        private bool _needsDebris;

        public Explosion(Texture2D texture, Texture2D debrisTexture, Vector2 position, Vector2 direction, FireballConfig config = null)
            : base(texture, position, direction, config ?? new ExplosionConfig()) {
            _debris = new Emitter(10f, debrisTexture);
            _needsDebris = true;
        }

        public Explosion(Texture2D[] textures, Texture2D[] debrisTextures, Vector2 position, Vector2 direction, FireballConfig config = null)
            : base(textures, position, direction, config ?? new ExplosionConfig()) {
            _debris = new Emitter(10f, debrisTextures);
            _needsDebris = true;
        }

        public override bool Active {
            get { return base.Active; }
            set {
                base.Active = value;
                _debris.Enabled = value;
            }
        }

        public bool ExplosionFinished { get { return TotalParticleCount() == 0 && _duration > 0; } }

        public override int TotalParticleCount() {
            return base.TotalParticleCount() + _debris.ParticleCount;
        }

        public override void Update(float delta, GameTime gameTime) {
            base.Update(delta, gameTime);
            var cfg = ((ExplosionConfig) Config);
            if (_needsDebris) {
                for (var i = 0; i < cfg.DebrisCount; i++) {
                    _debris.CreateParticle(delta, new ParticleConfig {
                        Position = Position,
                        Velocity = Utilities.RandomVelocity(Random, cfg.DebrisSpeed),
                        Color = cfg.DebrisColor,
                        Scale = cfg.DebrisTextureScale,
                        TimeToLive = cfg.DebrisLifetime,
                        ColorInterpolate = true
                    });
                }
                _needsDebris = false;
            }

            _duration += delta;
            if (_duration >= cfg.MaxDuration) {
                Active = false;
                _duration = 0f;
            }

            _debris.Update(delta, gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch) {
            _debris.Draw(spriteBatch);
            base.Draw(spriteBatch);
        }
    }
}
