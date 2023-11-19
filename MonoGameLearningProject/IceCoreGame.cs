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
    /// <summary>
    /// Creates all subsystems such as input and graphics for you
    /// </summary>
    public class IceCoreGame : Game
    {
        protected GraphicsDeviceManager _graphics;
        protected SpriteBatch _spriteBatch;

        /// <summary>
        /// Input manager
        /// </summary>
        protected InputManager _inputManager;
        /// <summary>
        /// Collision system
        /// WARNING! Not functional yet
        /// </summary>
        private CollisionSystem _collisionSystem;
        /// <summary>
        /// Graphics manager
        /// </summary>
        protected GraphicsManager _graphicsManager;

        /// <summary>
        /// Time since last frame
        /// </summary>
        protected float _deltaTime = 0.0f;

        public IceCoreGame()
        {
            _graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _collisionSystem = new CollisionSystem();
            _inputManager = new InputManager();
            _graphicsManager = new GraphicsManager(_graphics, _spriteBatch, Window);

            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            _deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);

            _inputManager.UpdateInputs();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}