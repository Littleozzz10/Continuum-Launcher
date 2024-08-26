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
using DescriptionBox = XeniaLauncher.OzzzFramework.DescriptionBox;
using Gradient = XeniaLauncher.OzzzFramework.Gradient;
using AnimationPath = XeniaLauncher.OzzzFramework.AnimationPath;
using MouseInput = XeniaLauncher.OzzzFramework.MouseInput;
using KeyboardInput = XeniaLauncher.OzzzFramework.KeyboardInput;
using Key = XeniaLauncher.OzzzFramework.KeyboardInput.Key;
using GamepadInput = XeniaLauncher.OzzzFramework.GamepadInput;
using AnalogPad = XeniaLauncher.OzzzFramework.GamepadInput.AnalogPad;
using DigitalPad = XeniaLauncher.OzzzFramework.GamepadInput.DigitalPad;
using SharpDX.MediaFoundation;
using Continuum_Launcher;
using System.Reflection;

namespace XeniaLauncher
{
    /// <summary>
    /// Class for making a Window
    /// </summary>
    public class Window : ObjectSprite
    {
        public Game1 game;
        public Gradient fadeGradient, blackGradient, selectGradient, whiteGradient, buttonGradient;
        public TextSprite titleSprite, descSprite;
        public List<TextSprite> sprites;
        public List<ObjectSprite> buttons;
        public List<DescriptionBox> descriptionBoxes;
        public List<Sprite> extraSprites;
        public List<string> strings;
        public Vector2 stickDelays;
        public Game1.State returnState;
        public IWindowEffects buttonEffects;
        public IButtonInputEvent inputEvents;
        public IStartEffects startEffects;
        public IButtonIndexChangeEffects changeEffects;
        public int buttonIndex, stringIndex, stickDelayFirst, stickDelaySecond;
        public bool useFade, skipMainStateTransition, firstFrame, preferEscapeExit, disableKeyboard;
        public Window(Game1 game, Rectangle rect, string title, IWindowEffects buttonEffects, IButtonInputEvent inputEvents, IStartEffects startEffects, Game1.State returnState) : this(game, rect, title, buttonEffects, inputEvents, startEffects, returnState, true) { }
        public Window(Game1 game, Rectangle rect, string title, IWindowEffects buttonEffects, IButtonInputEvent inputEvents, IStartEffects startEffects, Game1.State returnState, bool playSelectSound) : this(game, rect, title, "", buttonEffects, inputEvents, startEffects, returnState, playSelectSound) { }
        public Window(Game1 game, Rectangle rect, string title, string description, IWindowEffects buttonEffects, IButtonInputEvent inputEvents, IStartEffects startEffects, Game1.State returnState, bool playSelectSound) : base(game.rectTex, rect, Color.FromNonPremultiplied(0, 0, 0, 0))
        {
            this.game = game;
            sprites = new List<TextSprite>();
            buttons = new List<ObjectSprite>();
            descriptionBoxes = new List<DescriptionBox>();
            extraSprites = new List<Sprite>();
            strings = new List<string>();
            this.buttonEffects = buttonEffects;
            this.inputEvents = inputEvents;
            this.returnState = returnState;
            this.startEffects = startEffects;
            useFade = true;
            firstFrame = true;
            preferEscapeExit = false;
            disableKeyboard = false;

            ResetGradients();

            stringIndex = 0;
            buttonIndex = 0;
            stickDelayFirst = 36;
            stickDelaySecond = 8;
            stickDelays = Vector2.Zero;
            whiteGradient.ValueUpdate(0);
            whiteGradient.Update();

            if (playSelectSound)
            {
                game.selectSound.Play();
            }

            titleSprite = new TextSprite(game.bold, title, 0.65f, new Vector2(), Color.FromNonPremultiplied(0, 0, 0, 0));
            descSprite = new TextSprite(game.font, description, 0.4f, new Vector2(), Color.FromNonPremultiplied(0, 0, 0, 0));

            Logging.Write(Logging.LogType.Important, Logging.LogEvent.WindowOpen, "Window opened: " + title);
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
            AddButton(rect, "", DescriptionBox.SpawnPositions.BelowRight, 0.1f);
        }
        /// <summary>
        /// Adds a button to the Window, at a location of (0, 0), with a description
        /// </summary>
        /// <param name="rect"></param>
        public void AddButton(Rectangle rect, string description, DescriptionBox.SpawnPositions spawnPos, float textScale)
        {
            buttons.Add(new ObjectSprite(game.rectTex, rect, Color.FromNonPremultiplied(0, 0, 0, 0)));
            sprites.Add(new TextSprite(game.font, "", 0.5f, Vector2.Zero, Color.White));
            DescriptionBox descBox = new DescriptionBox(buttons.Last(), game.font, game.rectTex, game.descColor, 75, 25);
            descBox.spawnPos = spawnPos;
            descBox.textSprite.color = Ozzz.Helper.NewColorAlpha(game.fontSelectColor, 255);
            descBox.textSprite.text = description;
            descBox.textSprite.scale = textScale;
            descBox.textSprite.splitDraw = true;
            descBox.UpdateSpawnPos();
            descriptionBoxes.Add(descBox);
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

            Logging.Write(Logging.LogType.Debug, Logging.LogEvent.WindowStringAdded, "Button added: " + text);
        }
        /// <summary>
        /// Closes the Window
        /// </summary>
        public void CloseWindow()
        {
            bool fromMessage = game.state == Game1.State.Message;
            game.state = returnState;
            ResetGradients();
            game.backSound.Play();
            if (game.state == Game1.State.Main && !skipMainStateTransition && !fromMessage)
            {
                game.BeginMainTransition();
            }
            Logging.Write(Logging.LogType.Standard, Logging.LogEvent.WindowClose, "Window closed: " + titleSprite.text);
        }
        /// <summary>
        /// Trigger the Down event
        /// </summary>
        public void TriggerDown()
        {
            inputEvents.DownButton(game, this, buttonIndex);
            if (changeEffects != null)
            {
                changeEffects.IndexChanged(stringIndex);
            }
            Logging.Write(Logging.LogType.Debug, Logging.LogEvent.WindowButtonIndexChanged, "String Index changed (Down): " + stringIndex);
        }
        /// <summary>
        /// Trigger the Up event
        /// </summary>
        public void TriggerUp()
        {
            inputEvents.UpButton(game, this, buttonIndex);
            if (changeEffects != null)
            {
                changeEffects.IndexChanged(stringIndex);
            }
            Logging.Write(Logging.LogType.Debug, Logging.LogEvent.WindowButtonIndexChanged, "String Index changed (Up): " + stringIndex);
        }
        /// <summary>
        /// Trigger the Left event
        /// </summary>
        public void TriggerLeft()
        {
            inputEvents.LeftButton(game, this, buttonIndex);
            if (changeEffects != null)
            {
                changeEffects.IndexChanged(stringIndex);
            }
            Logging.Write(Logging.LogType.Debug, Logging.LogEvent.WindowButtonIndexChanged, "String Index changed (Left): " + stringIndex);
        }
        /// <summary>
        /// Trigger the Right event
        /// </summary>
        public void TriggerRight()
        {
            inputEvents.RightButton(game, this, buttonIndex);
            if (changeEffects != null)
            {
                changeEffects.IndexChanged(stringIndex);
            }
            Logging.Write(Logging.LogType.Debug, Logging.LogEvent.WindowButtonIndexChanged, "String Index changed (Right): " + stringIndex);
        }

