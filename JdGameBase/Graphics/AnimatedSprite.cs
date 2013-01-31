// Project: JdGameBase
// Filename: AnimatedSprite.cs
// 
// Author: Jason Recillo

using System;

using JdGameBase.Core;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Graphics {
    public class AnimatedSprite : Entity {
        private readonly int _height;
        private readonly int _totalFrames;

        private readonly int _width;
        public int Columns;
        public Vector2 Location;
        public int Rows;
        public Texture2D Texture;
        private int _currentFrame;

        public AnimatedSprite(Texture2D texture, int rows, int columns) {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            Location = Vector2.Zero;

            _currentFrame = 0;
            _totalFrames = rows * columns;
            _width = Texture.Width / Columns;
            _height = Texture.Height / Rows;
        }

        public override Rectangle BoundingBox { get { return new Rectangle(0, 0, _width, _height); } }

        public override void Draw(SpriteBatch spriteBatch) {
            var row = (int) ((float) _currentFrame / Columns);
            var column = _currentFrame % Columns;

            var sourceRectangle = new Rectangle(_width * column, _height * row, _width, _height);
            var destinationRectangle = new Rectangle((int) Location.X, (int) Location.Y, _width, _height);

            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
        }

        public override void Update(float delta) {
            _currentFrame = (_currentFrame + 1) % _totalFrames;
        }
    }
}
