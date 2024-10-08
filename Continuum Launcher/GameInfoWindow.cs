﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Ozzz = XeniaLauncher.OzzzFramework;
using Sprite = XeniaLauncher.OzzzFramework.Sprite;
using ObjectSprite = XeniaLauncher.OzzzFramework.ObjectSprite;
using TextSprite = XeniaLauncher.OzzzFramework.TextSprite;
using Layer = XeniaLauncher.OzzzFramework.SpriteGroup.Layer;
using Button = XeniaLauncher.OzzzFramework.Button;
using Gradient = XeniaLauncher.OzzzFramework.Gradient;
using AnimationPath = XeniaLauncher.OzzzFramework.AnimationPath;
using MouseInput = XeniaLauncher.OzzzFramework.MouseInput;
using KeyboardInput = XeniaLauncher.OzzzFramework.KeyboardInput;
using Key = XeniaLauncher.OzzzFramework.KeyboardInput.Key;
using GamepadInput = XeniaLauncher.OzzzFramework.GamepadInput;
using AnalogPad = XeniaLauncher.OzzzFramework.GamepadInput.AnalogPad;
using DigitalPad = XeniaLauncher.OzzzFramework.GamepadInput.DigitalPad;
using GameData = XeniaLauncher.Shared.GameData;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;
using XLCompanion;
using System.Drawing;
using SharpDX.XAudio2;

