// Project: JdGameBase
// Filename: JdComponent.cs
// 
// Author: Jason Recillo

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

using JdGameBase.Core.Services;
using JdGameBase.Extensions;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.GameComponents {
    public abstract class JdComponent : GameComponent {
        public JdComponent(JdGame game)
            : base(game) { }

        public override void Update(GameTime gameTime) {
            var delta = Game.GetService<TimeScaleManager>().UpdateTimescale(gameTime);
            Update(delta, gameTime);
            base.Update(gameTime);
        }

        public new JdGame Game {
            get {
                Debug.Assert(base.Game is JdGame);
                return (JdGame) base.Game;
            }
        }

        /// <summary>
        /// Called when this GameComponent needs to be updated. Override this method with component-specific update code.
        /// </summary>
        /// <param name="delta">Time elapsed since the last call to Update (affected by TimeScale).</param>
        /// <param name="gameTime">Time elapsed since the last call to Update (unaffected by TimeScale).</param>
        public virtual void Update(float delta, GameTime gameTime) { }
    }
}
