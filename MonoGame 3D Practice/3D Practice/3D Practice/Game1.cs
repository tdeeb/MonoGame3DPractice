using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace _3D_Practice
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Camera camera = null;
        private Robot robot = null;
        private VertexPositionTexture[] floorVerts = null;
        private BasicEffect effect = null;
        private Texture2D checkerboardTexture = null;

        private Vector3 cameraPosition = new Vector3(15, 10, 10);

        private RenderTarget2D RTarget = null;
        private readonly Vector2 WidthHeight = new Vector2(800, 480);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.GraphicsProfile = GraphicsProfile.Reach;

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            RTarget = new RenderTarget2D(GraphicsDevice, (int)WidthHeight.X, (int)WidthHeight.Y);

            floorVerts = new VertexPositionTexture[6];
            floorVerts[0].Position = new Vector3(20, 0, -20);
            floorVerts[1].Position = new Vector3(20, 0, 20);
            floorVerts[2].Position = new Vector3(-20, 0, -20);

            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(-20, 0, 20);
            floorVerts[5].Position = floorVerts[2].Position;

            int repetitions = 20;

            floorVerts[0].TextureCoordinate = Vector2.Zero;
            floorVerts[1].TextureCoordinate = new Vector2(0, repetitions);
            floorVerts[2].TextureCoordinate = new Vector2(repetitions, 0);

            floorVerts[3].TextureCoordinate = floorVerts[1].TextureCoordinate;
            floorVerts[4].TextureCoordinate = new Vector2(repetitions, repetitions);
            floorVerts[5].TextureCoordinate = floorVerts[2].TextureCoordinate;

            effect = new BasicEffect(GraphicsDevice);
            
            robot = new Robot();
            robot.Initialize(Content);

            camera = new Camera(GraphicsDevice, Window);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            using (FileStream fstream = new FileStream("Content/checkerboard.png", FileMode.Open))
            {
                checkerboardTexture = Texture2D.FromStream(GraphicsDevice, fstream);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            RTarget.Dispose();
            spriteBatch.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            robot.Update(gameTime);
            if (IsActive == true)
                camera.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(RTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawGround();

            robot.Draw(camera);

            GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearWrap, null, null, null, null);

            //Draw RT to back buffer
            spriteBatch.Draw(RTarget, Vector2.Zero, null, Color.White, 0f, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawGround()
        {
            effect.View = camera.ViewMatrix;
            effect.Projection = camera.ProjectionMatrix;

            effect.TextureEnabled = true;
            effect.Texture = checkerboardTexture;

            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, floorVerts,
                    0, 2);
            }
        }
    }
}
