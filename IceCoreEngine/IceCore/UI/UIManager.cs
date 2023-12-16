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
using GeonBit.UI.Entities;
using GeonBit.UI;

namespace IceCoreEngine
{
    public class UIManager
    {
        private IceCoreGame _game;

        public UIManager(IceCoreGame game)
        {
            _game = game;

            UserInterface.Initialize(_game.Content, BuiltinThemes.editor);

            // create a panel and position in center of screen
            Panel panel = new Panel(new Vector2(400, 400), PanelSkin.Default, Anchor.Center);
            UserInterface.Active.AddEntity(panel);

            // add title and text
            panel.AddChild(new Header("Example Panel"));
            panel.AddChild(new HorizontalLine());
            panel.AddChild(new Paragraph("This is a simple panel with a button."));

            // add a button at the bottom
            panel.AddChild(new Button("Click Me!", ButtonSkin.Default, Anchor.BottomCenter));
        }
        public void Update()
        {
            UserInterface.Active.Update(_game.GetGameTime());
        }
        public void Draw()
        {
            UserInterface.Active.Draw(_game.GetSpriteBatch());
        }
    }
}
