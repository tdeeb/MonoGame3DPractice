using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace _3D_Practice
{
    public class Robot
    {
        private Model model = null;
        private float angle;

        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("untitled");
        }

        public void Update(GameTime gameTime)
        {
            angle += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(Camera camera)
        {
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;

                    effect.World = GetWorldMatrix();
                    effect.View = camera.ViewMatrix;
                    effect.Projection = camera.ProjectionMatrix;
                }

                mesh.Draw();
            }
        }

        private Matrix GetWorldMatrix()
        {
            const float circleRadius = 8;
            const float heightOffGround = 1;

            //This matrix moves the model "out" from the origin
            Matrix translationMatrix = Matrix.CreateTranslation(circleRadius, heightOffGround, 0f);

            //This matrix rotates everything around the origin
            Matrix rotationMatrix = Matrix.CreateRotationY(angle);

            //We combine the two to have the model move in a circle
            Matrix combined = translationMatrix * rotationMatrix;

            return combined;
        }
    }
}
