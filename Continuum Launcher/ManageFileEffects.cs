using System;
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
using System.Text.RegularExpressions;
using System.Data;
using System.Diagnostics;
using STFS;
using SharpDX.MediaFoundation;
using System.Windows.Forms;

namespace XeniaLauncher
{
    public class ManageFileEffects : IWindowEffects
    {
        public enum OptionTypes
        {
            General, Image, STFS, SVOD, InstallSTFS, XeniaData, VideoSTFS, Extract
        }
        public static string OptionTypeToString(OptionTypes type)
        {
            switch (type)
            {
                case OptionTypes.General:
                    return "General Files";
                case OptionTypes.Image:
                    return "Image";
                case OptionTypes.STFS:
                    return "STFS";
                case OptionTypes.SVOD:
                    return "STFS + SVOD";
                case OptionTypes.InstallSTFS:
                    return "STFS Installable";
                case OptionTypes.XeniaData:
                    return "Xenia Data";
                case OptionTypes.VideoSTFS:
                    return "STFS Video";
                case OptionTypes.Extract:
                    return "Deletable Extract";
            }
            return "Unknown";
        }
        public static OptionTypes SubtitleToOptionType(string subTitle)
        {
            switch (subTitle)
            {
                case "Installed Disc Game":
                    return OptionTypes.SVOD;
                case "Installed Game on Demand":
                    return OptionTypes.SVOD;
                case "Xbox Live Arcade Title":
                    return OptionTypes.STFS;
                case "Xbox 360 Theme":
                    return OptionTypes.STFS;
                case "Gamer Picture":
                    return OptionTypes.STFS;
                case "Title Update":
                    return OptionTypes.InstallSTFS;
                case "Downloadable Content":
                    return OptionTypes.InstallSTFS;
                case "Localized Xenia Data":
                    return OptionTypes.XeniaData;
                case "Xenia Installed Content":
                    return OptionTypes.XeniaData;
                case "Xenia Game Save":
                    return OptionTypes.XeniaData;
                case "Video":
                    return OptionTypes.VideoSTFS;
                case "Extracted Content":
                    return OptionTypes.Extract;
            }
            return OptionTypes.General;
        }
        public void ActivateButton(Game1 game, Window source, ObjectSprite origin, int buttonIndex)
        {
            if (source.strings[buttonIndex] == Shared.FileManageStrings["explorer"])
            {
                game.selectSound.Play();
                try
                {
                    Process.Start("explorer", new FileInfo(game.dataFiles[game.selectedDataIndex][game.manageWindow.stringIndex].filepath).DirectoryName);
                }
                catch
                {
                    game.message = new MessageWindow(game, "Error", "Failed to launch Explorer. Filepath may be broken.", Game1.State.ManageFile);
                    game.state = Game1.State.Message;
                }
            }
            else if (source.strings[buttonIndex] == Shared.FileManageStrings["metadata"])
            {
                string desc = "";
                int yPos = 0;
                int height = 1080;
                if (game.hideSecretMetadata)
                {
                    desc = "Note: Potentially sensitive metadata has been hidden, as per user preferences";
                    yPos = -40;
                    height = 1120;
                }
                game.metadataWindow = new Window(game, new Rectangle(-200, yPos, 2320, height), "STFS Metadata Viewer", desc, new MessageButtonEffects(), new SingleButtonEvent(), new GenericStart(), Game1.State.ManageFile, true);
                
                // Loading and displaying STFS data
                STFS24 stfs = new STFS24(game.dataFiles[game.selectedDataIndex][game.manageWindow.stringIndex].filepath);

                // --- Header
                Vector2 headerOffset = new Vector2(40, 140);
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Header", 0.5f, new Vector2(headerOffset.X, headerOffset.Y + 0), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Last().tags.Add("gray");
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Magic: " + stfs.GetMagic(), 0.35f, new Vector2(headerOffset.X, headerOffset.Y + 50), Color.FromNonPremultiplied(255, 255, 255, 0)));

                // --- Signatures
                XSignature sig = stfs.ReturnSignature();
                if (stfs.GetMagic() == "CON ")
                {
                    Vector2 signatureOffset = new Vector2(40, 290);
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Signature Data", 0.5f, new Vector2(signatureOffset.X, signatureOffset.Y + 0), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Last().tags.Add("gray");
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Public Key Cert. Size: 0x" + STFS24.GetByteArrayAsHex(sig.GetCertificateSize()).Replace(" ", ""), 0.35f, new Vector2(signatureOffset.X, signatureOffset.Y + 50), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    if (game.hideSecretMetadata)
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Cert. Owner Console ID: 0x----------", 0.35f, new Vector2(signatureOffset.X, signatureOffset.Y + 90), Color.FromNonPremultiplied(255, 255, 255, 0)));
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Cert. Owner Console PN: ------------", 0.35f, new Vector2(signatureOffset.X, signatureOffset.Y + 130), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    else
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Cert. Owner Console ID: 0x" + STFS24.GetByteArrayAsHex(sig.GetOwnerConsoleID()).Replace(" ", ""), 0.35f, new Vector2(signatureOffset.X, signatureOffset.Y + 90), Color.FromNonPremultiplied(255, 255, 255, 0)));
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Cert. Owner Console PN: " + sig.GetOwnerConsolePartNum(), 0.35f, new Vector2(signatureOffset.X, signatureOffset.Y + 130), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Cert. Owner Console Type: " + sig.GetOwnerConsoleType() + " (" + (int?)sig.GetOwnerConsoleType() + ")", 0.35f, new Vector2(signatureOffset.X, signatureOffset.Y + 170), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Cert. Generation Date: " + sig.GetDateGenerated(), 0.35f, new Vector2(signatureOffset.X, signatureOffset.Y + 210), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Public Exponent: 0x" + STFS24.GetByteArrayAsHex(sig.GetPublicExponent()).Replace(" ", ""), 0.35f, new Vector2(signatureOffset.X, signatureOffset.Y + 250), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    string modulus = STFS24.GetByteArrayAsHex(sig.GetPublicModulus()).Replace(" ", "");
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Public Modulus: [" + (modulus.Length / 2) + " bytes]", 0.35f, new Vector2(signatureOffset.X, signatureOffset.Y + 290), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    string certSignature = STFS24.GetByteArrayAsHex(sig.GetCertificateSignature()).Replace(" ", "");
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Cert. Signature: [" + (certSignature.Length / 2) + " bytes]", 0.35f, new Vector2(signatureOffset.X, signatureOffset.Y + 330), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    string signature = STFS24.GetByteArrayAsHex(sig.GetSignature()).Replace(" ", "");
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Signature: [" + (signature.Length / 2) + " bytes]", 0.35f, new Vector2(signatureOffset.X, signatureOffset.Y + 370), Color.FromNonPremultiplied(255, 255, 255, 0)));
                }
                else if (stfs.GetMagic() == "LIVE" || stfs.GetMagic() == "PIRS")
                {
                    Vector2 signatureOffset = new Vector2(40, 290);
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Signature Data", 0.5f, new Vector2(signatureOffset.X, signatureOffset.Y + 0), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Last().tags.Add("gray");
                    string packageSignature = STFS24.GetByteArrayAsHex(sig.GetPackageSignature()).Replace(" ", "");
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Package Signature: [" + (packageSignature.Length / 2) + " bytes]", 0.35f, new Vector2(signatureOffset.X, signatureOffset.Y + 50), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    string padding = STFS24.GetByteArrayAsHex(sig.GetPadding()).Replace(" ", "");
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Padding: [" + (padding.Length / 2) + " bytes]", 0.35f, new Vector2(signatureOffset.X, signatureOffset.Y + 90), Color.FromNonPremultiplied(255, 255, 255, 0)));
                }

                // --- Metadata
                XMetadata meta = stfs.ReturnMetadata();
                Vector2 metadataOffset = new Vector2(550, 140);
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Shared Metadata", 0.5f, new Vector2(metadataOffset.X, metadataOffset.Y + 0), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Last().tags.Add("gray");
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Header Hash: 0x" + STFS24.GetByteArrayAsHex(meta.GetHeaderHash()).Replace(" ", ""), 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 50), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Header Size: " + meta.GetHeaderSize() + " bytes", 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 90), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Content Type: " + meta.GetContentType() + " (" + STFS24.GetByteArrayAsHex(meta.GetContentTypeAsBytes()) + ")", 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 130), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Metadata Version: " + (XMetadata.XMetadataType)meta.GetMetadataVersion() + " (" + meta.GetMetadataVersion() + ")", 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 170), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Content Size: " + game.ConvertDataSize("" + meta.GetContentSize()) + " (" + meta.GetContentSize() + " bytes)", 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 210), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Media ID: 0x" + STFS24.GetByteArrayAsHex(meta.GetMediaID()), 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 250), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Version: " + meta.GetVersion(), 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 290), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Base Version: " + meta.GetBaseVersion(), 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 330), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Title ID: 0x" + STFS24.GetByteArrayAsHex(meta.GetTitleID()), 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 370), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Platform: " + meta.GetPlatform() + " (" + (int)meta.GetPlatform() + ")", 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 410), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Executable Type: 0x" + STFS24.GetByteArrayAsHex(new byte[] { (byte)meta.GetExecutableType() }), 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 450), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Disc Number: " + meta.GetDiscNumber(), 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 490), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Disc in Set: " + meta.GetDiscInSet(), 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 530), Color.FromNonPremultiplied(255, 255, 255, 0)));
                if (game.hideSecretMetadata)
                {
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Save Game ID: 0x--------", 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 570), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Console ID: 0x----------", 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 610), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Profile ID: 0x----------------", 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 650), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Reserved Data: 0x----------------", 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 810), Color.FromNonPremultiplied(255, 255, 255, 0)));
                }
                else
                {
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Save Game ID: 0x" + STFS24.GetByteArrayAsHex(meta.GetSaveGameID()), 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 570), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Console ID: 0x" + STFS24.GetByteArrayAsHex(meta.GetConsoleID()), 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 610), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Profile ID: 0x" + STFS24.GetByteArrayAsHex(meta.GetProfileID()), 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 650), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Reserved Data: 0x" + STFS24.GetByteArrayAsHex(meta.GetReserved()), 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 810), Color.FromNonPremultiplied(255, 255, 255, 0)));
                }
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Data File Count: " + meta.GetDataFileCount(), 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 690), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Data File Combined Size: " + game.ConvertDataSize("" + meta.GetDataFileSize()) + " (" + meta.GetDataFileSize() + " bytes)", 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 730), Color.FromNonPremultiplied(255, 255, 255, 0)));
                game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Volume Descriptor Type: " + meta.GetDescriptorType() + " (" + (int)meta.GetDescriptorType() + ")", 0.35f, new Vector2(metadataOffset.X, metadataOffset.Y + 770), Color.FromNonPremultiplied(255, 255, 255, 0)));
                if ((XMetadata.XMetadataType)meta.GetMetadataVersion() == XMetadata.XMetadataType.V1 || (XMetadata.XMetadataType)meta.GetMetadataVersion() == XMetadata.XMetadataType.NotSpecified)
                {
                    Vector2 v1Offset = new Vector2(1400, 140);
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "V1 Metadata", 0.5f, new Vector2(v1Offset.X, v1Offset.Y + 0), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Last().tags.Add("gray");
                    string padding = STFS24.GetByteArrayAsHex(meta.GetPaddingV1()).Replace(" ", "");
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Padding: [" + (padding.Length / 2) + " bytes]", 0.35f, new Vector2(v1Offset.X, v1Offset.Y + 50), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    string deviceID = STFS24.GetByteArrayAsHex(meta.GetDeviceID());
                    if (game.hideSecretMetadata)
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Device ID: 0x--------------------", 0.35f, new Vector2(v1Offset.X, v1Offset.Y + 90), Color.FromNonPremultiplied(255, 255, 255, 0)));
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "                      --------------------", 0.35f, new Vector2(v1Offset.X, v1Offset.Y + 130), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    else
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Device ID: 0x" + deviceID.Substring(0, 20), 0.35f, new Vector2(v1Offset.X, v1Offset.Y + 90), Color.FromNonPremultiplied(255, 255, 255, 0)));
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "                      " + deviceID.Substring(20, 20), 0.35f, new Vector2(v1Offset.X, v1Offset.Y + 130), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Publisher Name: " + meta.GetPublisherName(), 0.35f, new Vector2(v1Offset.X, v1Offset.Y + 170), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Title Name: " + TextSprite.GetASCII(meta.GetTitleName()), 0.35f, new Vector2(v1Offset.X, v1Offset.Y + 210), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Thumbnail Image Size: " + meta.GetThumbnailImageSize(), 0.35f, new Vector2(v1Offset.X, v1Offset.Y + 250), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Title Thumbnail Image Size: " + meta.GetTitleThumbnailImageSize(), 0.35f, new Vector2(v1Offset.X, v1Offset.Y + 290), Color.FromNonPremultiplied(255, 255, 255, 0)));
                }

                // --- Volume Descriptor
                XVolumeDescriptor vd = meta.ReturnVolumeDescriptor();
                if (meta.GetDescriptorType() == XVolumeDescriptor.VDType.SVOD)
                {
                    Vector2 descriptorOffset = new Vector2(1400, 530);
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Volume Descriptor", 0.5f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 0), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Last().tags.Add("gray");
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "VD Size: 0x" + STFS24.GetByteArrayAsHex(new byte[] { (byte)vd.GetSize() }), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 50), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Block Cache Element Count: 0x" + STFS24.GetByteArrayAsHex(new byte[] { (byte)vd.GetBlockCacheElementCount() }), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 90), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Worker Thread Processor: 0x" + STFS24.GetByteArrayAsHex(new byte[] { (byte)vd.GetWorkerThreadProcessor() }), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 130), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Worker Thread Priority: 0x" + STFS24.GetByteArrayAsHex(new byte[] { (byte)vd.GetWorkerThreadPriority() }), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 170), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    string hash = STFS24.GetByteArrayAsHex(vd.GetHash()).Replace(" ", "");
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Hash: 0x" + hash.Substring(0, 20), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 210), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "               " + hash.Substring(20, 20), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 250), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Device Features: 0x" + STFS24.GetByteArrayAsHex(new byte[] { (byte)vd.GetDeviceFeatures() }), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 290), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Data Block Count: " + vd.GetDataBlockCount(), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 330), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    if (game.hideSecretMetadata)
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Padding/Reserved Data: 0x----------", 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 370), Color.FromNonPremultiplied(255, 255, 255, 0)));

                    }
                    else
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Padding/Reserved Data: 0x" + STFS24.GetByteArrayAsHex(vd.GetPadding()), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 370), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                }
                else if (meta.GetDescriptorType() == XVolumeDescriptor.VDType.STFS)
                {
                    Vector2 descriptorOffset = new Vector2(1400, 530);
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Volume Descriptor", 0.5f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 0), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Last().tags.Add("gray");
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Reserved: 0x" + STFS24.GetByteArrayAsHex(new byte[] { (byte)vd.GetReserved() }), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 50), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Block Separation: 0x" + STFS24.GetByteArrayAsHex(new byte[] { (byte)vd.GetBlockSeparation() }), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 90), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "File Table Block Count: " + vd.GetFileTableBlockCount(), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 130), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "File Table Block Number: " + vd.GetFileTableBlockNumber(), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 170), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    string hash = STFS24.GetByteArrayAsHex(vd.GetTopHashTableHash()).Replace(" ", "");
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "THT Hash: 0x" + hash.Substring(0, 20), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 210), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "                      " + hash.Substring(20, 20), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 250), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Allocated Block Count: " + vd.GetTotalAllocatedBlockCount(), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 290), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Unallocated Block Count: " + vd.GetTotalUnallocatedBlockCount(), 0.35f, new Vector2(descriptorOffset.X, descriptorOffset.Y + 330), Color.FromNonPremultiplied(255, 255, 255, 0)));
                }

                // --- Transfer Flags
                XTransferFlags? tFlags = meta.GetTransferFlags();
                if (tFlags != null)
                {
                    Vector2 tFlagOffset = new Vector2(40, 760);
                    game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "Transfer Flags", 0.5f, new Vector2(tFlagOffset.X, tFlagOffset.Y + 0), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    game.metadataWindow.extraSprites.Last().tags.Add("gray");

                    XTransferFlags? flags = (XTransferFlags)tFlags;
                    if ((flags & XTransferFlags.DeepLinkSupported) != 0)
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "DeepLinkSupported: Yes", 0.35f, new Vector2(tFlagOffset.X, tFlagOffset.Y + 50), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    else
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "DeepLinkSupported: No", 0.35f, new Vector2(tFlagOffset.X, tFlagOffset.Y + 50), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    if ((flags & XTransferFlags.DisableNetworkStorage) != 0)
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "DisableNetworkStorage: Yes", 0.35f, new Vector2(tFlagOffset.X, tFlagOffset.Y + 90), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    else
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "DisableNetworkStorage: No", 0.35f, new Vector2(tFlagOffset.X, tFlagOffset.Y + 90), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    if ((flags & XTransferFlags.KinectEnabled) != 0)
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "KinectEnabled: Yes", 0.35f, new Vector2(tFlagOffset.X, tFlagOffset.Y + 130), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    else
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "KinectEnabled: No", 0.35f, new Vector2(tFlagOffset.X, tFlagOffset.Y + 130), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    if ((flags & XTransferFlags.MoveOnlyTransfer) != 0)
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "MoveOnlyTransfer: Yes", 0.35f, new Vector2(tFlagOffset.X, tFlagOffset.Y + 170), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    else
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "MoveOnlyTransfer: No", 0.35f, new Vector2(tFlagOffset.X, tFlagOffset.Y + 170), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    if ((flags & XTransferFlags.DeviceIDTransfer) != 0)
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "DeviceIDTransfer: Yes", 0.35f, new Vector2(tFlagOffset.X, tFlagOffset.Y + 210), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    else
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "DeviceIDTransfer: No", 0.35f, new Vector2(tFlagOffset.X, tFlagOffset.Y + 210), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    if ((flags & XTransferFlags.ProfileIDTransfer) != 0)
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "ProfileIDTransfer: Yes", 0.35f, new Vector2(tFlagOffset.X, tFlagOffset.Y + 250), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                    else
                    {
                        game.metadataWindow.extraSprites.Add(new TextSprite(game.font, "ProfileIDTransfer: No", 0.35f, new Vector2(tFlagOffset.X, tFlagOffset.Y + 250), Color.FromNonPremultiplied(255, 255, 255, 0)));
                    }
                }

                stfs.CloseStream();

                game.state = Game1.State.Metadata;
                game.selectSound.Play();
            }
            else if (source.strings[buttonIndex] == Shared.FileManageStrings["install"])
            {
                if (!String.IsNullOrEmpty(game.extractPath))
                {
                    game.toImport = game.dataFiles[game.selectedDataIndex][game.manageWindow.stringIndex];
                    game.message = new MessageWindow(game, "Import DLC", "Do you want to import " + game.toImport.name + "?", Game1.State.ManageFile, MessageWindow.MessagePrompts.YesNo);
                    game.state = Game1.State.Message;
                }
                else
                {
                    game.message = new MessageWindow(game, "Import DLC", "Error: extraction tool not found or bad extract path", Game1.State.ManageFile);
                    game.state = Game1.State.Message;
                }
            }
            else if (source.strings[buttonIndex] == Shared.FileManageStrings["extract"])
            {
                if (!String.IsNullOrEmpty(game.extractPath))
                {
                    game.toExtract = game.dataFiles[game.selectedDataIndex][game.manageWindow.stringIndex];
                    game.message = new MessageWindow(game, "Extract STFS Container", "Do you want to extract " + game.toExtract.name + "? This will overwrite any existing extract.", Game1.State.ManageFile, MessageWindow.MessagePrompts.YesNo);
                    game.state = Game1.State.Message;
                }
                else
                {
                    game.message = new MessageWindow(game, "Extract STFS Container", "Error: extraction tool not found or bad extract path", Game1.State.ManageFile);
                    game.state = Game1.State.Message;
                }
            }
            else if (source.strings[buttonIndex] == Shared.FileManageStrings["delete"])
            {
                game.toDelete = game.dataFiles[game.selectedDataIndex][game.manageWindow.stringIndex];
                game.message = new MessageWindow(game, "Delete File", "Are you sure you want to delete " + game.toDelete.name + "?", Game1.State.ManageFile, MessageWindow.MessagePrompts.YesNo);
                game.state = Game1.State.Message;
            }
            else if (source.strings[buttonIndex] == Shared.FileManageStrings["video"])
            {
                string[] split = game.localData[game.selectedDataIndex].gamePath.Split("\\");
                string currentDir = "";
                for (int i = 0; i < split.Length - 2; i++)
                {
                    currentDir += split[i] + "\\";
                }
                currentDir += "_EXTRACT";
                currentDir += "\\000C0000\\" + game.dataFiles[game.selectedDataIndex][game.manageWindow.stringIndex].name;
                currentDir = game.GetFilepathString(currentDir, true).Insert(1, ":");
                if (Directory.Exists(currentDir))
                {
                    if (File.Exists(currentDir + "\\default.wmv"))
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo("explorer", currentDir + "\\default.wmv");
                        startInfo.WorkingDirectory = currentDir;
                        Process.Start(startInfo);
                    }
                }
            }
        }
        public void SetupEffects(Game1 game, Window window)
        {
            DataEntry data = game.dataFiles[game.selectedDataIndex][game.manageWindow.stringIndex];
            window.extraSprites.Add(new TextSprite(game.font, "File: " + data.name, 0.375f, new Vector2(1240, 200), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites.Add(new TextSprite(game.font, "Type: " + data.subTitle, 0.375f, new Vector2(1240, 240), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites.Add(new TextSprite(game.font, "Size: " + data.size, 0.375f, new Vector2(1240, 280), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites.Add(new TextSprite(game.font, "Options: " + OptionTypeToString(SubtitleToOptionType(data.subTitle)), 0.375f, new Vector2(1240, 360), Color.FromNonPremultiplied(255, 255, 255, 0)));
            window.extraSprites.Add(new ObjectSprite(data.icon, new Rectangle(300, 412, 256, 256), Color.FromNonPremultiplied(255, 255, 255, 0)));
        }
    }
}
