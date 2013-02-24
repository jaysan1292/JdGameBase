// Project: JdGameBase
// Filename: Focusable.cs
// 
// Author: Jason Recillo

using System;

using JdGameBase.Core;
using JdGameBase.Core.Interfaces;
using JdGameBase.Extensions;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Camera {
    /// <summary>
    /// Allows the camera to focus on any arbitrary point.
    /// </summary>
    public class Focusable : Entity, IFocusable {
        public Focusable(Vector2 focus) {
            FocusPosition = focus;
        }

        public override Rectangle BoundingBox { get { return FocusPosition.ToRectangle(0, 0); } }
        public Vector2 FocusPosition { get; private set; }

        public override void Draw(SpriteBatch spriteBatch) { }
        public override void Update(float delta, GameTime gameTime) { }
    }
}
