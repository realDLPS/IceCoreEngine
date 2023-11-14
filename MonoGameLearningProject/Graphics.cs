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
    public class Graphics
    {
        //// Camera variables
        /// <summary>
        /// Camera world position
        /// </summary>
        public Vector2 CameraPosition = new Vector2(0.0f);
        public float CameraZoom = 1.0f;

        //// Static variables
        private GraphicsDeviceManager GDM;


        //// Constructor

        public Graphics(GraphicsDeviceManager _graphics)
        {
            GDM = _graphics;
        }


        //// Functions
        
        /// <summary>
        /// WARNING! Not tested yet, if things move around weirdly while zooming or moving camera then this function is the issue
        /// 
        /// Converts a world position to view space
        /// You may use IsSquareInView to check is this position on screen
        /// </summary>
        /// <param name="WorldPosition"></param>
        /// <returns>Viewspace vector2</returns>
        public Vector2 WorldToViewSpace(Vector2 worldPosition)
        {
            // Let's convert the worldspace worldPosition to a cameraspace position
            Vector2 CameraSpacePosition = worldPosition - CameraPosition;
            // Now this CameraSpacePosition is {0, 0} if it's in the same spot as the camera

            // When we zoom the position moves away from the center and when we unzoom it moves closer
            Vector2 ZoomedPosition = CameraSpacePosition * CameraZoom;

            // We need to account for different screensizes, but we also want to avoid streching so let's say we scale using the larger difference from our base resolution
            Vector2 ViewportSize = GetViewportSize();
            float ScreenSizeScaling = ((ViewportSize.X / 1920 >= ViewportSize.Y / 1080) ? ViewportSize.X / 1920 : ViewportSize.Y / 1080);
            Vector2 ScreenSizedPosition = ZoomedPosition * ScreenSizeScaling;

            // Lastly we want to move this vector into proper position as the camera position is in the center of the screen instead of the pixel {0,0} that is in the top left corner
            return (ScreenSizedPosition - (ViewportSize/2));
        }

        /// <summary>
        /// WARNING! Not tested yet, if sprites appear to be dissapearing too soon this may be the issue
        /// 
        /// Used to check is a vector2 in view.
        /// </summary>
        /// <param name="viewSpacePosition"></param>
        /// <returns>Returns true if position is on screen</returns>
        public bool IsSquareInView(Square square)
        {
            Vector2 ViewportSize = GetViewportSize();

            if (square.Position.X + square.Size < 0 || square.Position.Y + square.Size < 0 || square.Position.X > ViewportSize.X + square.Size || square.Position.Y > ViewportSize.Y + square.Size)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns viewport size as a Vector2</returns>
        public Vector2 GetViewportSize()
        {
            return new Vector2(GDM.PreferredBackBufferWidth, GDM.PreferredBackBufferHeight);
        }
    }
}
