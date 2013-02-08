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
            Update(delta, gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Called when this GameComponent needs to be updated. Override this method with component-specific update code.
        /// </summary>
        /// <param name="delta">Time elapsed since the last call to Update (affected by TimeScale).</param>
        /// <param name="gameTime">Time elapsed since the last call to Update (unaffected by TimeScale).</param>
        public virtual void Update(float delta, GameTime gameTime) { }
    }
}
