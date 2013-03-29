using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
