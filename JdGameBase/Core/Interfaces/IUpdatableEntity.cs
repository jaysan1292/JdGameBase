// Project: JdGameBase
// Filename: IUpdatableEntity.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;

namespace JdGameBase.Core.Interfaces {
    public interface IUpdatableEntity : IEntity {
        /// <summary>
        /// Called when this entity should update itself.
        /// </summary>
        /// <param name="delta">The time passed since the last Update (premultiplied by the current TimeScale).</param>
        /// <param name="gameTime">Provides a snapshot of timing values (unaffected by TimeScale).</param>
        void Update(float delta, GameTime gameTime);
    }
}
