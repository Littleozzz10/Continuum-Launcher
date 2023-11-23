using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeniaLauncher
{
    /// <summary>
    /// Interface for controlling and executing the StartEffects() method from the IWindowEffects interface
    /// </summary>
    public interface IStartEffects
    {
        /// <summary>
        /// A method that will be executed on the first call to a Window's Update(). Used to execute the StartEffects() method
        /// </summary>
        /// <param name="game">A reference to the Game1 instance being run</param>
        /// <param name="source">The source Window</param>
        public abstract void Start(Game1 game, Window source);
    }
}
