using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XeniaLauncher;
using static XeniaLauncher.OzzzFramework;

namespace Continuum_Launcher
{
    public class TutorialMenu : IWindowEffects
    {
        public void SetupEffects(Game1 game, Window window)
        {

        }

        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (game.tutorial == null)
            {
                if (buttonIndex == 0)
                {
                    game.tutorial = new Tutorial(game, "Content\\Tutorials\\0001\\tutinfo.dat");
                    if (game.tutorial.StartTutorial())
                    {
                        game.state = Game1.State.Main;
                    }
                    game.selectSound.Play();
                }
                else if (buttonIndex == 1)
                {
                    game.tutorial = new Tutorial(game, "Content\\Tutorials\\0002\\tutinfo.dat");
                    if (game.tutorial.StartTutorial())
                    {
                        game.state = Game1.State.Main;
                    }
                    game.selectSound.Play();
                }
                else if (buttonIndex == 2)
                {
                    game.tutorial = new Tutorial(game, "Content\\Tutorials\\0003\\tutinfo.dat");
                    if (game.tutorial.StartTutorial())
                    {
                        game.state = Game1.State.Main;
                    }
                    game.selectSound.Play();
                }
                else if (buttonIndex == 3)
                {
                    game.tutorial = new Tutorial(game, "Content\\Tutorials\\0004\\tutinfo.dat");
                    if (game.tutorial.StartTutorial())
                    {
                        game.state = Game1.State.Main;
                    }
                    game.selectSound.Play();
                }
                else if (buttonIndex == 4)
                {
                    game.state = source.returnState;
                    game.backSound.Play();
                }
            }
            else
            {
                game.message = new MessageWindow(game, "We don't do Inception here.", "Cannot start a new Tutorial while one is active.", Game1.State.TutorialSelect);
                game.state = Game1.State.Message;
            }
        }
    }
}
