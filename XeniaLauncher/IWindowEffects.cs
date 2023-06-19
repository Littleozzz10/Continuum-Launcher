using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectSprite = XeniaLauncher.OzzzFramework.ObjectSprite;

namespace XeniaLauncher
{
    /// <summary>
    /// Interface for whenever a button in a Window is clicked/pressed
    /// </summary>
    public interface IWindowEffects
    {
        /// <summary>
        /// A method that will be executed whenever a button is clicked/pressed, otherwise known as activiating
        /// </summary>
        /// <param name="game">A reference to the Game1 instance being run</param>
        /// <param name="source">The Window the button was activated in</param>
        /// <param name="origin">The button that was activated, as an ObjectSprite</param>
        /// <param name="buttonIndex">The buttonIndex of the button that was activated (stringIndex is actually passed in from the Window class)</param>
        public abstract void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex);
        /// <summary>
        /// A method that will be executed on the first call to a Window's Update(), by way of Start() from the IStartEffects interface
        /// </summary>
        /// <param name="game">A reference to the Game1 instance being run</param>
        /// <param name="source">The source Window</param>
        public abstract void SetupEffects(Game1 game, Window source);
    }
}
