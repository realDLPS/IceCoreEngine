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
using System.Security.AccessControl;

namespace MonoGameLearningProject
{
    public class Input
    {
        private List<InputAction> InputActions;
        private MouseState PreviousMouseState;


        public void UpdateInputs()
        {
            foreach (InputAction action in InputActions)
            {
                float Sum = 0;

                foreach (var trigger in action.Triggers)
                {
                    if((int)trigger.Button == 2)
                    {
                        Sum += ConvertInputToMouse(trigger.Button);
                    }
                    if((int)trigger.Button == 1)
                    {
                        Sum += ConvertInputToKeyboard(trigger.Button);
                    }
                }
            }
        }

        /// <summary>
        /// Returns either 0 or 1 when a button or the value if something like the scroll wheel
        /// Default return is 0
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private float ConvertInputToMouse(EInput input)
        {
            if(input == EInput.MouseLeft)
            {
                return Mouse.GetState().LeftButton == ButtonState.Pressed ? 1 : 0;
            }
            else if(input == EInput.MouseRight)
            {
                return Mouse.GetState().RightButton == ButtonState.Pressed ? 1 : 0;
            }
            else if(input == EInput.MouseMiddle)
            {
                return Mouse.GetState().MiddleButton == ButtonState.Pressed? 1 : 0;
            }
            else if(input == EInput.MouseScroll)
            {
                return Mouse.GetState().ScrollWheelValue - PreviousMouseState.ScrollWheelValue;
            }
            else if(input == EInput.MouseXButton1)
            {
                return Mouse.GetState().XButton1 == ButtonState.Pressed ? 1 : 0;
            }
            else if(input == EInput.MouseXButton2)
            {
                return Mouse.GetState().XButton2 == ButtonState.Pressed ? 1 : 0;
            }
            else if(input == EInput.X)
            {
                return Mouse.GetState().X;
            }
            else if (input == EInput.Y)
            {
                return Mouse.GetState().Y;
            }
            return 0;
        }
        private float ConvertInputToKeyboard(EInput input)
        {
            if(Enum.TryParse(input.ToString(), true, out Keys keyboard))
            {
                return Keyboard.GetState().IsKeyDown(keyboard) ? 1 : 0;
            }
            return 0;
        }
    }
}
