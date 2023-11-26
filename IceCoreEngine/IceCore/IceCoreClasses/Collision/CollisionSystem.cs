using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

using System.Collections.Generic;
using System.Reflection.Metadata;
using nkast.Aether.Physics2D;
using nkast.Aether.Physics2D.Collision;
using nkast.Aether.Physics2D.Dynamics;

namespace IceCoreEngine
{
    /// <summary>
    /// 
    /// </summary>
    public class CollisionSystem
    {
        public CollisionSystem()
        {
            
        }




        /// <summary>
        /// Corners and edges count as overlap, makes some stuff easier, and the function way simpler
        /// 
        /// Currently not used. May be used for UI in the future.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>True if boxes overlap</returns>
        public static bool IceCoreAABB(Square a, Square b)
        {
            bool IsARight = false;
            if (a.Position.X >= b.Position.X)
            {
                IsARight = true;
            }

            // Checks is the box to the left too far away from the box on the right on the X-axis
            if (IsARight && a.Position.X - a.Size.X / 2 > b.Position.X + b.Size.X / 2)
            {
                return false;
            }
            else if (b.Position.X - b.Size.X / 2 > a.Position.X + a.Size.X / 2)
            {
                return false;
            }

            bool IsAHigh = false;
            if (a.Position.Y >= b.Position.Y)
            {
                IsAHigh = true;
            }

            // Checks if the lower box is too low from the box that is higher on the Y-axis
            if (IsAHigh && a.Position.Y - a.Size.Y / 2 > b.Position.Y + b.Size.Y / 2)
            {
                return false;
            }
            else if (b.Position.Y - b.Size.Y / 2 > a.Position.Y + a.Size.Y / 2)
            {
                return false;
            }

            return true;
        }
    }
}
