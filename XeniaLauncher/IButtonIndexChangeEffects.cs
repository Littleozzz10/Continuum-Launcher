using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeniaLauncher
{
    /// <summary>
    /// Interface for when a new button is selected in a Window
    /// </summary>
    public interface IButtonIndexChangeEffects
    {
        /// <summary>
        /// A method to be executed whenever the buttonIndex of a Window is changed
        /// </summary>
        /// <param name="buttonIndex">The new buttonIndex of the Window (In the Window class, the stringIndex variable is passed into this parameter)</param>
        public abstract void IndexChanged(int buttonIndex);
    }
}
