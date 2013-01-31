// Project: JdGameBase
// Filename: ParticleConfig.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Particles {
    public struct ParticleConfig {
        public ParticleBehavior Behavior;
        public Color Color;
        public Vector2 Direction;
        public bool FadeOverTime;
        public float FadeStartTime;
        public Vector2 Position;
        public float Rotation;
        public float RotationVelocity;
        public float Scale;
        public bool ScaleOverTime;
        public float TimeToLive;
        public Vector2 Velocity;
    }
}