        private float CalculateButtonPan(int buttonIndex)
        {
            return ((float)(buttons[buttonIndex].rect.X + buttons[buttonIndex].rect.Width / 2.0f - 960.0f)) / 960.0f;
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
            titleSprite.pos = titleSprite.Centerize(GetCenterPoint());
            titleSprite.pos.Y = pos.Y + 40;
            descSprite.pos = descSprite.Centerize(GetCenterPoint());
            descSprite.pos.Y = pos.Y + 105;

            if (titleSprite.GetSize().X + 60 > rect.Width)
            {
                rect.Width = (int)(titleSprite.GetSize().X + 60);
                pos.X = 960 - rect.Width / 2;
            }

            // Mouse input
            bool mouseClick = false;
            for (int i = 0; i < buttons.Count && game.IsActive; i++)
            {
                if (buttons[i].CheckMouse(false) && buttons[i].visible)
                {
                    if ((buttonIndex != i && !firstFrame && MouseInput.positions[0] != MouseInput.positions[1]) || MouseInput.IsLeftFirstDown())
                    {
                        SoundEffectInstance sound = game.buttonSwitchSound.CreateInstance();
                        sound.Play();
                        //sound.Pan = CalculateButtonPan(i);
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
            // Checking if click was outside of Window
            if (!mouseClick)
            {
                if (MouseInput.IsLeftFirstDown() && !CheckMouse(false) && !firstFrame && game.IsActive && !game.tutorialLock)
                {
                    Vector2 mousePos = new Vector2(MouseInput.GetMouseRect().X, MouseInput.GetMouseRect().Y);
                    if (mousePos.X >= 0 && mousePos.X <= Ozzz.GetResolution().X && mousePos.Y >= 0 && mousePos.Y <= Ozzz.GetResolution().Y)
                    {
                        if (!(game.tutorial != null && game.state == Game1.State.Select))
                        {
                            CloseWindow();
                        }                        
                    }
                }
            }

            // Triggering description box
            if (descriptionBoxes.Count > 0)
            {
                descriptionBoxes[buttonIndex].TriggerTransparencyUpdate();
            }

            // Executing an effect (A button, Enter key, Space key)
            if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.A, true) || (KeyboardInput.keys["Enter"].IsFirstDown() && !disableKeyboard) || (KeyboardInput.keys["Space"].IsFirstDown() && !preferEscapeExit) || mouseClick) && game.IsActive && !firstFrame && (!game.tutorialLock || game.state == Game1.State.Message))
            {
                game.buttonSwitchSound.Play();
                buttonEffects.ActivateButton(game, this, buttons[buttonIndex], stringIndex);
                Logging.Write(Logging.LogType.Standard, Logging.LogEvent.WindowButtonActivated, "Button activated", "buttonText", strings[stringIndex]);
            }
            // Exiting the window (B button, Backspace key, Right click, Enter key if disableKeyboard)
            else if (((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.B, true) || GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.Back, true) || ((KeyboardInput.keys["Backspace"].IsFirstDown() && !preferEscapeExit) || (KeyboardInput.keys["Enter"].IsFirstDown() && disableKeyboard) || MouseInput.IsRightFirstDown())) && game.IsActive && !firstFrame && (!game.tutorialLock || game.state == Game1.State.Message)))
            {
                CloseWindow();
            }
            // Cycling down (D-Pad Down, Down arrow key)
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.DPadDown, true) || (KeyboardInput.keys["Down"].IsFirstDown() && !disableKeyboard) || MouseInput.scrollChange < 0) && game.IsActive && (!game.tutorialLock || game.state == Game1.State.Message))
            {
                TriggerDown();
            }
            // Cycling up (D-Pad Up, Up arrow key)
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.DPadUp, true) || (KeyboardInput.keys["Up"].IsFirstDown() && !disableKeyboard) || MouseInput.scrollChange > 0) && game.IsActive && (!game.tutorialLock || game.state == Game1.State.Message))
            {
                TriggerUp();
            }
            // Cycling left (D-Pad Left, Left arrow key)
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.DPadLeft, true) || KeyboardInput.keys["Left"].IsFirstDown() && !disableKeyboard) && game.IsActive && (!game.tutorialLock || game.state == Game1.State.Message))
            {
                TriggerLeft();
            }
            // Cycling right (D-Pad Right, Right arrow key)
            else if ((GamepadInput.IsButtonDown(PlayerIndex.One, Buttons.DPadRight, true) || KeyboardInput.keys["Right"].IsFirstDown() && !disableKeyboard) && game.IsActive && (!game.tutorialLock || game.state == Game1.State.Message))
            {
                TriggerRight();
            }

            // Left stick controls
            if (GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftStickX) <= -0.3f && game.IsActive && (!game.tutorialLock || game.state == Game1.State.Message))
            {
                // Checking for stick movement
                if (stickDelays.X == -stickDelayFirst - 1 || stickDelays.X > 0)
                {
                    TriggerLeft();
                    stickDelays.X = -stickDelayFirst; // Initial delay for faster selection
                }
                else if (stickDelays.X == 0)
                {
                    TriggerLeft();
                    stickDelays.X = -stickDelaySecond; // Faster selection
                }
                else
                {
                    stickDelays.X++;
                }
            }
            else if (!(GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftStickX) >= 0.3f) && game.IsActive && (!game.tutorialLock || game.state == Game1.State.Message))
            {
                stickDelays.X = -stickDelayFirst - 1; // Stick is "centered", stopping any movement
            }
            // Right stick controls
            if (GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftStickX) >= 0.3f && game.IsActive && (!game.tutorialLock || game.state == Game1.State.Message))
            {
                // Checking for stick movement
                if (stickDelays.X == stickDelayFirst + 1 || stickDelays.X < 0)
                {
                    TriggerRight();
                    stickDelays.X = stickDelayFirst; // Initial delay for faster selection
                }
                else if (stickDelays.X == 0)
                {
                    TriggerRight();
                    stickDelays.X = stickDelaySecond; // Faster selection
                }
                else
                {
                    stickDelays.X--;
                }
            }
            else if (!(GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftStickX) <= -0.3f) && game.IsActive && (!game.tutorialLock || game.state == Game1.State.Message))
            {
                stickDelays.X = stickDelayFirst + 1; // Stick is "centered", stopping any movement
            }
            // Down stick controls
            if (GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftStickY) <= -0.3f && game.IsActive && (!game.tutorialLock || game.state == Game1.State.Message))
            {
                // Checking for stick movement
                if (stickDelays.Y == -stickDelayFirst - 1 || stickDelays.Y > 0)
                {
                    TriggerDown();
                    stickDelays.Y = -stickDelayFirst; // Initial delay for faster selection
                }
                else if (stickDelays.Y == 0)
                {
                    TriggerDown();
                    stickDelays.Y = -stickDelaySecond; // Faster selection
                }
                else
                {
                    stickDelays.Y++;
                }
            }
            else if (!(GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftStickY) >= 0.3f) && game.IsActive && (!game.tutorialLock || game.state == Game1.State.Message))
            {
                stickDelays.Y = -stickDelayFirst - 1; // Stick is "centered", stopping any movement
            }
            // Up stick controls
            if (GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftStickY) >= 0.3f && game.IsActive && (!game.tutorialLock || game.state == Game1.State.Message))
            {
                // Checking for stick movement
                if (stickDelays.Y == stickDelayFirst + 1 || stickDelays.Y < 0)
                {
                    TriggerUp();
                    stickDelays.Y = stickDelayFirst; // Initial delay for faster selection
                }
                else if (stickDelays.Y == 0)
                {
                    TriggerUp();
                    stickDelays.Y = stickDelaySecond; // Faster selection
                }
                else
                {
                    stickDelays.Y--;
                }
            }
            else if (!(GamepadInput.GetAnalogInputData(PlayerIndex.One, AnalogPad.AnalogInput.LeftStickY) <= -0.3f) && game.IsActive && (!game.tutorialLock || game.state == Game1.State.Message))
            {
                stickDelays.Y = stickDelayFirst + 1; // Stick is "centered", stopping any movement
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
                    sprites[i].color = game.fontSelectColor;
                }
                else
                {
                    buttons[i].color = buttonGradient.GetColor();
                    sprites[i].color = game.fontColor;
                }
            }
            //foreach (TextSprite sprite in sprites)
            //{
            //    sprite.color = whiteGradient.GetColor();
            //}
            color = blackGradient.GetColor();
            titleSprite.color = Ozzz.Helper.NewColorAlpha(game.fontAltColor, whiteGradient.GetColor().A);
            descSprite.color = Ozzz.Helper.NewColorAlpha(game.fontAltLightColor, whiteGradient.GetColor().A);
            foreach (Sprite sprite in extraSprites)
            {
                if (sprite.type == "ObjectSprite")
                {
                    sprite.ToObjectSprite().UpdatePos();
                    sprite.color = whiteGradient.GetColor();
                }
                else if (sprite.type == "TextSprite")
                {
                    sprite.ToTextSprite().UpdatePos();
                    sprite.color = Ozzz.Helper.NewColorAlpha(game.fontAltColor, (int)whiteGradient.values[3]);
                }
                else
                {
                    sprite.UpdatePos();
                }
                
                if (sprite.HasTag("select"))
                {
                    sprite.color = selectGradient.GetColor();
                }
                else if (sprite.HasTag("gray"))
                {
                    sprite.color = Ozzz.Helper.NewColorAlpha(game.fontAltLightColor, (int)whiteGradient.values[3]);
                }
                else if (sprite.HasTag("black"))
                {
                    sprite.color = Ozzz.Helper.DivideColor(blackGradient.GetColor(), 2f);
                }
            }

            // Updating description boxes
            if (!game.tutorialLock)
            {
                foreach (DescriptionBox box in descriptionBoxes)
                {
                    box.Update();
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
            descSprite.Draw(sb);
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
            foreach (DescriptionBox box in descriptionBoxes)
            {
                if (box.textSprite.text != "" && !game.tutorialLock)
                {
                    box.Draw(sb);
                }
            }
        }
    }
}
