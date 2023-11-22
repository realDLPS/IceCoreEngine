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
    public class IceCoreObjectManager
    {
        private Dictionary<Guid, IceCoreObject> _objects;


        public IceCoreObjectManager()
        {
            _objects = new Dictionary<Guid, IceCoreObject>();
        }

        public void UpdateObjects(float deltaTime)
        {
            foreach (IceCoreObject icobject in _objects.Values.ToList())
            {
                // Having two dictionaries may be faster, no guarantees.
                // For now this is good enough
                if(icobject.CanUpdate())
                {
                    icobject.Update(deltaTime);
                }
            }
        }

        public T SpawnObject<T>(bool canUpdate) where T : IceCoreObject, new()
        {
            T NewIceCoreObject = new T();

            NewIceCoreObject.SetGuid(Guid.NewGuid());
            NewIceCoreObject.SetCanUpdate(canUpdate);

            NewIceCoreObject.Created();

            return NewIceCoreObject;
        }
        public void RemoveObject(Guid guid)
        {
            _objects.Remove(guid);
        }
    }

    public class IceCoreObject
    {
        protected Guid _guid;
        protected IceCoreObjectManager _manager;
        protected bool _canUpdate;

        public IceCoreObject()
        {

        }
        /// <summary>
        /// Called when this object is created by ObjectManager
        /// 
        /// Can also be called manually.
        /// </summary>
        public void Created()
        {

        }
        /// <summary>
        /// Called each update
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {

        }
        /// <summary>
        /// Called when this object should be destroyed
        /// </summary>
        public void Destroy()
        {
            _manager.RemoveActor(_guid);
        }
        /// <summary>
        /// Sets the guid for this object
        /// </summary>
        /// <param name="guid"></param>
        public void SetGuid(Guid guid)
        {
            _guid = guid;
        }
        /// <summary>
        /// Returns the guid for this object
        /// </summary>
        /// <returns></returns>
        public Guid GetGuid()
        {
            return _guid;
        }
        /// <summary>
        /// Sets does this object use updates
        /// </summary>
        /// <param name="canUpdate"></param>
        public void SetCanUpdate(bool canUpdate)
        {
            _canUpdate = canUpdate;
        }
        /// <summary>
        /// Gets can this object update
        /// </summary>
        /// <returns></returns>
        public bool CanUpdate()
        {
            return _canUpdate;
        }
    }
}
