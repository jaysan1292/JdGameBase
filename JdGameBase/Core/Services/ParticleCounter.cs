// Project: JdGameBase
// Filename: ParticleCounter.cs
// 
// Author: Jason Recillo

using System;

using JdGameBase.Particles;

namespace JdGameBase.Core.Services {
    public class ParticleCounter {
        public int ParticleCount;

        public ParticleCounter() {
            Particle.OnParticleCreated += (sender, args) => { ParticleCount++; };
            Particle.OnParticleDestroyed += (sender, args) => { ParticleCount--; };
        }
    }
}
