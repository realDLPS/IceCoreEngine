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
        }

        public override void Created()
        {
            _body = _game.GetWorld().CreateBody(_owner.GetPosition());
        }
        /// <summary>
        /// Removes the old body
        /// </summary>
        /// <param name="body"></param>
        public void SetBody(Body body)
        {
            if(_body != null)
            {
                _game.GetWorld().Remove(_body); // Not checked, might be slow
            }

            _body = body;
        }
        public Body GetBody()
        {
            return _body;
        }
        public override void Update(float deltaTime)
        {
            _owner.SetPosition(_body.Position);
            _owner.SetRotation(ICFloatMath.ConvertRadiansToDegrees(_body.Rotation));

            base.Update(deltaTime);
        }
        public void UpdatePosition(Vector2 position)
        {
            _body.Position = position;
        }
        public void ClearFixtures()
        {
            foreach (Fixture fixture in _body.FixtureList)
            {
                _body.Remove(fixture);
            }
        }
        public void AddFixture(Fixture fixture)
        {
            _body.Add(fixture);
        }
    }

    /// <summary>
    /// Kind of worthless currently
    /// </summary>
    public class RectangleCollisionComponent : CollisionComponent
    {
        public Vector2 Size = new Vector2(1f);

        public RectangleCollisionComponent()
        {

        }

        public override void Created()
        {
            SetBody(_game.GetWorld().CreateRectangle(Size.X, Size.Y, 1f, _owner.GetPosition(), _owner.GetRotation(), BodyType.Dynamic));
        }
        public void ChangeSize(Vector2 size)
        {
            Size = size;
            ClearFixtures();
            GetBody().CreateRectangle(Size.X, Size.Y, 1f, Vector2.Zero);
        }
    }
}
