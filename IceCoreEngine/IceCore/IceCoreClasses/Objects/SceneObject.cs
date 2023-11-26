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
    public class SceneObject : IceCoreObject
    {
        protected Transform Transform;

        public SceneObject()
        {
            Transform = new Transform();
        }

        public Transform GetTransform()
        {
            return Transform;
        }
        public void SetTransform(Transform transform)
        {
            Transform = transform;
        }

        public Vector2 GetPosition()
        {
            return Transform.Position;
        }
        public void SetPosition(Vector2 position)
        {
            Transform.Position = position;
        }

        public float GetRotation()
        {
            return Transform.Rotation;
        }
        public void SetRotation(float rotation)
        {
            Transform.Rotation = rotation;
        }

        public Vector2 GetSize()
        {
            return Transform.Size;
        }
        public void SetSize(Vector2 size)
        {
            Transform.Size = size;
        }
    }
}
