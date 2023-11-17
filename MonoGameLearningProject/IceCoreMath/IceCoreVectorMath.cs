using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

using System.Collections.Generic;
using System.Reflection.Metadata;

namespace MonoGameLearningProject.IceCoreMath
{
    public class VectorMath2D
    {
        /// <summary>
        /// Calculates a unit vector that points from the origin to the target
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="target"></param>
        /// <returns>Returns a unit vector that points from origin to target. If the origin and target are the same returns {0.0f, 0.0f}</returns>
        public static Vector2 FindLookAtVector(Vector2 origin, Vector2 target)
        {
            Vector2 NonUnitVector = target - origin;

            float VectorLength = NonUnitVector.Length();

            // the vector cannot have a length of less that 0 but we also don't want to divide by zero
            if (VectorLength > 0)
            {
                return NonUnitVector / VectorLength;
            }

            // If the length is 0, which means the vectors are the same return a vector with {0.0f, 0.0f} for its components
            return new Vector2(0.0f);
        }
    }
}
