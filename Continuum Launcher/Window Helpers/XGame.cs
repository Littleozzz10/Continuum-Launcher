using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ObjectSprite = XeniaLauncher.OzzzFramework.ObjectSprite;
using Button = XeniaLauncher.OzzzFramework.Button;
using AnimationPath = XeniaLauncher.OzzzFramework.AnimationPath;

namespace XeniaLauncher
{
    public class XGame : Button
    {
        public AnimationPath leftPath, rightPath;
        public int index;
        public XGame(Texture2D texture, Rectangle rect) : base(texture, rect)
        {
            button.sprite.color = Color.White;
            button.sprite.ToObjectSprite().textures.Add(texture);
            button.sprite.visible = true;
            button.sprite.pos = new Vector2(rect.X, rect.Y);
            shrinkFactor = 1.0f;
            sprites[0].visible = false;
        }
        public XGame(Texture2D texture, Rectangle rect, Color color) : this(texture, rect)
        {
            button.sprite.color = color;
        }
        public void AdjustIndex(int dataCount)
        {
            if (dataCount == 1)
            {
                index = 0;
            }
            else if (dataCount == 2)
            {
                index = Math.Abs(index) % 2;
            }
            else
            {
                if (index >= dataCount)
                {
                    index = 0 + index - dataCount;
                }
                else if (index < 0)
                {
                    index += dataCount;
                }
            }
        }
    }
}
