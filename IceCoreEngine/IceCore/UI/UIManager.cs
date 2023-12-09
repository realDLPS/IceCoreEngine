using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

using System.Collections.Generic;
using System.Reflection.Metadata;
using IceCoreEngine;

namespace IceCoreEngine
{
    public class UIManager
    {
        /// <summary>
        /// These are widgets that have been added to the screen.
        /// </summary>
        private List<Widget> _rootWidgets = new List<Widget>();

        private int _highestZOrder = 0;

        private IceCoreGame _game;

        private bool _holdingMouse = false;
        private bool _clickedThisUpdate = false;

        public UIManager(IceCoreGame game)
        {
            _game = game;
        }

        /// <summary>
        /// Returns true if something was clicked
        /// </summary>
        /// <returns></returns>
        public bool Click(Vector2 screenPosition)
        {
            _clickedThisUpdate = true;

            if(!_holdingMouse)
            {
                _holdingMouse = true;
                return ClickWidgets(screenPosition, _rootWidgets, _highestZOrder);
            }
            return false;
        }

        public bool IsPlayerHolding()
        {
            return _holdingMouse;
        }

        public void Update()
        {
            if(_clickedThisUpdate && !_holdingMouse)
            {
                _holdingMouse = true;
            }
            if(!_clickedThisUpdate)
            {
                _holdingMouse = false;
            }

            _clickedThisUpdate = false;
        }

        public static bool ClickWidgets(Vector2 screenPosition, List<Widget> widgets, int highestZOrder)
        {
            for (int CurrentZOrder = 0; CurrentZOrder <= highestZOrder; CurrentZOrder++)
            {
                foreach (Widget widget in widgets)
                {
                    if (widget.GetZOrder() == CurrentZOrder)
                    {
                        if (widget.Click(screenPosition))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">Widget class</typeparam>
        /// <param name="parent">If null widget is added to the screen</param>
        /// <returns></returns>
        public Widget CreateWidget<T>(Widget parent, int zOrder) where T : Widget, new()
        {
            Widget NewWidget = _game.GetObjectManager().SpawnObject<T>(true, false);

            if (parent != null)
            {
                parent.AddChild(NewWidget);
            }
            else
            {
                _rootWidgets.Add(NewWidget);
            }

            NewWidget.SetZOrder(zOrder);

            NewWidget._uiManager = this;

            NewWidget.Created();

            if(zOrder > _highestZOrder)
            {
                _highestZOrder = zOrder;
            }

            return NewWidget;
        }
    }
}
