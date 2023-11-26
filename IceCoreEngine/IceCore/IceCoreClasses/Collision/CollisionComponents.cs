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
    public class CollisionComponent : ActorComponent
    {
        private Body _body;

        public CollisionComponent()
        {
            _body = _game.GetWorld().CreateBody();
            _body.OnCollision
        }
        /// <summary>
        /// Removes the old body
        /// </summary>
        /// <param name="body"></param>
        protected void SetBody(Body body)
        {
            _game.GetWorld().Remove(_body); // Not checked, might be slow
            _body = body;
        }
        public Body GetBody()
        {
            return _body;
        }
        public override void Update(float deltaTime)
        {
            _owner.SetPosition(_body.Position);

            base.Update(deltaTime);
        }
        public void UpdatePosition(Vector2 position)
        {
            _body.Position = position;
        }
    }
}
