// Project: JdGameBase
// Filename: JdComponent.cs
// 
// Author: Jason Recillo

using System;

using JdGameBase.Core.Services;
using JdGameBase.Extensions;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.GameComponents {
    public abstract class JdComponent : GameComponent {
        public JdComponent(Game game)
            : base(game) { }

        public override void Update(GameTime gameTime) {
            var delta = Game.GetService<TimeScaleManager>().UpdateTimescale(gameTime);
            Update(delta);
            base.Update(gameTime);
        }

        public virtual void Update(float delta) { }
    }
}