namespace XeniaLauncher
{
    public class GameInfoWindow : IWindowEffects
    {
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (buttonIndex == 0)
            {
                game.text = new TextInputWindow(game, "Edit Game Title", game.gameData[game.index].gameTitle, Game1.State.GameInfo);
            }
            else if (buttonIndex == 1)
            {
                game.text = new TextInputWindow(game, "Edit Game Developer", game.gameData[game.index].developer, Game1.State.GameInfo);
            }
            else if (buttonIndex == 2)
            {
                game.text = new TextInputWindow(game, "Edit Game Publisher", game.gameData[game.index].publisher, Game1.State.GameInfo);
            }
            else if (buttonIndex == 3)
            {
                game.text = new TextInputWindow(game, "Edit Title ID", game.gameData[game.index].titleId, Game1.State.GameInfo);
            }
            else if (buttonIndex == 4)
            {
                if (game.gameData[game.index].gameTitle != "No Title")
                {
                    game.text = new TextInputWindow(game, "Alphabetize As", game.gameData[game.index].alphaAs, Game1.State.GameInfo);
                }
                else
                {
                    game.message = new MessageWindow(game, "Brave, But Foolish", "This game doesn't have a title. Add a title before changing it's alphabetization!", Game1.State.GameInfo);
                    game.state = Game1.State.Message;
                }
            }
            else if (buttonIndex == 5)
            {
                game.tempYear = game.gameData[game.index].year;
                game.tempMonth = game.gameData[game.index].month;
                game.tempDay = game.gameData[game.index].day;
                game.OpenDateEditWindow(Game1.State.ReleaseYear, Game1.State.GameInfo);
            }
            else if (buttonIndex == 6)
            {
                game.gameData[game.index].minPlayers--;
                if (game.gameData[game.index].minPlayers < 0)
                {
                    game.gameData[game.index].minPlayers = 4;
                }
                game.SaveGames();
                AdjustMinPlayers(game, source);
            }
            else if (buttonIndex == 7)
            {
                game.gameData[game.index].minPlayers++;
                if (game.gameData[game.index].minPlayers > 4)
                {
                    game.gameData[game.index].minPlayers = 0;
                }
                game.SaveGames();
                AdjustMinPlayers(game, source);
            }
            else if (buttonIndex == 8)
            {
                game.gameData[game.index].maxPlayers--;
                if (game.gameData[game.index].maxPlayers < 0)
                {
                    game.gameData[game.index].maxPlayers = 4;
                }
                game.SaveGames();
                AdjustMaxPlayers(game, source);
            }
            else if (buttonIndex == 9)
            {
                game.gameData[game.index].maxPlayers++;
                if (game.gameData[game.index].maxPlayers > 4)
                {
                    game.gameData[game.index].maxPlayers = 0;
                }
                game.SaveGames();
                AdjustMaxPlayers(game, source);
            }
            else if (buttonIndex == 10)
            {
                game.state = Game1.State.GameMenu;
                game.backSound.Play();
            }
        }
        public void SetupEffects(Game1 game, Window window)
        {
            for (int i = 0; i < 8; i++)
            {
                window.extraSprites.Add(new TextSprite(game.font, ""));
            }
            foreach (TextSprite sprite in window.extraSprites)
            {
                sprite.scale = 0.6f;
                sprite.color = Color.FromNonPremultiplied(255, 255, 255, 0);
            }
            AdjustMinPlayers(game, window);
            AdjustMaxPlayers(game, window);
        }
        private void AdjustMinPlayers(Game1 game, Window source)
        {
            source.extraSprites[0].ToTextSprite().text = "Min Players: " + game.gameData[game.index].minPlayers;
            source.extraSprites[0].Centerize(new Vector2(1305, 580));
        }
        private void AdjustMaxPlayers(Game1 game, Window source)
        {
            source.extraSprites[1].ToTextSprite().text = "Max Players: " + game.gameData[game.index].maxPlayers;
            source.extraSprites[1].Centerize(new Vector2(1305, 680));
        }
    }
    public class GameInfoInput : IButtonInputEvent
    {
        public void UpButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 0 || buttonIndex == 4)
            {
                buttonIndex = 10;
            }
            else if (buttonIndex <= 6)
            {
                buttonIndex--;
            }
            else if (buttonIndex == 10)
            {
                buttonIndex = 3;
            }
            else
            {
                buttonIndex -= 2;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void DownButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 3 || buttonIndex == 8 || buttonIndex == 9)
            {
                buttonIndex = 10;
            }
            else if (buttonIndex < 5)
            {
                buttonIndex++;
            }
            else if (buttonIndex == 10)
            {
                buttonIndex = 0;
            }
            else
            {
                buttonIndex += 2;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void LeftButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 0)
            {
                buttonIndex = 4;
            }
            else if (buttonIndex == 1)
            {
                buttonIndex = 5;
            }
            else if (buttonIndex == 2)
            {
                buttonIndex = 7;
            }
            else if (buttonIndex == 3)
            {
                buttonIndex = 9;
            }
            else if (buttonIndex == 4)
            {
                buttonIndex = 0;
            }
            else if (buttonIndex == 5)
            {
                buttonIndex = 1;
            }
            else if (buttonIndex == 6)
            {
                buttonIndex = 2;
            }
            else if (buttonIndex == 7 || buttonIndex == 9)
            {
                buttonIndex--;
            }
            else if (buttonIndex == 8)
            {
                buttonIndex = 3;
            }
            else if (buttonIndex == 10)
            {
                buttonIndex = 3;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
        public void RightButton(Game1 game, Window source, int buttonIndex)
        {
            if (buttonIndex == 10)
            {
                buttonIndex = 9;
            }
            else if (buttonIndex <= 1)
            {
                buttonIndex += 4;
            }
            else if (buttonIndex == 2)
            {
                buttonIndex = 6;
            }
            else if (buttonIndex == 3)
            {
                buttonIndex = 8;
            }
            else if (buttonIndex == 4)
            {
                buttonIndex = 0;
            }
            else if (buttonIndex == 5)
            {
                buttonIndex = 1;
            }
            else if (buttonIndex == 6 || buttonIndex == 8)
            {
                buttonIndex++;
            }
            else if (buttonIndex == 7)
            {
                buttonIndex = 2;
            }
            else if (buttonIndex == 9)
            {
                buttonIndex = 3;
            }
            game.buttonSwitchSound.Play();
            source.buttonIndex = buttonIndex;
            source.stringIndex = buttonIndex;
        }
    }
}
