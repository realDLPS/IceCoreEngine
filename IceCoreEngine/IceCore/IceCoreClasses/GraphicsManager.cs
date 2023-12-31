﻿using Microsoft.Xna.Framework;
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
    public class GraphicsManager
    {
        //// Camera variables
        /// <summary>
        /// Camera world position
        /// </summary>
        public Vector2 CameraPosition = new Vector2(0.0f);
        public float CameraZoom = 1.0f;
        public float CameraRotation = 0.0f;

        private List<Sprite> DrawQueue = new List<Sprite>();

        private bool DesiredMouseVisibility = true;

        private bool ClampMouseToGameWindow = false;

        private bool WasInFocus = false;

        //// Static variables
        private GraphicsDeviceManager GDM;
        private SpriteBatch SpriteBatch;
        private GraphicsDevice GraphicsDevice;
        private GameWindow Window;
        private IceCoreGame Game;



        public GraphicsManager(IceCoreGame game, GraphicsDeviceManager graphics, SpriteBatch spriteBatch, GameWindow window)
        {
            Game = game;
            GDM = graphics;
            SpriteBatch = spriteBatch;
            GraphicsDevice = spriteBatch.GraphicsDevice;
            Window = window;
        }

        #region Drawing

        public void BeginDraw()
        {
            SpriteBatch.Begin();
        }
        public void EndDraw()
        {
            SpriteBatch.End();
        }

        public void Update(float deltaTime)
        {
            if (Game.IsActive && ClampMouseToGameWindow)
            {
                ClampMouseToWindow();
            }

            if (!Game.IsActive || DesiredMouseVisibility)
            {
                Game.IsMouseVisible = true;
            }
            else
            {
                Game.IsMouseVisible = false;
            }

            if (Game.IsActive != WasInFocus)
            {
                if (WasInFocus)
                {
                    OnLoseFocus();
                }
                else
                {
                    OnFocus();
                }

                WasInFocus = Game.IsActive;
            }
        }

        /// <summary>
        /// Draws all sprites that are added to the draw queue
        /// 
        /// Expects that the begin method has been called on the sprite batch.
        /// </summary>
        public void Draw()
        {
            foreach (Sprite Sprite in DrawQueue)
            {
                SpriteBatch.Draw(Sprite.Texture, 
                    Sprite.ScreenPosition, 
                    null, Sprite.Color, 
                    ICFloatMath.ConvertDegreesToRadians(Sprite.Rotation) * -1 + (Sprite.RotatedByCamera ? ICFloatMath.ConvertDegreesToRadians(CameraRotation) : 0), 
                    Sprite.Origin, 
                    Sprite.Scale, 
                    Sprite.SpriteEffects, 
                    Sprite.LayerDepth);
            }
            DrawQueue.Clear();
        }
        #endregion
        #region Viewport methods
        /// <summary>
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

            Vector2 RotatedPosition = ICVec2Math.RotateVector(ScreenSizedPosition, CameraRotation);

            // Lastly we want to move this vector into proper position as the camera position is in the center of the screen instead of the pixel {0,0} that is in the top left corner
            return (RotatedPosition + (ViewportSize / 2));
        }

        /// <summary>
        /// Used to check is a vector2 in view.
        /// </summary>
        /// <param name="viewSpacePosition"></param>
        /// <returns>Returns true if position is on screen</returns>
        public bool IsSquareInView(Square square)
        {
            Vector2 ViewportSize = GetViewportSize();

            if (square.Position.X + (square.Scale.X / 2) * CameraZoom < 0 || square.Position.Y + (square.Scale.Y / 2) * CameraZoom < 0 || square.Position.X > ViewportSize.X + (square.Scale.X / 2) * CameraZoom || square.Position.Y > ViewportSize.Y + (square.Scale.Y / 2) * CameraZoom)
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

        public void OnFocus()
        {

        }
        public void OnLoseFocus()
        {

        }

        public void ClampMouseToWindow()
        {
            Point MousePosition = Mouse.GetState().Position;
            Vector2 ViewportSize = GetViewportSize();

            MousePosition.X = Math.Clamp(MousePosition.X, 0, (int)ViewportSize.X);
            MousePosition.Y = Math.Clamp(MousePosition.Y, (IsFullScreen() ? 0 : -30), (int)ViewportSize.Y);

            if (!DesiredMouseVisibility)
            {
                MousePosition.X = (int)ViewportSize.X / 2;
                MousePosition.Y = (int)ViewportSize.Y / 2;
            }

            Mouse.SetPosition(MousePosition.X, MousePosition.Y);
        }
        #endregion

        #region Adding to draw queue
        /// <summary>
        /// Convinience function to draw a sprite at a worldspace position
        /// 
        /// Adds the sprite to a queue. Drawn when the DrawQueue() method is called.
        /// 
        /// Checks is the sprite on screen
        /// </summary>
        /// <param name="worldspacePosition">Worldspace position</param>
        /// <param name="texture"></param>
        /// <param name="scale">Multiplier for the scale of the texture</param>
        public void AddWorldSpriteCentered(Vector2 worldspacePosition, Texture2D texture, float scale, float rotation)
        {
            AddSpriteCentered(WorldToViewSpace(worldspacePosition), texture, scale, rotation);
        }

        /// <summary>
        /// Draws a sprite centered at this position
        /// 
        /// Adds the sprite to a queue. Drawn when the DrawQueue() method is called.
        /// 
        /// Checks is the sprite on screen
        /// </summary>
        /// <param name="screenspacePosition"></param>
        /// <param name="texture"></param>
        /// <param name="scale">Multiplier for the scale of the texture</param>
        public void AddSpriteCentered(Vector2 screenspacePosition, Texture2D texture, float scale, float rotation)
        {
            Vector2 SpriteSize = new Vector2(texture.Width, texture.Height);
            Vector2 ViewportSize = GetViewportSize();
            float ScreenSizeScaling = ((ViewportSize.X / 1920 >= ViewportSize.Y / 1080) ? ViewportSize.X / 1920 : ViewportSize.Y / 1080);

            if (IsSquareInView(new Square(screenspacePosition, SpriteSize)))
            {
                DrawQueue.Add(new Sprite(screenspacePosition, texture, rotation, SpriteSize / 2, CameraZoom * scale * ScreenSizeScaling, 1f, true));
            }
        }
        public void AddSprite(Sprite sprite)
        {
            Vector2 ViewportSize = GetViewportSize();
            float ScreenSizeScaling = ((ViewportSize.X / 1920 >= ViewportSize.Y / 1080) ? ViewportSize.X / 1920 : ViewportSize.Y / 1080);

            if (IsSquareInView(new Square(sprite.ScreenPosition, sprite.Scale)))
            {
                Sprite ModifiedSprite = sprite;

                sprite.Scale = sprite.Scale * ScreenSizeScaling;

                DrawQueue.Add(sprite);
            }
        }
        #endregion


        /// <summary>
        /// Convinience function that makes sure the zoom never goes to anything unreasonable
        /// </summary>
        /// <param name="zoom"></param>
        public void UpdateZoom(float zoom)
        {
            CameraZoom = Math.Clamp(zoom, 0.1f, 10f);
        }

        #region Mouse
        public void SetMouseVisibility(bool visible)
        {
            DesiredMouseVisibility = visible;
        }
        public void ToggleMouseVisibility()
        {
            DesiredMouseVisibility = !DesiredMouseVisibility;
        }
        public bool IsMouseVisible()
        {
            return DesiredMouseVisibility;
        }

        public void SetMouseClamppingToWindow(bool clampMouseToWindow)
        {
            ClampMouseToGameWindow = clampMouseToWindow;
        }
        public void ToggleMouseClamppingToWindow()
        {
            ClampMouseToGameWindow = !ClampMouseToGameWindow;
        }
        public bool IsMouseClamppedToWindow()
        {
            return ClampMouseToGameWindow;
        }
        #endregion

        #region Window
        #region Fullscreen
        /// <summary>
        /// Toggles fullscreen
        /// </summary>
        public void ToggleFullscreen()
        {
            GDM.IsFullScreen = !GDM.IsFullScreen;
        }

        /// <summary>
        /// Sets fullscreen state
        /// </summary>
        /// <param name="isFullScreen"></param>
        public void SetFullScreen(bool isFullScreen)
        {
            GDM.IsFullScreen = isFullScreen;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns fullscreen state</returns>
        public bool IsFullScreen()
        {
            return GDM.IsFullScreen;
        }

        /// <summary>
        /// Toggles is the fullscreen borderless
        /// </summary>
        public void ToggleBorderlessWindow()
        {
            GDM.HardwareModeSwitch = !GDM.HardwareModeSwitch;
        }

        /// <summary>
        /// Sets borderless window state
        /// </summary>
        /// <param name="isBorderlessWindow"></param>
        public void SetBorderlessWindow(bool isBorderlessWindow)
        {
            // As false is borderless switch the bool around
            GDM.HardwareModeSwitch = !isBorderlessWindow;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns true if fullscreen window is borderless</returns>
        public bool IsBorderlessWindow()
        {
            return !GDM.HardwareModeSwitch;
        }
        #endregion
        /// <summary>
        /// Toggles can window be resized by user
        /// </summary>
        public void ToggleAllowResize()
        {
            Window.AllowUserResizing = !Window.AllowUserResizing;
        }

        /// <summary>
        /// Sets can window be resized by user
        /// </summary>
        /// <param name="allowResize"></param>
        public void SetAllowResize(bool allowResize)
        {
            Window.AllowUserResizing = allowResize;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Returns is window resizing allowed</returns>
        public bool IsResizingAllowed()
        {
            return Window.AllowUserResizing;
        }
        #endregion

        /// <summary>
        /// Applies changes such as fullscreen and borderless fullscreen
        /// </summary>
        public void ApplySettings()
        {
            GDM.ApplyChanges();
        }
    }
}
