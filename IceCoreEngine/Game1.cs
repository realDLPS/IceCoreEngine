﻿using Microsoft.Xna.Framework;
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

        public Game1()
        {
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize(); // It is recommended that you do not do anything before this


            _graphicsManager.SetBorderlessWindow(true);
            _graphicsManager.SetFullScreen(true);
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

            var temp = _actorManager.SpawnActor<TestActor>(true, new Transform(new Vector2(-25f, 0f)));
            temp.Velocity = new Vector2(25f, 0);
            temp = _actorManager.SpawnActor<TestActor>(true, new Transform(new Vector2(25f, 0f)));
            temp.Velocity = new Vector2(-25f, 0);
        }

        protected override void LoadContent()
        {
            base.LoadContent(); // It is recommended that you do not do anything before this

            cross = Content.Load<Texture2D>("cross");
            ball = Content.Load<Texture2D>("ball");
            // TODO: use this to load your game content
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime); // It is recommended that you do not do anything before this

            Debug.WriteLine(GetWorld().BodyList.Count());

            Move();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GetGraphicsManager().BeginDraw();

            GetGraphicsManager().AddSpriteCentered(_graphicsManager.GetViewportSize() / 2, ball, 1f);
            GetGraphicsManager().AddWorldSpriteCentered(new Vector2(100, 250), cross, .5f);
            GetGraphicsManager().AddWorldSpriteCentered(new Vector2(500, 350), cross, .5f);
            GetGraphicsManager().AddWorldSpriteCentered(new Vector2(-200, -100), cross, .5f);

            GetGraphicsManager().Draw();

            GetGraphicsManager().EndDraw();

            base.Draw(gameTime);
        }

        private void Move()
        {
            Vector2 Input = new Vector2(_inputManager.GetActionValue(2), _inputManager.GetActionValue(1));
            if(Input.LengthSquared() > 0)
            {
                Input.Normalize();
            }
            GetGraphicsManager().CameraRotation = _graphicsManager.CameraRotation + _inputManager.GetActionValue(3) * 1 * GetDeltaTime();
            _graphicsManager.CameraPosition = _graphicsManager.CameraPosition + Input * 125f * GetDeltaTime();
        }

        #region Input callbacks
        protected void ExitGame(float value)
        {
            Exit();
        }
        #endregion
    }
}