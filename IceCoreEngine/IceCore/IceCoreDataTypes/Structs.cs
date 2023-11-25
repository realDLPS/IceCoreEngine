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
    public struct Transform
    {
        public Vector2 Position;
        public float Rotation;
        public Vector2 Size;
        
        public Transform(Vector2 postion, float rotation, Vector2 size)
        {
            Position = postion;
            Rotation = rotation;
            Size = size;
        }
        public Transform(Vector2 postion, float rotation)
        {
            Position = postion;
            Rotation = rotation;
            Size = new Vector2(1.0f);
        }
        public Transform(Vector2 postion, Vector2 size)
        {
            Position = postion;
            Rotation = 0.0f;
            Size = size;
        }
        public Transform(float rotation, Vector2 size)
        {
            Position = new Vector2(0.0f);
            Rotation = rotation;
            Size = size;
        }
        public Transform(Vector2 postion)
        {
            Position = postion;
            Rotation = 0.0f;
            Size = new Vector2(1.0f);
        }
        public Transform(float rotation)
        {
            Position = new Vector2(0.0f);
            Rotation = rotation;
            Size = new Vector2(1.0f);
        }
        public Transform()
        {
            Position = new Vector2(0.0f);
            Rotation = 0.0f;
            Size = new Vector2(1.0f);
        }
    }
    #region Input
    public struct InputAction
    {
        /// <summary>
        /// If InputType is digital, delegate will trigger when value changes, will return either 0 or 1.
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

        /// <summary>
        /// Delegate that is invoked by this action
        /// </summary>
        private InputDelegate InputUpdateDelegate;

        /// <summary>
        /// Is this action active?
        /// </summary>
        private bool Active;

        /// <summary>
        /// For analog this is the combined value
        /// 
        /// For digital this is either 0 or 1
        /// </summary>
        private float Value;

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
            Value = 0;
        }
        public InputAction(EInputType inputType)
        {
            InputType = inputType;
            InputTriggers = new List<InputTrigger>();
            InputUpdateDelegate = null;
            Active = true;
            Value = 0;
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
        public void SetValue(float value)
        {
            Value = value;
        }
        public float GetValue()
        {
            return Value;
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
    #endregion


    #region Graphics

    public struct DrawQueueMember
    {
        public Vector2 ScreenPosition;
        public Texture2D Texture;
        public Color Color = Color.White;
        public float Rotation;
        public Vector2 Origin;
        public float Scale;
        public SpriteEffects SpriteEffects = SpriteEffects.None;
        public float LayerDepth;

        public DrawQueueMember(Vector2 screenPosition, Texture2D texture, float rotation, Vector2 origin, float scale, float layerDepth)
        {
            ScreenPosition = screenPosition;
            Texture = texture;
            Rotation = rotation;
            Origin = origin;
            Scale = scale;
            LayerDepth = layerDepth;
        }
    }
    #endregion
}
