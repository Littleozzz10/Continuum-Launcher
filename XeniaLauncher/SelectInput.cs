using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeniaLauncher
{
    public class SelectInput : IButtonInputEvent
    {
        // For the record, this lovely chunk of spaghetti code is due to
        // me being too lazy to update indexes for existing buttons, as
        // prior to the Compatibility window being implemented, the Select
        // Window simply used StdInputEvent and didn't corresponding buttons
        public void UpButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 0)
            {
                buttonIndex = 3;
            }
            else if (buttonIndex == 4)
            {
                buttonIndex = 2;
            }
            else
            {
                buttonIndex--;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void DownButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 3 || buttonIndex == 4)
            {
                buttonIndex = 0;
            }
            else
            {
                buttonIndex++;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void LeftButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 3)
            {
                buttonIndex = 4;
            }
            else if (buttonIndex == 4)
            {
                buttonIndex = 3;
            }
            else if (buttonIndex == 0)
            {
                buttonIndex = 3;
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
            if (buttonIndex == 3)
            {
                buttonIndex = 4;
            }
            else if (buttonIndex == 4)
            {
                buttonIndex = 3;
            }
            else if (buttonIndex == 2)
            {
                buttonIndex = 4;
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
}
