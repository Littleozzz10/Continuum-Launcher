using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XeniaLauncher
{
    public class StdInputEvent : BaseStdInputEvent
    {
        public int buttonCount;
        public StdInputEvent(int buttonCount) : base(buttonCount)
        {

        }
        public new void DownButton(Game1 game, Window source, int buttonIndex)
        {
            base.DownButton(game, source, buttonIndex);
        }
        public new void UpButton(Game1 game, Window source, int buttonIndex)
        {
            base.UpButton(game, source, buttonIndex);
        }
        public new void LeftButton(Game1 game, Window source, int buttonIndex)
        {
            UpButton(game, source, buttonIndex);
        }
        public new void RightButton(Game1 game, Window source, int buttonIndex)
        {
            DownButton(game, source, buttonIndex);
        }
    }
}
