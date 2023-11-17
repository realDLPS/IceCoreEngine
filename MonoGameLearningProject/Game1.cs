using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private SpriteBatch _spriteBatch;

        public InputManager IM;
        public Utility Utility;
        /// <summary>
        /// Collision system
        /// </summary>
        public CollisionSystem CS;

        /// <summary>
        /// Graphics
        /// </summary>
        public Graphics GM;

        public float DeltaTime = 0.0f;

        // Debug
        QuadTreeNode CurrentNode;

        // Content
        public Texture2D ball;
        public Texture2D cross;

        public KeyboardState KBState;
        public MouseState MouseState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            Window.AllowUserResizing = true;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Utility = new Utility(_graphics);

            CS = new CollisionSystem();
            CS.CreateQuadTree(512);


            IM = new InputManager();
            IM.AddInputAction(new InputAction(EInputType.Analog, MoveUp));
            IM.QuickAddTriggerToAction(EInput.W, 1f);
            IM.QuickAddTriggerToAction(EInput.S, -1f);

            CurrentNode = CS.QuadTreeRoot;
            
            KBState = Keyboard.GetState();
            MouseState = Mouse.GetState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            GM = new Graphics(_graphics, _spriteBatch, GraphicsDevice);
            GM.SetBorderlessWindow(true);
            GM.SetFullScreen(true);
            GM.ApplySettings();


            cross = Content.Load<Texture2D>("cross");
            ball = Content.Load<Texture2D>("ball");
            CurrentNode.debugTexture = ball;
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            DeltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if(Keyboard.GetState().IsKeyDown(Keys.F11) && !KBState.IsKeyDown(Keys.F11))
            {
                _graphics.IsFullScreen = !_graphics.IsFullScreen;
                if(!_graphics.IsFullScreen)
                {
                    //_graphics.PreferredBackBufferWidth = 640;
                    //_graphics.PreferredBackBufferHeight = 480;
                }
                _graphics.ApplyChanges();
            }

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
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Right) && !KBState.IsKeyDown(Keys.Right))
            {
                if(!CurrentNode.IsRoot)
                {
                    CurrentNode = CurrentNode.Parent.Children[((CurrentNode.ChildIndex+1>3) ? 0 : CurrentNode.ChildIndex + 1)];
                }
            }

            IM.UpdateInputs();

            GM.CameraPosition = GM.CameraPosition + new Vector2(((KeyDown(Keys.D) ? 1 : 0) + (KeyDown(Keys.A) ? -1 : 0)), 0/*, ((KeyDown(Keys.W) ? 1 : 0) + (KeyDown(Keys.S) ? -1 : 0))*/) * 125f * DeltaTime;

            GM.UpdateZoom(GM.CameraZoom + (Mouse.GetState().ScrollWheelValue - MouseState.ScrollWheelValue) * DeltaTime * 0.03f);

            KBState = Keyboard.GetState();
            MouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        protected void MoveUp(float value)
        {
            Debug.WriteLine("Test");
            GM.CameraPosition = GM.CameraPosition + new Vector2(0, value) * 125f * DeltaTime;
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            //CS.QuadTreeRoot.DebugDraw(_spriteBatch, cross);
            //CurrentNode.DebugDrawSingle(_spriteBatch, ball, 1.0f);

            GM.DrawSpriteCentered(GM.GetViewportSize()/2, ball, 1f);
            GM.DrawWorldSpriteCentered(new Vector2(100, 250), cross, .5f);
            GM.DrawWorldSpriteCentered(new Vector2(500, 350), cross, .5f);
            GM.DrawWorldSpriteCentered(new Vector2(-200, -100), cross, .5f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Convinience function
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool KeyDown(Keys key)
        {
            return Keyboard.GetState().IsKeyDown(key);
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