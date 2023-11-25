using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IceCoreEngine
{
    public class TestActor : Actor
    {
        public Game1 Game1;
        public TestActor()
        {
            
        }

        public override void Created()
        {
            base.Created();

            
        }
        public override void Update(float deltaTime)
        {
            if(Game1 != null && _components.Count() == 0)
            {
                var temp = _actorManager.AddComponentToActor<SpriteRenderComponent>(true, this);
                temp.SetTexture(Game1.ball);
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
            Debug.WriteLine(Texture);

            _game.GetGraphicsManager().AddWorldSpriteCentered(_owner.GetPosition(), Texture, 1.0f);
        }
    }
}
