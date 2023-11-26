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
    public class ActorManager
    {
        protected IceCoreObjectManager _objectManager;

        public ActorManager(IceCoreObjectManager objectManager)
        {
            _objectManager = objectManager;
        }

        /// <summary>
        /// Spawns actor at specified transform
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="canUpdate">Can this actor update?</param>
        /// <param name="transform"></param>
        /// <returns></returns>
        public T SpawnActor<T>(bool canUpdate, Transform transform) where T : Actor, new()
        {
            T NewActor = _objectManager.SpawnObject<T>(canUpdate, false);
            NewActor.SetTransform(transform);
            NewActor.SetManager(this);

            NewActor.Created();

            return NewActor;
        }

        /// <summary>
        /// Creates and adds a component to an actor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="canUpdate">Can this component update?</param>
        /// <param name="actor">Actor to add this component to</param>
        public T AddComponentToActor<T>(bool canUpdate, Actor actor) where T : ActorComponent, new()
        {
            T NewComponent = _objectManager.SpawnObject<T>(canUpdate, false);
            NewComponent.SetOwner(actor);

            actor._components.Add(NewComponent);

            NewComponent.Created();

            return NewComponent;
        }
    }

    public class Actor : SceneObject
    {
        public List<ActorComponent> _components;
        protected ActorManager _actorManager;

        public Actor()
        {
            _components = new List<ActorComponent>();
        }

        public void SetManager(ActorManager actorManager)
        {
            _actorManager = actorManager;
        }
    }

    public class ActorComponent : IceCoreObject
    {
        protected Actor _owner;

        public ActorComponent()
        {
            
        }
        public void SetOwner(Actor owner)
        {
            _owner = owner;
        }
    }
}
