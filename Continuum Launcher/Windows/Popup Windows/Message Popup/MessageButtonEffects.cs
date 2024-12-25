using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeniaLauncher
{
    public class MessageButtonEffects : IWindowEffects
    {
        public void SetupEffects(Game1 game, Window window)
        {

        }
        public void ActivateButton(Game1 game, Window source, OzzzFramework.ObjectSprite origin, int buttonIndex)
        {
            if (game.message != null)
            {
                if (buttonIndex == 0)
                {
                    game.state = source.returnState;
                    if (game.message.buttons.Count > 1)
                    {
                        game.backSound.Play();
                    }
                    else
                    {
                        game.selectSound.Play();
                    }
                }
                else if (buttonIndex == 1)
                {
                    game.state = source.returnState;
                    game.selectSound.Play();
                    game.messageYes = true;
                }
            }
            else
            {
                game.state = source.returnState;
                game.selectSound.Play();
            }
        }
    }
}
