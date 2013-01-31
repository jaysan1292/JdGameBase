// Project: JdGameBase
// Filename: JdComponent.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.GameComponents {
    public abstract class JdComponent : GameComponent {
        public JdComponent(Game game)
            : base(game) { }

        public override void Update(GameTime gameTime) {
            var delta = (float) gameTime.ElapsedGameTime.TotalSeconds;
            Update(delta);
            base.Update(gameTime);
        }

        public virtual void Update(float delta) { }
    }
}
