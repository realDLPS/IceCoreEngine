using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonoGameVectorMath;
using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

using System.Collections.Generic;
using System.Reflection.Metadata;

namespace MonoGameLearningProject
{
    public struct Square
    {
        public Vector2 Position;
        public Vector2 Size;

        public Square(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
        }
    }
}
