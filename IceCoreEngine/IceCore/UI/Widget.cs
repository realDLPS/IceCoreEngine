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
    public class Widget : IceCoreObject
    {
        public int _zOrder;
        public List<Widget> _childWidgets = new List<Widget>();
        public int _highestChildZOrder = 0;
        public Widget _parent;
        public UIManager _uiManager;

        public Widget()
        {

        }

        public void SetZOrder(int zOrder)
        {
            _zOrder = zOrder;
        }
        public int GetZOrder()
        {
            return _zOrder;
        }

        public void AddChild(Widget child)
        {
            child._parent = this;

            _childWidgets.Add(child);

            if (child._zOrder > _highestChildZOrder)
            {
                _highestChildZOrder = child._zOrder;
            }
        }

        /// <summary>
        /// Return true if you consumed the click
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public virtual bool Click(Vector2 screenPosition)
        {
            return false;
        }
    }

    /// <summary>
    /// A canvas widget itself is not visible, its only job is to have child widgets that are actually visible
    /// </summary>
    public class CanvasWidget : Widget
    {
        public CanvasWidget()
        {

        }

        public override bool Click(Vector2 screenPosition)
        {
            return UIManager.ClickWidgets(screenPosition, _childWidgets, _highestChildZOrder);
        }
    }

    /// <summary>
    /// Positioned widgets have a position, rotation, scale and an anchor point
    /// 
    /// Note to self:
    /// You might be able to do mouse checking against buttons by rotating the mouse position vector instead of trying to do complex stuff with rotated buttons.
    /// Then act like the button is straight
    /// Goes for everything interactable.
    /// </summary>
    public class PositionedWidget : Widget
    {
        /// <summary>
        /// Transform relative to parent
        /// </summary>
        public Transform _relativeTransform;
        public EAnchor _anchor = EAnchor.TopLeft;

        public PositionedWidget()
        {
            _relativeTransform = new Transform();
        }

        /// <summary>
        /// Checks does this widget have a parent that has a position.
        /// </summary>
        /// <returns></returns>
        private bool IsValidParent()
        {
            if(_parent !=  null)
            {
                if (_parent.GetType() == typeof(PositionedWidget) || _parent.GetType().IsSubclassOf(typeof(PositionedWidget)))
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        public Transform GetTransform()
        {
            if(IsValidParent())
            {
                return ((PositionedWidget)_parent).GetTransform() + _relativeTransform;
            }
            else
            {
                return _relativeTransform;
            }
        }
        public Transform GetAnchoredTransform()
        {
            Transform Transform = new Transform();
            Transform.Size = _relativeTransform.Size;
            Transform.Rotation = _relativeTransform.Rotation;

            Vector2 Position;

            // I hate this, I hope there isn't a better way to do this :´)
            switch (_anchor)
            {
                case EAnchor.TopLeft:
                    Position = _relativeTransform.Position; 
                    break;
                case EAnchor.Top:
                    Position = _relativeTransform.Position - new Vector2(_relativeTransform.Size.X / 2, 0f);
                    break;
                case EAnchor.TopRight:
                    Position = _relativeTransform.Position - new Vector2(_relativeTransform.Size.X, 0f);
                    break;
                case EAnchor.Left:
                    Position = _relativeTransform.Position - new Vector2(0f, _relativeTransform.Size.Y / 2);
                    break;
                case EAnchor.Middle:
                    Position = _relativeTransform.Position - new Vector2(_relativeTransform.Size.X / 2, _relativeTransform.Size.Y / 2);
                    break;
                case EAnchor.Right:
                    Position = _relativeTransform.Position - new Vector2(_relativeTransform.Size.X, _relativeTransform.Size.Y / 2);
                    break;
                case EAnchor.BottomLeft:
                    Position = _relativeTransform.Position - new Vector2(0f, _relativeTransform.Size.Y);
                    break;
                case EAnchor.Bottom:
                    Position = _relativeTransform.Position - new Vector2(_relativeTransform.Size.X / 2, _relativeTransform.Size.Y);
                    break;
                case EAnchor.BottomRight:
                    Position = _relativeTransform.Position - new Vector2(_relativeTransform.Size.X, _relativeTransform.Size.Y);
                    break;
                default:
                    Position = _relativeTransform.Position;
                    break;
            }

            Transform.Position = Position;

            return Transform;
        }
    }
    public class DrawableWidget : PositionedWidget
    {
        /// <summary>
        /// ScreenPosition will be offset by the widget position
        /// </summary>
        public List<Sprite> _sprites = new List<Sprite>();

        public DrawableWidget()
        {

        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            foreach(Sprite sprite in _sprites)
            {
                Sprite NewSprite = sprite;

                NewSprite.ScreenPosition = NewSprite.ScreenPosition + GetAnchoredTransform().Position;

                _game.GetGraphicsManager().AddSprite(NewSprite);
            }
        }
    }

    public class ClickableWidget : DrawableWidget
    {
        private bool _clickable = true;
        public WidgetClick _clickDelegate;

        private bool _pressed = false;

        Color Color = new Color(new Vector4(0.4f, 0.4f, 0.4f, 1f));

        public ClickableWidget()
        {
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);

            Color ModifiedColor = Color * (_pressed ? 0.8f : 1f);

            _sprites.Clear();
            _sprites.Add(new Sprite(Vector2.Zero, ((Game1)_game).dot, 0f, Vector2.Zero, GetTransform().Size, 0, false, ModifiedColor));

            if(!_uiManager.IsPlayerHolding())
            {
                _pressed = false;
            }
        }

        public void AddClickListener(WidgetClick listener)
        {
            _clickDelegate += listener;
        }
        public void RemoveClickListener(WidgetClick listener)
        {
            _clickDelegate -= listener;
        }
        public override bool Click(Vector2 screenPosition)
        {
            

            if(_clickable)
            {
               
                Square ButtonSquare = new Square(GetTransform().Position + GetTransform().Size/2, GetTransform().Size);
                
                if (CollisionSystem.IsPointInSquare(ButtonSquare, screenPosition, GetTransform().Rotation))
                {
                    
                    if(_clickDelegate != null)
                    {
                        _clickDelegate.Invoke();
                    }

                    _pressed = true;

                    return true;
                }
            }

            return false;
        }
    }
}
