using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Core.Interfaces {
    public interface ISprite: IDrawableEntity {
        Color Color { get; set; }
        SpriteEffects Effects { get; set; }
        float LayerDepth { get; set; }
        Vector2 Origin { get; set; }
        Vector2 Position { get; set; }
        float Rotation { get; set; }
        float Scale { get; set; }
        Rectangle? SourceRect { get; set; }
        Texture2D Texture { get; set; }

        float Width { get; }
        float Height { get; }
    }
}