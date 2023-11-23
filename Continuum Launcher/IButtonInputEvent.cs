using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeniaLauncher
{
    /// <summary>
    /// Interface for controlling input events for Window objects. Used primarily to control which button to be highlighted by different keystroke and button inputs on keyboard/controller
    /// </summary>
    public interface IButtonInputEvent
    {
        /// <summary>
        /// A method that is executed to handle Up Button presses
        /// </summary>
        /// <param name="game">A reference to the Game1 instance beign run</param>
        /// <param name="source">The source Window to control</param>
        /// <param name="buttonIndex">The currently highlighted buttonIndex</param>
        public abstract void UpButton(Game1 game, Window source, int buttonIndex);
        /// <summary>
        /// A method that is executed to handle Down Button presses
        /// </summary>
        /// <param name="game">A reference to the Game1 instance beign run</param>
        /// <param name="source">The source Window to control</param>
        /// <param name="buttonIndex">The currently highlighted buttonIndex</param>
        public abstract void DownButton(Game1 game, Window source, int buttonIndex);
        /// <summary>
        /// A method that is executed to handle Left Button presses
        /// </summary>
        /// <param name="game">A reference to the Game1 instance beign run</param>
        /// <param name="source">The source Window to control</param>
        /// <param name="buttonIndex">The currently highlighted buttonIndex</param>
        public abstract void LeftButton(Game1 game, Window source, int buttonIndex);
        /// <summary>
        /// A method that is executed to handle Right Button presses
        /// </summary>
        /// <param name="game">A reference to the Game1 instance beign run</param>
        /// <param name="source">The source Window to control</param>
        /// <param name="buttonIndex">The currently highlighted buttonIndex</param>
        public abstract void RightButton(Game1 game, Window source, int buttonIndex);
    }
}
