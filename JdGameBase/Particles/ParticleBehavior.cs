using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace JdGameBase.Particles {
    public enum ParticleBehavior {
        /// <summary>
        /// Particle default behavior.
        /// </summary>
        Default,

        /// <summary>
        /// Particles will move in the specified direction, at the specified speed. Requires Direction config property to be set.
        /// </summary>
        Directional
    }
}
