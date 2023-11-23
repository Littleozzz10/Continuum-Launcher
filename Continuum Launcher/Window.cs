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
    /// <summary>
    /// Class for making a Window
    /// </summary>
    public class Window : ObjectSprite
    {
        public Game1 game;
        public Gradient fadeGradient, blackGradient, selectGradient, whiteGradient, buttonGradient;
        public TextSprite titleSprite;
        public List<TextSprite> sprites;
        public List<ObjectSprite> buttons;
        public List<Sprite> extraSprites;
        public List<string> strings;
        public Game1.State returnState;
        public IWindowEffects buttonEffects;
        public IButtonInputEvent inputEvents;
        public IStartEffects startEffects;
        public IButtonIndexChangeEffects changeEffects;
        public int buttonIndex, stringIndex;
        public bool useFade, skipMainStateTransition, firstFrame;
        public Window(Game1 game, Rectangle rect, string title, IWindowEffects buttonEffects, IButtonInputEvent inputEvents, IStartEffects startEffects, Game1.State returnState) : this(game, rect, title, buttonEffects, inputEvents, startEffects, returnState, true) { }
        public Window(Game1 game, Rectangle rect, string title, IWindowEffects buttonEffects, IButtonInputEvent inputEvents, IStartEffects startEffects, Game1.State returnState, bool playSelectSound) : base(game.rectTex, rect, Color.FromNonPremultiplied(0, 0, 0, 0))
        {
            this.game = game;
            sprites = new List<TextSprite>();
            buttons = new List<ObjectSprite>();
            extraSprites = new List<Sprite>();
            strings = new List<string>();
            this.buttonEffects = buttonEffects;
            this.inputEvents = inputEvents;
            this.returnState = returnState;
            this.startEffects = startEffects;
            useFade = true;
            firstFrame = true;

            ResetGradients();

            stringIndex = 0;
            buttonIndex = 0;
            whiteGradient.ValueUpdate(0);
            whiteGradient.Update();

            if (playSelectSound)
            {
                game.selectSound.Play();
            }

            titleSprite = new TextSprite(game.bold, title, 0.65f, new Vector2(), Color.FromNonPremultiplied(0, 0, 0, 0));
            titleSprite.pos = titleSprite.Centerize(GetCenterPoint());
            titleSprite.pos.Y = pos.Y + 40;
        }
        /// <summary>
        /// Resets the Window's Gradients, for transitioning purposes
        /// </summary>
        public void ResetGradients()
        {
            fadeGradient = new Gradient(Color.FromNonPremultiplied(0, 0, 0, 0), 20);
            fadeGradient.colors.Add(Color.FromNonPremultiplied(0, 0, 0, 155));
            fadeGradient.ValueUpdate(0);
            blackGradient = new Gradient(Color.FromNonPremultiplied(133, 133, 133, 0), 20);
            blackGradient.colors = game.blackGradient.colors;
            blackGradient.ValueUpdate(0);
            buttonGradient = new Gradient(Color.FromNonPremultiplied(133, 133, 133, 0), 20);
            buttonGradient.colors = game.buttonGradient.colors;
            buttonGradient.ValueUpdate(0);
            selectGradient = new Gradient(Color.FromNonPremultiplied(133, 133, 133, 0), 20);
            selectGradient.colors = game.selectGradient.colors;
            selectGradient.ValueUpdate(0);
            whiteGradient = new Gradient(Color.FromNonPremultiplied(133, 133, 133, 0), 20);
            whiteGradient.colors.Add(Color.FromNonPremultiplied(255, 255, 255, 255));
            whiteGradient.ValueUpdate(0);
        }
        public void ResetTextPositions()
        {
            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Centerize(buttons[i].GetCenterPoint());
            }
        }
        /// <summary>
        /// Adds a button to the Window, at a location of (0, 0)
        /// </summary>
        /// <param name="rect"></param>
        public void AddButton(Rectangle rect)
        {
            buttons.Add(new ObjectSprite(game.rectTex, rect, Color.FromNonPremultiplied(0, 0, 0, 0)));
            sprites.Add(new TextSprite(game.font, "", 0.5f, Vector2.Zero, Color.White));
        }
        /// <summary>
        /// Adds text to a button, in order (Ex: The third call to AddText() will add text to the third button)
        /// </summary>
        /// <param name="text"></param>
        public void AddText(string text)
        {
            strings.Add(text);
            if (strings.Count <= buttons.Count)
            {
                sprites[strings.Count - 1].text = strings.Last();
                sprites[strings.Count - 1].color = Color.FromNonPremultiplied(0, 0, 0, 0);
            }
            ResetTextPositions();
        }
        /// <summary>
        /// Updates the Window. Should be called once every frame, if the Window is to be updated
        /// </summary>
        public void Update()
        {
            UpdatePos();
            if (firstFrame)
            {
                startEffects.Start(game, this);
            }

            // Mouse input
            bool mouseClick = false;
            for (int i = 0; i < buttons.Count && game.IsActive; i++)
            {
                if (buttons[i].CheckMouse(false))
                {
                    if ((buttonIndex != i && !firstFrame && MouseInput.positions[0] != MouseInput.positions[1]) || MouseInput.IsLeftFirstDown())
                    {
                        game.buttonSwitchSound.Play();
                        stringIndex += i - buttonIndex;
                        buttonIndex = i;
                        if (changeEffects != null)
                        {
                            changeEffects.IndexChanged(stringIndex);
                        }
                    }
                    if (MouseInput.IsLeftFirstDown())
                    {
                        mouseClick = true;
                    }
                }
            }
            // Executing an effect (A button, Enter key, Space key)
            if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.A, true) || KeyboardInput.keys["Enter"].IsFirstDown() || KeyboardInput.keys["Space"].IsFirstDown() || mouseClick) && game.IsActive && !firstFrame)
            {
                buttonEffects.ActivateButton(game, this, buttons[buttonIndex], stringIndex);
            }
            // Exiting the window (B button, Backspace key, Right click)
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.B, true) || KeyboardInput.keys["Backspace"].IsFirstDown() || MouseInput.IsRightFirstDown()) && game.IsActive && !firstFrame)
            {
                bool fromMessage = game.state == Game1.State.Message;
                game.state = returnState;
                ResetGradients();
                game.backSound.Play();
                if (game.state == Game1.State.Main && !skipMainStateTransition && !fromMessage)
                {
                    game.BeginMainTransition();
                }
            }
            // Cycling down (D-Pad Down, Down arrow key)
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.DPadDown, true) || (KeyboardInput.keys["Down"].IsFirstDown()) || MouseInput.scrollChange < 0) && game.IsActive)
            {
                inputEvents.DownButton(game, this, buttonIndex);
                if (changeEffects != null)
                {
                    changeEffects.IndexChanged(stringIndex);
                }
            }
            // Cycling up (D-Pad Up, Up arrow key)
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.DPadUp, true) || (KeyboardInput.keys["Up"].IsFirstDown()) || MouseInput.scrollChange > 0) && game.IsActive)
            {
                inputEvents.UpButton(game, this, buttonIndex);
                if (changeEffects != null)
                {
                    changeEffects.IndexChanged(stringIndex);
                }
            }
            // Cycling left (D-Pad Left, Left arrow key)
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.DPadLeft, true) || KeyboardInput.keys["Left"].IsFirstDown()) && game.IsActive)
            {
                inputEvents.LeftButton(game, this, buttonIndex);
                if (changeEffects != null)
                {
                    changeEffects.IndexChanged(stringIndex);
                }
            }
            // Cycling right (D-Pad Right, Right arrow key)
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.DPadRight, true) || KeyboardInput.keys["Right"].IsFirstDown()) && game.IsActive)
            {
                inputEvents.RightButton(game, this, buttonIndex);
                if (changeEffects != null)
                {
                    changeEffects.IndexChanged(stringIndex);
                }
            }

            // Updating text and Gradients
            ResetTextPositions();
            if (whiteGradient.frameCycle != 20)
            {
                fadeGradient.Update();
                blackGradient.Update();
                selectGradient.Update();
                whiteGradient.Update();
                buttonGradient.Update();
            }

            // Updating colors and extra sprites
            for (int i = 0; i < buttons.Count; i++)
            {
                if (i == buttonIndex)
                {
                    buttons[i].color = selectGradient.GetColor();
                }
                else
                {
                    buttons[i].color = buttonGradient.GetColor();
                }
            }
            foreach (TextSprite sprite in sprites)
            {
                sprite.color = whiteGradient.GetColor();
            }
            color = blackGradient.GetColor();
            titleSprite.color = whiteGradient.GetColor();
            foreach (Sprite sprite in extraSprites)
            {
                if (sprite.type == "ObjectSprite")
                {
                    sprite.ToObjectSprite().UpdatePos();
                }
                else if (sprite.type == "TextSprite")
                {
                    sprite.ToTextSprite().UpdatePos();
                }
                else
                {
                    sprite.UpdatePos();
                }
                sprite.color = whiteGradient.GetColor();
                if (sprite.HasTag("select"))
                {
                    sprite.color = selectGradient.GetColor();
                }
                else if (sprite.HasTag("gray"))
                {
                    sprite.color = Ozzz.Helper.DivideColor(whiteGradient.GetColor(), 2f);
                }
            }

            firstFrame = false;
        }
        public void Draw(SpriteBatch sb)
        {
            if (useFade)
            {
                sb.Draw(game.white, Ozzz.Scaling.RectangleScaled(0, 0, 1920, 1080), fadeGradient.GetColor());
            }
            base.Draw(sb);
            titleSprite.Draw(sb);
            foreach (ObjectSprite button in buttons)
            {
                button.Draw(sb);
            }
            foreach (TextSprite sprite in sprites)
            {
                sprite.Draw(sb);
            }
            foreach (Sprite sprite in extraSprites)
            {
                sprite.Draw(sb);
            }
        }
    }
}
