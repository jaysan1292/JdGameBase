// Project: JdGameBase
// Filename: SpriteBatchOptions.cs
// 
// Author: Jason Recillo

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JdGameBase.Utils {
    public class SpriteBatchOptions {
        public BlendState BlendState;
        public DepthStencilState DepthStencilState;
        public Effect Effect;
        public RasterizerState RasterizerState;
        public SamplerState SamplerState;
        public SpriteSortMode SpriteSortMode = SpriteSortMode.Deferred;
        public Matrix TransformMatrix;

        /// <summary>
        /// Simply calls SpriteBatch.Begin() on the given SpriteBatch with the options set.
        /// </summary>
        public void BeginDraw(SpriteBatch batch) {
            batch.Begin(SpriteSortMode, BlendState, SamplerState, DepthStencilState, RasterizerState, Effect, TransformMatrix);
        }
    }
}
