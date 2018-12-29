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
        public Vector3 Position = Vector3.Zero;
        public Vector3 Rotation = Vector3.Zero;
        public Vector3 Scale = Vector3.One;

        private Model model = null;

        public void Initialize(ContentManager contentManager)
        {
            model = contentManager.Load<Model>("untitled");

            Position.Y = 1f;
        }

        public void Update(GameTime gameTime)
        {
            Rotation.X += .01f;
            Rotation.Z += .01f;
            Rotation.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position.X = (float)Math.Cos(Rotation.Y) * 8f;
            Position.Z = (float)Math.Sin(Rotation.Y) * 8f;
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
            //const float circleRadius = 8;
            //const float heightOffGround = 1;
            
            //This matrix moves the model "out" from the origin
            //Matrix translationMatrix = Matrix.CreateTranslation(circleRadius, heightOffGround, 0f);
            //
            ////This matrix rotates everything around the origin
            //Matrix rotationMatrix = Matrix.CreateRotationY(angle);
            //
            ////We combine the two to have the model move in a circle
            //Matrix combined = translationMatrix * rotationMatrix;

            //This matrix rotates everything around the origin
            Matrix rotationX = Matrix.CreateRotationX(Rotation.X);
            Matrix rotationY = Matrix.CreateRotationY(Rotation.Y);
            Matrix rotationZ = Matrix.CreateRotationZ(Rotation.Z);

            //This matrix moves the model "out" from the origin
            Matrix translationMatrix = Matrix.CreateTranslation(Position);

            //We combine everything:
            //1. First apply the rotation with the object at the origin
            //2. Translate the object to its position
            Matrix combined = (rotationX * rotationY * rotationZ) * translationMatrix;

            return combined;
        }
    }
}
