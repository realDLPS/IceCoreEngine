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
    public class TestActor : Actor
    {
        private SpriteRenderComponent SRC;
        private CollisionComponent CC;
        public Vector2 Velocity = new Vector2(0.0f);
        private float Rotation = 0.0f;

        public TestActor()
        {
            
        }

        public override void Created()
        {
            base.Created();

            SRC = _actorManager.AddComponentToActor<SpriteRenderComponent>(true, this);
            CC = _actorManager.AddComponentToActor<CollisionComponent>(true, this);
            CC.SetBody(_game.GetWorld().CreateCircle(8f, 1f, GetPosition(), BodyType.Dynamic));
        }
        public override void Update(float deltaTime)
        {
            if(SRC != null)
            {
                SRC.SetTexture(((Game1)_game).ball);
            }
            if(CC != null)
            {
                //CC.GetBody().ApplyForce(Velocity);
                CC.GetBody().LinearVelocity = ICVec2Math.RotateVector(Velocity, Rotation);
            }

            Rotation += deltaTime*90f;

            base.Update(deltaTime);
        }
    }

    public class TestPlayer : Actor
    {
        private SpriteRenderComponent SRC;
        private CollisionComponent CC;
        public Vector2 Velocity = new Vector2(0.0f);

        public TestPlayer()
        {

        }

        public override void Created()
        {
            base.Created();

            SRC = _actorManager.AddComponentToActor<SpriteRenderComponent>(true, this);
            CC = _actorManager.AddComponentToActor<CollisionComponent>(true, this);
            CC.SetBody(_game.GetWorld().CreateCircle(8f, 1f, GetPosition(), BodyType.Dynamic));
        }
        public override void Update(float deltaTime)
        {
            Vector2 Input = new Vector2(_game.GetInputManager().GetActionValue(2), _game.GetInputManager().GetActionValue(1));
            if (Input.LengthSquared() > 0)
            {
                Input.Normalize();
                Input = ICVec2Math.RotateVector(Input, _game.GetGraphicsManager().CameraRotation);
            }



            if (SRC != null)
            {
                SRC.SetTexture(((Game1)_game).ball);
            }
            if (CC != null)
            {
                //CC.GetBody().ApplyForce(Velocity);
                CC.GetBody().LinearVelocity = Input * 500f;
            }

            _game.GetGraphicsManager().CameraPosition = GetPosition();
            base.Update(deltaTime);
        }
    }

    public class SpriteRenderComponent : ActorComponent
    {
        private Texture2D Texture;

        public SpriteRenderComponent()
        {
        }

        public void SetTexture(Texture2D texture)
        {
            Texture = texture;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            _game.GetGraphicsManager().AddWorldSpriteCentered(_owner.GetPosition(), Texture, 1.0f);
        }
    }
}
