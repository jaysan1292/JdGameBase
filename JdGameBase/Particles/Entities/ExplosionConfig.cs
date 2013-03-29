using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

namespace JdGameBase.Particles.Entities {
    public class ExplosionConfig : FireballConfig {
        public Color DebrisColor = Color.Gray;
        public int DebrisCount = 25;
        public float DebrisLifetime = 1000f;
        public float DebrisSpeed = 0.5f;
        public float DebrisTextureScale = 2f;
        public float MaxDuration = 1000f;

        public ExplosionConfig() {
            BaseFireballColor = Color.Yellow;
            SecondaryFireballColor = Color.Orange;
            TertiaryFireballColor = Color.WhiteSmoke;
            BaseSmokeColor = new Color(new Vector4(0.7f, 0.7f, 0.7f, 0.25f));
            SecondarySmokeColor = new Color(new Vector4(0.55f, 0.55f, 0.55f, 0.25f));
            SmokeLifetime = 2500f;
            SmokeSize = 0.75f;
            SmokeSpeed = 5f;
            Scale = 1.5f;
        }
    }
}
