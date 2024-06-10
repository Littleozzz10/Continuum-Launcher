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
using SharpDX.XAudio2;

namespace XeniaLauncher
{
    // Year stuff
    public class EditYearEffects : IWindowEffects
    {
        int oldYear;
        public EditYearEffects()
        {
            
        }
        public void SetupEffects(Game1 game, Window window)
        {
            window.extraSprites.Add(new TextSprite(game.bold, "2005", 0.6f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.buttonIndex = game.tempYear - 2005;
            window.stringIndex = window.buttonIndex;
            window.changeEffects.IndexChanged(window.stringIndex);
            oldYear = game.tempYear;
        }
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex >= 0 && buttonIndex <= 13)
            {
                game.tempYear = Convert.ToInt32(source.strings[source.stringIndex]);
                game.OpenDateEditWindow(Game1.State.ReleaseMonth, source.returnState);
            }
            else
            {
                game.backSound.Play();
                game.state = source.returnState;
                game.tempYear = oldYear;
            }
        }
    }
    public class EditYearChangeEffects : IButtonIndexChangeEffects
    {
        public Window window;
        public EditYearChangeEffects(Window window)
        {
            this.window = window;
        }
        public void IndexChanged(int buttonIndex)
        {
            if (window.extraSprites.Count > 0)
            {
                if (buttonIndex >= 0 && buttonIndex <= 13)
                {
                    window.extraSprites[0].ToTextSprite().text = window.strings[buttonIndex];
                    window.extraSprites[0].Centerize(new Vector2(960, 760));
                }
            }
        }
    }
    public class EditYearInput : IButtonInputEvent
    {
        public void UpButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex >= 0 && buttonIndex <= 3)
            {
                buttonIndex = 14;
            }
            else if (buttonIndex == 12 || buttonIndex == 13)
            {
                buttonIndex -= 3;
            }
            else if (buttonIndex == 14)
            {
                buttonIndex = 12;
            }
            else
            {
                buttonIndex -= 4;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void DownButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 8 || buttonIndex == 9)
            {
                buttonIndex = 12;
            }
            else if (buttonIndex == 10 || buttonIndex == 11)
            {
                buttonIndex = 13;
            }
            else if (buttonIndex == 12 || buttonIndex == 13)
            {
                buttonIndex = 14;
            }
            else if (buttonIndex == 14)
            {
                buttonIndex = 0;
            }
            else
            {
                buttonIndex += 4;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void LeftButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 0 || buttonIndex == 4 || buttonIndex == 8)
            {
                buttonIndex += 3;
            }
            else if (buttonIndex == 12)
            {
                buttonIndex++;
            }
            else if (buttonIndex == 14)
            {
                buttonIndex = 8;
            }
            else
            {
                buttonIndex--;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void RightButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 3 || buttonIndex == 7 || buttonIndex == 11)
            {
                buttonIndex -= 3;
            }
            else if (buttonIndex == 12)
            {
                buttonIndex++;
            }
            else if (buttonIndex == 13)
            {
                buttonIndex--;
            }
            else if (buttonIndex == 14)
            {
                buttonIndex = 11;
            }
            else
            {
                buttonIndex++;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
    }
    // Month stuff
    public class EditMonthEffects : IWindowEffects
    {
        int oldMonth;
        public EditMonthEffects()
        {

        }
        public void SetupEffects(Game1 game, Window window)
        {
            window.extraSprites.Add(new TextSprite(game.bold, "01-2005", 0.6f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.buttonIndex = game.tempMonth - 1;
            window.stringIndex = window.buttonIndex;
            window.changeEffects.IndexChanged(window.stringIndex);
            if (game.tempYear == 2005)
            {
                for (int i = 0; i < 10; i++)
                {
                    window.buttons[i].visible = false;
                    window.sprites[i].visible = false;
                }
            }
            oldMonth = game.tempYear;
        }
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex >= 0 && buttonIndex <= 11)
            {
                game.tempMonth = Convert.ToInt32(source.strings[source.stringIndex]);
                game.OpenDateEditWindow(Game1.State.ReleaseDay, source.returnState);
            }
            else if (buttonIndex == 12)
            {
                game.OpenDateEditWindow(Game1.State.ReleaseYear, source.returnState);
            }
            else
            {
                game.backSound.Play();
                game.state = source.returnState;
                game.tempMonth = oldMonth;
            }
        }
    }
    public class EditMonthInput : IButtonInputEvent
    {
        public void UpButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex >= 0 && buttonIndex <= 3)
            {
                buttonIndex = 12;
            }
            else if (buttonIndex == 12)
            {
                buttonIndex = 8;
            }
            else
            {
                buttonIndex -= 4;
            }
            buttonIndex = ReadjustIndex(source, buttonIndex, 12, false);
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void DownButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex >= 8 && buttonIndex <= 11)
            {
                buttonIndex = 12;
            }
            else if (buttonIndex == 12)
            {
                buttonIndex = 0;
            }
            else
            {
                buttonIndex += 4;
            }
            buttonIndex = ReadjustIndex(source, buttonIndex, 12, true);
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void LeftButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 0 || buttonIndex == 4 || buttonIndex == 8)
            {
                buttonIndex += 3;
            }
            else if (buttonIndex == 12)
            {
                buttonIndex = 8;
            }
            else
            {
                buttonIndex--;
            }
            buttonIndex = ReadjustIndex(source, buttonIndex, 12, false);
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void RightButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 3 || buttonIndex == 7 || buttonIndex == 11)
            {
                buttonIndex -= 3;
            }
            else if (buttonIndex == 12)
            {
                buttonIndex = 11;
            }
            else
            {
                buttonIndex++;
            }
            buttonIndex = ReadjustIndex(source, buttonIndex, 12, true);
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        private int ReadjustIndex(Window source, int buttonIndex, int maxIndex, bool increment)
        {
            while (!source.buttons[buttonIndex].visible)
            {
                if (increment)
                {
                    buttonIndex++;
                    if (buttonIndex > maxIndex)
                    {
                        buttonIndex = 0;
                    }
                }
                else
                {
                    buttonIndex--;
                    if (buttonIndex < 0)
                    {
                        buttonIndex = maxIndex;
                    }
                }
            }
            return buttonIndex;
        }
    }
    public class EditMonthChangeEffects : IButtonIndexChangeEffects
    {
        public Window window;
        public EditMonthChangeEffects(Window window)
        {
            this.window = window;
        }
        public void IndexChanged(int buttonIndex)
        {
            if (window.extraSprites.Count > 0)
            {
                if (buttonIndex >= 0 && buttonIndex <= 11)
                {
                    window.extraSprites[0].ToTextSprite().text = window.strings[buttonIndex] + "-" + window.game.tempYear;
                    window.extraSprites[0].Centerize(new Vector2(960, 760));
                }
            }
        }
    }
    // Day stuff
    public class EditDayEffects : IWindowEffects
    {
        int oldDay;
        public EditDayEffects()
        {

        }
        public void SetupEffects(Game1 game, Window window)
        {
            window.extraSprites.Add(new TextSprite(game.bold, "01-2005", 0.6f, new Vector2(), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.buttonIndex = game.tempMonth - 1;
            window.stringIndex = window.buttonIndex;
            window.changeEffects.IndexChanged(window.stringIndex);
            if (game.tempYear == 2005 && game.tempMonth == 11)
            {
                for (int i = 0; i < 21; i++)
                {
                    window.buttons[i].visible = false;
                    window.sprites[i].visible = false;
                }
            }
            if (game.tempMonth == 2 || game.tempMonth == 4 || game.tempMonth == 6 || game.tempMonth == 9 || game.tempMonth == 11)
            {
                HideButton(window, 30);
            }
            if (game.tempMonth == 2)
            {
                HideButton(window, 29);
                if (game.tempYear % 4 != 0)
                {
                    HideButton(window, 28);
                }
            }
            oldDay = game.tempYear;
        }
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex >= 0 && buttonIndex <= 30)
            {
                game.tempDay = Convert.ToInt32(source.strings[source.stringIndex]);
                game.selectSound.Play();
                game.state = source.returnState;
            }
            else if (buttonIndex == 31)
            {
                game.OpenDateEditWindow(Game1.State.ReleaseMonth, source.returnState);
            }
            else
            {
                game.backSound.Play();
                game.state = source.returnState;
                game.tempDay = oldDay;
            }
        }
        private void HideButton(Window window, int buttonIndex)
        {
            window.buttons[buttonIndex].visible = false;
            window.sprites[buttonIndex].visible = false;
            for (int i = 24; i < 30; i++)
            {
                window.buttons[i].pos.X += 50;
                window.buttons[i].UpdatePos();
                window.sprites[i].pos.X += 50;
                window.sprites[i].UpdatePos();
            }
        }
    }
    public class EditDayChangeEffects : IButtonIndexChangeEffects
    {
        public Window window;
        public EditDayChangeEffects(Window window)
        {
            this.window = window;
        }
        public void IndexChanged(int buttonIndex)
        {
            if (window.extraSprites.Count > 0)
            {
                if (buttonIndex >= 0 && buttonIndex <= 30)
                {
                    window.extraSprites[0].ToTextSprite().text = "" + window.game.tempMonth + "-" + window.strings[buttonIndex] + "-" + window.game.tempYear;
                    window.extraSprites[0].Centerize(new Vector2(960, 760));
                }
            }
        }
    }
}
