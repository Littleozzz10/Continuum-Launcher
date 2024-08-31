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
using System.Security.Cryptography;
using System.Windows;
using System.Threading;
using System.Windows.Forms;

namespace XeniaLauncher
{

    public class TextInputWindow : Window
    {
        // Retrieved from https://stackoverflow.com/questions/73227944/getting-clipboard-text-using-net-6-and-c-sharp
        internal static void RunAsSTAThread(Action goForIt)
        {
            AutoResetEvent @event = new AutoResetEvent(false);
            Thread thread = new Thread(
                () =>
                {
                    goForIt();
                    @event.Set();
                });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            @event.WaitOne();
        }
        internal static string GetTextFromClipboard()
        {
            string clipText = "";

            RunAsSTAThread(
           () =>
           {
               clipText = Clipboard.GetText();
           });

            return clipText;
        }
        // End retrieved code

        public static readonly List<string> keys = new List<string>{ "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "-", "=", "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "[", "]", "\\", "A", "S", "D", "F", "G", "H", "J", "K", "L", ";", "'", "Z", "X", "C", "V", "B", "N", "M", ",", ".", "/" };
        public static readonly List<string> shiftKeys = new List<string> { "!", "@", "#", "$", "%", "^", "&", "*", "(", ")", "_", "+", "{", "}", "|", ":", "\"", "<", ">", "?" };
        public bool shift, caps, shiftLock, ctrl, ctrlLock;
        public TextInputWindow(Game1 game, string title, string message, Game1.State returnState) : base (game, new Rectangle(60, 60, 1800, 960), title, new TextWindowEffects(), new TextInput(), new GenericStart(), returnState)
        {
            extraSprites.Add(new ObjectSprite(game.white, new Rectangle(90, 200, 1740, 80), Color.White));
            extraSprites.Add(new TextSprite(game.font, message, 0.5f, new Vector2(110, 0), Color.FromNonPremultiplied(0, 0, 0, 255)));
            extraSprites[1].tags.Add("black");
            disableKeyboard = game.enterCloseTextInput;

            // Key Buttons
            for (int i = 0; i < 12; i++)
            {
                AddButton(new Rectangle(190 + i * 110, 370, 100, 100));
                AddText(keys[i]);
            }
            for (int i = 0; i < 13; i++)
            {
                AddButton(new Rectangle(300 + i * 110, 480, 100, 100));
                AddText(keys[i + 12].ToLower());
            }
            for (int i = 0; i < 11; i++)
            {
                AddButton(new Rectangle(355 + i * 110, 590, 100, 100));
                AddText(keys[i + 25].ToLower());
            }
            for (int i = 0; i < 10; i++)
            {
                AddButton(new Rectangle(410 + i * 110, 700, 100, 100));
                AddText(keys[i + 36].ToLower());
            }
            // Special buttons (Starting at index 46)
            AddButton(new Rectangle(410, 810, 1090, 100));
            AddText("S P A C E");
            AddButton(new Rectangle(1510, 370, 210, 100));
            AddText("Backspace");
            AddButton(new Rectangle(1565, 590, 155, 100));
            AddText("Done");
            AddButton(new Rectangle(1510, 700, 210, 100));
            AddText("Shift");
            AddButton(new Rectangle(1510, 810, 210, 100));
            AddText("Ctrl");
            AddButton(new Rectangle(190, 480, 100, 100));
            AddText("Tab");
            AddButton(new Rectangle(190, 590, 155, 100));
            AddText("Caps");
            AddButton(new Rectangle(190, 700, 210, 100));
            AddText("Shift");
            AddButton(new Rectangle(190, 810, 210, 100));
            AddText("Ctrl");

            shift = false;
            caps = false;
            preferEscapeExit = true;
            game.state = Game1.State.Text;
        }
        public new void Update()
        {
            base.Update();

            float oldX = extraSprites[1].pos.X;
            extraSprites[1].Centerize(new Vector2(110, 240));
            extraSprites[1].pos.X = oldX;

            //bool updateButtonText = false;
            if (KeyboardInput.keys["LShift"].IsDown() || KeyboardInput.keys["RShift"].IsDown())
            {
                shift = true;
            }
            else
            {
                shift = false;
            }
            if (KeyboardInput.keys["Caps"].IsFirstDown())
            {
                caps = !caps;
            }
            if (KeyboardInput.keys["LCtrl"].IsDown() || KeyboardInput.keys["RCtrl"].IsDown())
            {
                ctrl = true;
            }
            else
            {
                ctrl = false;
            }
            if (disableKeyboard && KeyboardInput.keys["Enter"].IsFirstDown())
            {
                game.text.buttonEffects.ActivateButton(game, game.text, game.text.buttons[48], 48);
            }

            if (shift || shiftLock || caps)
            {
                for (int i = 0; i < 12 && (shift || shiftLock); i++)
                {
                    sprites[i].text = shiftKeys[i];
                }
                for (int i = 12; i < 46; i++)
                {
                    sprites[i].text = sprites[i].text.ToUpper();
                    if ((i == 22 || i == 23 || i == 24 || i == 34 || i == 35 || i == 43 || i == 44 || i == 45) && (shift || shiftLock))
                    {
                        switch (i)
                        {
                            case 22:
                                sprites[i].text = shiftKeys[12];
                                break;
                            case 23:
                                sprites[i].text = shiftKeys[13];
                                break;
                            case 24:
                                sprites[i].text = shiftKeys[14];
                                break;
                            case 34:
                                sprites[i].text = shiftKeys[15];
                                break;
                            case 35:
                                sprites[i].text = shiftKeys[16];
                                break;
                            case 43:
                                sprites[i].text = shiftKeys[17];
                                break;
                            case 44:
                                sprites[i].text = shiftKeys[18];
                                break;
                            case 45:
                                sprites[i].text = shiftKeys[19];
                                break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 12; i++)
                {
                    sprites[i].text = keys[i];
                }
                for (int i = 12; i < 46; i++)
                {
                    sprites[i].text = sprites[i].text.ToLower();
                    if (i == 22 || i == 23 || i == 24 || i == 34 || i == 35 || i == 43 || i == 44 || i == 45)
                    {
                        sprites[i].text = keys[i];
                    }
                }
            }

            if ((ctrl || ctrlLock) && !KeyboardInput.keys["Backspace"].IsFirstDown())
            {
                // Pasting
                if (KeyboardInput.keys["V"].IsFirstDown())
                {
                    string t = GetTextFromClipboard();
                    t = t.Replace("\"", "");
                    game.text.extraSprites[1].ToTextSprite().text += t;
                }
                if (KeyboardInput.keys["Enter"].IsFirstDown())
                {
                    if (game.text.extraSprites[1].ToTextSprite().text.Length > 0 && !disableKeyboard)
                    {
                        game.text.extraSprites[1].ToTextSprite().text = game.text.extraSprites[1].ToTextSprite().text.Substring(0, game.text.extraSprites[1].ToTextSprite().text.Length - 1);
                    }
                    game.text.buttonEffects.ActivateButton(game, game.text, game.text.buttons[48], 48);
                }
            }
            else
            {
                // Managing text input
                for (int i = 0; i < 46; i++)
                {
                    if (KeyboardInput.keys[keys[i]].IsFirstDown())
                    {
                        game.text.buttonEffects.ActivateButton(game, game.text, game.text.buttons[i], i);
                    }
                }
                if (KeyboardInput.keys["Space"].IsFirstDown())
                {
                    game.text.buttonEffects.ActivateButton(game, game.text, game.text.buttons[46], 46);
                }
                if (KeyboardInput.keys["Backspace"].IsFirstDown())
                {
                    game.text.buttonEffects.ActivateButton(game, game.text, game.text.buttons[47], 47);
                }
                if (KeyboardInput.keys["Tab"].IsFirstDown())
                {
                    game.text.buttonEffects.ActivateButton(game, game.text, game.text.buttons[51], 51);
                }
            }

            stringIndex = buttonIndex;
        }
    }
    public class TextWindowEffects : IWindowEffects
    {
        public void SetupEffects(Game1 game, Window window)
        {

        }
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex >= 0 && buttonIndex < 46)
            {
                source.extraSprites[1].ToTextSprite().text += source.sprites[buttonIndex].text;
                game.text.shiftLock = false;
            }
            else if (buttonIndex == 46)
            {
                source.extraSprites[1].ToTextSprite().text += " ";
            }
            else if (buttonIndex == 47)
            {
                if (source.extraSprites[1].ToTextSprite().text.Count() > 0)
                {
                    source.extraSprites[1].ToTextSprite().text = source.extraSprites[1].ToTextSprite().text.Substring(0, source.extraSprites[1].ToTextSprite().text.Count() - 1);
                    if (game.text.ctrl || game.text.ctrlLock)
                    {
                        source.extraSprites[1].ToTextSprite().text = "";
                        if (game.text.ctrlLock)
                        {
                            game.text.ctrlLock = false;
                        }
                    }
                }
            }
            else if (buttonIndex == 48)
            {
                game.textWindowInput = source.extraSprites[1].ToTextSprite().text;
                game.state = source.returnState;
                game.backSound.Play();
            }
            else if (buttonIndex == 49 || buttonIndex == 53)
            {
                game.text.shiftLock = true;
            }
            else if (buttonIndex == 50 || buttonIndex == 54)
            {
                game.text.ctrlLock = true;
            }
            else if (buttonIndex == 51)
            {
                source.extraSprites[1].ToTextSprite().text += "    ";
            }
            else if (buttonIndex == 52)
            {
                game.text.caps = !game.text.caps;
            }
        }
    }
    public class TextInput : IButtonInputEvent
    {
        public void LeftButton(Game1 game, Window source, int buttonIndex)
        {
            if ((buttonIndex > 0 && buttonIndex <= 11) || (buttonIndex > 12 && buttonIndex <= 24) || (buttonIndex > 25 && buttonIndex <= 35) || (buttonIndex > 36 && buttonIndex <= 45))
            {
                source.buttonIndex--;
            }
            else if (buttonIndex == 47)
            {
                source.buttonIndex = 11;
            }
            else if (buttonIndex == 51)
            {
                source.buttonIndex = 24;
            }
            else if (buttonIndex == 48)
            {
                source.buttonIndex = 35;
            }
            else if (buttonIndex == 49)
            {
                source.buttonIndex = 45;
            }
            else if (buttonIndex == 50)
            {
                source.buttonIndex = 46;
            }
            else if (buttonIndex == 0)
            {
                source.buttonIndex = 47;
            }
            else if (buttonIndex == 52)
            {
                source.buttonIndex = 48;
            }
            else if (buttonIndex == 53)
            {
                source.buttonIndex = 49;
            }
            else if (buttonIndex == 54)
            {
                source.buttonIndex = 50;
            }
            else if (buttonIndex == 12)
            {
                source.buttonIndex = 51;
            }
            else if (buttonIndex == 25)
            {
                source.buttonIndex = 52;
            }
            else if (buttonIndex == 36)
            {
                source.buttonIndex = 53;
            }
            else if (buttonIndex == 46)
            {
                source.buttonIndex = 54;
            }
            game.buttonSwitchSound.Play();
        }
        public void RightButton(Game1 game, Window source, int buttonIndex)
        {
            if ((buttonIndex >= 0 && buttonIndex < 11) || (buttonIndex >= 12 && buttonIndex < 24) || (buttonIndex >= 25 && buttonIndex < 35) || (buttonIndex >= 36 && buttonIndex < 45))
            {
                source.buttonIndex++;
            }
            else if (buttonIndex == 11)
            {
                source.buttonIndex = 47;
            }
            else if (buttonIndex == 24)
            {
                source.buttonIndex = 51;
            }
            else if (buttonIndex == 35)
            {
                source.buttonIndex = 48;
            }
            else if (buttonIndex == 45)
            {
                source.buttonIndex = 49;
            }
            else if (buttonIndex == 46)
            {
                source.buttonIndex = 50;
            }
            else if (buttonIndex == 47)
            {
                source.buttonIndex = 0;
            }
            else if (buttonIndex == 48)
            {
                source.buttonIndex = 52;
            }
            else if (buttonIndex == 49)
            {
                source.buttonIndex = 53;
            }
            else if (buttonIndex == 50)
            {
                source.buttonIndex = 54;
            }
            else if (buttonIndex == 51)
            {
                source.buttonIndex = 12;
            }
            else if (buttonIndex == 52)
            {
                source.buttonIndex = 25;
            }
            else if (buttonIndex == 53)
            {
                source.buttonIndex = 36;
            }
            else if (buttonIndex == 54)
            {
                source.buttonIndex = 46;
            }
            game.buttonSwitchSound.Play();
        }
        public void UpButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex >= 1 && buttonIndex <= 11)
            {
                source.buttonIndex = 46;
            }
            else if (buttonIndex >= 12 && buttonIndex < 23)
            {
                source.buttonIndex -= 11;
            }
            else if (buttonIndex >= 25 && buttonIndex <= 35)
            {
                source.buttonIndex -= 13;
            }
            else if (buttonIndex >= 36 && buttonIndex <= 45)
            {
                source.buttonIndex -= 11;
            }
            else if (buttonIndex == 46)
            {
                source.buttonIndex = 40;
            }
            else if (buttonIndex == 47)
            {
                source.buttonIndex = 50;
            }
            else if (buttonIndex == 48)
            {
                source.buttonIndex = 24;
            }
            else if (buttonIndex == 49)
            {
                source.buttonIndex = 48;
            }
            else if (buttonIndex == 50)
            {
                source.buttonIndex = 49;
            }
            else if (buttonIndex == 0)
            {
                source.buttonIndex = 54;
            }
            else if (buttonIndex == 52)
            {
                source.buttonIndex = 51;
            }
            else if (buttonIndex == 53)
            {
                source.buttonIndex = 52;
            }
            else if (buttonIndex == 54)
            {
                source.buttonIndex = 53;
            }
            else if (buttonIndex == 51)
            {
                source.buttonIndex = 0;
            }
            else if (buttonIndex == 23 || buttonIndex == 24)
            {
                source.buttonIndex = 47;
            }
            game.buttonSwitchSound.Play();
        }
        public void DownButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex >= 1 && buttonIndex <= 11)
            {
                source.buttonIndex += 11;
            }
            else if (buttonIndex >= 12 && buttonIndex < 23)
            {
                source.buttonIndex += 13;
            }
            else if (buttonIndex > 25 && buttonIndex <= 35)
            {
                source.buttonIndex += 10;
            }
            else if (buttonIndex >= 36 && buttonIndex <= 45)
            {   
                source.buttonIndex = 46;
            }
            else if (buttonIndex == 46)
            {
                source.buttonIndex = 5;
            }
            else if (buttonIndex == 47)
            {
                source.buttonIndex = 24;
            }
            else if (buttonIndex == 48)
            {
                source.buttonIndex = 49;
            }
            else if (buttonIndex == 49)
            {
                source.buttonIndex = 50;
            }
            else if (buttonIndex == 50)
            {
                source.buttonIndex = 47;
            }
            else if (buttonIndex == 0)
            {
                source.buttonIndex = 51;
            }
            else if (buttonIndex == 52)
            {
                source.buttonIndex = 53;
            }
            else if (buttonIndex == 53)
            {
                source.buttonIndex = 54;
            }
            else if (buttonIndex == 54)
            {
                source.buttonIndex = 0;
            }
            else if (buttonIndex == 51)
            {
                source.buttonIndex = 52;
            }
            else if (buttonIndex == 23 || buttonIndex == 24)
            {
                source.buttonIndex = 48;
            }
            else if (buttonIndex == 25)
            {
                source.buttonIndex += 11;
            }
            game.buttonSwitchSound.Play();
        }
    }
}
