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
        public Game1 Game1;
        private SpriteRenderComponent SRC;
        private CollisionComponent CC;
        public Vector2 Velocity = new Vector2(0.0f);

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
                CC.GetBody().ApplyForce(Velocity);
                Debug.WriteLine(CC.GetBody().LinearVelocity);
            }
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
