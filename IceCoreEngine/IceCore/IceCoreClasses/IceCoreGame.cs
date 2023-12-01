using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

using System.Collections.Generic;
using System.Reflection.Metadata;
using nkast.Aether.Physics2D.Dynamics;

namespace IceCoreEngine
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
        /// Used to create objects
        /// </summary>
        protected IceCoreObjectManager _objectManager;

        /// <summary>
        /// Simplifies creating actors
        /// </summary>
        protected ActorManager _actorManager;

        /// <summary>
        /// Aether world
        /// </summary>
        protected World _world;
        /// <summary>
        /// Gravity
        /// </summary>
        protected Vector2 _gravity = new Vector2(0f);

        /// <summary>
        /// Time since last frame
        /// </summary>
        private float _deltaTime = 0.0f;

        protected float _timeScale = 1.0f;

        public IceCoreGame()
        {
            _graphics = new GraphicsDeviceManager(this);
        }

        protected override void Initialize()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _inputManager = new InputManager();
            _graphicsManager = new GraphicsManager(_graphics, _spriteBatch, Window);
            _objectManager = new IceCoreObjectManager(this);
            _actorManager = new ActorManager(_objectManager);
            _world = new World(_gravity);
            _collisionSystem = new CollisionSystem(this);

            base.Initialize();
        }

        protected override void LoadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            _deltaTime = Convert.ToSingle(gameTime.ElapsedGameTime.TotalSeconds);
            
            _world.Step(GetDeltaTime());

            _inputManager.UpdateInputs();
            _objectManager.UpdateObjects(GetDeltaTime());


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }

        #region Delta time
        /// <summary>
        /// Get scaled delta time
        /// </summary>
        /// <returns></returns>
        public float GetDeltaTime()
        {
            return _deltaTime * _timeScale;
        }

        /// <summary>
        /// Gets unscaled delta time
        /// </summary>
        /// <returns></returns>
        public float GetUnscaledDeltaTime()
        {
            return _deltaTime;
        }
        #endregion

        #region Subsystem getting
        public GraphicsManager GetGraphicsManager()
        {
            return _graphicsManager;
        }
        public SpriteBatch GetSpriteBatch()
        {
            return _spriteBatch;
        }
        public InputManager GetInputManager()
        {
            return _inputManager;
        }
        public IceCoreObjectManager GetObjectManager()
        {
            return _objectManager;
        }
        public ActorManager GetActorManager()
        {
            return _actorManager;
        }
        public World GetWorld()
        {
            return _world;
        }
        public CollisionSystem GetCollisionSystem()
        {
            return _collisionSystem;
        }
        #endregion
    }
}