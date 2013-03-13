// Project: GameAssignment5
// Filename: Skybox.cs
// 
// Author: Jason Recillo

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameAssignment5 {
    //http://rbwhitaker.wikidot.com/skyboxes-1
    public class Skybox {
        private readonly Model _skybox;
        private readonly TextureCube _skyboxTexture;
        private readonly Effect _skyboxEffect;
        private const float Size = 5000f;

        public Skybox(string skyboxTexture, string skyboxCube, string skyboxEffect, ContentManager content) {
            _skybox = content.Load<Model>(skyboxCube);
            _skyboxTexture = content.Load<TextureCube>(skyboxTexture);
            _skyboxEffect = content.Load<Effect>(skyboxEffect);
        }

        public void Draw(Matrix view, Matrix projection, Vector3 cameraPosition) {
            foreach (var pass in _skyboxEffect.CurrentTechnique.Passes) {
                pass.Apply();
                foreach (var mesh in _skybox.Meshes) {
                    foreach (var part in mesh.MeshParts) {
                        part.Effect = _skyboxEffect;
                        part.Effect.Parameters["World"].SetValue(Matrix.CreateScale(Size) *
                                                                 Matrix.CreateTranslation(cameraPosition));
                        part.Effect.Parameters["View"].SetValue(view);
                        part.Effect.Parameters["Projection"].SetValue(projection);
                        part.Effect.Parameters["SkyboxTexture"].SetValue(_skyboxTexture);
                        part.Effect.Parameters["CameraPosition"].SetValue(cameraPosition);
                    }
                    mesh.Draw();
                }
            }
        }
    }
}