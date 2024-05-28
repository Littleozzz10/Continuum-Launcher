using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

// Initializing OzzzFramework with a scale of (1, 1) and a target FPS of 60
OzzzFramework.Initialize(new Vector2(1, 1), 60.0f);

// Reinitializing OzzzFramework with a psuedo-dynamic resolution and a target FPS of 60
// Using a "psuedo-dynamic resolution" allows for OzzzFramework to keep math consistent at
//    different resolutions
// As an example, a resolution of 1280x720 would provide the 'scale' parameter as a Vector2 with
//    approximately 0.667f as both values
float resX = (float)GraphicsDevice.Viewport.Width;
float resY = (float)GraphicsDevice.Viewport.Height;
OzzzFramework.Initialize(new Vector2(resX / 1920, resY / 1080), 60);