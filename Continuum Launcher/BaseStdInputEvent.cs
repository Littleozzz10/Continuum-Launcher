using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeniaLauncher
{
    public abstract class BaseStdInputEvent : IButtonInputEvent
    {
        public int buttonCount;
        public BaseStdInputEvent(int buttonCount)
        {
            this.buttonCount = buttonCount;
        }
        public virtual void DownButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex < buttonCount - 1)
            {
                buttonIndex++;
                source.stringIndex++;
            }
            else
            {
                source.stringIndex++;
                if (source.stringIndex >= source.strings.Count)
                {
                    source.stringIndex = 0;
                    buttonIndex = 0;
                    for (int i = 0; i < buttonCount; i++)
                    {
                        source.sprites[i].text = source.strings[source.stringIndex + i];
                    }
                }
                else
                {
                    for (int i = 0; i < buttonCount; i++)
                    {
                        source.sprites[i].text = source.strings[source.stringIndex - buttonCount + 1 + i];
                    }
                }
            }
            source.buttonIndex = buttonIndex;
            game.buttonSwitchSound.Play();
        }
        public virtual void UpButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex > 0)
            {
                buttonIndex--;
                source.stringIndex--;
            }
            else
            {
                source.stringIndex--;
                if (source.stringIndex < 0)
                {
                    source.stringIndex = source.strings.Count - 1;
                    buttonIndex = buttonCount - 1;
                    for (int i = 0; i < buttonCount; i++)
                    {
                        source.sprites[i].text = source.strings[source.stringIndex - buttonCount + 1 + i];
                    }
                }
                else
                {
                    for (int i = 0; i < buttonCount; i++)
                    {
                        source.sprites[i].text = source.strings[source.stringIndex + i];
                    }
                }
            }
            source.buttonIndex = buttonIndex;
            game.buttonSwitchSound.Play();
        }
        public virtual void LeftButton(Game1 game, Window source, int buttonIndex)
        {
            UpButton(game, source, buttonIndex);
        }
        public virtual void RightButton(Game1 game, Window source, int buttonIndex)
        {
            DownButton(game, source, buttonIndex);
        }
    }
}
