using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace XeniaLauncher
{
    public class DataWindowInput : BaseStdInputEvent
    {
        public int gameTitleIndex, oldIndex;
        public DataWindowInput() : base(6)
        {
            
        }
        public override void DownButton(Game1 game, Window source, int buttonIndex)
        {
            base.DownButton(game, source, buttonIndex);
            if (buttonIndex >= buttonCount - 1)
            {
                gameTitleIndex = source.stringIndex;
                if (gameTitleIndex == 0)
                {
                    gameTitleIndex = 0;
                    for (int i = 0; i < buttonCount; i++)
                    {
                        source.extraSprites[i].ToTextSprite().text = game.dataStrings.dataStringList[source.stringIndex + i];
                        source.extraSprites[i + 6].ToTextSprite().text = game.dataStrings.dataSizeList[source.stringIndex + i];
                        source.extraSprites[i + 6].JustifyRight(new Vector2(1820, 175 + i * 130));
                        source.extraSprites[i + 12].ToTextSprite().text = game.dataStrings.dataIdList[source.stringIndex + i];
                        if (source.extraSprites[i].ToTextSprite().text != "")
                        {
                            source.extraSprites[i + 18].visible = true;
                            source.extraSprites[i + 18].ToObjectSprite().textures[0] = game.icons[source.extraSprites[i].ToTextSprite().text];
                        }
                        else
                        {
                            source.extraSprites[i + 18].visible = false;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < buttonCount; i++)
                    {
                        source.extraSprites[i].ToTextSprite().text = game.dataStrings.dataStringList[source.stringIndex - buttonCount + 1 + i];
                        source.extraSprites[i + 6].ToTextSprite().text = game.dataStrings.dataSizeList[source.stringIndex - buttonCount + 1 + i];
                        source.extraSprites[i + 6].JustifyRight(new Vector2(1820, 175 + i * 130));
                        source.extraSprites[i + 12].ToTextSprite().text = game.dataStrings.dataIdList[source.stringIndex - buttonCount + 1 + i];
                        if (source.extraSprites[i].ToTextSprite().text != "")
                        {
                            source.extraSprites[i + 18].visible = true;
                            source.extraSprites[i + 18].ToObjectSprite().textures[0] = game.icons[source.extraSprites[i].ToTextSprite().text];
                        }
                        else
                        {
                            source.extraSprites[i + 18].visible = false;
                        }
                    }
                }
            }
        }
        public override void UpButton(Game1 game, Window source, int buttonIndex)
        {
            base.UpButton(game, source, buttonIndex);
            if (buttonIndex <= 0)
            {
                gameTitleIndex = source.stringIndex;
                if (gameTitleIndex == source.strings.Count - 1)
                {
                    gameTitleIndex = source.strings.Count - 1;
                    for (int i = 0; i < buttonCount; i++)
                    {
                        source.extraSprites[i].ToTextSprite().text = game.dataStrings.dataStringList[source.stringIndex - buttonCount + 1 + i];
                        source.extraSprites[i + 6].ToTextSprite().text = game.dataStrings.dataSizeList[source.stringIndex - buttonCount + 1 + i];
                        source.extraSprites[i + 6].JustifyRight(new Vector2(1820, 175 + i * 130));
                        source.extraSprites[i + 12].ToTextSprite().text = game.dataStrings.dataIdList[source.stringIndex - buttonCount + 1 + i];
                        if (source.extraSprites[i].ToTextSprite().text != "")
                        {
                            source.extraSprites[i + 18].visible = true;
                            source.extraSprites[i + 18].ToObjectSprite().textures[0] = game.icons[source.extraSprites[i].ToTextSprite().text];
                        }
                        else
                        {
                            source.extraSprites[i + 18].visible = false;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < buttonCount; i++)
                    {
                        source.extraSprites[i].ToTextSprite().text = game.dataStrings.dataStringList[source.stringIndex + i];
                        source.extraSprites[i + 6].ToTextSprite().text = game.dataStrings.dataSizeList[source.stringIndex + i];
                        source.extraSprites[i + 6].JustifyRight(new Vector2(1820, 175 + i * 130));
                        source.extraSprites[i + 12].ToTextSprite().text = game.dataStrings.dataIdList[source.stringIndex + i];
                        if (source.extraSprites[i].ToTextSprite().text != "")
                        {
                            source.extraSprites[i + 18].visible = true;
                            source.extraSprites[i + 18].ToObjectSprite().textures[0] = game.icons[source.extraSprites[i].ToTextSprite().text];
                        }
                        else
                        {
                            source.extraSprites[i + 18].visible = false;
                        }
                    }
                }
            }
        }
        public override void LeftButton(Game1 game, Window source, int buttonIndex)
        {
            UpButton(game, source, buttonIndex);
        }
        public override void RightButton(Game1 game, Window source, int buttonIndex)
        {
            DownButton(game, source, buttonIndex);
        }
    }
    public class DataWindowChangeEffects : IButtonIndexChangeEffects
    {
        public Game1 game;
        public Window source;
        public DataWindowChangeEffects(Game1 game, Window source)
        {
            this.game = game;
            this.source = source;
        }

        public void IndexChanged(int buttonIndex)
        {
            source.extraSprites[source.extraSprites.Count - 2].ToTextSprite().text = "" + (source.stringIndex + 1) + " of " + source.strings.Count;
        }
    }
}
