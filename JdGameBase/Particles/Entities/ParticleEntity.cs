// Project: JdGameBase
// Filename: ParticleEntity.cs
// 
// Author: Jason Recillo

using System;

using JdGameBase.Core;

namespace JdGameBase.Particles.Entities {
    public abstract class ParticleEntity : Entity {
        public abstract int TotalParticleCount();
    }
}
