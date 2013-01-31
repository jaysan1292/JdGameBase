// Project: JdGameBase
// Filename: IDrawableEntity.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Core.Interfaces {
    public interface IDrawableEntity {
        void Draw(SpriteBatch spriteBatch);
    }
}
