using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XeniaLauncher;
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
using SaveData = XeniaLauncher.Shared.SaveData;
using SaveDataObject = XeniaLauncher.Shared.SaveData.SaveDataObject;
using SaveDataChunk = XeniaLauncher.Shared.SaveData.SaveDataChunk;
using SequenceFade = XeniaLauncher.OzzzFramework.SequenceFade;
using GameData = XeniaLauncher.Shared.GameData;
using DescriptionBox = XeniaLauncher.OzzzFramework.DescriptionBox;
using DBSpawnPos = XeniaLauncher.OzzzFramework.DescriptionBox.SpawnPositions;
using LogType = Continuum_Launcher.Logging.LogType;
using Event = Continuum_Launcher.Logging.LogEvent;

namespace Continuum_Launcher
{
    public class TutorialBox : DescriptionBox
    {
        public Game1 game;
        public TextSprite continueText;
        public string tutorialText;
        public int cooldownFrames;
        public bool rollingText;
        public Game1.State targetState;
        public TutorialBox(Game1 game, Vector2 pos, string text, float scale, int cooldownFrames, bool rollingText) : base(new ObjectSprite(game.white, new Rectangle((int)pos.X, (int)pos.Y, 1, 1), Color.FromNonPremultiplied(0, 0, 0, 0)), game.font, game.rectTex, game.descColor, 1, 255)
        {
            this.game = game;
            tutorialText = text;
            this.cooldownFrames = cooldownFrames;
            this.rollingText = rollingText;

            spawnPos = DBSpawnPos.CenterPerfect;
            textSprite.color = Ozzz.Helper.NewColorAlpha(game.fontSelectColor, 255);
            textSprite.scale = scale;
            textSprite.splitDraw = true;
            if (!rollingText)
            {
                textSprite.text = text;
            }
            else
            {
                textSprite.text = tutorialText.Substring(0, 1);
            }
            UpdateSpawnPos();

            continueText = new TextSprite(game.font, "[ENTER], (A), or {CLICK} to continue", 0.3f);
            continueText.color = game.fontAltLightColor;
            continueText.visible = false;

            targetState = Game1.State.None;
        }
        public TutorialBox(Game1 game, Vector2 pos, string text, float scale, Game1.State targetState, bool rollingText) : this(game, pos, text, scale, 11222005, rollingText)
        {
            this.targetState = targetState;
        }
        private void ShowContinueText()
        {
            textSprite.text += "\n";
            continueText.Centerize(new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height - continueText.GetSize().Y * continueText.scale + 5));
            continueText.visible = true;
        }
        public void Update()
        {
            base.Update();
            TriggerTransparencyUpdate();

            // Rolling text
            if (rollingText)
            {
                if (textSprite.text.Length < tutorialText.Length)
                {
                    textSprite.text += tutorialText.Substring(textSprite.text.Length, 1);
                    game.buttonSwitchSound.Play(0.3f, 0.25f, 0f);
                }
            }

            continueText.UpdatePos();

            // Cooldown effects
            if (rollingText)
            {
                if (textSprite.text.Length == tutorialText.Length && targetState == Game1.State.None)
                {
                    ShowContinueText();
                    cooldownFrames = -1;
                }
            }
            else
            {
                if (cooldownFrames >= 0)
                {
                    cooldownFrames--;
                }
                if (cooldownFrames == 0)
                {
                    ShowContinueText();
                }
            }
            if (cooldownFrames == -1)
            {
                Vector2 mousePos = new Vector2(MouseInput.GetMouseRect().X, MouseInput.GetMouseRect().Y);
                if (((MouseInput.IsLeftFirstDown() && (mousePos.X >= 0 && mousePos.X <= Ozzz.GetResolution().X && mousePos.Y >= 0 && mousePos.Y <= Ozzz.GetResolution().Y)) || KeyboardInput.keys["Enter"].IsFirstDown() || GamepadInput.digitals[PlayerIndex.One].IsFirstButtonDown(Buttons.A)) && game.IsActive)
                {
                    game.tutorial.TriggerNextStep();
                    game.selectSound.Play();
                }
            }
#if DEBUG
            else if (KeyboardInput.keys["LCtrl"].IsFirstDown())
            {
                game.tutorial.TriggerNextStep();
            }
#endif

            // targetState stuff
            if (game.state == targetState)
            {
                game.tutorial.TriggerNextStep();
            }
        }
        public void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            continueText.Draw(sb);
        }
    }
}
