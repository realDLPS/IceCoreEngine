using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

using System.Collections.Generic;
using System.Reflection.Metadata;

namespace IceCoreEngine
{
    public class Game1 : IceCoreGame
    {
        // Content
        public Texture2D ball;
        public Texture2D cross;
        public Texture2D dot;


        public Game1()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize(); // It is recommended that you do not do anything before this


            _graphicsManager.SetBorderlessWindow(true);
            _graphicsManager.SetFullScreen(false);
            _graphicsManager.SetAllowResize(true);
            _graphicsManager.SetMouseClamppingToWindow(false);
            _graphicsManager.SetMouseVisibility(false);
            _graphicsManager.ApplySettings();

            _inputManager.AddInputAction(new InputAction(EInputType.Digital, ExitGame));
            _inputManager.QuickAddTriggerToAction(EInput.Escape, 1f);

            _inputManager.AddInputAction(new InputAction(EInputType.Analog));
            _inputManager.QuickAddTriggerToAction(EInput.W, 1f);
            _inputManager.QuickAddTriggerToAction(EInput.S, -1f);

            _inputManager.AddInputAction(new InputAction(EInputType.Analog));
            _inputManager.QuickAddTriggerToAction(EInput.D, 1f);
            _inputManager.QuickAddTriggerToAction(EInput.A, -1f);

            _inputManager.AddInputAction(new InputAction(EInputType.Analog));
            _inputManager.QuickAddTriggerToAction(EInput.Right, -1f);
            _inputManager.QuickAddTriggerToAction(EInput.Left, 1f);

            var temp = _actorManager.SpawnActor<TestActor>(true, new Transform(new Vector2(-250f, 0f)));
            temp.Velocity = new Vector2(100f, 0);
            temp = _actorManager.SpawnActor<TestActor>(true, new Transform(new Vector2(250f, 0f)));
            temp.Velocity = new Vector2(-50f, 0);

            var temp2 = _actorManager.SpawnActor<CrossActor>(true, new Transform(new Vector2(100, 250)));
            temp2.RotSpeed = 20f;
            _actorManager.SpawnActor<CrossActor>(true, new Transform(new Vector2(500, 350)));
            _actorManager.SpawnActor<CrossActor>(true, new Transform(new Vector2(-200, -100)));

            _actorManager.SpawnActor<TestPlayer>(true, new Transform(new Vector2(0f)));
        }

        protected override void LoadContent()
        {
            base.LoadContent(); // It is recommended that you do not do anything before this

            cross = Content.Load<Texture2D>("cross");
            ball = Content.Load<Texture2D>("ball");
            dot = Content.Load<Texture2D>("dot");
            // TODO: use this to load your game content
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // It is recommended that you do not do anything before this

            GetGraphicsManager().CameraRotation = _graphicsManager.CameraRotation + _inputManager.GetActionValue(3) * 25 * GetDeltaTime();

            //Move();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GetGraphicsManager().BeginDraw();

            GetGraphicsManager().Draw();

            GetGraphicsManager().EndDraw();

            GetUIManager().Draw();

            base.Draw(gameTime);
        }
        #region Input callbacks
        protected void ExitGame(float value)
        {
            Exit();
        }
        #endregion
    }
}