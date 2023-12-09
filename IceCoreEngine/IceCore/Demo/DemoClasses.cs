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
    public class CrossActor : Actor
    {
        private SpriteRenderComponent SRC;
        private CollisionComponent CC;
        private float Rotation = 0.0f;
        public float RotSpeed = 0.0f;

        public CrossActor()
        {

        }

        public override void Created()
        {
            SRC = _actorManager.AddComponentToActor<SpriteRenderComponent>(true, this);
            CC = _actorManager.AddComponentToActor<RectangleCollisionComponent>(true, this);
            CC.GetBody().BodyType = BodyType.Static;
            CC.GetBody().CreateRectangle(2f, 128f, 1f, Vector2.Zero);
            CC.GetBody().CreateRectangle(128f, 2f, 1f, Vector2.Zero);
        }
        public override void Update(float deltaTime)
        {
            if (SRC != null)
            {
                SRC.SetTexture(((Game1)_game).cross);
                SRC._scale = 0.5f;
            }
            if (CC != null)
            {
                Rotation += deltaTime * RotSpeed;
                CC.GetBody().Rotation = ICFloatMath.ConvertDegreesToRadians(Rotation);
                CC.GetBody().LinearVelocity = Vector2.Zero;
            }

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

            Vector2 ForwardVector = ICVec2Math.RotateVector(new Vector2(0f, 1f), _game.GetGraphicsManager().CameraRotation);

            _game.GetCollisionSystem().LinetraceSingle(GetPosition(), GetPosition() + ForwardVector * 500f, LinetraceHit);

            _game.GetGraphicsManager().CameraPosition = GetPosition();
            base.Update(deltaTime);
        }

        public void LinetraceHit(List<LinetraceResponse> hits)
        {
            foreach (var item in hits)
            {
                _game.GetGraphicsManager().AddWorldSpriteCentered(item.Position, ((Game1)_game).ball, 0.5f, 0f);
            }
            
        }
    }

    public class SpriteRenderComponent : ActorComponent
    {
        private Texture2D Texture;
        public float _scale = 1f;

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

            _game.GetGraphicsManager().AddWorldSpriteCentered(_owner.GetPosition(), Texture, _scale, _owner.GetRotation());
        }
    }

    public class DemoButton : ClickableWidget
    {
        public DemoButton()
        {

        }

    }
}
