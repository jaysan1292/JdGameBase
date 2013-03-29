using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace JdGameBase.Particles.Entities {
    public class FireballConfig {
        public Color BaseFireballColor = new Color(new Vector4(0.99607f, 0.87058f, 0f, 1f));
        public Color BaseSmokeColor = new Color(new Vector4(1.15f / 2, 0.15f, 0.15f, 1f));
        public float Scale = 1f;
        public Color SecondaryFireballColor = new Color(new Vector4(1f, 0.5f, 0f, 1f));
        public Color SecondarySmokeColor = new Color(new Vector4(0.05f, 0.05f, 0.05f, 0.85f));
        public float SmokeLifetime = 5000f;
        public float SmokeSize = 1f;
        public float SmokeSpeed = 1f;
        public Color TertiaryFireballColor = new Color(new Vector4(1f, 1f, 1f, 1f));
    }
}
