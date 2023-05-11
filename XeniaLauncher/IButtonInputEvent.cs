using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeniaLauncher
{
    public interface IButtonInputEvent
    {
        public abstract void UpButton(Game1 game, Window source, int buttonIndex);
        public abstract void DownButton(Game1 game, Window source, int buttonIndex);
        public abstract void LeftButton(Game1 game, Window source, int buttonIndex);
        public abstract void RightButton(Game1 game, Window source, int buttonIndex);
    }
}
