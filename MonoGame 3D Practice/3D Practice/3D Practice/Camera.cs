using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3D_Practice
{
    public class Camera
    {
        private GraphicsDevice graphicsDevice = null;

        /// <summary>
        /// This serves as the camera's world transform and holds position, rotation, and scale information.
        /// This allows moving the camera properly through the world space.
        /// </summary>
        private Matrix cameraWorld = Matrix.CreateWorld(new Vector3(0f, 1f, 10f), Vector3.Forward, Vector3.Up);

        /// <summary>
        /// The view matrix is created from the cameras world matrix but has special properties.
        /// Using CreateLookAt to create this matrix, you move from world space to view space.
        /// If you are working with world objects, you should not take individual elements from this to directly operate on world matrix components.
        /// In addition, the multiplication of a view matrix by a world matrix moves the resulting matrix into view space itself.
        /// </summary>
        private Matrix viewMatrix = Matrix.Identity;

        private MouseState mState = default(MouseState);
        private KeyboardState kbState = default(KeyboardState);

        private GameWindow gameWindow = null;

        private float unitsPerSecond = 5;
        private float anglesPerSecond = 3f;

        public float FieldOfView = MathHelper.PiOver4;
        public float NearClipPlane = .1f;
        public float FarClipPlane = 200f;

        private bool Angling = false;

        public Matrix ViewMatrix
        {
            get
            {
                RecreateViewMatrix();

                return viewMatrix;
            }
        }

        public Matrix ProjectionMatrix
        {
            get
            {
                float aspectRatio = graphicsDevice.Viewport.Width / (float)graphicsDevice.Viewport.Height;

                return Matrix.CreatePerspectiveFieldOfView(FieldOfView, aspectRatio, NearClipPlane, FarClipPlane);
            }
        }

        public Vector3 Position
        {
            get => cameraWorld.Translation;
            set
            {
                cameraWorld.Translation = value;

                RecreateViewMatrix();
            }
        }

        public Vector3 LookAtDirection
        {
            get => cameraWorld.Forward;
            set
            {
                cameraWorld = Matrix.CreateWorld(cameraWorld.Translation, value, cameraWorld.Up);
                // since we know here that a change has occured to the cameras world orientations we can update the view matrix.
                RecreateViewMatrix();
            }
        }

        public Camera(GraphicsDevice gfxDevice, GameWindow window)
        {
            graphicsDevice = gfxDevice;
            gameWindow = window;
            RecreateViewMatrix();
        }

        private void RecreateViewMatrix()
        {
            viewMatrix = Matrix.CreateLookAt(cameraWorld.Translation, cameraWorld.Forward + cameraWorld.Translation, cameraWorld.Up);
        }

        public void Update(GameTime gameTime)
        {
            MouseState state = Mouse.GetState(gameWindow);
            KeyboardState kstate = Keyboard.GetState();

            float unitsPS = unitsPerSecond;

            if (kstate.IsKeyDown(Keys.LeftShift) == true || kstate.IsKeyDown(Keys.RightShift) == true)
            {
                unitsPS += 3f;
            }

            if (kstate.IsKeyDown(Keys.Space) && kbState.IsKeyDown(Keys.Space) == false)
            {
                //Reset everything
                cameraWorld.Up = Vector3.Up;
                Position = new Vector3(0f, 1f, 10f);
                LookAtDirection = Vector3.Forward;
            }

            if (kstate.IsKeyDown(Keys.W))
            {
                Position += (cameraWorld.Forward * unitsPS) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (kstate.IsKeyDown(Keys.S) == true)
            {
                Position += (cameraWorld.Backward * unitsPS) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kstate.IsKeyDown(Keys.A) == true)
            {
                Position += (cameraWorld.Left * unitsPS) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (kstate.IsKeyDown(Keys.D) == true)
            {
                Position += (cameraWorld.Right * unitsPS) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (kstate.IsKeyDown(Keys.Q) == true)
            {
                Position += (Vector3.Down * unitsPS) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else if (kstate.IsKeyDown(Keys.E) == true)
            {
                Position += (Vector3.Up * unitsPS) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (state.RightButton == ButtonState.Pressed)
            {
                if (Angling == false)
                {
                    Angling = true;
                }
                else
                {
                    Vector2 diff = state.Position.ToVector2() - mState.Position.ToVector2();

                    if (diff.X != 0f)
                    {
                        float diff2 = -(diff.X * anglesPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                        //Rotate about the camera's Up vector, then transform the matrix and normalize it
                        Matrix matrix = Matrix.CreateFromAxisAngle(cameraWorld.Up, MathHelper.ToRadians(diff2));
                        LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
                        RecreateViewMatrix();
                    }

                    if (diff.Y != 0f)
                    {
                        float diff2 = -(diff.Y * anglesPerSecond) * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    
                        //Rotate about the camera's Right vector, then transform the matrix and normalize it
                        Matrix matrix = Matrix.CreateFromAxisAngle(cameraWorld.Right, MathHelper.ToRadians(diff2));
                        LookAtDirection = Vector3.TransformNormal(LookAtDirection, matrix);
                        RecreateViewMatrix();
                    }
                }
            }
            else
            {
                Angling = false;
            }

            mState = state;
            kbState = kstate;
        }
    }
}
