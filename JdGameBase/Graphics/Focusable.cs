// Project: JdGameBase
// Filename: Focusable.cs
// 
// Author: Jason Recillo

using System;

using JdGameBase.Core;
using JdGameBase.Core.Interfaces;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Graphics {
    public class Focusable : Entity, IFocusable {
        public Focusable(Vector2 focus) {
            FocusPosition = focus;
        }

        public override Rectangle BoundingBox { get { return new Rectangle((int) FocusPosition.X, (int) FocusPosition.Y, 0, 0); } }
        public Vector2 FocusPosition { get; private set; }

        public override void Update(float delta) { }
        public override void Draw(SpriteBatch spriteBatch) { }
    }
}
