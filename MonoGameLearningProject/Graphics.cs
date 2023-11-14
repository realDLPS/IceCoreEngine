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
        private SpriteBatch SpriteBatch;
        private GraphicsDevice GraphicsDevice;


        //// Constructor

        public Graphics(GraphicsDeviceManager _graphics, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            GDM = _graphics;
            SpriteBatch = spriteBatch;
            GraphicsDevice = graphicsDevice;
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
            // As positive is usually up we multiply the world position and camera position Y values by -1 to make them work as expected
            Vector2 CameraSpacePosition = worldPosition * new Vector2(1, -1) - CameraPosition * new Vector2(1, -1);
            // Now this CameraSpacePosition is {0, 0} if it's in the same spot as the camera

            // When we zoom the position moves away from the center and when we unzoom it moves closer
            Vector2 ZoomedPosition = CameraSpacePosition * CameraZoom;

            // We need to account for different screensizes, but we also want to avoid streching so let's say we scale using the larger difference from our base resolution
            Vector2 ViewportSize = GetViewportSize();
            float ScreenSizeScaling = ((ViewportSize.X / 1920 >= ViewportSize.Y / 1080) ? ViewportSize.X / 1920 : ViewportSize.Y / 1080);
            Vector2 ScreenSizedPosition = ZoomedPosition * ScreenSizeScaling;

            // Lastly we want to move this vector into proper position as the camera position is in the center of the screen instead of the pixel {0,0} that is in the top left corner
            return (ScreenSizedPosition + (ViewportSize/2));
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

            if (square.Position.X + square.Size.X / 2 < 0 || square.Position.Y + square.Size.Y / 2 < 0 || square.Position.X > ViewportSize.X + square.Size.X / 2 || square.Position.Y > ViewportSize.Y + square.Size.Y / 2)
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
            return new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
        }

        /// <summary>
        /// Convinience function to draw a sprite at a worldspace position
        /// Checks is the sprite on screen
        /// </summary>
        /// <param name="position"></param>
        /// <param name="texture"></param>
        public void DrawWorldSpriteCentered(Vector2 position, Texture2D texture, float scale)
        {
            DrawSpriteCentered(WorldToViewSpace(position), texture, scale);
        }

        /// <summary>
        /// Draws a sprite centered at this position
        /// Checks is the sprite on screen
        /// </summary>
        /// <param name="viewportPosition"></param>
        /// <param name="texture"></param>
        public void DrawSpriteCentered(Vector2 viewportPosition, Texture2D texture, float scale)
        {
            Vector2 SpriteSize = new Vector2(texture.Width, texture.Height);

            if (IsSquareInView(new Square(viewportPosition, SpriteSize)))
            {
                SpriteBatch.Draw(texture, viewportPosition, null, Color.White, 0f, SpriteSize / 2, CameraZoom * scale, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Convinience function that makes sure the zoom never goes to anything stupid
        /// </summary>
        /// <param name="zoom"></param>
        public void UpdateZoom(float zoom)
        {
            CameraZoom = Math.Clamp(zoom, 0.1f, 10f);
        }
    }
}
