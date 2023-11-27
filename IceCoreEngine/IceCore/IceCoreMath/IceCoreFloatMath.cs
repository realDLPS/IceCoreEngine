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
    public class ICFloatMath
    {
        public static float ConvertDegreesToRadians(float degrees)
        {
            return degrees * ((float)Math.PI / 180f);
        }
        public static float ConvertRadiansToDegrees(float radians)
        {
            return radians / ((float)Math.PI / 180f);
        }
    }
}
