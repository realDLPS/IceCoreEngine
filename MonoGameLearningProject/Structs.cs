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
    public struct InputAction
    {
        /// <summary>
        /// If InputType is digital, delegate will return either 0 or 1. Triggers are first summed up and then clamped to a range of 0 to 1,
        /// then if the value is higher that 0 a value of 1 will be returned.
        /// 
        /// If InputType is analog, delegate will return the sum of all triggers, this may be greater that 1 or less that -1
        /// </summary>
        public EInputType InputType;
        /// <summary>
        /// A list of triggers for this input action
        /// 
        /// Each Trigger has a button (keyboard or mouse) and Multiplier
        /// </summary>
        public List<(EInput Button, float Multiplier)> Triggers;
        public InputDelegate Delegate;
    }
}
