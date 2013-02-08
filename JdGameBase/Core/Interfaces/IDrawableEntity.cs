// Project: JdGameBase
// Filename: IDrawableEntity.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Core.Interfaces {
    public interface IDrawableEntity : IEntity {
        void Draw(SpriteBatch spriteBatch);
    }
}
