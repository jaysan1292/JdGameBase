// Project: JdGameBase
// Filename: Emitter.cs
// 
// Author: Jason Recillo

using System;
using System.Collections.Generic;

using JdGameBase.Core;
using JdGameBase.Extensions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Particles {
    //TODO: Add support for SpriteSheets and AnimatedSprites
    public class Emitter : Entity {
        public readonly List<Particle> AllParticles;
        public readonly List<Texture2D> ParticleTextures;
        public bool Enabled;
        public float SpawnRate;
        private float _lastSpawn;
        private bool _singleTexture;

        public int ParticleCount { get { return AllParticles.Count; } }

        #region Constructors

        private Emitter(float spawnRate) {
            SpawnRate = spawnRate;
            AllParticles = new List<Particle>();
            Enabled = true;
        }

        public Emitter(Texture2D particleTexture)
            : this(0.0167f, particleTexture) {
            ParticleTextures = new List<Texture2D> { particleTexture };
        }

        public Emitter(params Texture2D[] textures)
            : this(0.0167f, textures) { }

        public Emitter(float spawnRate, Texture2D particleTexture)
            : this(spawnRate) {
            ParticleTextures = new List<Texture2D> { particleTexture };
            _singleTexture = true;
        }

        public Emitter(float spawnRate, params Texture2D[] particleTextures)
            : this(spawnRate) {
            ParticleTextures = new List<Texture2D>(particleTextures);
            _singleTexture = false;
        }

        #endregion

        public override void Draw(SpriteBatch spriteBatch) {
            if (AllParticles.Count == 0) return;
            AllParticles.ForEach(x => x.Draw(spriteBatch));
        }

        public override void Update(float delta, GameTime gameTime) {
            if (AllParticles.Count == 0) return;
            AllParticles.ForEach(x => x.Update(delta, gameTime));
            AllParticles.RemoveAll(x => !x.Alive && x.Config.TimeToLive <= 0f);
        }

        public void SetTexture(Texture2D texture, bool overwrite = false) {
            if (overwrite) ParticleTextures.Clear();
            ParticleTextures.Add(texture);
            _singleTexture = ParticleTextures.Count == 1;
        }

        public void CreateParticle(float delta, ParticleConfig config) {
            if (!Enabled) return;
            _lastSpawn += delta;
            if (_lastSpawn <= SpawnRate * delta) return;
            if (_singleTexture) AllParticles.Add(new Particle(ParticleTextures[0], config));
            else AllParticles.Add(new Particle(ParticleTextures.GetRandom(), config));
            _lastSpawn = 0;
        }
    }
}
