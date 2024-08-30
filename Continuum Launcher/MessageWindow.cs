using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Ozzz = XeniaLauncher.OzzzFramework;
using Sprite = XeniaLauncher.OzzzFramework.Sprite;
using ObjectSprite = XeniaLauncher.OzzzFramework.ObjectSprite;
using TextSprite = XeniaLauncher.OzzzFramework.TextSprite;
using Layer = XeniaLauncher.OzzzFramework.SpriteGroup.Layer;
using Button = XeniaLauncher.OzzzFramework.Button;
using Gradient = XeniaLauncher.OzzzFramework.Gradient;
using AnimationPath = XeniaLauncher.OzzzFramework.AnimationPath;
using MouseInput = XeniaLauncher.OzzzFramework.MouseInput;
using KeyboardInput = XeniaLauncher.OzzzFramework.KeyboardInput;
using Key = XeniaLauncher.OzzzFramework.KeyboardInput.Key;
using GamepadInput = XeniaLauncher.OzzzFramework.GamepadInput;
using AnalogPad = XeniaLauncher.OzzzFramework.GamepadInput.AnalogPad;
using DigitalPad = XeniaLauncher.OzzzFramework.GamepadInput.DigitalPad;
using Continuum_Launcher;

namespace XeniaLauncher
{
    public class MessageWindow : Window
    {
        public enum MessagePrompts
        {
            OK, OKCancel, YesNo, GameXenia
        }
        public MessageWindow(Game1 game, string title, string message, Game1.State returnState) : this(game, title, message, returnState, MessagePrompts.OK)
        {
            
        }
        public MessageWindow(Game1 game, string title, string message, Game1.State returnState, MessagePrompts prompt) : base (game, new Rectangle(560, 365, 800, 350), title, new MessageButtonEffects(), new SingleButtonEvent(), new GenericStart(), returnState)
        {
            extraSprites.Add(new TextSprite(game.font, message, 0.4f, new Vector2(), Color.FromNonPremultiplied(0, 0, 0, 0)));
            Logging.Write(Logging.LogType.Standard, Logging.LogEvent.MessageWindow, "New message displayed", new Dictionary<string, string>()
            {
                { "title", title },
                { "message", message }
            });
            extraSprites[0].Centerize(rect.Center.ToVector2() - new Vector2(0, 10));
            if (extraSprites[0].GetSize().X + 60 > rect.Width)
            {
                rect.Width = (int)(extraSprites[0].GetSize().X + 60);
                pos.X = 960 - rect.Width / 2;
            }
            if (prompt == MessagePrompts.OK)
            {
                AddButton(new Rectangle(760, 600, 400, 90));
                AddText("OK");
            }
            else if (prompt == MessagePrompts.OKCancel)
            {
                AddButton(new Rectangle(980, 600, 250, 90));
                AddText("Cancel");
                AddButton(new Rectangle(680, 600, 250, 90));
                AddText("OK");
                inputEvents = new StdInputEvent(2);
            }
            else if (prompt == MessagePrompts.YesNo)
            {
                AddButton(new Rectangle(980, 600, 250, 90));
                AddText("No");
                AddButton(new Rectangle(680, 600, 250, 90));
                AddText("Yes");
                inputEvents = new StdInputEvent(2);
            }
            else if (prompt == MessagePrompts.GameXenia)
            {
                AddButton(new Rectangle(1010, 600, 290, 90));
                AddText("Xenia Settings");
                AddButton(new Rectangle(630, 600, 290, 90));
                AddText("Game Settings");
                inputEvents = new StdInputEvent(2);
            }
        }
    }
}
