using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ObjectSprite = XeniaLauncher.OzzzFramework.ObjectSprite;

namespace XeniaLauncher
{
    public interface IWindowEffects
    {
        public abstract void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex);
        public abstract void SetupEffects(Game1 game, Window source);
    }
}
