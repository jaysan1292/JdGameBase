// Project: JdGameBase
// Filename: JdDrawableComponent.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.GameComponents {
    public class JdDrawableComponent : DrawableGameComponent {
        public JdDrawableComponent(Game game)
            : base(game) { }

        public override void Update(GameTime gameTime) {
            var delta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            Update(delta);
            base.Update(gameTime);
        }

        public virtual void Update(float delta) { }
    }
}
