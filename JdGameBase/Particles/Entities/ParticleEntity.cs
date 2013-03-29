using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using JdGameBase.Core;

namespace JdGameBase.Particles.Entities {
    public abstract class ParticleEntity : Entity {
        public abstract int TotalParticleCount();
    }
}
