using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private EInputType InputType;
        /// <summary>
        /// A list of triggers for this input action
        /// 
        /// Each Trigger has a button (keyboard or mouse) and multiplier
        /// </summary>
        private List<InputTrigger> InputTriggers;
        private InputDelegate InputUpdateDelegate;

        private bool Active;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputType">Is this an analog or digital action?</param>
        /// <param name="inputTriggers">List of input that will trigger this action</param>
        /// <param name="inputUpdateDelegate">Delegate that will be invoked every input update</param>
        public InputAction(EInputType inputType, InputDelegate inputUpdateDelegate)
        {
            InputType = inputType;
            InputTriggers = new List<InputTrigger>();
            InputUpdateDelegate = inputUpdateDelegate;
            Active = true;
        }
        public EInputType GetInputType()
        {
            return InputType;
        }
        public List<InputTrigger> GetInputTriggers()
        {
            return InputTriggers;
        }
        public InputDelegate GetInputUpdateDelegate()
        {
            return InputUpdateDelegate;
        }

        public bool IsActive()
        {
            return Active;
        }
        public void Deactivate()
        {
            Active = false;
        }
        public void Activate()
        {
            Active = true;
        }
        public void AddTrigger(InputTrigger trigger)
        {
            InputTriggers.Add(trigger);
        }
    }
    public struct InputTrigger
    {
        private EInput Button;
        private float Multiplier;

        public InputTrigger(EInput button, float multiplier)
        {
            Button = button;
            Multiplier = multiplier;
        }

        public EInput GetButton()
        {
            return Button;
        }
        public float GetMultiplier()
        {
            return Multiplier;
        }
    }
}
