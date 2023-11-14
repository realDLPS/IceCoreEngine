using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameVectorMath;
using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

using System.Collections.Generic;
using System.Reflection.Metadata;

namespace MonoGameLearningProject
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        public Utility Utility;
        /// <summary>
        /// Collision system
        /// </summary>
        public CollisionSystem CS;
        private SpriteBatch _spriteBatch;

        // Debug
        QuadTreeNode CurrentNode;

        // Content
        public Texture2D ball;
        public Texture2D cross;

        public KeyboardState KBState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Utility = new Utility(_graphics);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            CS = new CollisionSystem();
            CS.CreateQuadTree(512);

            CurrentNode = CS.QuadTreeRoot;
            

            KBState = Keyboard.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            cross = Content.Load<Texture2D>("cross");
            ball = Content.Load<Texture2D>("ball");
            CurrentNode.debugTexture = ball;
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(Keyboard.GetState().IsKeyDown(Keys.F) && !KBState.IsKeyDown(Keys.F))
            {
                CurrentNode.Split();
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up) && !KBState.IsKeyDown(Keys.Up))
            {
                if (!CurrentNode.IsRoot)
                {
                    CurrentNode = CurrentNode.Parent;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down) && !KBState.IsKeyDown(Keys.Down))
            {
                if(CurrentNode.HasSplit)
                {
                    Random rnd = new Random();
                    CurrentNode = CurrentNode.Children[rnd.Next(0,3)];
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !KBState.IsKeyDown(Keys.Enter))
            {
                CS.RebuildQuadTree(CS.QuadTreeRoot);
            }
            if(Keyboard.GetState().IsKeyDown(Keys.T) && !KBState.IsKeyDown(Keys.T))
            {
                foreach (QuadTreeNode node in CurrentNode.Neighbours)
                {
                    node.DrawNeighbourInfo = true;
                }
                Debug.WriteLine(CurrentNode.Neighbours.Count());
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Right) && !KBState.IsKeyDown(Keys.Right))
            {
                if(!CurrentNode.IsRoot)
                {
                    CurrentNode = CurrentNode.Parent.Children[((CurrentNode.ChildIndex+1>3) ? 0 : CurrentNode.ChildIndex + 1)];
                }
            }

            // TODO: Add your update logic here
            //Debug.WriteLine(CurrentNode.HasSplit);

            KBState = Keyboard.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            CS.QuadTreeRoot.DebugDraw(_spriteBatch, cross);
            CurrentNode.DebugDrawSingle(_spriteBatch, ball, 1.0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public class Utility
    {
        public GraphicsDeviceManager GDM;

        public Utility(GraphicsDeviceManager _graphics) 
        {
            GDM = _graphics;
        }
        public Vector2 GetViewportSize()
        {
            return new Vector2(GDM.PreferredBackBufferWidth, GDM.PreferredBackBufferHeight);
        }

        public Vector2 ClampToViewport(Vector2 Vector)
        {
            Vector2 ViewportSize = GetViewportSize();
            return Vector2.Clamp(Vector, new Vector2(0.0f), ViewportSize);
        }

        public Vector2 GetViewportBoundMousePosition()
        {
            return ClampToViewport(Mouse.GetState().Position.ToVector2());
        }
    }
}