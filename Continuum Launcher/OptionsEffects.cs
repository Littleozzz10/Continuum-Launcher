using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace XeniaLauncher
{
    public class OptionsEffects : IWindowEffects
    {
        public List<string> themeKeys;
        public Game1.CWSettings compat;
        public int themeIndex;
        public OptionsEffects()
        {
            themeKeys = new List<string>() { "original", "green", "orange", "blue", "gray", "purple" };
        }
        public void SetupEffects(Game1 game, Window window)
        {
            // Volume
            AdjustVolume(game, window);
            // Themes
            if (System.IO.File.Exists("Content\\XLTheme.txt"))
            {
                themeKeys = new List<string>() { "original", "green", "orange", "blue", "gray", "purple", "custom" };
            }
            for (int i = 0; i < themeKeys.Count; i++)
            {
                if (themeKeys[i] == game.theme.ToString().ToLower())
                {
                    themeIndex = i;
                    break;
                }
            }
            // Compat
            compat = game.cwSettings;
            AdjustCompat(game, window);
        }
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            switch (buttonIndex)
            {
                case 0:
                    if (SoundEffect.MasterVolume > 0)
                    {
                        SoundEffect.MasterVolume -= 0.1f;
                    }
                    else
                    {
                        SoundEffect.MasterVolume = 1f;
                    }
                    AdjustVolume(game, source);
                    break;
                case 1:
                    if (SoundEffect.MasterVolume < 1f)
                    {
                        SoundEffect.MasterVolume += 0.1f;
                    }
                    else
                    {
                        SoundEffect.MasterVolume = 0f;
                    }
                    AdjustVolume(game, source);
                    break;
                case 2:
                    if (themeIndex > 0)
                    {
                        themeIndex--;
                    }
                    else
                    {
                        themeIndex = game.themeThumbnails.Count - 1;
                    }
                    AdjustTheme(game, source);
                    break;
                case 3:
                    if (themeIndex < game.themeThumbnails.Count - 1)
                    {
                        themeIndex++;
                    }
                    else
                    {
                        themeIndex = 0;
                    }
                    AdjustTheme(game, source);
                    break;
                case 4:
                    if (compat == Game1.CWSettings.Off)
                    {
                        compat = Game1.CWSettings.All;
                    }
                    else if (compat == Game1.CWSettings.Untested)
                    {
                        compat = Game1.CWSettings.Off;
                    }
                    else
                    {
                        compat = Game1.CWSettings.Untested;
                    }
                    AdjustCompat(game, source);
                    break;
                case 5:
                    if (compat == Game1.CWSettings.Off)
                    {
                        compat = Game1.CWSettings.Untested;
                    }
                    else if (compat == Game1.CWSettings.Untested)
                    {
                        compat = Game1.CWSettings.All;
                    }
                    else
                    {
                        compat = Game1.CWSettings.Off;
                    }
                    AdjustCompat(game, source);
                    break;
                case 6:
                    game.graphicsWindow = new Window(game, new Rectangle(460, 190, 1000, 740), "Graphics Settings", "Continuum Graphics Options", new GraphicsWindowEffects(), new OptionsInput(), new GraphicsWindowStart(), Game1.State.Options, true);
                    // Resolution buttons
                    game.graphicsWindow.AddButton(new Rectangle(910, 355, 90, 90), "Decrease Continuum's resolution.", Ozzz.DescriptionBox.SpawnPositions.BelowRight, 0.4f);
                    game.graphicsWindow.AddText("<");
                    game.graphicsWindow.AddButton(new Rectangle(1310, 355, 90, 90), "Increase Continuum's resolution.", Ozzz.DescriptionBox.SpawnPositions.BelowLeft, 0.4f);
                    game.graphicsWindow.AddText(">");
                    // Fullscreen buttons
                    game.graphicsWindow.AddButton(new Rectangle(910, 455, 90, 90), "Enable or disable Fullscreen mode.\nNote: Fullscreen can be buggy.", Ozzz.DescriptionBox.SpawnPositions.BelowRight, 0.4f);
                    game.graphicsWindow.AddText("<");
                    game.graphicsWindow.AddButton(new Rectangle(1310, 455, 90, 90), "Enable or disable Fullscreen mode.\nNote: Fullscreen can be buggy.", Ozzz.DescriptionBox.SpawnPositions.BelowLeft, 0.4f);
                    game.graphicsWindow.AddText(">");
                    // V-Sync buttons
                    game.graphicsWindow.AddButton(new Rectangle(910, 555, 90, 90), "Enable or Disable V-Sync for Continuum.", Ozzz.DescriptionBox.SpawnPositions.BelowRight, 0.4f);
                    game.graphicsWindow.AddText("<");
                    game.graphicsWindow.AddButton(new Rectangle(1310, 555, 90, 90), "Enable or Disable V-Sync for Continuum.", Ozzz.DescriptionBox.SpawnPositions.BelowLeft, 0.4f);
                    game.graphicsWindow.AddText(">");
                    // Rings buttons
                    game.graphicsWindow.AddButton(new Rectangle(910, 655, 90, 90), "Show or Hide Continuum's background ring effect.", Ozzz.DescriptionBox.SpawnPositions.BelowRight, 0.4f);
                    game.graphicsWindow.AddText("<");
                    game.graphicsWindow.AddButton(new Rectangle(1310, 655, 90, 90), "Show or Hide Continuum's background ring effect.", Ozzz.DescriptionBox.SpawnPositions.BelowLeft, 0.4f);
                    game.graphicsWindow.AddText(">");

                    game.graphicsWindow.AddButton(new Rectangle(660, 790, 600, 100));
                    game.graphicsWindow.AddText("Back To Options");

                    game.graphicsWindow.extraSprites.Add(new TextSprite(game.font, "Resolution:", 0.6f, new Vector2(500, 360), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.graphicsWindow.extraSprites.Add(new TextSprite(game.bold, "1728x972", 0.7f, new Vector2(1140, 360), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.graphicsWindow.extraSprites.Add(new TextSprite(game.font, "Fullscreen:", 0.6f, new Vector2(500, 460), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.graphicsWindow.extraSprites.Add(new TextSprite(game.bold, "Off", 0.7f, new Vector2(1140, 460), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.graphicsWindow.extraSprites.Add(new TextSprite(game.font, "V-Sync:", 0.6f, new Vector2(500, 560), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.graphicsWindow.extraSprites.Add(new TextSprite(game.bold, "Off", 0.7f, new Vector2(1140, 560), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.graphicsWindow.extraSprites.Add(new TextSprite(game.font, "Rings:", 0.6f, new Vector2(500, 660), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.graphicsWindow.extraSprites.Add(new TextSprite(game.bold, "On", 0.7f, new Vector2(1140, 660), Color.FromNonPremultiplied(0, 0, 0, 0)));

                    bool custom = true;
                    foreach (Vector2 res in GraphicsWindowEffects.resolutions)
                    {
                        if (res.Equals(game.GetResolution()))
                        {
                            custom = false;
                            break;
                        }
                    }
                    if (custom)
                    {
                        game.message = new MessageWindow(game, "Warning", "A custom resolution has been applied. Changing settings will remove it.", Game1.State.Graphics);
                        game.state = Game1.State.Message;
                    }
                    else
                    {
                        game.state = Game1.State.Graphics;
                    }
                    break;
                case 7:
                    game.settingsWindow = new Window(game, new Rectangle(460, 190, 1000, 740), "Xenia Settings", "Options for all games", new XeniaSettingsEffects(), new OptionsInput(), new GraphicsWindowStart(), Game1.State.Options, true);
                    // Resolution buttons
                    game.settingsWindow.AddButton(new Rectangle(910, 355, 90, 90), "Xenia Log Levels:\n  - 0: Critical errors only\n  - 1: Critical errors and other warnings\n  - 2: Critical errors, warnings, and other info\n  - 3: Debugging info (Highly storage intensive)", Ozzz.DescriptionBox.SpawnPositions.BelowRight, 0.4f);
                    game.settingsWindow.AddText("<");
                    game.settingsWindow.AddButton(new Rectangle(1310, 355, 90, 90), "Xenia Log Levels:\n  - 0: Critical errors only\n  - 1: Critical errors and other warnings\n  - 2: Critical errors, warnings, and other info\n  - 3: Debugging info (Highly storage intensive)", Ozzz.DescriptionBox.SpawnPositions.BelowLeft, 0.4f);
                    game.settingsWindow.AddText(">");
                    // Fullscreen buttons
                    game.settingsWindow.AddButton(new Rectangle(910, 455, 90, 90), "Whether or not to launch Xenia in Fullscreen.", Ozzz.DescriptionBox.SpawnPositions.BelowRight, 0.4f);
                    game.settingsWindow.AddText("<");
                    game.settingsWindow.AddButton(new Rectangle(1310, 455, 90, 90), "Whether or not to launch Xenia in Fullscreen.", Ozzz.DescriptionBox.SpawnPositions.BelowLeft, 0.4f);
                    game.settingsWindow.AddText(">");
                    // V-Sync buttons
                    game.settingsWindow.AddButton(new Rectangle(910, 555, 90, 90), "When enabled, Continuum will consolidate Xenia data within\nthe Launcher's files. Uses storage temporarily until manually\ncleared. Required to use the Manage Data window.", Ozzz.DescriptionBox.SpawnPositions.BelowRight, 0.4f);
                    game.settingsWindow.AddText("<");
                    game.settingsWindow.AddButton(new Rectangle(1310, 555, 90, 90), "When enabled, Continuum will consolidate Xenia data within\nthe Launcher's files. Uses storage temporarily until manually\ncleared. Required to use the Manage Data window.", Ozzz.DescriptionBox.SpawnPositions.BelowLeft, 0.4f);
                    game.settingsWindow.AddText(">");
                    // Rings buttons
                    game.settingsWindow.AddButton(new Rectangle(910, 655, 90, 90), "When running Headless, Xenia will not show any prompts\nthat require user interaction, instead using default actions.", Ozzz.DescriptionBox.SpawnPositions.BelowRight, 0.4f);
                    game.settingsWindow.AddText("<");
                    game.settingsWindow.AddButton(new Rectangle(1310, 655, 90, 90), "When running Headless, Xenia will not show any prompts\nthat require user interaction, instead using default actions.", Ozzz.DescriptionBox.SpawnPositions.BelowLeft, 0.4f);
                    game.settingsWindow.AddText(">");

                    game.settingsWindow.AddButton(new Rectangle(620, 790, 600, 100));
                    game.settingsWindow.AddText("Back To Options");

                    game.settingsWindow.extraSprites.Add(new TextSprite(game.font, "Log Level:", 0.6f, new Vector2(500, 360), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.settingsWindow.extraSprites.Add(new TextSprite(game.bold, "2 (Info)", 0.6f, new Vector2(1140, 360), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.settingsWindow.extraSprites.Add(new TextSprite(game.font, "Start Fullscreen:", 0.6f, new Vector2(500, 460), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.settingsWindow.extraSprites.Add(new TextSprite(game.bold, "No", 0.7f, new Vector2(1140, 460), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.settingsWindow.extraSprites.Add(new TextSprite(game.font, "Consolidate Saves:", 0.6f, new Vector2(500, 560), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.settingsWindow.extraSprites.Add(new TextSprite(game.bold, "Yes", 0.7f, new Vector2(1140, 560), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.settingsWindow.extraSprites.Add(new TextSprite(game.font, "Run Headless:", 0.6f, new Vector2(500, 660), Color.FromNonPremultiplied(0, 0, 0, 0)));
                    game.settingsWindow.extraSprites.Add(new TextSprite(game.bold, "No", 0.7f, new Vector2(1140, 660), Color.FromNonPremultiplied(0, 0, 0, 0)));

                    game.state = Game1.State.Settings;
                    break;
                case 8:
                    game.state = Game1.State.Menu;
                    game.backSound.Play();
                    break;
            }
        }
        private void AdjustVolume(Game1 game, Window source)
        {
            SoundEffect.MasterVolume = (float)Math.Round(SoundEffect.MasterVolume, 1);
            source.extraSprites[1].ToTextSprite().text = "" + SoundEffect.MasterVolume * 10;
            source.extraSprites[1].ToTextSprite().Centerize(new Vector2(1155, 350));
            game.SaveConfig();
        }
        private void AdjustTheme(Game1 game, Window source)
        {
            source.extraSprites[3].ToObjectSprite().textures[0] = game.themeThumbnails[themeKeys[themeIndex]];
            if (themeKeys[themeIndex] == "original")
            {
                game.ResetTheme(Game1.Theme.Original, true);
            }
            else if (themeKeys[themeIndex] == "green")
            {
                game.ResetTheme(Game1.Theme.Green, true);
            }
            else if (themeKeys[themeIndex] == "orange")
            {
                game.ResetTheme(Game1.Theme.Orange, true);
            }
            else if (themeKeys[themeIndex] == "blue")
            {
                game.ResetTheme(Game1.Theme.Blue, true);
            }
            else if (themeKeys[themeIndex] == "gray")
            {
                game.ResetTheme(Game1.Theme.Gray, true);
            }
            else if (themeKeys[themeIndex] == "purple")
            {
                game.ResetTheme(Game1.Theme.Purple, true);
            }
            else if (themeKeys[themeIndex] == "custom")
            {
                game.ResetTheme(Game1.Theme.Custom, true);
            }
            game.SaveConfig();
        }
        private void AdjustCompat(Game1 game, Window source)
        {
            if (compat == Game1.CWSettings.Off)
            {
                source.extraSprites[5].ToTextSprite().text = "Never Show";
                source.extraSprites[5].ToTextSprite().Centerize(new Vector2(1155, 650));
            }
            else if (compat == Game1.CWSettings.Untested)
            {
                source.extraSprites[5].ToTextSprite().text = "Untested Only";
                source.extraSprites[5].ToTextSprite().Centerize(new Vector2(1155, 650));
            }
            else if (compat == Game1.CWSettings.All)
            {
                source.extraSprites[5].ToTextSprite().text = "All Games";
                source.extraSprites[5].ToTextSprite().Centerize(new Vector2(1155, 650));
            }
            game.cwSettings = compat;
            game.SaveConfig();
        }
    }
}
