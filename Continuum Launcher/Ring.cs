using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ObjectSprite = XeniaLauncher.OzzzFramework.ObjectSprite;
using Gradient = XeniaLauncher.OzzzFramework.Gradient;
using AnimationPath = XeniaLauncher.OzzzFramework.AnimationPath;

namespace XeniaLauncher
{
    public class Ring : ObjectSprite
    {
        public Gradient fade;
        public AnimationPath path;
        public enum RingType
        {
            LightGreen, Gray
        }
        public RingType type;
        public Ring(Texture2D ringTex, Rectangle rect, int fadeFrames, RingType type, Game1 game) : base(ringTex, rect)
        {
            if (type == RingType.LightGreen)
            {
                if (game.theme == Game1.Theme.Gray)
                {
                    fade = new Gradient(Color.FromNonPremultiplied(210, 210, 210, 255), fadeFrames);
                }
                if (game.theme == Game1.Theme.Purple)
                {
                    fade = new Gradient(Color.FromNonPremultiplied(120, 0, 120, 255), fadeFrames);
                }
                else
                {
                    fade = new Gradient(Color.FromNonPremultiplied(152, 211, 21, 255), fadeFrames);
                }
                fade.colors.Add(game.darkGradient.colors[0]);
            }
            else if (type == RingType.Gray)
            {
                if (game.theme == Game1.Theme.Original || game.theme == Game1.Theme.Gray)
                {
                    fade = new Gradient(Color.Gray, fadeFrames);
                }
                else if (game.theme == Game1.Theme.Green)
                {
                    fade = new Gradient(Color.FromNonPremultiplied(76, 106, 11, 255), fadeFrames);
                }
                else if (game.theme == Game1.Theme.Orange)
                {
                    fade = new Gradient(Color.FromNonPremultiplied(85, 40, 20, 255), fadeFrames);
                }
                else if (game.theme == Game1.Theme.Blue)
                {
                    fade = new Gradient(Color.DarkGray, fadeFrames);
                }
                else if (game.theme == Game1.Theme.Purple)
                {
                    fade = new Gradient(Color.FromNonPremultiplied(60, 60, 60, 255), fadeFrames);
                }
                fade.colors.Add(game.darkGradient.colors[1]);
            }
            fade.ValueUpdate(0);
            path = new AnimationPath(this, pos, 2.5f, fadeFrames);
            this.type = type;
        }
        public void Update()
        {
            UpdatePos();
            fade.Update();
            color = fade.GetColor();
            path.Update();
        }
    }
}
