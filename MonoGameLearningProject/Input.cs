using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Security.AccessControl;

namespace MonoGameLearningProject
{
    public class InputManager
    {
        private List<InputAction> InputActions = new List<InputAction>();


        private MouseState CurrentMouseState;
        private KeyboardState CurrentKeyboardState;

        private MouseState PreviousMouseState;
        private KeyboardState PreviousKeyboardState;


        private List<float> InputStates = new List<float>();

        public InputManager()
        {
            PreviousMouseState = Mouse.GetState();
            PreviousKeyboardState = Keyboard.GetState();

            foreach (EInput input in Enum.GetValues(typeof(EInput)))
            {
                InputStates.Add(0);
            }
        }

        #region Input updating
        public void UpdateInputs()
        {
            CurrentMouseState = Mouse.GetState();
            CurrentKeyboardState = Keyboard.GetState();

            // Update the InputStates list
            foreach (EInput input in Enum.GetValues(typeof(EInput)))
            {
                if((int)input < 160)
                {
                    InputStates[(int)input] = ConvertInputToKeyboard(input);
                }
                else
                {
                    InputStates[(int)input] = ConvertInputToMouse(input);
                }
            }
            
            if(InputActions != null && InputActions.Count() > 0)
            {
                foreach (InputAction action in InputActions)
                {
                    if(action.IsActive())
                    {
                        float Sum = 0;

                        foreach (var trigger in action.GetInputTriggers())
                        {
                            Sum += InputStates[(int)trigger.GetButton()] * trigger.GetMultiplier();
                        }

                        if (action.GetInputType() == EInputType.Analog)
                        {
                            action.GetInputUpdateDelegate().Invoke(Sum);
                        }
                        else if (action.GetInputType() == EInputType.Digital)
                        {
                            action.GetInputUpdateDelegate().Invoke(Sum > 0 ? 1 : 0);
                        }
                    }
                }
            }

            // Finishing
            PreviousMouseState = CurrentMouseState;
            PreviousKeyboardState = CurrentKeyboardState;
        }
        #endregion

        #region Converting from EInput to a float
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
                return CurrentMouseState.LeftButton == ButtonState.Pressed ? 1 : 0;
            }
            else if(input == EInput.MouseRight)
            {
                return CurrentMouseState.RightButton == ButtonState.Pressed ? 1 : 0;
            }
            else if(input == EInput.MouseMiddle)
            {
                return CurrentMouseState.MiddleButton == ButtonState.Pressed? 1 : 0;
            }
            else if(input == EInput.MouseScroll)
            {
                return CurrentMouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue;
            }
            else if(input == EInput.MouseXButton1)
            {
                return CurrentMouseState.XButton1 == ButtonState.Pressed ? 1 : 0;
            }
            else if(input == EInput.MouseXButton2)
            {
                return CurrentMouseState.XButton2 == ButtonState.Pressed ? 1 : 0;
            }
            else if(input == EInput.X)
            {
                return CurrentMouseState.X - PreviousMouseState.X;
            }
            else if (input == EInput.Y)
            {
                return CurrentMouseState.Y - PreviousMouseState.Y;
            }
            return 0;
        }

        /// <summary>
        /// Converts the key to it's float value of either 0 or 1.
        /// 0 if not pressed, 1 if pressed
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private float ConvertInputToKeyboard(EInput input)
        {
            if(Enum.TryParse(input.ToString(), true, out Keys keyboard))
            {
                return CurrentKeyboardState.IsKeyDown(keyboard) ? 1 : 0;
            }
            return 0;
        }
        #endregion

        /// <summary>
        /// Adds an input action that will be called.
        /// 
        /// Note, if you wish to delete an action they aren't actually deleted just deactive and you can reactivate easily with ReactivateInputAction
        /// </summary>
        /// <param name="action"></param>
        public void AddInputAction(InputAction action)
        {
            InputActions.Add(action);
        }

        /// <summary>
        /// Deactivates an action
        /// </summary>
        /// <param name="index"></param>
        public void DeactivateInputAction(int index)
        {
            InputActions[index].Deactivate();
        }

        /// <summary>
        /// Reactivates an action
        /// </summary>
        /// <param name="index"></param>
        public void ReactivateInputAction(int index)
        {
            InputActions[index].Activate();
        }

        /// <summary>
        /// Adds a trigger to an action
        /// </summary>
        /// <param name="index"></param>
        public void AddTriggerToAction(EInput button, float multiplier, int index)
        {
            Debug.WriteLine("test2");
            InputActions[index].AddTrigger(new InputTrigger(button, multiplier));
        }
        /// <summary>
        /// Quickly adds a trigger to the most recently added action
        /// </summary>
        /// <param name="button"></param>
        /// <param name="multiplier"></param>
        public void QuickAddTriggerToAction(EInput button, float multiplier)
        {
            AddTriggerToAction(button, multiplier, InputActions.Count - 1);
        }
    }
}
