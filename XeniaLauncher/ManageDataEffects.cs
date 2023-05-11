using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeniaLauncher
{
    public class ManageDataEffects : IWindowEffects
    {
        public void ActivateButton(Game1 game, Window source, OzzzFramework.ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex != 0)
            {
                if (game.dataFiles[game.selectedDataIndex][buttonIndex].subTitle != "Resources")
                {
                    game.toDelete = game.dataFiles[game.selectedDataIndex][buttonIndex];
                    game.message = new MessageWindow(game, "Delete File", "Are you sure you want to delete " + game.dataFiles[game.selectedDataIndex][buttonIndex].name + "?", Game1.State.Data, MessageWindow.MessagePrompts.YesNo);
                    game.state = Game1.State.Message;
                }
                else
                {
                    game.message = new MessageWindow(game, "Error", "Unable to delete resources currently loaded into Continuum.", Game1.State.Manage);
                    game.state = Game1.State.Message;
                }
            }
            else
            {
                game.message = new MessageWindow(game, "Error", "While Continuum is in a beta state, as a precaution, it cannot delete game files.", Game1.State.Manage);
                game.state = Game1.State.Message;
            }
        }
        public void SetupEffects(Game1 game, Window source)
        {

        }
    }
}
