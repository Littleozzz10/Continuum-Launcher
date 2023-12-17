using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeniaLauncher
{
    public class OptionsInput : IButtonInputEvent
    {
        public void UpButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex <= 1)
            {
                buttonIndex = 8;
            }
            else
            {
                buttonIndex -= 2;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void DownButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex >= 8)
            {
                buttonIndex = 0;
            }
            else if (buttonIndex == 7)
            {
                buttonIndex = 8;
            }
            else
            {
                buttonIndex += 2;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void LeftButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex != 8)
            {
                if (buttonIndex % 2 == 0)
                {
                    buttonIndex++;
                }
                else
                {
                    buttonIndex--;
                }
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void RightButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex != 8)
            {
                if (buttonIndex % 2 == 0)
                {
                    buttonIndex++;
                }
                else
                {
                    buttonIndex--;
                }
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
    }
}
