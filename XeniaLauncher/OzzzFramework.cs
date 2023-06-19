using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XeniaLauncher
{
    /// <summary>
    /// Static class that contains all classes and helper methods from the OzzzFramework.
    /// </summary>
    public static class OzzzFramework
    {
        /// <summary>
        /// The global scale that all Framework components adhere to.
        /// </summary>
        public static Vector2 scale;
        /// <summary>
        /// The most recent calculation of the frame delta.
        /// </summary>
        public static float frameDelta;
        /// <summary>
        /// The target frame rate that CalculateFrameDelta uses to base its calculations on.
        /// </summary>
        public static float targetFrameRate;
        /// <summary>
        /// The number of samples to use when calculating FPS. More samples means frame drops are less noticeable.
        /// </summary>
        public static int frameSamples;
        private static Queue<float> frameTimes;
        /// <summary>
        /// Initializes the variables necessary for the Framework, including scaling.
        /// </summary>
        /// <param name="scale">The scale that all Framework components will adhere to. 
        /// Any calculations done outside of the Framework should call the appropriate scaling methods to ensure compatibility with all resolutions.</param>
        public static void Initialize(Vector2 scale, float targetFrameRate)
        {
            MouseInput.state = Mouse.GetState();
            MouseInput.positions = new List<Vector2>();
            KeyboardInput.state = Keyboard.GetState();
            KeyboardInput.keys = new Dictionary<string, KeyboardInput.Key>();
            GamepadInput.digitals = new Dictionary<PlayerIndex, GamepadInput.DigitalPad>();
            GamepadInput.analogs = new Dictionary<PlayerIndex, GamepadInput.AnalogPad>();
            GamepadInput.indexes = new List<PlayerIndex>();
            OzzzFramework.scale = scale;
            OzzzFramework.targetFrameRate = targetFrameRate;
            frameTimes = new Queue<float>();
            frameSamples = 1;
        }
        /// <summary>
        /// Calculates and returns the frame delta (How long the last frame took to render compared to how long it should according to the target FPS)
        /// </summary>
        /// <param name="gameTime">The GameTime object parameter from the default Update method.</param>
        /// <returns></returns>
        public static float CalculateFrameDelta(GameTime gameTime)
        {
            while (frameTimes.Count >= frameSamples)
            {
                frameTimes.Dequeue();
            }
            if (gameTime.ElapsedGameTime.Milliseconds > 0)
            {
                frameTimes.Enqueue(gameTime.ElapsedGameTime.Milliseconds);
                frameDelta = gameTime.ElapsedGameTime.Milliseconds / 1000f / (1.0f / targetFrameRate);
            }
            else
            {
                frameDelta = 0;
            }
            return frameDelta;
        }
        /// <summary>
        /// Gets the FPS
        /// </summary>
        public static float GetFPS()
        {
            float times = 0;
            int samples = frameTimes.Count;
            for (int i = 0; i < samples; i++)
            {
                float time = frameTimes.Dequeue();
                times += 1000 / time;
                frameTimes.Enqueue(time);
            }
            return times / frameTimes.Count;
        }
        /// <summary>
        /// Static class to handle scaling.
        /// </summary>
        public static class Scaling
        {
            /// <summary>
            /// Adjusts a byte based on the Horizontal Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static byte ScaleByteX(byte num)
            {
                return (byte)(num * scale.X);
            }
            /// <summary>
            /// Adjusts a byte based on the Vertical Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static byte ScaleByteY(byte num)
            {
                return (byte)(num * scale.Y);
            }
            /// <summary>
            /// Adjusts a short based on the Horizontal Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static short ScaleShortX(short num)
            {
                return (short)(num * scale.X);
            }
            /// <summary>
            /// Adjusts a short based on the Vertical Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static short ScaleShortY(short num)
            {
                return (short)(num * scale.Y);
            }
            /// <summary>
            /// Adjusts a ushort based on the Horizontal Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static ushort ScaleUShortX(ushort num)
            {
                return (ushort)(num * scale.X);
            }
            /// <summary>
            /// Adjusts a ushort based on the Vertical Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static ushort ScaleUShortY(ushort num)
            {
                return (ushort)(num * scale.Y);
            }
            /// <summary>
            /// Adjusts an int based on the Horizontal Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static int ScaleIntX(int num)
            {
                return (int)(num * scale.X);
            }
            /// <summary>
            /// Adjusts an int based on the Vertical Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static int ScaleIntY(int num)
            {
                return (int)(num * scale.Y);
            }
            /// <summary>
            /// Adjusts a uint based on the Horizontal Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static uint ScaleUIntX(uint num)
            {
                return (uint)(num * scale.X);
            }
            /// <summary>
            /// Adjusts a uint based on the Vertical Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static uint ScaleUIntY(uint num)
            {
                return (uint)(num * scale.Y);
            }
            /// <summary>
            /// Adjusts a long based on the Horizontal Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static long ScaleLongX(long num)
            {
                return (long)(num * scale.X);
            }
            /// <summary>
            /// Adjusts a long based on the Vertical Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static long ScaleLongY(long num)
            {
                return (long)(num * scale.Y);
            }
            /// <summary>
            /// Adjusts a ulong based on the Horizontal Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static ulong ScaleULongX(ulong num)
            {
                return (ulong)(num * scale.X);
            }
            /// <summary>
            /// Adjusts a ulong based on the Vertical Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static ulong ScaleULongY(ulong num)
            {
                return (ulong)(num * scale.Y);
            }
            /// <summary>
            /// Adjusts a float based on the Horizontal Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static float ScaleFloatX(float num)
            {
                return (float)(num * scale.X);
            }
            /// <summary>
            /// Adjusts a float based on the Vertical Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static float ScaleFloatY(float num)
            {
                return (float)(num * scale.Y);
            }
            /// <summary>
            /// Adjusts a double based on the Horizontal Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static double ScaleDoubleX(double num)
            {
                return (double)(num * scale.X);
            }
            /// <summary>
            /// Adjusts a double based on the Vertical Scale.
            /// </summary>
            /// <param name="num">The number to adjust.</param>
            /// <returns>An adjusted number</returns>
            public static double ScaleDoubleY(double num)
            {
                return (double)(num * scale.Y);
            }
            /// <summary>
            /// Adjusts a Point based on the Scale.
            /// </summary>
            /// <param name="num">The Point to adjust</param>
            /// <returns>An adjusted Point</returns>
            public static Point ScalePoint(Point num)
            {
                return new Point(ScaleIntX(num.X), ScaleIntY(num.Y));
            }
            /// <summary>
            /// Adjusts a Vector2 based on the Scale.
            /// </summary>
            /// <param name="num">The Vector2 to adjust</param>
            /// <returns>An adjusted Vector2</returns>
            public static Vector2 ScaleVector2(Vector2 num)
            {
                return num * scale;
            }
            /// <summary>
            /// Adjusts a Rectangle based on the Scale.
            /// </summary>
            /// <param name="rect">The Rectangle to adjust</param>
            /// <returns>An adjusted Vector2</returns>
            public static Rectangle ScaleRectangle(Rectangle rect)
            {
                rect.X = ScaleIntX(rect.X);
                rect.Y = ScaleIntY(rect.Y);
                rect.Width = ScaleIntX(rect.Width);
                rect.Height = ScaleIntY(rect.Height);
                return rect;
            }
            /// <summary>
            /// Returns a new Vector2 automatically scaled.
            /// </summary>
            public static Vector2 Vector2Scaled(float x, float y)
            {
                return ScaleVector2(new Vector2(x, y));
            }
            /// <summary>
            /// Returns a new Rectangle automatically scaled.
            /// </summary>
            public static Rectangle RectangleScaled(int x, int y, int width, int height)
            {
                return ScaleRectangle(new Rectangle(x, y, width, height));
            }
            /// <summary>
            /// Resizes a Rectangle according to the provided size.
            /// </summary>
            /// <param name="rect">The Rectangle to resize.</param>
            /// <param name="size">The size to resize the Rectangle to. 1.0 is 100%.</param>
            /// <returns>The resized Rectangle</returns>
            public static Rectangle ResizeRectangle(Rectangle rect, float size)
            {
                Rectangle newRect = new Rectangle();
                newRect.X = (int)(rect.X + (rect.Width - rect.Width * size) / 2);
                newRect.Y = (int)(rect.Y + (rect.Height - rect.Height * size) / 2);
                newRect.Width = (int)(size * rect.Width);
                newRect.Height = (int)(size * rect.Height);
                return newRect;
            }
            /// <summary>
            /// Resizes a Rectangle according to the provided size. Also scales the Rectangle with the global framework scale.
            /// </summary>
            /// <param name="rect">The Rectangle to resize.</param>
            /// <param name="size">The size to resize the Rectangle to. 1.0 is 100%.</param>
            /// <returns>The resized Rectangle</returns>
            public static Rectangle ResizeRectangleToScale(Rectangle rect, float size)
            {
                Rectangle newRect = ResizeRectangle(rect, size);
                newRect = ScaleRectangle(newRect);
                return newRect;
            }
            /// <summary>
            /// Resizes a Vector2 according to the provided size.
            /// </summary>
            /// <param name="vec">The Vector2 to resize.</param>
            /// <param name="wh">The Width and Height to base resizing on.</param>
            /// <param name="size">The size to resize the Vector2 by. 1.0 is 100%.</param>
            public static Vector2 ResizeVector2(Vector2 vec, Vector2 wh, float size)
            {
                Vector2 newVec = new Vector2();
                newVec.X = (int)(vec.X + (wh.X - wh.X * size) / 2);
                newVec.Y = (int)(vec.Y + (wh.Y - wh.Y * size) / 2);
                return newVec;
            }
        }
        /// <summary>
        /// Basic helper class for miscellaneous functions.
        /// </summary>
        public static class Helper
        {
            /// <summary>
            /// Creates an array of random numbers.
            /// </summary>
            /// <param name="count">Number of random integers to populate the array with.</param>
            /// <param name="min">The smallest number allowed to be generated.</param>
            /// <param name="max">The largest number allowed to be generated.</param>
            /// <returns>An int array of COUNT random integers within the specified MIN and MAX</returns>
            public static int[] RandomNumberArray(int count, int min, int max)
            {
                int[] result = new int[count];
                Random rng = new Random();
                for (int i = 0; i < count; i++)
                {
                    result[i] = rng.Next(min, max);
                }
                return result;
            }
            /// <summary>
            /// Creates a string with the elements of the given array. Does not contain brackets like a typical ToString() would. 
            /// Deliminates with the deliminator parameter, does not automatically insert a space.
            /// </summary>
            /// <typeparam name="T">Type is recommended to have a built-in ToString().</typeparam>
            /// <param name="array">The array to use.</param>
            /// <param name="deliminator">The string to deliminate each element by. Spaces are not automatically added, add space(s) if spacing is desired.</param>
            /// <returns>A string containing each element of the given array, deliminated by the given deliminator</returns>
            public static string ArrayString<T>(T[] array, string deliminator)
            {
                string str = "";
                for (int i = 0; i < array.Length; i++)
                {
                    str = str + array[i].ToString();
                    if (i + 1 < array.Length)
                    {
                        str = str + deliminator;
                    }
                }
                return str;
            }
            /// <summary>
            /// Returns a string containing the int, with enough leading zeros to reach the specified target length.
            /// </summary>
            /// <param name="num">The number to use.</param>
            /// <param name="targetLength">How long the returned string should be. Will add leading zeros until this length is achieved.</param>
            public static string IntToString(int num, int targetLength)
            {
                string str = "" + num;
                if (targetLength > -1)
                {
                    while (str.Length < targetLength)
                    {
                        str = "0" + str;
                    }
                }
                return str;
            }
            /// <summary>
            /// Divides each value in a color by a divisor.
            /// </summary>
            /// <param name="color">The color to divide.</param>
            /// <param name="divisor">The number to divide by.</param>
            public static Color DivideColor(Color color, float divisor)
            {
                return Color.FromNonPremultiplied((byte)(color.R / divisor), (byte)(color.G / divisor), (byte)(color.B / divisor), color.A);
            }
            public static float VectorToAngle(Vector2 vector)
            {
                //float angle = 0f;
                throw new NotImplementedException();
                //return angle;
            }
        }
        /// <summary>
        /// An abstract class used for polymorphic organization and inheritance purposes.
        /// </summary>
        public abstract class Sprite
        {
            public Color color;
            public Vector2 pos, velocity, rect, origin;
            /// <summary>
            /// Holds the applicable static forces acting on the Sprite on an update call. 
            /// The Z position in each Vector3 is the duration in frames the force will continue for.
            /// </summary>
            public List<Vector3> staticForces;
            public SpriteEffects effects;
            public float rotation, layerDepth;
            /// <summary>
            /// The size ratio of the sprite. A value of 1.0 is 100% size.
            /// </summary>
            public float size;
            /// <summary>
            /// Holds general purpose client-defined string tags.
            /// </summary>
            public List<string> tags;
            /// <summary>
            /// Mainly used for internal comparisons of ObjectSprite and TextSprite types when the normal type() method doesn't work as well.
            /// </summary>
            public string type;
            /// <summary>
            /// Dictates whether or not the Sprite should be drawn when the Draw() method is called.
            /// </summary>
            public bool visible;
            /// <summary>
            /// Dictates whether or not the Sprite's position will be updated based on the Framework's frame delta calculations.
            /// </summary>
            public bool useFrameDelta;
            /// <summary>
            /// Dictates whether or not the Sprite will be drawn by any Layers it is part of.
            /// </summary>
            public bool skipLayerDraw;
            public Sprite()
            {
                color = Color.White;
                effects = SpriteEffects.None;
                rotation = 0f;
                tags = new List<string>();
                velocity = Vector2.Zero;
                pos = Vector2.Zero;
                visible = true;
                staticForces = new List<Vector3>();
                type = "Sprite";
                size = 1.0f;
            }
            /// <summary>
            /// Checks for intersection with a Rectangle object.
            /// </summary>
            /// <param name="rect">The Rectangle to be checked against.</param>
            /// <returns>A bool dictating intersection</returns>
            public abstract bool Intersects(Rectangle rect);
            /// <summary>
            /// Checks for intersection with an ObjectSprite object.
            /// </summary>
            /// <param name="sprite">The ObjectSprite to be checked against</param>
            /// <returns>A bool dictating intersection</returns>
            public abstract bool Intersects(ObjectSprite sprite);
            /// <summary>
            /// Checks for intersection with a TextSprite object
            /// </summary>
            /// <param name="sprite">The TextSprite to be checked against</param>
            /// <returns>A bool dictating intersection</returns>
            public abstract bool Intersects(TextSprite sprite);
            /// <summary>
            /// Returns a Rectangle representing the Sprite.
            /// </summary>
            public abstract Rectangle GenerateRect();
            /// <summary>
            /// Returns the X and Y coordinates where the Sprite would be in the center of the screen. Assumes a resolution of 1920x1080.
            /// </summary>
            public Vector2 GetCenterCoords()
            {
                return new Vector2(960 - GetSize().X / 2, 540 - GetSize().Y / 2);
            }
            /// <summary>
            /// Centers the Sprite around the given point.
            /// </summary>
            /// <param name="c">The points to center the Sprite around.</param>
            /// <returns>The new position of the Sprite</returns>
            public Vector2 Centerize(Vector2 c)
            {
                pos = new Vector2(c.X - GetSize().X / 2, c.Y - GetSize().Y / 2);
                return pos;
            }
            /// <summary>
            /// Positions the Sprite so that the right-most edge of the Sprite is
            /// on the x-coordinate of 'rightEdge'.
            /// </summary>
            /// <param name="rightEdge"></param>
            /// <returns></returns>
            public Vector2 JustifyRight(Vector2 rightEdge)
            {
                pos = new Vector2(rightEdge.X - GetSize().X, rightEdge.Y);
                return pos;
            }
            /// <summary>
            /// Checks to see if the mouse is hovering over the Sprite, and can check if the Left Mouse Button is clicked. Requires Initialization.
            /// </summary>
            /// <param name="click">If true, also checks if the Left Mouse Button is down.</param>
            /// <returns>A bool dictating if the conditions have been met</returns>
            public bool CheckMouse(bool click)
            {
                //if (click)
                //{
                //    return Intersects(MouseInput.GetMouseRect()) && MouseInput.IsLeftDown();
                //}
                //return Intersects(MouseInput.GetMouseRect());
                if (click)
                {
                    return Scaling.ScaleRectangle(GenerateRect()).Intersects(MouseInput.GetMouseRect()) && MouseInput.IsLeftDown();
                }
                return Scaling.ScaleRectangle(GenerateRect()).Intersects(MouseInput.GetMouseRect());
            }
            /// <summary>
            /// Checks if the Sprite has a tag.
            /// </summary>
            /// <param name="tag">The tag to find.</param>
            /// <returns>A bool dictating if the given tag was found in the Sprite</returns>
            public bool HasTag(string tag)
            {
                foreach (string t in tags)
                {
                    if (tag == t)
                    {
                        return true;
                    }
                }
                return false;
            }
            /// <summary>
            /// Checks if this Sprite and the parameter sprite have a tag.
            /// </summary>
            /// <param name="tag">The tag to find.</param>
            /// <param name="sprite">The Sprite to additionally check for the tag.</param>
            /// <returns>A bool dictating if both Sprites have the given tag</returns>
            public bool HasTag(string tag, Sprite sprite)
            {
                return HasTag(tag) && sprite.HasTag(tag);
            }
            /// <summary>
            /// Gets the Width and Height of the Sprite as a Vector2.
            /// </summary>
            public abstract Vector2 GetSize();
            /// <summary>
            /// Gets the Center point of the Sprite as a Vector2.
            /// </summary>
            public abstract Vector2 GetCenterPoint();
            /// <summary>
            /// Updates the Position of the Sprite, applying forces in the staticForces List and applying velocity. 
            /// Will usually be overriden in a concrete implementation.
            /// </summary>
            /// <returns>A Vector2 of the Sprite's new position</returns>
            public Vector2 UpdatePos()
            {
                if (!useFrameDelta)
                {
                    pos += velocity;
                    for (int i = 0; i < staticForces.Count; i++)
                    {
                        pos += Scaling.ScaleVector2(new Vector2(staticForces[i].X, staticForces[i].Y));
                        staticForces[i] -= new Vector3(0, 0, 1);
                        if (staticForces[i].Z <= 0)
                        {
                            staticForces.RemoveAt(i);
                            i--;
                        }
                    }
                }
                else
                {
                    pos += velocity * new Vector2(frameDelta, frameDelta);
                    for (int i = 0; i < staticForces.Count; i++)
                    {
                        pos += Scaling.ScaleVector2(new Vector2(staticForces[i].X * frameDelta, staticForces[i].Y * frameDelta));
                        staticForces[i] -= new Vector3(0, 0, 1 * frameDelta);
                        if (staticForces[i].Z <= 0)
                        {
                            staticForces.RemoveAt(i);
                            i--;
                        }
                    }
                }
                return pos;
            }
            /// <summary>
            /// Draws the Sprite to the given SpriteBatch.
            /// </summary>
            /// <param name="sb">The SpriteBatch to draw to.</param>
            public abstract void Draw(SpriteBatch sb);
            /// <summary>
            /// Draws the Sprite to the given SpriteBatch.
            /// </summary>
            /// <param name="sb">The SpriteBatch to draw to.</param>
            public abstract void Draw(SpriteBatch sb, bool noScale);
            /// <summary>
            /// Returns the Sprite as a TextSprite. Used for polymorphic interation. Should only be used if the client knows the Sprite will always be a TextSprite object.
            /// </summary>
            /// <exception cref="InvalidCastException"></exception>
            public abstract TextSprite ToTextSprite();
            /// <summary>
            /// Returns the Sprite as an ObjectSprite. Used for polymorphic interation. Should only be used if the client knows the Sprite will always be an ObjectSprite object.
            /// </summary>
            /// <exception cref="InvalidCastException"></exception>
            public abstract ObjectSprite ToObjectSprite();
            /// <summary>
            /// Returns the Sprite as a Layer (SpriteGroup.Layer). Used for polymorphic interaction. Should only be used if the client knows the Sprite will always be a Layer object.
            /// </summary>
            /// <exception cref="InvalidCastException"></exception>
            public abstract SpriteGroup.Layer ToLayer();
            /// <summary>
            /// Returns the Sprite as a Button. Used for polymorphic interaction. Should only be used if the client knows the Sprite will always be a Button object.
            /// </summary>
            /// <exception cref="InvalidCastException"></exception>
            public abstract Button ToButton();
        }
        /// <summary>
        /// A concrete implementation of the abstract Sprite class. Used for texture-based Sprites.
        /// </summary>
        public class ObjectSprite : Sprite
        {
            /// <summary>
            /// Holds the textures for the Sprite.
            /// </summary>
            public List<Texture2D> textures;
            /// <summary>
            /// 'rect': The destination rectangle for the Sprite. 'sourceRect': The source rectangle for the Sprite's texture.
            /// </summary>
            public Rectangle rect, sourceRect;
            /// <summary>
            /// The index of the texture to display when drawing the Sprite.
            /// </summary>
            public int texIndex;
            /// <param name="initialTexture">The first texture to add to the Sprite's List of textures.</param>
            /// <param name="rect">The destination rectangle. Automatically scales.</param>
            public ObjectSprite(Texture2D initialTexture, Rectangle rect) : base()
            {
                textures = new List<Texture2D>();
                textures.Add(initialTexture);
                this.rect = rect;
                pos = new Vector2(this.rect.X, this.rect.Y);
                sourceRect = new Rectangle(0, 0, initialTexture.Width, initialTexture.Height);
                texIndex = 0;
                type = "ObjectSprite";
            }
            /// <param name="initialTexture">The first texture to add to the Sprite's List of textures.</param>
            /// <param name="rect">The destination rectangle.</param>
            /// <param name="color">The Color for drawing the Sprite.</param>
            public ObjectSprite(Texture2D initialTexture, Rectangle rect, Color color) : base()
            {
                textures = new List<Texture2D>();
                textures.Add(initialTexture);
                this.rect = rect;
                pos = new Vector2(this.rect.X, this.rect.Y);
                sourceRect = new Rectangle(0, 0, initialTexture.Width, initialTexture.Height);
                texIndex = 0;
                this.color = color;
                type = "ObjectSprite";
            }
            private Rectangle AdjustRectScale()
            {
                return Scaling.ResizeRectangle(rect, size);
            }
            public override bool Intersects(Rectangle rect)
            {
                return AdjustRectScale().Intersects(rect);
            }
            public override bool Intersects(ObjectSprite sprite)
            {
                return AdjustRectScale().Intersects(sprite.rect);
            }
            public override bool Intersects(TextSprite sprite)
            {
                return AdjustRectScale().Intersects(sprite.GenerateRect());
            }
            public override Rectangle GenerateRect()
            {
                return AdjustRectScale();
            }
            /// <summary>
            /// Generates a "pixel map" based on the Sprite's current texture, for use with per-pixel collision and/or lighting.
            /// </summary>
            /// <exception cref="NotImplementedException"></exception>
            public void GeneratePixelMap()
            {
                Color[,] colors = new Color[textures[texIndex].Width, textures[texIndex].Height];
                //textures[texIndex].GetData(colors);
                throw new NotImplementedException();
            }
            public override Vector2 GetSize()
            {
                return new Vector2(rect.Width, rect.Height);
            }
            public override Vector2 GetCenterPoint()
            {
                return new Vector2(pos.X + rect.Width / 2, pos.Y + rect.Height / 2);
            }
            /// <summary>
            /// Updates the position and rectangle of the Sprite. Also updates the source rectangle's Width and Height to match the current texture.
            /// </summary>
            /// <returns>A Vector2 of the Sprite's new position</returns>
            public new Vector2 UpdatePos()
            {
                base.UpdatePos();
                rect.X = (int)pos.X;
                rect.Y = (int)pos.Y;
                sourceRect.Width = textures[texIndex].Width;
                sourceRect.Height = textures[texIndex].Height;
                return pos;
            }
            /// <summary>
            /// Sets the Vector2 position with the X and Y integer variables from the rectangle.
            /// </summary>
            public void UpdatePosFromRect()
            {
                pos.X = rect.X;
                pos.Y = rect.Y;
            }
            /// <summary>
            /// Cycles to the previous texture in the List.
            /// </summary>
            public void PrevTexture()
            {
                texIndex--;
                if (texIndex < 0)
                {
                    texIndex = textures.Count - 1;
                }
            }
            /// <summary>
            /// Cycles to the next texture in the List.
            /// </summary>
            public void NextTexture()
            {
                texIndex++;
                if (texIndex == textures.Count)
                {
                    texIndex = 0;
                }
            }
            public override void Draw(SpriteBatch sb)
            {
                if (visible)
                {
                    sb.Draw(textures[texIndex], Scaling.ResizeRectangleToScale(rect, size), sourceRect, color, rotation, origin, effects, layerDepth);
                }
            }
            public override void Draw(SpriteBatch sb, bool noScale)
            {
                if (visible)
                {
                    if (noScale)
                    {
                        sb.Draw(textures[texIndex], rect, sourceRect, color, rotation, origin, effects, layerDepth);
                    }
                    else
                    {
                        sb.Draw(textures[texIndex], Scaling.ResizeRectangleToScale(rect, size), sourceRect, color, rotation, origin, effects, layerDepth);
                    }
                }
            }
            public override TextSprite ToTextSprite()
            {
                throw new InvalidCastException();
            }
            public override ObjectSprite ToObjectSprite()
            {
                return (ObjectSprite)(this);
            }
            public override SpriteGroup.Layer ToLayer()
            {
                throw new InvalidCastException();
            }
            public override Button ToButton()
            {
                throw new InvalidCastException();
            }
        }
        /// <summary>
        /// A concrete implementation of the Sprite class. Used for font-based Sprites.
        /// </summary>
        public class TextSprite : Sprite
        {
            /// <summary>
            /// The font to use to display the Sprite.
            /// </summary>
            public SpriteFont font;
            public float scale;
            /// <summary>
            /// The text to display.
            /// </summary>
            public string text;
            /// <param name="font">The font to use to display the Sprite</param>
            /// <param name="text">The text to display</param>
            public TextSprite(SpriteFont font, string text) : base()
            {
                this.font = font;
                this.text = text;
                scale = OzzzFramework.scale.X;
                type = "TextSprite";
            }
            /// <param name="font">The font to use to display the Sprite</param>
            /// <param name="text">The text to display</param>
            /// <param name="scale">The scale to display the Sprite at, where 1.0 is 100% size</param>
            public TextSprite(SpriteFont font, string text, float scale) : base()
            {
                this.font = font;
                this.text = text;
                this.scale = scale;
                type = "TextSprite";
            }
            /// <param name="font">The font to use to display the Sprite</param>
            /// <param name="text">The text to display</param>
            /// <param name="scale">The scale to display the Sprite at, where 1.0 is 100% size</param>
            /// <param name="pos">The position for the Sprite.</param>
            /// <param name="color">The color of the Sprite's text.</param>
            public TextSprite(SpriteFont font, string text, float scale, Vector2 pos, Color color) : base()
            {
                this.font = font;
                this.scale = scale;
                this.text = text;
                this.pos = pos;
                this.color = color;
                type = "TextSprite";
            }

            private static string GetASCII(string str)
            {
                try
                {
                    return Encoding.ASCII.GetString(Encoding.Convert(Encoding.UTF8, Encoding.GetEncoding(Encoding.ASCII.EncodingName, new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback()), Encoding.UTF8.GetBytes(str)));
                }
                catch
                {
                    return "";
                }
            }
            public override Vector2 GetSize()
            {
                return font.MeasureString(GetASCII(text)) * new Vector2(scale, scale);
            }
            /// <summary>
            /// Gets the Width and Height of the Sprite if the text was the given text.
            /// </summary>
            /// <param name="text">The text to use to measure size.</param>
            /// <returns></returns>
            public Vector2 GetSize(string text)
            {
                return font.MeasureString(GetASCII(text)) * new Vector2(scale, scale);
            }
            public override Vector2 GetCenterPoint()
            {
                return new Vector2(pos.X + GenerateRect().Width / 2, pos.Y + GenerateRect().Height / 2);
            }
            /// <summary>
            /// Generates a rectangle for the TextSprite, as SpriteFonts have no built-in methods for getting a Rectangle object.
            /// </summary>
            /// <returns>A Rectangle representing the Position, Width, and Height of the TextSprite.</returns>
            public override Rectangle GenerateRect()
            {
                return new Rectangle((int)pos.X, (int)pos.Y, (int)GetSize().X, (int)GetSize().Y);
            }
            public override bool Intersects(Rectangle rect)
            {
                return GenerateRect().Intersects(rect);
            }
            public override bool Intersects(ObjectSprite sprite)
            {
                return sprite.Intersects(GenerateRect());
            }
            public override bool Intersects(TextSprite sprite)
            {
                return GenerateRect().Intersects(sprite.GenerateRect());
            }
            public override void Draw(SpriteBatch sb)
            {
                if (visible)
                {
                    sb.DrawString(font, GetASCII(text), Scaling.ScaleVector2(Scaling.ResizeVector2(pos, GetSize(), size)), color, rotation, origin, Scaling.ScaleFloatX(scale * size), effects, layerDepth);
                }
            }
            public override void Draw(SpriteBatch sb, bool noScale)
            {
                if (visible)
                {
                    if (noScale)
                    {
                        sb.DrawString(font, GetASCII(text), Scaling.ResizeVector2(pos, GetSize(), size), color, rotation, origin, scale * size, effects, layerDepth);
                    }
                    else
                    {
                        sb.DrawString(font, GetASCII(text), Scaling.ScaleVector2(Scaling.ResizeVector2(pos, GetSize(), size)), color, rotation, origin, Scaling.ScaleFloatX(scale * size), effects, layerDepth);
                    }
                }
            }
            public override TextSprite ToTextSprite()
            {
                return (TextSprite)(this);
            }
            public override ObjectSprite ToObjectSprite()
            {
                throw new InvalidCastException();
            }
            public override SpriteGroup.Layer ToLayer()
            {
                throw new InvalidCastException();
            }
            public override Button ToButton()
            {
                throw new InvalidCastException();
            }
        }
        /// <summary>
        /// Class for making a Sprite more interactive. Holds a Sprite, does not create one.
        /// </summary>
        public class InteractiveSprite
        {
            public Sprite sprite;
            /// <summary>
            /// The Colors to use for mouse hovers and clicks.
            /// </summary>
            public List<Color> colors;
            /// <summary>
            /// Stores if the mouse is currently hovering over the Sprite.
            /// </summary>
            public bool isHovered;
            /// <summary>
            /// Stores if the mouse is currently clicking the Sprite.
            /// </summary>
            public bool isClicked;
            /// <summary>
            /// Stores whether or not to change color/fade whenever the mouse clicks/hovers with the Sprite.
            /// </summary>
            public bool mouseFade;
            public InteractiveSprite(Sprite sprite)
            {
                this.sprite = sprite;
                mouseFade = false;
                colors = new List<Color>() { Color.White, Color.White, Color.White };
            }
            /// <param name="colors">The list of colors for the Sprite. Must have three Colors.</param>
            public InteractiveSprite(Sprite sprite, List<Color> colors)
            {
                this.sprite = sprite;
                this.colors = colors;
                mouseFade = true;
            }
            /// <summary>
            /// Updates the Sprite and it's interacterability components. Contains an embedded call to UpdatePos(). Automatically updates the color.
            /// </summary>
            public void Update()
            {
                if (sprite.type == "ObjectSprite")
                {
                    sprite.ToObjectSprite().UpdatePos();
                }
                else if (sprite.type == "TextSprite")
                {
                    sprite.ToTextSprite().UpdatePos();
                }
                else if (sprite.type == "Layer")
                {
                    sprite.ToLayer().Update();
                }
                else
                {
                    sprite.UpdatePos();
                }
                if (mouseFade)
                {
                    if (sprite.CheckMouse(false))
                    {
                        isHovered = true;
                        if (sprite.CheckMouse(true))
                        {
                            isClicked = true;
                            sprite.color = colors[2];
                        }
                        else
                        {
                            isClicked = false;
                            sprite.color = colors[1];
                        }
                    }
                    else
                    {
                        isHovered = false;
                        sprite.color = colors[0];
                    }
                }
            }
            /// <summary>
            /// Draws the Sprite to the given SpriteBatch.
            /// </summary>
            /// <param name="sb">The SpriteBatch to draw to.</param>
            public void Draw(SpriteBatch sb)
            {
                if (sprite.type == "ObjectSprite")
                {
                    sprite.ToObjectSprite().Draw(sb);
                }
                else if (sprite.type == "TextSprite")
                {
                    sprite.ToTextSprite().Draw(sb);
                }
                else if (sprite.type == "Layer")
                {
                    sprite.ToLayer().Draw(sb);
                }
                else
                {
                    sprite.Draw(sb);
                }
            }
        }
        /// <summary>
        /// Class for making a clickable Button object.
        /// </summary>
        public class Button : SpriteGroup.Layer
        {
            public InteractiveSprite button;
            /// <summary>
            /// Controls how much the button will visibly shrink when clicked.
            /// </summary>
            public float shrinkFactor;
            /// <summary>
            /// The GamePad Button(s) that will cause the Button object to shrink when pressed.
            /// </summary>
            public List<Buttons> downButtons;
            /// <summary>
            /// The player index to check for a downed button
            /// </summary>
            public PlayerIndex playerIndex;
            /// <summary>
            /// The Sprite to display on top of the Button. Will be automatically centerized with the Button.
            /// </summary>
            public Sprite topSprite;
            public enum GPButtonState
            {
                Off, Any, All, AnyFirst
            }
            /// <summary>
            /// How this Button should interact with the controller.
            /// </summary>
            public GPButtonState gpButtonState;
            public Button(Texture2D initialTexture, Rectangle rect) : base(initialTexture, rect)
            {
                sprites.Add(new ObjectSprite(initialTexture, rect));
                button = new InteractiveSprite(new ObjectSprite(initialTexture, rect));
                visible = false;
                shrinkFactor = 1.0f;
                type = "Button";
                downButtons = new List<Buttons>();
            }
            public Button(Texture2D initialTexture, Rectangle rect, Texture2D buttonTex, Rectangle clickRect) : base(initialTexture, rect)
            {
                sprites.Add(new ObjectSprite(initialTexture, rect));
                button = new InteractiveSprite(new ObjectSprite(buttonTex, clickRect));
                visible = false;
                shrinkFactor = 1.0f;
                type = "Button";
                downButtons = new List<Buttons>();
            }
            public Button(Texture2D initialTexture, Rectangle rect, Texture2D buttonTex, Rectangle clickRect, GPButtonState state, PlayerIndex index, List<Buttons> buttons) : base(initialTexture, rect)
            {
                sprites.Add(new ObjectSprite(initialTexture, rect));
                button = new InteractiveSprite(new ObjectSprite(buttonTex, clickRect));
                visible = false;
                shrinkFactor = 1.0f;
                type = "Button";
                gpButtonState = state;
                playerIndex = index;
                downButtons = buttons;
            }
            /// <summary>
            /// Returns whether or not the button is clicked.
            /// </summary>
            public bool GetValue()
            {
                return (button.sprite.CheckMouse(true) && MouseInput.IsLeftFirstDown()) || IsDownFromGP();
            }
            /// <summary>
            /// Adds a new outline to the Button.
            /// </summary>
            /// <param name="offset">How far from the border the outline will be.</param>
            public void AddNewOutline(int offset)
            {
                sprites.Add(new ObjectSprite(textures[0], new Rectangle(rect.X + offset, rect.Y + offset, rect.Width - offset * 2, rect.Height - offset * 2)));
            }
            public override Button ToButton()
            {
                return (Button)this;
            }
            private bool IsDownFromGP()
            {
                bool gp = false;
                if (gpButtonState == GPButtonState.All)
                {
                    gp = true;
                    foreach (Buttons b in downButtons)
                    {
                        if (!(GamepadInput.IsButtonDown(playerIndex, b)))
                        {
                            gp = false;
                        }
                    }
                }
                else if (gpButtonState == GPButtonState.Any)
                {
                    foreach (Buttons b in downButtons)
                    {
                        if (GamepadInput.IsButtonDown(playerIndex, b))
                        {
                            gp = true;
                        }
                    }
                }
                else if (gpButtonState == GPButtonState.AnyFirst)
                {
                    foreach (Buttons b in downButtons)
                    {
                        if (GamepadInput.IsButtonDown(playerIndex, b, true))
                        {
                            gp = true;
                        }
                    }
                }
                return gp;
            }
            private bool IsDownFromGP(GPButtonState tempState)
            {
                GPButtonState original = gpButtonState;
                gpButtonState = tempState;
                bool toReturn = IsDownFromGP();
                gpButtonState = original;
                return toReturn;
            }
            /// <summary>
            /// Updates the Button.
            /// </summary>
            public new void Update()
            {
                base.Update();
                button.Update();
                bool gp = IsDownFromGP();
                if (gpButtonState == GPButtonState.AnyFirst)
                {
                    gp = IsDownFromGP(GPButtonState.Any);
                }
                if (CheckMouse(true) || button.sprite.CheckMouse(true) || gp)
                {
                    if (button.sprite.size > shrinkFactor)
                    {
                        button.sprite.size -= ((1.0f - shrinkFactor) * 0.3f);
                        if (topSprite != null)
                        {
                            topSprite.size -= ((1.0f - shrinkFactor) * 0.3f);
                        }
                    }
                }
                else if (button.sprite.size < 1.0f && shrinkFactor != 1.0f)
                {
                    button.sprite.size += ((1.0f - shrinkFactor) * 0.3f);
                    if (topSprite != null)
                    {
                        topSprite.size += ((1.0f - shrinkFactor) * 0.3f);
                    }
                }
                if (topSprite != null)
                {
                    topSprite.Centerize(button.sprite.GetCenterPoint());
                }
            }
            public override void Draw(SpriteBatch sb)
            {
                base.Draw(sb);
                button.Draw(sb);
                if (topSprite != null)
                {
                    topSprite.Draw(sb);
                }
            }
        }
        /// <summary>
        /// Class for making a Counter object. Makes use of Buttons to allow the user to make an integer number.
        /// </summary>
        public class Counter : SpriteGroup.Layer
        {
            public Button upButton, downButton;
            public TextSprite numberSprite;
            public int min, max, increment, decrement, number;
            public Counter(Texture2D initialTexture, Rectangle rect, SpriteFont font, int buttonHeight, Texture2D buttonTex) : base(initialTexture, rect)
            {
                upButton = new Button(initialTexture, new Rectangle(rect.X, rect.Y, rect.Width, buttonHeight), buttonTex, new Rectangle(rect.X, rect.Y, rect.Width, buttonHeight));
                downButton = new Button(initialTexture, new Rectangle(rect.X, rect.Y + rect.Height - buttonHeight, rect.Width, buttonHeight), buttonTex, new Rectangle(rect.X, rect.Y + rect.Height - buttonHeight, rect.Width, buttonHeight));
                min = 1;
                max = 10;
                increment = 1;
                decrement = 1;
                numberSprite = new TextSprite(font, "" + max, 1f, new Vector2(rect.X + 2, rect.Y + 2 + buttonHeight), Color.White);
                while (numberSprite.GetSize().X >= rect.Width - 2 || numberSprite.GetSize().Y >= rect.Height - buttonHeight * 2)
                {
                    numberSprite.scale -= 0.01f;
                }
                numberSprite.text = "" + min;
                type = "Counter";
                sprites.Add(upButton);
                sprites.Add(downButton);
                sprites.Add(numberSprite);
            }
            public Counter(Texture2D initialTexture, Rectangle rect, SpriteFont font, int buttonHeight, Texture2D buttonTex, int min, int max, int increment, int decrement) : this(initialTexture, rect, font, buttonHeight, buttonTex)
            {
                this.min = min;
                this.max = max;
                this.increment = increment;
                this.decrement = decrement;
                numberSprite.scale = 1f;
                while (numberSprite.GetSize().X >= rect.Width - 2 || numberSprite.GetSize().Y >= rect.Height - buttonHeight * 2)
                {
                    numberSprite.scale -= 0.01f;
                }
            }
            public int GetValue()
            {
                return number;
            }
            public void SetButtonColor(Color color)
            {
                upButton.button.sprite.color = color;
                downButton.button.sprite.color = color;
            }
            public new void Update()
            {
                base.Update();
                if (upButton.GetValue())
                {
                    number += increment;
                }
                if (downButton.GetValue())
                {
                    number -= decrement;
                }
                if (number > max)
                {
                    number = max;
                }
                else if (number < min)
                {
                    number = min;
                }
                numberSprite.text = "" + number;
                numberSprite.Centerize(rect.Center.ToVector2());
            }
        }
        /// <summary>
        /// Class for making a Slider object. The user can drag the control in a specified direction to change the returned value.
        /// </summary>
        public class Slider : ObjectSprite
        {
            /// <summary>
            /// The possible directions for the Slider to move in
            /// </summary>
            public enum SlideDirection
            {
                HorizontalLeft, VerticalDown, HorizontalRight, VerticalUp
            }
            public SlideDirection direction;
            public InteractiveSprite slider;
            public bool clicked;
            /// <summary>
            /// The position of the mouse on it's most recent click on the Slider.
            /// </summary>
            public Vector2 mousePos;
            /// <param name="direction">The direction for the slider to go in.</param>
            /// <param name="sliderTexture">The initial texture for the Slider and its background.</param>
            /// <param name="sliderRect">The Rectangle to assign to the Slider.</param>
            /// <param name="length">The length that the slider can move.</param>
            /// <param name="thickness">The vertical thickness of the slider's background.</param>
            /// <param name="colors">The Colors to give to the Slider. Must have three Colors.</param>
            public Slider(SlideDirection direction, Texture2D sliderTexture, Rectangle sliderRect, int length, int thickness, List<Color> colors) : base(sliderTexture, new Rectangle(sliderRect.X, sliderRect.Y + sliderRect.Height / 2 - thickness, length + sliderRect.Width, thickness * 2), Color.Gray)
            {
                this.direction = direction;
                if (direction == SlideDirection.VerticalDown || direction == SlideDirection.VerticalUp)
                {
                    rect = new Rectangle(sliderRect.X + sliderRect.Width / 2 - thickness, sliderRect.Y, thickness * 2, length + sliderRect.Height);
                    UpdatePosFromRect();
                }
                slider = new InteractiveSprite(new ObjectSprite(sliderTexture, sliderRect), colors);
            }
            /// <summary>
            /// Returns the value of the Slider, from 0 to 1.
            /// </summary>
            public float GetValue()
            {
                if (direction == SlideDirection.HorizontalLeft)
                {
                    float num = rect.Width - slider.sprite.GenerateRect().Width - (slider.sprite.pos.X - pos.X);
                    return num / (rect.Width - slider.sprite.GenerateRect().Width);
                }
                else if (direction == SlideDirection.HorizontalRight)
                {
                    return (slider.sprite.pos.X - pos.X) / (rect.Width - slider.sprite.GenerateRect().Width);
                }
                else if (direction == SlideDirection.VerticalUp)
                {
                    float num = rect.Height - slider.sprite.GenerateRect().Height - (slider.sprite.pos.Y - pos.Y);
                    return num / (rect.Height - slider.sprite.GenerateRect().Height);
                }
                else
                {
                    return (slider.sprite.pos.Y - pos.Y) / (rect.Height - slider.sprite.GenerateRect().Height);
                }
            }
            /// <summary>
            /// Sets the value of the Slider.
            /// </summary>
            /// <param name="value">The value to set the slider to, from 0 to 1.</param>
            public void SetValue(float value)
            {
                if (value > 1)
                {
                    value = 1;
                }
                else if (value < 0)
                {
                    value = 0;
                }
                // TODO: Add the rest of the directions
                if (direction == SlideDirection.VerticalUp)
                {
                    slider.sprite.pos.Y = (rect.Y + rect.Height) - (rect.Height * value) - slider.sprite.GenerateRect().Height / 2;
                }
            }
            /// <summary>
            /// Updates the Slider.
            /// </summary>
            public void Update()
            {
                UpdatePos();
                clicked = false;
                if (MouseInput.IsLeftFirstDown())
                {
                    Rectangle mouseRect = MouseInput.GetMouseRect();
                    Rectangle temp = slider.sprite.GenerateRect();
                    temp = Scaling.ScaleRectangle(temp);
                    if (temp.Intersects(mouseRect))
                    {
                        mousePos = new Vector2(mouseRect.X, mouseRect.Y);
                        clicked = true;
                    }
                    else
                    {
                        mousePos = new Vector2(-1, -1);
                    }
                }
                else if (MouseInput.IsLeftDown() && slider.sprite.type != "Layer")
                {
                    Rectangle unscaledMouse = MouseInput.GetMouseRect();
                    if (mousePos.X != -1)
                    {
                        if (direction == SlideDirection.HorizontalLeft || direction == SlideDirection.HorizontalRight)
                        {
                            slider.sprite.pos.X = (float)(unscaledMouse.X) * (1 / scale.X);
                            if (slider.sprite.pos.X < pos.X)
                            {
                                slider.sprite.pos.X = pos.X;
                            }
                            else if (slider.sprite.pos.X > pos.X + rect.Width - slider.sprite.GenerateRect().Width)
                            {
                                slider.sprite.pos.X = pos.X + rect.Width - slider.sprite.GenerateRect().Width;
                            }
                        }
                        else
                        {
                            slider.sprite.pos.Y = (float)(unscaledMouse.Y) * (1 / scale.Y);
                            if (slider.sprite.pos.Y < pos.Y)
                            {
                                slider.sprite.pos.Y = pos.Y;
                            }
                            else if (slider.sprite.pos.Y > pos.Y + rect.Height - slider.sprite.GenerateRect().Height)
                            {
                                slider.sprite.pos.Y = pos.Y + rect.Height - slider.sprite.GenerateRect().Height;
                            }
                        }
                        clicked = true;
                    }
                }
                else if (MouseInput.IsLeftDown())
                {
                    SpriteGroup.Layer layer = slider.sprite.ToLayer();
                    Rectangle unscaledMouse = MouseInput.GetMouseRect();
                    if (mousePos.X != -1)
                    {
                        if (direction == SlideDirection.HorizontalLeft || direction == SlideDirection.HorizontalRight)
                        {
                            layer.MoveTo(new Vector2((float)(unscaledMouse.X) * (1 / scale.X), layer.pos.Y));
                            if (layer.pos.X < pos.X)
                            {
                                layer.MoveTo(new Vector2(pos.X, layer.pos.Y));
                            }
                            else if (layer.pos.X > pos.X + rect.Width - layer.GenerateRect().Width)
                            {
                                layer.MoveTo(new Vector2(pos.X + rect.Width - layer.GenerateRect().Width, layer.pos.Y));
                            }
                        }
                        else
                        {
                            layer.MoveTo(new Vector2(layer.pos.X, (float)(unscaledMouse.Y) * (1 / scale.Y)));
                            if (layer.pos.Y < pos.Y)
                            {
                                layer.MoveTo(new Vector2(layer.pos.X, pos.Y));
                            }
                            else if (layer.pos.Y > pos.Y + rect.Height - layer.GenerateRect().Height)
                            {
                                layer.MoveTo(new Vector2(layer.pos.X, pos.Y + rect.Height - layer.GenerateRect().Height));
                            }
                        }
                        clicked = true;
                    }
                }
                else
                {
                    mousePos = new Vector2(-1, -1);
                }
                slider.Update();
                if (clicked)
                {
                    slider.sprite.color = slider.colors[2];
                }
            }
            public new void Draw(SpriteBatch sb)
            {
                base.Draw(sb);
                slider.Draw(sb);
            }
        }
        /// <summary>
        /// Class for creating an object similar to a tooltip. Is 'bound' to a Sprite by reference
        /// </summary>
        public class DescriptionBox : SpriteGroup.Layer
        {
            /// <summary>
            /// The reference to the Sprite for hovering over.
            /// </summary>
            public Sprite hostSprite;
            /// <summary>
            /// The TextSprite responsible for holding the Description Box's text.
            /// </summary>
            public TextSprite textSprite;
            /// <summary>
            /// The text to display in the Description Box.
            /// </summary>
            public string text;
            /// <summary>
            /// How many frames to wait before showing the box.
            /// </summary>
            public float hoverDelay;
            /// <summary>
            /// Tracks how many frames the host Sprite has been hovered over for.
            /// </summary>
            public float delayTemp;
            /// <summary>
            /// The offset to display the text at in the Description Box.
            /// </summary>
            public int offset;
            /// <summary>
            /// How much to increase the transparency by every frame the host Sprite is hovered over.
            /// </summary>
            public byte transparencyIncrease;
            /// <summary>
            /// The variable controlling transparency of the box.
            /// </summary>
            public byte transparency;
            /// <summary>
            /// Whether or not the Description Box can be showed.
            /// </summary>
            public bool enabled;
            /// <summary>
            /// Where the Description Box will appear.
            /// </summary>
            public enum SpawnPositions
            {
                BelowRight, AboveRight, BelowLeft, AboveLeft
            }
            /// <summary>
            /// Where the Description Box will appear.
            /// </summary>
            public SpawnPositions spawnPos;
            /// <param name="sprite">The host Sprite to bind the Description Box to.</param>
            /// <param name="initialTexture">The initial texture for the box.</param>
            /// <param name="size">The size the box.</param>
            /// <param name="color">The color for the box.</param>
            /// <param name="hoverDelay">How many frames to wait before showing the box.</param>
            /// <param name="transparencyIncrease">How much to increase the transparency of the Description Box every frame the mouse hovers over the host.</param>
            public DescriptionBox(Sprite sprite, SpriteFont font, Texture2D initialTexture, Vector2 size, Color color, float hoverDelay, byte transparencyIncrease) : base(initialTexture, new Rectangle(0, 0, (int)size.X, (int)size.Y), color)
            {
                hostSprite = sprite;
                this.hoverDelay = hoverDelay;
                this.transparencyIncrease = transparencyIncrease;
                transparency = 0;
                offset = 10;
                spawnPos = SpawnPositions.BelowRight;
                textSprite = new TextSprite(font, "");
                enabled = true;
            }
            /// <summary>
            /// Updates the box, including displaying the box if the mouse has hovered over the box for long enough.
            /// </summary>
            public new void Update()
            {
                //base.Update();
                if (spawnPos == SpawnPositions.BelowRight)
                {
                    rect.X = MouseInput.state.X;
                    rect.Y = MouseInput.state.Y;
                }
                else if (spawnPos == SpawnPositions.AboveRight)
                {
                    rect.X = MouseInput.state.X;
                    rect.Y = MouseInput.state.Y - rect.Height;
                }
                else if (spawnPos == SpawnPositions.BelowLeft)
                {
                    rect.X = MouseInput.state.X - rect.Width;
                    rect.Y = MouseInput.state.Y;
                }
                else
                {
                    rect.X = MouseInput.state.X - rect.Width;
                    rect.Y = MouseInput.state.Y - rect.Height;
                }
                foreach (Sprite sprite in sprites)
                {
                    sprite.pos.X = rect.X + ((rect.Width - sprite.GenerateRect().Width) / 2);
                    sprite.pos.Y = rect.Y + ((rect.Height - sprite.GenerateRect().Height) / 2);
                    if (sprite.type == "ObjectSprite")
                    {
                        sprite.ToObjectSprite().UpdatePos();
                    }
                    else
                    {
                        sprite.UpdatePos();
                    }
                }
                textSprite.pos.X = rect.X + offset;
                textSprite.pos.Y = rect.Y + offset;
                if (hostSprite.CheckMouse(false))
                {
                    delayTemp++;
                    if (delayTemp >= hoverDelay && transparency + transparencyIncrease <= 255)
                    {
                        transparency += transparencyIncrease;
                    }
                    else if (delayTemp >= hoverDelay && transparency < 255)
                    {
                        transparency = 255;
                    }
                }
                else
                {
                    delayTemp = 0;
                    transparency = 0;
                }
                color.A = transparency;
                foreach (Sprite sprite in sprites)
                {
                    sprite.color.A = transparency;
                    if (transparency == 0)
                    {
                        sprite.visible = false;
                    }
                    else
                    {
                        sprite.visible = true;
                    }
                }
                if (transparency == 0)
                {
                    textSprite.visible = false;
                }
                else
                {
                    textSprite.visible = true;
                }
            }
            /// <summary>
            /// Adds a new outline to the Description Box
            /// </summary>
            /// <param name="offset">How far from the border the outline will be.</param>
            public void AddNewOutline(int offset)
            {
                sprites.Add(new ObjectSprite(textures[0], new Rectangle(rect.X + offset, rect.Y - offset, rect.Width - offset * 2, rect.Height - offset * 2)));
            }
            /// <summary>
            /// Draws the Description Box to the screen
            /// </summary>
            /// <param name="sb"></param>
            public new void Draw(SpriteBatch sb)
            {
                if (enabled)
                {
                    hostSprite.Draw(sb);
                    if (transparency == 0)
                    {
                        visible = false;
                    }
                    else
                    {
                        visible = true;
                    }
                    base.Draw(sb, true);
                    textSprite.Draw(sb, true);
                }
            }
        }
        /// <summary>
        /// Class for transitioning from one Layer of Sprites to another. Useful in menus
        /// </summary>
        public class SequenceFade
        {
            /// <summary>
            /// Holds the Layers for transitioning.
            /// </summary>
            public List<SpriteGroup.Layer> layers;
            /// <summary>
            /// The Gradient for displaying the current Layer.
            /// </summary>
            public Gradient displayGradient;
            /// <summary>
            /// The next index to transition to.
            /// </summary>
            public int nextIndex;
            /// <summary>
            /// The number of frames to wait before transitioning to a new Layer.
            /// </summary>
            public int frames;
            /// <summary>
            /// Keeps track of how long until a transition.
            /// </summary>
            public int currentFrames;
            /// <summary>
            /// The Layer currently being displayed.
            /// </summary>
            public int currentIndex;
            /// <summary>
            /// Whether or transitioning is in progress.
            /// </summary>
            public bool inTrans;
            /// <param name="initialLayer">The first Layer for the SequenceFade.</param>
            /// <param name="startingColor">The Color that the SequenceFade starts with.</param>
            /// <param name="color">The color that the SequenceLayer will display.</param>
            /// <param name="displayFrames">The number of frames to wait before transitioning to a new Layer.</param>
            /// <param name="switchFrames">The number of frames for the transition between Layers to take.</param>
            public SequenceFade(SpriteGroup.Layer initialLayer, Color startingColor, Color color, int displayFrames, int switchFrames)
            {
                layers = new List<SpriteGroup.Layer>();
                layers.Add(initialLayer);
                displayGradient = new Gradient(startingColor, switchFrames / 2);
                displayGradient.colors.Add(color);
                displayGradient.ValueUpdate(1);
                frames = displayFrames;
                currentFrames = frames;
                inTrans = false;
                currentIndex = -1;
            }
            /// <summary>
            /// Adds a Layer to the SequenceFade.
            /// </summary>
            /// <param name="layer">The Layer to add.</param>
            /// <returns>The added Layer</returns>
            public SpriteGroup.Layer AddLayer(SpriteGroup.Layer layer)
            {
                layers.Add(layer);
                nextIndex = GetNextIndex();
                return layer;
            }
            /// <summary>
            /// Resets the frame timer on the current Layer.
            /// </summary>
            public void Reset()
            {
                currentFrames = frames;
                displayGradient.ValueUpdate(1);
                Update();
                inTrans = false;
            }
            /// <summary>
            /// Switches to a specified Layer.
            /// </summary>
            /// <param name="index">The index of the Layer to switch to.</param>
            public void Switch(int index)
            {
                if (index == currentIndex)
                {
                    Reset();
                }
                else
                {
                    nextIndex = index;
                    displayGradient.ValueUpdate(0);
                    currentFrames = 0;
                }
            }
            public void ForceSwitch(int index)
            {
                if (index == currentIndex)
                {
                    Reset();
                }
                else
                {
                    nextIndex = index;
                    displayGradient.ValueUpdate(1);
                    if (currentIndex != -1)
                    {
                        foreach (Sprite sprite in layers[currentIndex].sprites)
                        {
                            sprite.visible = false;
                        }
                    }
                    currentIndex = nextIndex;
                    nextIndex = GetNextIndex();
                    foreach (Sprite sprite in layers[currentIndex].sprites)
                    {
                        sprite.visible = true;
                    }
                    currentFrames = frames;
                }
            }
            private int GetNextIndex()
            {
                if (currentIndex + 1 >= layers.Count)
                {
                    return 0;
                }
                return currentIndex + 1;
            }
            public void Update()
            {
                if (inTrans)
                {
                    displayGradient.Update();
                    if (displayGradient.GetColor() == displayGradient.colors[1])
                    {
                        inTrans = false;
                        currentFrames = frames;
                    }
                    else if (displayGradient.GetColor() == displayGradient.colors[0])
                    {
                        if (currentIndex != -1)
                        {
                            foreach (Sprite sprite in layers[currentIndex].sprites)
                            {
                                sprite.visible = false;
                            }
                        }
                        currentIndex = nextIndex;
                        nextIndex = GetNextIndex();
                        foreach (Sprite sprite in layers[currentIndex].sprites)
                        {
                            sprite.visible = true;
                        }
                    }
                }
                else
                {
                    currentFrames--;
                    if (currentFrames <= 0)
                    {
                        inTrans = true;
                    }
                }
                foreach (SpriteGroup.Layer layer in layers)
                {
                    foreach (Sprite sprite in layer.sprites)
                    {
                        sprite.color = displayGradient.GetColor();
                    }
                }
            }
            public void Draw(SpriteBatch sb)
            {
                if (currentIndex != -1)
                {
                    layers[currentIndex].Draw(sb);
                }
            }
        }
        /// <summary>
        /// Basic class for grouping together Layer objects
        /// </summary>
        public class SpriteGroup
        {
            /// <summary>
            /// Class for organizing and partially automating Sprites, making much more complex interactions involving multiple Sprites easier. 
            /// Inherits the ObjectSprite class, making the Layer itself the background Sprite. 
            /// The Layer's background Sprite will automatically resize itself to enclose all other Sprites the Layer holds.
            /// </summary>
            public class Layer : ObjectSprite
            {
                /// <summary>
                /// Holds the Sprites part of the Layer. Polymorphic by design.
                /// To keep unique names in code for Sprites, simply pass in the reference for a Sprite to the List. Because of how Objects work, both should reference the same Sprite.
                /// </summary>
                public List<Sprite> sprites;
                /// <summary>
                /// The ObjectSprite that will be used as a template when AddObject() is called.
                /// </summary>
                public ObjectSprite objTemplate;
                /// <summary>
                /// The TextSprite that will be used as a template when AddText() is called.
                /// </summary>
                public TextSprite textTemplate;
                /// <summary>
                /// The offsets that each new Sprite of the corresponding type will be at.
                /// </summary>
                public Vector2 objOffsets, textOffsets;
                /// <summary>
                /// Keeps track of the number of offsets that have been used, so that the next offset is in the right place.
                /// </summary>
                public int offsetCount;
                /// <param name="initialTexture">The first texture to add to the Sprite's List of textures.</param>
                /// <param name="rect">The destination rectangle.</param>
                public Layer(Texture2D initialTexture, Rectangle rect) : base(initialTexture, rect)
                {
                    sprites = new List<Sprite>();
                    objTemplate = null;
                    textTemplate = null;
                    objOffsets = new Vector2();
                    textOffsets = new Vector2();
                    type = "Layer";
                    visible = false;
                }
                /// <param name="initialTexture">The first texture to add to the Sprite's List of textures.</param>
                /// <param name="rect">The destination rectangle.</param>
                /// <param name="color">The Color for drawing the Sprite.</param>
                public Layer(Texture2D initialTexture, Rectangle rect, Color color) : base(initialTexture, rect, color)
                {
                    sprites = new List<Sprite>();
                    objTemplate = null;
                    textTemplate = null;
                    objOffsets = new Vector2();
                    textOffsets = new Vector2();
                    type = "Layer";
                    visible = false;
                }
                /// <param name="initialTexture">The first texture to add to the Sprite's List of textures.</param>
                /// <param name="rect">The destination rectangle.</param>
                /// <param name="color">The Color for drawing the Sprite.</param>
                /// <param name="objTemplate">The ObjectSprite to use as the template.</param>
                /// <param name="textTemplate">The TextSprite to use as the template.</param>
                /// <param name="objOffsets">The offsets for the ObjectSprite template.</param>
                /// <param name="textOffsets">The offsets for the TextSprite template.</param>
                public Layer(Texture2D initialTexture, Rectangle rect, Color color, ObjectSprite objTemplate, TextSprite textTemplate, Vector2 objOffsets, Vector2 textOffsets) : base(initialTexture, rect, color)
                {
                    sprites = new List<Sprite>();
                    this.objTemplate = objTemplate;
                    this.textTemplate = textTemplate;
                    this.objOffsets = objOffsets;
                    this.textOffsets = textOffsets;
                    type = "Layer";
                    visible = false;
                }
                /// <summary>
                /// Returns a reference to the Sprite highest in the Layer that was clicked, if clicked.
                /// </summary>
                /// <param name="click">If true, also checks if the Left Mouse Button is down.</param>
                /// <returns></returns>
                public Sprite CheckMouseLayered(bool click)
                {
                    Sprite sprite = null;
                    if (CheckMouse(click))
                    {
                        sprite = this;
                        foreach (Sprite s in sprites)
                        {
                            if (s.visible && s.CheckMouse(click))
                            {
                                sprite = s;
                            }
                        }
                    }
                    return sprite;
                }
                /// <summary>
                /// Updates the Layer's size so that it visually contains all of it's Sprites.
                /// </summary>
                public void UpdateSize()
                {
                    if (sprites.Count > 0)
                    {
                        pos = sprites[0].pos;
                        rect.Width = 0;
                        rect.Height = 0;
                        foreach (Sprite sprite in sprites)
                        {
                            if (!sprite.skipLayerDraw)
                            {
                                if (sprite.pos.X < pos.X)
                                {
                                    pos.X = sprite.pos.X;
                                }
                                else if (sprite.pos.X + sprite.GetSize().X > rect.X + rect.Width)
                                {
                                    rect.Width += (int)(sprite.pos.X + sprite.GetSize().X - (rect.X + rect.Width));
                                }
                                if (sprite.pos.Y < pos.Y)
                                {
                                    pos.Y = sprite.pos.Y;
                                }
                                else if (sprite.pos.Y + sprite.GetSize().Y > rect.Y + rect.Height)
                                {
                                    rect.Height += (int)(sprite.pos.Y + sprite.GetSize().Y - (rect.Y + rect.Height));
                                }
                            }
                        }
                        rect.X = (int)pos.X;
                        rect.Y = (int)pos.Y;
                    }
                }
                /// <summary>
                /// Adds the given Sprite to the Layer. Includes an embedded call to UpdateSize().
                /// </summary>
                /// <param name="sprite">The Sprite to add.</param>
                public void Add(Sprite sprite)
                {
                    sprites.Add(sprite);
                    UpdateSize();
                }
                /// <summary>
                /// Adds an ObjectSprite based on the template.
                /// </summary>
                /// <returns>A reference to the created ObjectSprite</returns>
                public ObjectSprite AddObject()
                {
                    return AddObject(objTemplate.textures[0]);
                }
                /// <summary>
                /// Adds an ObjectSprite based on the template with an initial texture.
                /// </summary>
                /// <param name="texture">The initial texture that will be applied to the created ObjectSprite.</param>
                /// <returns>A reference to the created ObjectSprite</returns>
                public ObjectSprite AddObject(Texture2D texture)
                {
                    ObjectSprite obj = new ObjectSprite(texture, objTemplate.rect);
                    for (int i = 1; i < objTemplate.textures.Count; i++)
                    {
                        obj.textures.Add(objTemplate.textures[i]);
                    }
                    obj.color = objTemplate.color;
                    obj.effects = objTemplate.effects;
                    obj.visible = objTemplate.visible;
                    obj.velocity = objTemplate.velocity;
                    obj.texIndex = objTemplate.texIndex;
                    obj.tags = objTemplate.tags;
                    obj.layerDepth = objTemplate.layerDepth;
                    obj.origin = objTemplate.origin;
                    obj.pos = objTemplate.pos;
                    obj.pos += objOffsets * new Vector2(offsetCount, offsetCount);
                    obj.rotation = objTemplate.rotation;
                    obj.sourceRect = objTemplate.sourceRect;
                    obj.UpdatePos();
                    sprites.Add(obj);
                    offsetCount++;
                    return obj;
                }
                /// <summary>
                /// Adds a TextSprite based on the template.
                /// </summary>
                /// <returns>A reference to the created TextSprite</returns>
                public TextSprite AddText()
                {
                    return AddText(textTemplate.text);
                }
                /// <summary>
                /// Adds a TextSprite based on the template with new text.
                /// </summary>
                /// <param name="text">The text that will be assigned to the new TextSprite.</param>
                /// <returns>A reference to the created TextSprite</returns>
                public TextSprite AddText(string text)
                {
                    TextSprite obj = new TextSprite(textTemplate.font, textTemplate.text);
                    obj.color = textTemplate.color;
                    obj.effects = textTemplate.effects;
                    obj.visible = textTemplate.visible;
                    obj.velocity = textTemplate.velocity;
                    obj.tags = textTemplate.tags;
                    obj.layerDepth = textTemplate.layerDepth;
                    obj.origin = textTemplate.origin;
                    obj.pos = textTemplate.pos;
                    obj.pos += textOffsets * new Vector2(offsetCount, offsetCount);
                    obj.rotation = textTemplate.rotation;
                    obj.scale = textTemplate.scale;
                    obj.text = text;
                    sprites.Add(obj);
                    offsetCount++;
                    return obj;
                }
                /// <summary>
                /// Removes the Sprite at the specified index. Includes an embedded call to UpdateSize().
                /// </summary>
                /// <exception cref="ArgumentOutOfRangeException"></exception>
                /// <param name="index">The index of the Sprite to remove.</param>
                public void RemoveAt(int index)
                {
                    sprites.RemoveAt(index);
                    UpdateSize();
                }
                /// <summary>
                /// Returns the number of Sprites in the Layer.
                /// </summary>
                public int Size()
                {
                    return sprites.Count;
                }
                /// <summary>
                /// Moves all Sprites in the Layer.
                /// </summary>
                /// <param name="offset">The amount to move each Sprite.</param>
                public void Move(Vector2 offset)
                {
                    pos += Scaling.ScaleVector2(offset);
                    for (int i = 0; i < sprites.Count; i++)
                    {
                        sprites[i].pos += offset;
                    }
                }
                /// <summary>
                /// Moves all Sprites in the Layer to the specified position.
                /// </summary>
                /// <param name="pos">The position to move all Sprites to.</param>
                public void MoveTo(Vector2 pos)
                {
                    Vector2 offset = pos - this.pos;
                    this.pos = pos;
                    for (int i = 0; i < sprites.Count; i++)
                    {
                        sprites[i].pos += offset;
                    }
                }
                public override Layer ToLayer()
                {
                    return (Layer)this;
                }
                /// <summary>
                /// Sets the visibility of every Sprite in the Layer.
                /// </summary>
                /// <param name="visible">The desired visibility to apply to all Sprites.</param>
                public void SetVisibility(bool visible)
                {
                    foreach (Sprite sprite in sprites)
                    {
                        sprite.visible = visible;
                    }
                }
                /// <summary>
                /// Updates the Layer. Calls UpdatePos() for all Sprites in the Layer, and calls UpdateSize() as well.
                /// </summary>
                public void Update()
                {
                    Update(true);
                }
                /// <summary>
                /// Updates the Layer. Also calls UpdateSize().
                /// </summary>
                /// <param name="updateSprites">Whether or not Sprites in the Layer will also be updated with this call.</param>
                public void Update(bool updateSprites)
                {
                    UpdateSize();
                    if (updateSprites)
                    {
                        for (int i = 0; i < sprites.Count; i++)
                        {
                            if (sprites[i].type == "ObjectSprite")
                            {
                                sprites[i].ToObjectSprite().UpdatePos();
                            }
                            else if (sprites[i].type == "TextSprite")
                            {
                                sprites[i].ToTextSprite().UpdatePos();
                            }
                            else if (sprites[i].type == "Layer")
                            {
                                sprites[i].ToLayer().Update();
                            }
                            else if (sprites[i].type == "Button")
                            {
                                sprites[i].ToButton().Update();
                            }
                            else
                            {
                                sprites[i].UpdatePos();
                            }
                        }
                    }
                }
                /// <summary>
                /// Draws the Layer and all Sprites in it to the given SpriteBatch.
                /// </summary>
                /// <param name="sb">The SpriteBatch to draw to.</param>
                public override void Draw(SpriteBatch sb)
                {
                    base.Draw(sb);
                    foreach (Sprite sprite in sprites)
                    {
                        if (!sprite.skipLayerDraw)
                        {
                            if (sprite.type == "ObjectSprite")
                            {
                                sprite.ToObjectSprite().Draw(sb);
                            }
                            else if (sprite.type == "TextSprite")
                            {
                                sprite.ToTextSprite().Draw(sb);
                            }
                            else
                            {
                                sprite.Draw(sb);
                            }
                        }
                    }
                }
                /// <summary>
                /// Draws the Layer and all Sprites in it to the given SpriteBatch.
                /// </summary>
                /// <param name="sb">The SpriteBatch to draw to.</param>
                /// <param name="noScale">Forces a draw without scaling.</param>
                public override void Draw(SpriteBatch sb, bool noScale)
                {
                    base.Draw(sb, noScale);
                    foreach (Sprite sprite in sprites)
                    {
                        if (!sprite.skipLayerDraw)
                        {
                            if (sprite.type == "ObjectSprite")
                            {
                                sprite.ToObjectSprite().Draw(sb, noScale);
                            }
                            else if (sprite.type == "TextSprite")
                            {
                                sprite.ToTextSprite().Draw(sb, noScale);
                            }
                            else
                            {
                                sprite.Draw(sb, noScale);
                            }
                        }
                    }
                }
            }
            /// <summary>
            /// Contains the Group's Layers.
            /// </summary>
            public List<Layer> layers;
            /// <summary>
            /// Determins if the SpriteGroup is visible or not.
            /// </summary>
            public bool visible;
            public SpriteGroup()
            {
                layers = new List<Layer>();
            }
            /// <param name="layers">The Layers that the SpriteGroup will contain.</param>
            public SpriteGroup(List<Layer> layers)
            {
                this.layers = layers;
            }
            /// <summary>
            /// Moves all the Sprites in every Layer.
            /// </summary>
            /// <param name="offset">The amount to move each Sprite by.</param>
            public void Move(Vector2 offset)
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    layers[i].Move(offset);
                }
            }
            /// <summary>
            /// Updates all the Layers, and all the Sprites in them as a result.
            /// </summary>
            public void Update()
            {
                for (int i = 0; i < layers.Count; i++)
                {
                    layers[i].Update();
                }
            }
            /// <summary>
            /// Draws all the Layers in the SpriteGroup to the given SpriteBatch.
            /// </summary>
            /// <param name="sb">The SpriteBatch to draw to.</param>
            public void Draw(SpriteBatch sb)
            {
                if (visible)
                {
                    foreach (Layer layer in layers)
                    {
                        layer.Draw(sb);
                    }
                }
            }
        }
        /// <summary>
        /// Class for creating an animation with a Sprite. Has extra built-in support for ObjectSprite and TextSprite classes. 
        /// Note that animations are not locked in, meaning that other forces acting on the Sprite will still alter its course.
        /// </summary>
        public class AnimationPath
        {
            /// <summary>
            /// The Sprite to animate.
            /// </summary>
            public Sprite sprite;
            /// <summary>
            /// How much to move the Sprite every frame.
            /// </summary>
            public Vector2 offset;
            /// <summary>
            /// How much to resize the Sprite every frame.
            /// </summary>
            public float sizeOffset;
            /// <summary>
            /// The number of frames to continue playing the animation.
            /// </summary>
            public int frames;
            /// <summary>
            /// Whether or not the animation is paused.
            /// </summary>
            public bool paused;
            /// <param name="sprite">The Sprite to animate.</param>
            /// <param name="newPos">How much to move the Sprite every frame.</param>
            /// <param name="newSize">How much to resize the Sprite every frame.</param>
            /// <param name="frames">The number of frames to continue playing the animation.</param>
            public AnimationPath(Sprite sprite, Vector2 newPos, float newSize, int frames)
            {
                this.sprite = sprite;
                offset = new Vector2((newPos.X - sprite.pos.X) / frames, (newPos.Y - sprite.pos.Y) / frames);
                sizeOffset = (newSize - sprite.size) / frames;
                this.frames = frames;
            }
            /// <summary>
            /// Reverse the animation.
            /// </summary>
            public void ReverseAnimation()
            {
                offset *= new Vector2(-1, -1);
                sizeOffset *= -1;
            }
            /// <summary>
            /// Changes the ending position of the animation.
            /// </summary>
            /// <param name="newPos">The new position to change to.</param>
            public void ChangeEndPos(Vector2 newPos)
            {
                offset = new Vector2((newPos.X - sprite.pos.X) / frames, (newPos.Y - sprite.pos.Y) / frames);
            }
            /// <summary>
            /// Updates the AnimationPath, animating the attached Sprite if frames are left in the animation.
            /// </summary>
            /// <returns>The number of frames remaining in the animation</returns>
            public int Update()
            {
                if (!paused && frames > 0)
                {
                    if (sprite.type == "Layer")
                    {
                        sprite.ToLayer().Move(offset);
                    }
                    else
                    {
                        sprite.pos += offset;
                        sprite.size += sizeOffset;
                    }
                    frames--;
                }
                return frames;
            }
        }
        /// <summary>
        /// [INCOMPLETE] Class for interacting with a series of Button objects through the Gamepad. Selection is done with the D-Pad and a user-defined button.
        /// </summary>
        public class GamepadMenu
        {
            /// <summary>
            /// Holds the Buttons containing each of the menu options.
            /// </summary>
            public Button[] buttons;
            /// <summary>
            /// The texture to display when an option is being hovered over.
            /// </summary>
            public Texture2D selectTexture;
            /// <summary>
            /// The player index that controls the menu.
            /// </summary>
            public PlayerIndex index;
            /// <summary>
            /// The controller button that will select menu options.
            /// </summary>
            public Buttons selectButton;
            /// <param name="playerIndex">The player index that controls the menu.</param>
            /// <param name="selectButton">The controller button that will select menu options.</param>
            /// <param name="menuOptions">Holds the Buttons containing each of the menu options.</param>
            /// <param name="selectTexture">The texture to display when an option is being hovered over.</param>
            public GamepadMenu(PlayerIndex playerIndex, Buttons selectButton, Button[] menuOptions, Texture2D selectTexture)
            {
                index = playerIndex;
                this.selectButton = selectButton;
                buttons = menuOptions;
                this.selectTexture = selectTexture;
            }

        }
        /// <summary>
        /// Interface for creating a CommandProcessor subclass, for processing commands sent from an implementation of the GameConsole.
        /// </summary>
        public interface CommandProcessor
        {
            /// <summary>
            /// Processes the given command.
            /// </summary>
            /// <returns>If the command was executed without error.</returns>
            public bool Process(string command);
        }
        /// <summary>
        /// Abstract class for creating a CommandInterpreter subclass, for processing command fragments still being typed in. 
        /// Very similar to Visual Studio's IntelliSense.
        /// </summary>
        public abstract class CommandInterpreter
        {
            /// <summary>
            /// Holds the commands that can be previewed.
            /// </summary>
            public Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, bool>>>>>>> library;
            /// <summary>
            /// Returns a list of all possible commands from the fragment given.
            /// </summary>
            /// <param name="command"></param>
            public List<string> GetPreviews(string command, string paramChar)
            {
                List<string> previews = new List<string>();
                string[] c = command.Split(" ");
                bool good = true;
                try
                {
                    if (c.Length == 1)
                    {
                        previews = library.Keys.ToList();
                    }
                    else if (c.Length == 2)
                    {
                        previews = library[c[0]].Keys.ToList();
                    }
                    else if (c.Length == 3)
                    {
                        previews = library[c[0]][c[1]].Keys.ToList();
                    }
                    else if (c.Length == 4)
                    {
                        previews = library[c[0]][c[1]][c[2]].Keys.ToList();
                    }
                    else if (c.Length == 5)
                    {
                        previews = library[c[0]][c[1]][c[2]][c[3]].Keys.ToList();
                    }
                    else if (c.Length == 6)
                    {
                        previews = library[c[0]][c[1]][c[2]][c[3]][c[4]].Keys.ToList();
                    }
                    else if (c.Length == 7)
                    {
                        previews = library[c[0]][c[1]][c[2]][c[3]][c[4]][c[5]].Keys.ToList();
                    }
                }
                catch
                {
                    good = false;
                }

                if (good)
                {
                    for (int i = 0; i < previews.Count; i++)
                    {
                        if (previews[i].Length < c[c.Length - 1].Length || (previews[i] != c[c.Length - 1] && previews[i].Substring(0, c[c.Length - 1].Length).ToLower() != c[c.Length - 1].ToLower()))
                        {
                            if (previews[i].IndexOf(paramChar) == -1)
                            {
                                previews.RemoveAt(i);
                                i--;
                            }
                        }
                    }
                }

                previews.Sort();
                return previews;
            }
        }
        /// <summary>
        /// Abstract class for creating a text-based Console in the program. Text scale must be manually set (Default 1.0f). 
        /// Adds all letter keys, number keys, Right Control, Space, F1, F2, and Left Shift to KeyboardInput.
        /// </summary>
        public abstract class GameConsole : ObjectSprite
        {
            /// <summary>
            /// The TextSprite containing the text in the CLI.
            /// </summary>
            public TextSprite text;
            /// <summary>
            /// The CommandProcessor to send completed commands to for processing.
            /// </summary>
            public CommandProcessor commandProcessor;
            /// <summary>
            /// The CommandInterpreter to use when previewing commands.
            /// </summary>
            public CommandInterpreter interpreter;
            /// <summary>
            /// Layer that holds all the console's previous output.
            /// </summary>
            public SpriteGroup.Layer pastText;
            /// <summary>
            /// Layer that holds the preview text options.
            /// </summary>
            public SpriteGroup.Layer previewText;
            /// <summary>
            /// The keys to check to type while the CLI is open.
            /// </summary>
            public List<string> keys;
            /// <summary>
            /// Keeps track of all previous commands.
            /// </summary>
            public Stack<string> pastCommands;
            /// <summary>
            /// Keeps track of the commands that have been cycled past when going through previous commands.
            /// </summary>
            public Stack<string> downCommands;
            /// <summary>
            /// The key to close the Console. Must already be registered in KeyboardInput.
            /// </summary>
            public string closeKey;
            /// <summary>
            /// The character or sequence at the start of an autofill suggestion indicating that the suggestion
            /// is merely a descriptor, without the ability to fill in. Default is '{}'
            /// </summary>
            public string paramChar;
            /// <summary>
            /// When previews are enabled, stores the full text of what will be filled when Tab is pressed.
            /// </summary>
            public string fill;
            /// <summary>
            /// Whether or not to show previews with a CommandInterpreter object.
            /// </summary>
            public bool previews;
            /// <summary>
            /// Whether or not to close the console once a command has been executed.
            /// </summary>
            public bool closeOnCommand;
            public GameConsole(Texture2D initialTexture, SpriteFont font, string closeKey, CommandProcessor commandProcessor) : base(initialTexture, new Rectangle(0, 1020, 1920, 60), Color.FromNonPremultiplied(10, 10, 10, 180))
            {
                text = new TextSprite(font, "", 1.0f, new Vector2(5, 1022), Color.White);
                this.commandProcessor = commandProcessor;
                KeyboardInput.keys.Add("Q", new KeyboardInput.Key(Keys.Q));
                KeyboardInput.keys.Add("W", new KeyboardInput.Key(Keys.W));
                KeyboardInput.keys.Add("E", new KeyboardInput.Key(Keys.E));
                KeyboardInput.keys.Add("R", new KeyboardInput.Key(Keys.R));
                KeyboardInput.keys.Add("T", new KeyboardInput.Key(Keys.T));
                KeyboardInput.keys.Add("Y", new KeyboardInput.Key(Keys.Y));
                KeyboardInput.keys.Add("U", new KeyboardInput.Key(Keys.U));
                KeyboardInput.keys.Add("I", new KeyboardInput.Key(Keys.I));
                KeyboardInput.keys.Add("O", new KeyboardInput.Key(Keys.O));
                KeyboardInput.keys.Add("P", new KeyboardInput.Key(Keys.P));
                KeyboardInput.keys.Add("A", new KeyboardInput.Key(Keys.A));
                KeyboardInput.keys.Add("S", new KeyboardInput.Key(Keys.S));
                KeyboardInput.keys.Add("D", new KeyboardInput.Key(Keys.D));
                KeyboardInput.keys.Add("F", new KeyboardInput.Key(Keys.F));
                KeyboardInput.keys.Add("G", new KeyboardInput.Key(Keys.G));
                KeyboardInput.keys.Add("H", new KeyboardInput.Key(Keys.H));
                KeyboardInput.keys.Add("J", new KeyboardInput.Key(Keys.J));
                KeyboardInput.keys.Add("K", new KeyboardInput.Key(Keys.K));
                KeyboardInput.keys.Add("L", new KeyboardInput.Key(Keys.L));
                KeyboardInput.keys.Add("Z", new KeyboardInput.Key(Keys.Z));
                KeyboardInput.keys.Add("X", new KeyboardInput.Key(Keys.X));
                KeyboardInput.keys.Add("C", new KeyboardInput.Key(Keys.C));
                KeyboardInput.keys.Add("V", new KeyboardInput.Key(Keys.V));
                KeyboardInput.keys.Add("B", new KeyboardInput.Key(Keys.B));
                KeyboardInput.keys.Add("N", new KeyboardInput.Key(Keys.N));
                KeyboardInput.keys.Add("M", new KeyboardInput.Key(Keys.M));
                KeyboardInput.keys.Add("1", new KeyboardInput.Key(Keys.D1));
                KeyboardInput.keys.Add("2", new KeyboardInput.Key(Keys.D2));
                KeyboardInput.keys.Add("3", new KeyboardInput.Key(Keys.D3));
                KeyboardInput.keys.Add("4", new KeyboardInput.Key(Keys.D4));
                KeyboardInput.keys.Add("5", new KeyboardInput.Key(Keys.D5));
                KeyboardInput.keys.Add("6", new KeyboardInput.Key(Keys.D6));
                KeyboardInput.keys.Add("7", new KeyboardInput.Key(Keys.D7));
                KeyboardInput.keys.Add("8", new KeyboardInput.Key(Keys.D8));
                KeyboardInput.keys.Add("9", new KeyboardInput.Key(Keys.D9));
                KeyboardInput.keys.Add("0", new KeyboardInput.Key(Keys.D0));
                KeyboardInput.keys.Add("LShift", new KeyboardInput.Key(Keys.LeftShift));
                KeyboardInput.keys.Add("Space", new KeyboardInput.Key(Keys.Space));
                KeyboardInput.keys.Add("BSpace", new KeyboardInput.Key(Keys.Back));
                KeyboardInput.keys.Add("Enter", new KeyboardInput.Key(Keys.Enter));
                KeyboardInput.keys.Add("F1", new KeyboardInput.Key(Keys.F1));
                KeyboardInput.keys.Add("F2", new KeyboardInput.Key(Keys.F2));
                KeyboardInput.keys.Add("Tab", new KeyboardInput.Key(Keys.Tab));
                KeyboardInput.keys.Add("Dot", new KeyboardInput.Key(Keys.OemPeriod));
                KeyboardInput.keys.Add("BSlash", new KeyboardInput.Key(Keys.OemPipe));
                KeyboardInput.keys.Add("Colon", new KeyboardInput.Key(Keys.OemSemicolon));
                KeyboardInput.keys.Add("Up", new KeyboardInput.Key(Keys.Up));
                KeyboardInput.keys.Add("Down", new KeyboardInput.Key(Keys.Down));
                KeyboardInput.keys.Add("Dash", new KeyboardInput.Key(Keys.OemMinus));
                KeyboardInput.keys.Add("LeftCtrl", new KeyboardInput.Key(Keys.LeftControl));
                this.closeKey = closeKey;
                keys = new List<string>() { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P", "A", "S", "D", "F", "G", "H", "J", "K", "L", "Z", "X", "C", "V", "B", "N", "M", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "Space", "F1", "F2", "Dot", "BSlash", "Colon", "Dash" };
                pastText = new SpriteGroup.Layer(initialTexture, rect, Color.FromNonPremultiplied(0, 0, 0, 180), null, new TextSprite(font, "", 1.0f, new Vector2(5, 962), Color.White), Vector2.Zero, new Vector2(0, -50));
                previewText = new SpriteGroup.Layer(initialTexture, rect, Color.Black, null, new TextSprite(font, "", 1.0f, new Vector2(5, 962), Color.White), Vector2.Zero, new Vector2(0, -50));
                pastCommands = new Stack<string>();
                downCommands = new Stack<string>();
                previewText.visible = false;
                pastText.visible = false;
                visible = false;
                closeOnCommand = false;
                paramChar = "{}";
            }
            /// <summary>
            /// Adds output to the Console.
            /// </summary>
            public void AddOutput(string output, Color color)
            {
                pastText.AddText();
                if (pastText.sprites.Count > 1)
                {
                    for (int i = pastText.sprites.Count - 2; i >= 0; i--)
                    {
                        pastText.sprites[i + 1].ToTextSprite().text = pastText.sprites[i].ToTextSprite().text;
                        pastText.sprites[i + 1].color = pastText.sprites[i].color;
                    }
                }
                pastText.sprites[0].ToTextSprite().text = output;
                pastText.sprites[0].color = color;
                rect.Height = 60 + pastText.sprites.Count * 60;
                rect.Y = 1022 - pastText.sprites.Count * 60;
            }
            /// <summary>
            /// Adds output to the Console.
            /// </summary>
            public void AddOutput(string output)
            {
                AddOutput(output, Color.White);
            }
            /// <summary>
            /// Adds an error (colored red) to the Console.
            /// </summary>
            public void AddErrorOutput(string error)
            {
                AddOutput("Error: " + error, Color.Red);
            }
            /// <summary>
            /// Updates the Console.
            /// </summary>
            public bool Update()
            {
                int down = 0;
                if (KeyboardInput.keys["LShift"].IsDown())
                {
                    foreach (string key in keys)
                    {
                        if (KeyboardInput.keys[key].IsFirstDown())
                        {
                            if (key == "Space")
                            {
                                text.text = text.text + " ";
                            }
                            else if (key == "Dot")
                            {
                                text.text = text.text + ">";
                            }
                            else if (key == "BSlash")
                            {
                                text.text = text.text + "|";
                            }
                            else if (key == "Colon")
                            {
                                text.text = text.text + ":";
                            }
                            else if (key == "Dash")
                            {
                                text.text = text.text + "_";
                            }
                            else
                            {
                                text.text = text.text + key;
                            }
                            down++;
                        }
                    }
                }
                else
                {
                    foreach (string key in keys)
                    {
                        if (KeyboardInput.keys[key].IsFirstDown())
                        {
                            if (key == "Space")
                            {
                                text.text = text.text + " ";
                            }
                            else if (key == "Dot")
                            {
                                text.text = text.text + ".";
                            }
                            else if (key == "BSlash")
                            {
                                text.text = text.text + "\\";
                            }
                            else if (key == "Colon")
                            {
                                text.text = text.text + ";";
                            }
                            else if (key == "Dash")
                            {
                                text.text = text.text + "-";
                            }
                            else
                            {
                                text.text = text.text + key.ToLower();
                            }
                            down++;
                        }
                    }
                }

                if ((down > 0 || KeyboardInput.keys["Tab"].IsFirstDown()) && previews)
                {
                    previewText.sprites.Clear();
                    List<string> previewStrings = interpreter.GetPreviews(text.text, paramChar);
                    string[] split = text.text.Split(' ');
                    fill = "";
                    if (previewStrings.Count > 0)
                    {
                        for (int i = 0; i < split.Length - 1; i++)
                        {
                            fill = fill + split[i] + " ";
                        }
                        fill = fill + previewStrings[0];
                        fill = fill.TrimStart();
                    }
                    if (KeyboardInput.keys["Tab"].IsFirstDown() && fill != "")
                    {
                        if (split.Length < 2 || (split.Length >= 2 && fill.IndexOf(paramChar) == -1))
                        {
                            text.text = fill;
                        }
                    }
                    else
                    {
                        previewText.offsetCount = 0;
                        previewText.textTemplate.pos.X = 5 + text.GetSize().X;
                        foreach (string previewString in previewStrings)
                        {
                            previewText.AddText(previewString.Replace(paramChar, ""));
                        }

                        int width = 0;
                        foreach (Sprite sprite in previewText.sprites)
                        {
                            if (sprite.GenerateRect().Width > width)
                            {
                                width = sprite.GenerateRect().Width;
                            }
                        }
                        if (previewText.sprites.Count > 0)
                        {
                            previewText.sprites.Insert(0, new ObjectSprite(textures[0], new Rectangle((int)(previewText.sprites[previewText.sprites.Count - 1].pos.X - 2), (int)(previewText.sprites[previewText.sprites.Count - 1].pos.Y - 2), width, previewText.sprites.Count * 50), Color.Black));
                        }
                    }
                }

                if (KeyboardInput.keys["BSpace"].IsFirstDown() && text.text.Length > 0)
                {
                    if (KeyboardInput.keys["LeftCtrl"].IsDown())
                    {
                        string[] strings = text.text.Split(" ");
                        string newText = "";
                        foreach (string s in strings)
                        {
                            if (s != strings.Last())
                            {
                                newText = newText + s + " ";
                            }
                        }
                        text.text = newText.Trim();
                    }
                    else
                    {
                        text.text = text.text.Substring(0, text.text.Length - 1);
                    }
                }
                else if (KeyboardInput.keys["Enter"].IsFirstDown())
                {
                    if (text.text.Trim().Length > 0)
                    {
                        commandProcessor.Process(text.text.Trim());
                        pastCommands.Push(text.text.Trim());
                        text.text = "";
                    }
                    fill = "";
                    return closeOnCommand;
                }
                else if (KeyboardInput.keys["Up"].IsFirstDown())
                {
                    if (pastCommands.Count > 0)
                    {
                        downCommands.Push(text.text);
                        text.text = pastCommands.Pop();
                    }
                }
                else if (KeyboardInput.keys["Down"].IsFirstDown())
                {
                    if (downCommands.Count > 0)
                    {
                        pastCommands.Push(text.text);
                        text.text = downCommands.Pop();
                    }
                }

                return KeyboardInput.keys[closeKey].IsFirstDown();
            }
            public new void Draw(SpriteBatch sb)
            {
                base.Draw(sb);
                if (visible)
                {
                    pastText.Draw(sb);
                    if (previews)
                    {
                        if (fill != null && previewText.sprites.Count > 0 && fill.Replace(paramChar, "") != fill)
                        {
                            previewText.sprites[1].color = Color.Yellow;
                        }
                        else if (previewText.sprites.Count > 0)
                        {
                            previewText.sprites[1].color = Color.White;
                        }
                        if (fill != "")
                        {
                            previewText.Draw(sb);
                        }
                        text.color = Color.Gray;
                        string temp = text.text;
                        if (!String.IsNullOrEmpty(fill) && fill == fill.Replace(paramChar, ""))
                        {
                            text.text = fill.Replace(paramChar, "");
                        }
                        text.Draw(sb);
                        text.text = temp;
                        text.color = Color.White;
                    }
                    text.Draw(sb);
                }
            }
        }
        /// <summary>
        /// Class for managing the fluid transition between Colors. 
        /// Can hold multiple Colors, and seamlessely cycle between them in a given amount of frames.
        /// </summary>
        public class Gradient
        {
            /// <summary>
            /// Holds the Colors to cycle between.
            /// </summary>
            public List<Color> colors;
            /// <summary>
            /// Contains the pre-calculated values to adjust the color by every frame. Primarily only used internally.
            /// </summary>
            public double[] cScales;
            /// <summary>
            /// Contains the current exact RGBA values for the color. 
            /// Values must be converted to integers or bytes before they can be used with XNA's Color class.
            /// </summary>
            public double[] values;
            /// <summary>
            /// Denotes the current cycle that the Gradient is transitioning through. Primarily only used internally.
            /// </summary>
            public int cycle;
            /// <summary>
            /// The number of frames every cycle will take.
            /// </summary>
            public int frames;
            /// <summary>
            /// Keeps track of how many frames are left before changing to the next cycle.
            /// </summary>
            public int frameCycle;
            /// <param name="startingColor">The Color for the Gradient to start off with.</param>
            /// <param name="frames">The number of frames every cycle should last.</param>
            public Gradient(Color startingColor, int frames)
            {
                colors = new List<Color>();
                cScales = new double[4];
                values = new double[4];
                colors.Add(startingColor);
                values[0] = colors[0].R;
                values[1] = colors[0].G;
                values[2] = colors[0].B;
                values[3] = colors[0].A;
                this.frames = frames;
                frameCycle = frames;
            }
            /// <summary>
            /// Returns a Color based on the Gradient's current values.
            /// </summary>
            /// <returns></returns>
            public Color GetColor()
            {
                return Color.FromNonPremultiplied((int)values[0], (int)values[1], (int)values[2], (int)values[3]);
            }
            /// <summary>
            /// Transitions to the given cycle and sets all variables accordingly.
            /// </summary>
            /// <param name="cycle">The cycle index to begin transitioning to.</param>
            public void ValueUpdate(int cycle)
            {
                try
                {
                    cScales = new double[4];
                    values = new double[4];
                    this.cycle = cycle;
                    values[0] = colors[cycle].R;
                    values[1] = colors[cycle].G;
                    values[2] = colors[cycle].B;
                    values[3] = colors[cycle].A;
                    int nextCycle = 0;
                    if (colors.Count - 1 == cycle)
                    {
                        nextCycle = 0;
                    }
                    else
                    {
                        nextCycle = cycle + 1;
                    }
                    cScales[0] = (colors[cycle].R - colors[nextCycle].R) / (double)frames;
                    cScales[1] = (colors[cycle].G - colors[nextCycle].G) / (double)frames;
                    cScales[2] = (colors[cycle].B - colors[nextCycle].B) / (double)frames;
                    cScales[3] = (colors[cycle].A - colors[nextCycle].A) / (double)frames;
                }
                catch (Exception e)
                {
                    if (!(cycle >= colors.Count))
                    {
                        throw new Exception(e.ToString());
                    }
                }
            }
            /// <summary>
            /// Updates the Gradient and moves along the color transition process by one frame. 
            /// If at the end of a cycle, ValueUpdate() will automatically be called to move along to the next cycle.
            /// </summary>
            public void Update()
            {
                values[0] -= cScales[0];
                values[1] -= cScales[1];
                values[2] -= cScales[2];
                values[3] -= cScales[3];
                frameCycle--;
                if (frameCycle <= 0)
                {
                    if (colors.Count - 1 == cycle)
                    {
                        cycle = 0;
                    }
                    else
                    {
                        cycle++;
                    }
                    ValueUpdate(cycle);
                    frameCycle = frames;
                }
            }
        }
        public class Overlay
        {
            public class OverlayLayer
            {
                public List<string> lines;
                public Color color;
                public Vector2 pos, origin, offset;
                public SpriteEffects effects;
                public float scale, rotation, layerDepth;
                public bool visible;
                public OverlayLayer(Vector2 pos, Vector2 offset)
                {
                    lines = new List<string>();
                    this.pos = pos;
                    this.offset = offset;
                    origin = Vector2.Zero;
                    color = Color.White;
                    effects = SpriteEffects.None;
                    visible = true;
                }
                public void Draw(SpriteBatch sb, SpriteFont font)
                {
                    for (int i = 0; i < lines.Count && visible; i++)
                    {
                        sb.DrawString(font, lines[i], Scaling.ScaleVector2(new Vector2(pos.X + offset.X * i, pos.Y + offset.Y * i)), color, rotation, origin, Scaling.ScaleFloatX(scale), effects, layerDepth);
                    }
                }
                public void Draw(SpriteBatch sb, SpriteFont font, Color color)
                {
                    for (int i = 0; i < lines.Count && visible; i++)
                    {
                        sb.DrawString(font, lines[i], Scaling.ScaleVector2(new Vector2(pos.X + offset.X * i, pos.Y + offset.Y * i)), color, rotation, origin, Scaling.ScaleFloatX(scale), effects, layerDepth);
                    }
                }
            }
            public SpriteFont font;
            public List<OverlayLayer> layers;
            public bool visible;
            public Overlay()
            {
                layers = new List<OverlayLayer>();
                visible = true;
            }
            public Overlay(List<OverlayLayer> layers)
            {
                this.layers = layers;
                visible = true;
            }
            public void Add(OverlayLayer layer)
            {
                layers.Add(layer);
            }
            public void Draw(SpriteBatch sb)
            {
                for (int i = 0; i < layers.Count && visible; i++)
                {
                    layers[i].Draw(sb, font);
                }
            }
            public void Draw(SpriteBatch sb, Color color)
            {
                for (int i = 0; i < layers.Count && visible; i++)
                {
                    layers[i].Draw(sb, font, color);
                }
            }
        }
        public class DebugOverlay : Overlay
        {

        }
        public static class MouseInput
        {
            public static MouseState state;
            public static List<Vector2> positions;
            public static int posCapacity, leftDownFrames, rightDownFrames, currentScroll, scrollChange, scrollTick;
            public static bool leftReleased, rightReleased;
            public static void Update()
            {
                state = Mouse.GetState();
                leftReleased = false;
                rightReleased = false;
                positions.Add(new Vector2(state.X, state.Y));
                if (posCapacity > 0 && positions.Count > posCapacity)
                {
                    positions.RemoveAt(0);
                }
                if (state.LeftButton == ButtonState.Pressed)
                {
                    leftDownFrames++;
                }
                else
                {
                    if (leftDownFrames > 0)
                    {
                        leftReleased = true;
                    }
                    leftDownFrames = 0;
                }
                if (state.RightButton == ButtonState.Pressed)
                {
                    rightDownFrames++;
                }
                else
                {
                    if (rightDownFrames > 0)
                    {
                        rightReleased = true;
                    }
                    rightDownFrames = 0;
                }

                // Scrolling
                scrollChange = 0;
                if (currentScroll != state.ScrollWheelValue)
                {
                    if (currentScroll == 0)
                    {
                        scrollTick = Math.Abs(state.ScrollWheelValue);
                    }
                    scrollChange = (currentScroll - state.ScrollWheelValue) * -1;
                    currentScroll = state.ScrollWheelValue;
                }
            }
            public static Rectangle GetMouseRect()
            {
                return new Rectangle(state.X, state.Y, 1, 1);
            }
            public static bool IsLeftDown()
            {
                return leftDownFrames > 0;
            }
            public static bool IsLeftFirstDown()
            {
                return leftDownFrames == 1;
            }
            public static bool IsRightDown()
            {
                return rightDownFrames > 0;
            }
            public static bool IsRightFirstDown()
            {
                return rightDownFrames == 1;
            }
        }
        public static class KeyboardInput
        {
            public class Key
            {
                public Keys key;
                public int downFrames;
                public bool released;
                public Key(Keys key)
                {
                    this.key = key;
                }
                public bool Update()
                {
                    released = false;
                    if (state.IsKeyDown(key))
                    {
                        downFrames++;
                        return true;
                    }
                    else
                    {
                        if (downFrames > 0)
                        {
                            released = true;
                        }
                        downFrames = 0;
                    }
                    return false;
                }
                public bool IsDown()
                {
                    return downFrames > 0;
                }
                public bool IsFirstDown()
                {
                    return downFrames == 1;
                }
            }
            public static KeyboardState state;
            public static Dictionary<string, Key> keys;
            public static List<Keys> Update()
            {
                state = Keyboard.GetState();
                List<Keys> returnKeys = new List<Keys>();
                foreach (string key in keys.Keys)
                {
                    if (keys[key].Update())
                    {
                        returnKeys.Add(keys[key].key);
                    }
                }
                return returnKeys;
            }
            public static List<Keys> GetTrackedKeys()
            {
                List<Keys> returnKeys = new List<Keys>();

                foreach (string key in keys.Keys)
                {
                    returnKeys.Add(keys[key].key);
                }
                return returnKeys;
            }
        }
        /// <summary>
        /// Static class for managing Input/Output for controllers.
        /// </summary>
        public static class GamepadInput
        {
            /// <summary>
            /// Class responsible for tracking digital inputs on the GamePad (Buttons, DPad, Bumpers, etc).
            /// </summary>
            public class DigitalPad
            {
                /// <summary>
                /// The Controller index to track.
                /// </summary>
                public PlayerIndex index;
                /// <summary>
                /// Holds how long each Button has been down for
                /// </summary>
                public Dictionary<Buttons, int> downs;
                /// <param name="index">The Controller index to track.</param>
                public DigitalPad(PlayerIndex index)
                {
                    this.index = index;
                    downs = new Dictionary<Buttons, int>();
                    downs.Add(Buttons.A, 0);
                    downs.Add(Buttons.B, 0);
                    downs.Add(Buttons.X, 0);
                    downs.Add(Buttons.Y, 0);
                    downs.Add(Buttons.DPadLeft, 0);
                    downs.Add(Buttons.DPadRight, 0);
                    downs.Add(Buttons.DPadUp, 0);
                    downs.Add(Buttons.DPadDown, 0);
                    downs.Add(Buttons.LeftShoulder, 0);
                    downs.Add(Buttons.RightShoulder, 0);
                    downs.Add(Buttons.LeftStick, 0);
                    downs.Add(Buttons.RightStick, 0);
                    downs.Add(Buttons.Start, 0);
                    downs.Add(Buttons.Back, 0);
                    downs.Add(Buttons.BigButton, 0);
                }
                /// <summary>
                /// Returns true if the given Button is currently pressed. Does not require Update() to represent current info.
                /// </summary>
                /// <param name="button">The button to check.</param>
                public bool IsButtonDown(GamePadState state, Buttons button)
                {
                    return state.IsButtonDown(button);
                }
                /// <summary>
                /// Returns true if the given Button was just pressed. Requires Update() to represent accurate info.
                /// </summary>
                /// <param name="button">The Button to check.</param>
                public bool IsFirstButtonDown(Buttons button)
                {
                    return downs[button] == 1;
                }
                /// <summary>
                /// Updates the inputs.
                /// </summary>
                public void Update(GamePadState state)
                {
                    foreach (Buttons button in downs.Keys)
                    {
                        if (IsButtonDown(state, button))
                        {
                            downs[button]++;
                        }
                        else
                        {
                            downs[button] = 0;
                        }
                    }
                }
            }
            /// <summary>
            /// Class responsible for tracking analog inputs on the GamePad (Stick movements and triggers)
            /// </summary>
            public class AnalogPad
            {
                /// <summary>
                /// The controller index to track.
                /// </summary>
                public PlayerIndex index;
                /// <summary>
                /// Holds the positions of the Left Stick.
                /// </summary>
                public List<Vector2> leftStickPositions;
                /// <summary>
                /// Holds the positions of the Right Stick.
                /// </summary>
                public List<Vector2> rightStickPositions;
                /// <summary>
                /// Holds the positions of the Left Trigger.
                /// </summary>
                public List<float> leftTriggerPositions;
                /// <summary>
                /// Holds the positions of the Right Trigger.
                /// </summary>
                public List<float> rightTriggerPositions;
                /// <summary>
                /// The different analog inputs.
                /// </summary>
                public enum AnalogInput
                {
                    LeftStickX, LeftStickY, RightStickX, RightStickY, LeftTrigger, RightTrigger
                }
                /// <param name="index">The index to track.</param>
                public AnalogPad(PlayerIndex index)
                {
                    this.index = index;
                    leftStickPositions = new List<Vector2>();
                    rightStickPositions = new List<Vector2>();
                    leftTriggerPositions = new List<float>();
                    rightTriggerPositions = new List<float>();
                }
                /// <summary>
                /// Returns the current position of the given input.
                /// </summary>
                /// <param name="state"></param>
                /// <param name="input">The input to check.</param>
                public float GetInputData(GamePadState state, AnalogInput input)
                {
                    switch (input)
                    {
                        case AnalogInput.LeftStickX:
                            return state.ThumbSticks.Left.X;
                        case AnalogInput.LeftStickY:
                            return state.ThumbSticks.Left.Y;
                        case AnalogInput.RightStickX:
                            return state.ThumbSticks.Right.X;
                        case AnalogInput.RightStickY:
                            return state.ThumbSticks.Right.Y;
                        case AnalogInput.LeftTrigger:
                            return state.Triggers.Left;
                        case AnalogInput.RightTrigger:
                            return state.Triggers.Right;
                    }
                    return 0f;
                }
                /// <summary>
                /// Returns true if the specified input control is currently moved beyond the given deadzone.
                /// </summary>
                /// <param name="input">The input to check.</param>
                /// <param name="deadzone">The range for the control that will not trigger as moved. Cannot be a negative number.</param>
                /// <exception cref="ArgumentOutOfRangeException"></exception>
                public bool IsMoved(GamePadState state, AnalogInput input, float deadzone)
                {
                    if (deadzone < 0)
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                    switch (input)
                    {
                        case AnalogInput.LeftStickX:
                            return Math.Abs(state.ThumbSticks.Left.X) > deadzone;
                        case AnalogInput.LeftStickY:
                            return Math.Abs(state.ThumbSticks.Left.Y) > deadzone;
                        case AnalogInput.RightStickX:
                            return Math.Abs(state.ThumbSticks.Right.X) > deadzone;
                        case AnalogInput.RightStickY:
                            return Math.Abs(state.ThumbSticks.Right.Y) > deadzone;
                        case AnalogInput.LeftTrigger:
                            return Math.Abs(state.Triggers.Left) > deadzone;
                        case AnalogInput.RightTrigger:
                            return Math.Abs(state.Triggers.Right) > deadzone;
                    }
                    return false;
                }
                /// <summary>
                /// Returns if the input value has changed.
                /// </summary>
                /// <param name="input">The input to check.</param>
                /// <param name="deviation">The amount of 'leniency' the input has. The higher the deviation, the more movement it takes to trigger. 
                /// Requires Update() to represent accurate info.</param>
                public bool HasMoved(AnalogInput input, float deviation)
                {
                    float recent;
                    float second;
                    switch (input)
                    {
                        case AnalogInput.LeftStickX:
                            recent = leftStickPositions[leftStickPositions.Count - 1].X;
                            second = leftStickPositions[leftStickPositions.Count - 2].X;
                            if (recent == second)
                            {
                                return false;
                            }
                            else if ((recent < second && recent > second + deviation) || (recent > second && recent < second + deviation))
                            {
                                return false;
                            }
                            return true;
                        case AnalogInput.LeftStickY:
                            recent = leftStickPositions[leftStickPositions.Count - 1].Y;
                            second = leftStickPositions[leftStickPositions.Count - 2].Y;
                            if (recent == second)
                            {
                                return false;
                            }
                            else if ((recent < second && recent > second + deviation) || (recent > second && recent < second + deviation))
                            {
                                return false;
                            }
                            return true;
                        case AnalogInput.RightStickX:
                            recent = rightStickPositions[leftStickPositions.Count - 1].X;
                            second = rightStickPositions[leftStickPositions.Count - 2].X;
                            if (recent == second)
                            {
                                return false;
                            }
                            else if ((recent < second && recent > second + deviation) || (recent > second && recent < second + deviation))
                            {
                                return false;
                            }
                            return true;
                        case AnalogInput.RightStickY:
                            recent = rightStickPositions[leftStickPositions.Count - 1].Y;
                            second = rightStickPositions[leftStickPositions.Count - 2].Y;
                            if (recent == second)
                            {
                                return false;
                            }
                            else if ((recent < second && recent > second + deviation) || (recent > second && recent < second + deviation))
                            {
                                return false;
                            }
                            return true;
                        case AnalogInput.LeftTrigger:
                            recent = leftTriggerPositions[leftStickPositions.Count - 1];
                            second = leftTriggerPositions[leftStickPositions.Count - 2];
                            if (recent == second)
                            {
                                return false;
                            }
                            else if ((recent < second && recent > second + deviation) || (recent > second && recent < second + deviation))
                            {
                                return false;
                            }
                            return true;
                        case AnalogInput.RightTrigger:
                            recent = rightTriggerPositions[leftStickPositions.Count - 1];
                            second = rightTriggerPositions[leftStickPositions.Count - 2];
                            if (recent == second)
                            {
                                return false;
                            }
                            else if ((recent < second && recent > second + deviation) || (recent > second && recent < second + deviation))
                            {
                                return false;
                            }
                            return true;
                    }
                    return false;
                }
                /// <summary>
                /// Updates the inputs.
                /// </summary>
                public void Update(GamePadState state)
                {
                    leftStickPositions.Add(new Vector2(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y));
                    rightStickPositions.Add(new Vector2(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y));
                    leftTriggerPositions.Add(state.Triggers.Left);
                    rightTriggerPositions.Add(state.Triggers.Right);
                }
            }
            /// <summary>
            /// Holds the controller indexes to track.
            /// </summary>
            public static List<PlayerIndex> indexes;
            public static Dictionary<PlayerIndex, DigitalPad> digitals;
            public static Dictionary<PlayerIndex, AnalogPad> analogs;
            /// <summary>
            /// Adds a controller index to be tracked on every update.
            /// </summary>
            /// <param name="index">The controller index to begin tracking</param>.
            /// <exception cref="InvalidOperationException"></exception>
            public static void AddIndex(PlayerIndex index)
            {
                if (indexes.Contains(index))
                {
                    throw new InvalidOperationException("Specified controller index is already being tracked.");
                }
                indexes.Add(index);
                digitals.Add(index, new DigitalPad(index));
                analogs.Add(index, new AnalogPad(index));
            }
            /// <summary>
            /// Returns if the given button is currently pressed on the indicated controller.
            /// </summary>
            /// <param name="index">The controller index to look at.</param>
            /// <param name="button">The button to detect.</param>
            public static bool IsButtonDown(PlayerIndex index, Buttons button)
            {
                return digitals[index].IsButtonDown(GamePad.GetState(index), button);
            }
            /// <summary>
            /// Returns if the given button is currently pressed on the indicated controller, 
            /// or if the button was just pressed.
            /// </summary>
            /// <param name="index">The controller to look at.</param>
            /// <param name="button">The button to detect.</param>
            /// <param name="firstDown">Determines if the button must have been just pressed.</param>
            /// <returns></returns>
            public static bool IsButtonDown(PlayerIndex index, Buttons button, bool firstDown)
            {
                if (firstDown)
                {
                    return digitals[index].IsFirstButtonDown(button);
                }
                return digitals[index].IsButtonDown(GamePad.GetState(index), button);
            }
            /// <summary>
            /// Returns the current position of the given input.
            /// </summary>
            /// <param name="input">The input to check.</param>
            public static float GetAnalogInputData(PlayerIndex index, AnalogPad.AnalogInput input)
            {
                return analogs[index].GetInputData(GamePad.GetState(index), input);
            }
            /// <summary>
            /// Returns true if the specified input control is currently moved beyond the given deadzone.
            /// </summary>
            /// <param name="input">The input to check.</param>
            /// <param name="deadzone">The range for the control that will not trigger as moved. Cannot be a negative number.</param>
            /// <exception cref="ArgumentOutOfRangeException"></exception>
            public static bool IsInputMoved(PlayerIndex index, AnalogPad.AnalogInput input, float deadzone)
            {
                return analogs[index].IsMoved(GamePad.GetState(index), input, deadzone);
            }
            /// <summary>
            /// Returns if the input value has changed on the specified controller.
            /// </summary>
            /// <param name="index">The controller index to check.</param>
            /// <param name="input">The input to check.</param>
            /// <param name="deviation">The amount of 'leniency' the input has. The higher the deviation, the more movement it takes to trigger.</param>
            public static bool HasInputMoved(PlayerIndex index, AnalogPad.AnalogInput input, float deviation)
            {
                return analogs[index].HasMoved(input, deviation);
            }
            /// <summary>
            /// Updates all tracked controllers.
            /// </summary>
            public static void Update()
            {
                foreach (PlayerIndex index in indexes)
                {
                    digitals[index].Update(GamePad.GetState(index));
                    analogs[index].Update(GamePad.GetState(index));
                }
            }
        }
        /// <summary>
        /// Class for controlling saved data.
        /// </summary>
        public class SaveData
        {
            /// <summary>
            /// The different data types that can be saved in a SaveData file
            /// </summary>
            public enum DataType
            {
                String, Number, Decimal, Boolean, Chunk
            }
            /// <summary>
            /// Class for storing a read-in piece of data from a SaveData object.
            /// </summary>
            public class SaveDataObject
            {
                /// <summary>
                /// The saved data. Even if the data type if something other than a string, it is saved
                /// as such until it needs to be converted.
                /// </summary>
                public string data;
                /// <summary>
                /// The name of the data.
                /// </summary>
                public string name;
                /// <summary>
                /// The type of the data read in.
                /// </summary>
                public DataType dataType;
                /// <summary>
                /// The containing SaveDataChunk of this piece of save data. Nulled if this object is the eldest.
                /// </summary>
                public SaveDataChunk parentChunk;
                public SaveDataObject(string name, string data, DataType dataType)
                {
                    this.name = name;
                    this.data = data;
                    this.dataType = dataType;
                    parentChunk = null;
                }
                public SaveDataObject(string name, string data, DataType dataType, SaveDataChunk parentChunk) : this(name, data, dataType)
                {
                    this.parentChunk = parentChunk;
                }
                /// <summary>
                /// Returns the SaveDataObject as a SaveDataChunk, if possible.
                /// </summary>
                public SaveDataChunk GetChunk()
                {
                    if (dataType == DataType.Chunk)
                    {
                        return (SaveDataChunk)this;
                    }
                    return null;
                }
                public static int GetDataAsInt(SaveDataObject data)
                {
                    if (data.dataType == DataType.Number || data.dataType == DataType.Decimal)
                    {
                        return Convert.ToInt32(data.data);
                    }
                    throw new InvalidCastException();
                }
                public static double GetDataAsDouble(SaveDataObject data)
                {
                    if (data.dataType == DataType.Decimal)
                    {
                        return Convert.ToDouble(data.data);
                    }
                    throw new InvalidCastException();
                }
                public static bool GetDataAsBool(SaveDataObject data)
                {
                    if (data.dataType == DataType.Boolean)
                    {
                        return Convert.ToBoolean(data.data);
                    }
                    throw new InvalidCastException();
                }
            }
            /// <summary>
            /// Class for storing a chunk of SaveData. 
            /// </summary>
            public class SaveDataChunk : SaveDataObject
            {
                /// <summary>
                /// List for storing SaveDataObjects.
                /// </summary>
                public List<SaveDataObject> saveDataObjects;
                public SaveDataChunk(string name) : base(name, null, DataType.Chunk)
                {
                    saveDataObjects = new List<SaveDataObject>();
                }
                public SaveDataChunk(string name, SaveDataChunk parentChunk) : this(name)
                {
                    this.parentChunk = parentChunk;
                }
                /// <summary>
                /// Adds a SaveDataObject to the chunk's data.
                /// </summary>
                /// <param name="name">The name of the data.</param>
                /// <param name="data">The saved data.</param>
                /// <param name="dataType">The type of the data.</param>
                public void AddData(string name, string data, DataType dataType)
                {
                    saveDataObjects.Add(new SaveDataObject(name, data, dataType));
                }
                /// <summary>
                /// Adds a SaveDataChunk to the chunk's data.
                /// </summary>
                /// <param name="name">The name of the chunk.</param>
                public void AddChunk(string name)
                {
                    saveDataObjects.Add(new SaveDataChunk(name));
                }
                /// <summary>
                /// Adds a SaveDataChunk to the chunk's data.
                /// </summary>
                /// <param name="name">The name of the chunk.</param>
                public void AddChunk(SaveDataChunk chunk)
                {
                    saveDataObjects.Add(chunk);
                }
                public void AddToLastChunk(SaveDataObject obj)
                {
                    saveDataObjects.Last().GetChunk().AddData(obj.name, obj.data, obj.dataType);
                }
                public void AddChunkToLastChunk(SaveDataChunk chunk)
                {
                    saveDataObjects.Last().GetChunk().saveDataObjects.Add(chunk);
                }
                /// <summary>
                /// Returns the last SaveDataChunk added to the data.
                /// </summary>
                public SaveDataChunk GetLastChunk()
                {
                    return saveDataObjects.Last().GetChunk();
                }
                /// <summary>
                /// Finds and returns data with the same name as the one given.
                /// </summary>
                /// <param name="name">The name to search for.</param>
                /// <param name="destructive">Whether or not to destroy the original data in the save file. Recommended if duplicate names may be present.</param>
                public SaveDataObject FindData(string name, bool destructive)
                {
                    SaveDataObject toReturn = null;
                    foreach (SaveDataObject obj in saveDataObjects)
                    {
                        if (obj.name == name)
                        {
                            toReturn = obj;
                            break;
                        }
                    }
                    if (destructive && toReturn != null)
                    {
                        saveDataObjects.Remove(toReturn);
                    }
                    return toReturn;
                }
            }
            /// <summary>
            /// Class for storing and managing a SaveData object's data.
            /// </summary>
            public class SaveDataCollection
            {
                /// <summary>
                /// The List used to store all of the data. SaveDataChunks are polymorphic with SaveDataObjects.
                /// </summary>
                public List<SaveDataObject> data;
                /// <summary>
                /// The SaveData object that created this collection.
                /// </summary>
                public SaveData origin;
                public SaveDataCollection(SaveData origin)
                {
                    data = new List<SaveDataObject>();
                    this.origin = origin;
                }
                /// <summary>
                /// Clears the SaveDataCollection of all data.
                /// </summary>
                public void Clear()
                {
                    data.Clear();
                }
                /// <summary>
                /// Adds a SaveDataObject.
                /// </summary>
                /// <param name="name">The name of the data.</param>
                /// <param name="data">The saved data.</param>
                /// <param name="dataType">The type of the data.</param>
                public void AddData(string name, string data, DataType dataType)
                {
                    this.data.Add(new SaveDataObject(name, data, dataType));
                }
                /// <summary>
                /// Adds a SaveDataChunk to the chunk's data.
                /// </summary>
                /// <param name="chunkName">The name of the chunk.</param>
                public void AddData(string chunkName)
                {
                    data.Add(new SaveDataChunk(chunkName));
                }
                /// <summary>
                /// Returns the last SaveDataChunk added to the data.
                /// </summary>
                public SaveDataChunk GetLastChunk()
                {
                    return data.Last().GetChunk();
                }
                /// <summary>
                /// Finds and returns data with the same name as the one given.
                /// </summary>
                /// <param name="name">The name to search for.</param>
                public SaveDataObject FindData(string name)
                {
                    return FindData(name, false);
                }
                /// <summary>
                /// Finds and returns data with the same name as the one given.
                /// </summary>
                /// <param name="name">The name to search for.</param>
                /// <param name="destructive">Whether or not to destroy the original data in the save file. Recommended if duplicate names may be present.</param>
                public SaveDataObject FindData(string name, bool destructive)
                {
                    SaveDataObject toReturn = null;
                    foreach (SaveDataObject obj in data)
                    {
                        if (obj.name == name)
                        {
                            toReturn = obj;
                            break;
                        }
                    }
                    if (destructive && toReturn != null)
                    {
                        data.Remove(toReturn);
                    }
                    return toReturn;
                }
                /// <summary>
                /// Finds and returns data with the same name as the one given, searching through all chunks.
                /// </summary>
                /// <param name="name">The name to search for.</param>
                /// <param name="destructive">Whether or not to destroy the original data in the save file. Recommended if duplicate names may be present.</param>
                public SaveDataObject FindChunkedData(string name, bool destructive)
                {
                    SaveDataObject toReturn = null;
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i].name == name)
                        {
                            toReturn = data[i];
                            if (destructive)
                            {
                                data.Remove(toReturn);
                            }
                            break;
                        }
                        // Beginning recursion to search through chunks
                        else if (data[i].dataType == DataType.Chunk)
                        {
                            toReturn = FindChunkedData(name, destructive, data[i].GetChunk());
                            if (toReturn != null)
                            {
                                break;
                            }
                        }
                    }
                    return toReturn;
                }
                private SaveDataObject FindChunkedData(string name, bool destructive, SaveDataChunk currentChunk)
                {
                    SaveDataObject toReturn = null;
                    for (int i = 0; i < currentChunk.saveDataObjects.Count; i++)
                    {
                        if (currentChunk.saveDataObjects[i].name == name)
                        {
                            toReturn = currentChunk.saveDataObjects[i];
                            if (destructive)
                            {
                                currentChunk.saveDataObjects.Remove(toReturn);
                            }
                            return toReturn;
                        }
                        else if (currentChunk.saveDataObjects[i].dataType == DataType.Chunk)
                        {
                            toReturn = FindChunkedData(name, destructive, data[i].GetChunk());
                        }
                    }
                    return toReturn;
                }
            }

            /// <summary>
            /// The StreamReader object that reads in data.
            /// </summary>
            public StreamReader reader;
            /// <summary>
            /// The StreamWrite object that writes data to the file.
            /// </summary>
            public StreamWriter writer;
            /// <summary>
            /// The saved data from reading the file.
            /// </summary>
            public SaveDataCollection savedData;
            /// <summary>
            /// The filepath where the save file is located.
            /// </summary>
            public string filepath;
            /// <summary>
            /// The version of the SaveData reader/writer.
            /// </summary>
            public string version;
            public SaveData(string filepath)
            {
                this.filepath = filepath;
                version = "beta1";
                savedData = new SaveDataCollection(this);
                // Creating the file if no file exists at 'filepath'
                if (!File.Exists(filepath))
                {
                    File.Create(filepath).Close();
                }
            }
            /// <summary>
            /// Reads in the entire save file. Deletes anything previously stored in saved data.
            /// </summary>
            /// <returns>Whether or not the file was successfully read in its entirety</returns>
            public bool ReadFile()
            {
                reader = new StreamReader(filepath);
                try
                {
                    savedData.Clear();
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    reader.ReadLine();
                    // Checking for version compatibility
                    string verCheck = reader.ReadLine();
                    verCheck = verCheck.Replace("ver ", "");
                    if (verCheck == version)
                    {
                        SaveDataChunk chunk = null;
                        reader.ReadLine();
                        // Reading the save data
                        while (!(reader.EndOfStream))
                        {
                            // Reference: nameLine header: 'name='
                            string nameLine = reader.ReadLine().TrimStart();
                            nameLine = nameLine.Substring(5);
                            // Reference: typeLine header: 'type='
                            string typeLine = reader.ReadLine().TrimStart();
                            typeLine = typeLine.Substring(5);
                            // Reference: dataLine header: 'data='
                            string dataLine = reader.ReadLine().TrimStart();
                            dataLine = dataLine.Substring(5);
                            reader.ReadLine();
                            // Checking if to exit the chunk
                            if (dataLine == "endchunk" && typeLine == "chk" && chunk != null)
                            {
                                chunk = chunk.parentChunk;
                            }
                            else
                            {
                                if (chunk == null)
                                {
                                    switch (typeLine)
                                    {
                                        case "str":
                                            savedData.AddData(nameLine, dataLine, DataType.String);
                                            break;
                                        case "num":
                                            savedData.AddData(nameLine, dataLine, DataType.Number);
                                            break;
                                        case "dec":
                                            savedData.AddData(nameLine, dataLine, DataType.Decimal);
                                            break;
                                        case "tof":
                                            savedData.AddData(nameLine, dataLine, DataType.Boolean);
                                            break;
                                        case "chk":
                                            savedData.AddData(nameLine);
                                            chunk = savedData.GetLastChunk();
                                            break;
                                    }
                                }
                                else
                                {
                                    switch (typeLine)
                                    {
                                        case "str":
                                            chunk.AddData(nameLine, dataLine, DataType.String);
                                            break;
                                        case "num":
                                            chunk.AddData(nameLine, dataLine, DataType.Number);
                                            break;
                                        case "dec":
                                            chunk.AddData(nameLine, dataLine, DataType.Decimal);
                                            break;
                                        case "tof":
                                            chunk.AddData(nameLine, dataLine, DataType.Boolean);
                                            break;
                                        case "chk":
                                            chunk.AddChunk(nameLine);
                                            chunk.GetLastChunk().parentChunk = chunk;
                                            chunk = chunk.GetLastChunk();
                                            break;
                                    }
                                }
                            }
                        }
                        reader.Close();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    reader.Close();
                }
                // Returning false since reading must have failed
                return false;
            }
            /// <summary>
            /// Adds a SaveDataObject to the save data to be written to file.
            /// </summary>
            /// <param name="data">The data to be added.</param>
            /// <returns>The provided SaveDataObject</returns>
            public SaveDataObject AddSaveObject(SaveDataObject data)
            {
                savedData.data.Add(data);
                return data;
            }
            /// <summary>
            /// Adds a SaveDataChunk to the save data to be written to file.
            /// </summary>
            /// <param name="chunk">The chunk to be added.</param>
            /// <returns>The provided SaveDataChunk</returns>
            public SaveDataChunk AddSaveChunk(SaveDataChunk chunk)
            {
                savedData.data.Add(chunk);
                return chunk;
            }
            /// <summary>
            /// Forces the StreamReader and StreamWriter closed.
            /// </summary>
            public void ForceCloseStreams()
            {
                reader.Close();
                writer.Close();
            }
            private void Write(string name, DataType dataType, string data, string indent)
            {
                string type = "";
                switch (dataType)
                {
                    case DataType.String:
                        type = "str";
                        break;
                    case DataType.Number:
                        type = "num";
                        break;
                    case DataType.Decimal:
                        type = "dec";
                        break;
                    case DataType.Boolean:
                        type = "tof";
                        break;
                    case DataType.Chunk:
                        type = "chk";
                        break;
                }
                writer.WriteLine(indent + "name=" + name);
                writer.WriteLine(indent + "type=" + type);
                writer.WriteLine(indent + "data=" + data);
                writer.WriteLine();
            }
            /// <summary>
            /// Saves all saved data to file. Truncates the file's contents.
            /// <returns>Whether or not the file was saved in its entirety</returns>
            /// </summary>
            public bool SaveToFile()
            {
                writer = new StreamWriter(filepath, false);
                writer.WriteLine("OzzzFramework Save Data File");
                try
                {
                    writer.WriteLine("ver " + version);
                    writer.WriteLine("----------------------------");
                    // Writing the saved data
                    foreach (SaveDataObject obj in savedData.data)
                    {
                        if (obj.dataType == DataType.Chunk)
                        {
                            Write(obj.name, DataType.Chunk, "", "");
                            SaveChunkToFile(obj.GetChunk(), "");
                        }
                        else
                        {
                            Write(obj.name, obj.dataType, obj.data, "");
                        }
                    }
                    writer.Close();
                    return true;
                }
                catch
                {
                    writer.Close();
                }
                return false;
            }
            private void SaveChunkToFile(SaveDataChunk chunk, string indent)
            {
                indent = indent + "  ";
                foreach (SaveDataObject obj in chunk.saveDataObjects)
                {
                    if (obj.dataType == DataType.Chunk)
                    {
                        Write(obj.name, DataType.Chunk, "", indent);
                        SaveChunkToFile(obj.GetChunk(), indent);
                    }
                    else
                    {
                        Write(obj.name, obj.dataType, obj.data, indent);
                    }
                }
                Write("", DataType.Chunk, "endchunk", indent);
            }
        }
    }
}
