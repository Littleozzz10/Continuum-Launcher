using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Drawing;

namespace XLCompanion
{
    /// <summary>
    /// Old code from an old project of mine that I kind of abandoned.
    /// Reusing it so that I don't have to go down the rabbit hole of STFS and SVOD headers again
    /// </summary>
    public class STFS
    {
        public class ImportMetadata
        {
            public bool svod;
            public int magic, headerSize, dataFileCount;
            public long contentSize, dataFileSize;
            public string vd;
            // variables for an STFS volume descriptor
            public int blockSeparator, ftBlockCount, ftBlockNumber, allocBlockCount, unallocBlockCount;
            public byte[] topTableHash;
            // variables for an SVOD volume descriptor
            public int blockCacheElementCount, workerThreadProcessor, workerThreadPriority, features, dataBlockCount, dataBlockOffset;
            public byte[] hash;
            // other random variables
            public string displayName, desc;
            // Processes the data from the volume descriptor
            public void ProcessDescriptor()
            {
                if (vd != "")
                {
                    if (!(svod))
                    {
                        blockSeparator = STFS.FromHexInt(vd.Substring(4, 2));
                        ftBlockCount = STFS.FromHexInt(vd.Substring(6, 4));
                        ftBlockNumber = STFS.FromHexInt(vd.Substring(10, 6));
                        // Reading the hash
                        string tempHex = vd.Substring(16, 40);
                        topTableHash = new byte[tempHex.Length / 2];
                        for (int i = 0; i < topTableHash.Length; i++)
                        {
                            topTableHash[i] = Convert.ToByte(tempHex.Substring(i * 2, 2), 16);
                        }
                        allocBlockCount = STFS.FromHexInt(vd.Substring(56, 8));
                        unallocBlockCount = STFS.FromHexInt(vd.Substring(64, 8));
                    }
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
        }
        public FileStream stfsFile;
        public enum HexState
        {
            AsHex, FromHex, BothHexFirst, BothHexLast
        }
        public enum ConvertType
        {
            String, Integer, Long, Short
        }
        private int fileIndex;
        public int thumbSize, metaVer;
        public ImportMetadata data;
        public List<FileInfo> files;
        public List<string> dirs;
        public string contentType, mediaID, version, baseVer, titleID;
        public Image icon;
        public STFS(string stfsFileName)
        {
            // Reading info about the STFS Package
            fileIndex = 0;
            string temp = "";
            data = new ImportMetadata();
            try
            {
                stfsFile = new FileStream(stfsFileName, FileMode.Open);
                // Reading magic
                temp = Decode(4, ConvertType.String, HexState.AsHex, "Failed to decode magic", "Decoded magic in");
                data.magic = FromHexInt(temp);
                SkipDecode(828);
                // Reading header size
                data.headerSize = Convert.ToInt32(Decode(4, ConvertType.Integer, HexState.FromHex, "Failed to decode header size", "Decoded header size in"));
                // Reading content type
                temp = Decode(4, ConvertType.String, HexState.AsHex, "Failed to decode content type", "Decoded content type in");
                contentType = temp;
                // Reading metadata version
                temp = Decode(4, ConvertType.Integer, HexState.AsHex, "Failed to decode metadata version, assuming version 1", "Decoded metadata version in");
                metaVer = Convert.ToInt32(temp);
                // Reading content size
                temp = Decode(8, ConvertType.Long, HexState.FromHex, "Failed to decode content size", "Decoded content size in");
                data.contentSize = Convert.ToInt64(temp);
                // Reading media id
                mediaID = Decode(4, ConvertType.Integer, HexState.AsHex, "Failed to decode media id", "Decoded media id in");
                // Reading version
                version = Decode(4, ConvertType.Integer, HexState.FromHex, "Failed to decode version", "Decoded version in");
                // Reading base version
                baseVer = Decode(4, ConvertType.Integer, HexState.FromHex, "Failed to decode base version", "Decoded base version in");
                // Reading title id
                titleID = Decode(4, ConvertType.Integer, HexState.AsHex, "Failed to decode title id", "Decoded title id in");

                // Everything getting skipped here is currently not needed for Continuum, but it may
                // be useful in the future somehow, so I'm leaving the code here commented out

                /*
                // Reading platform
                temp = Decode(1, ConvertType.Integer, HexState.FromHex, "Failed to decode platform", "Decoded platform in");
                if (temp == "2")
                {
                    platLabel.Text = platLabel.Text + temp + " (Xbox 360)";
                }
                else if (temp == "4")
                {
                    platLabel.Text = platLabel.Text + temp + " (PC)";
                }
                else
                {
                    platLabel.Text = platLabel.Text + temp;
                }
                // Reading executable type
                exeTypeLabel.Text = exeTypeLabel.Text + Decode(1, ConvertType.Integer, HexState.FromHex, "Failed to decode executable type", "Decoded executable type in");
                // Reading disc number
                discNumLabel.Text = discNumLabel.Text + Decode(1, ConvertType.Integer, HexState.FromHex, "Failed to decode disc number", "Decoded disc number in");
                // Reading disc in set
                discSetLabel.Text = discSetLabel.Text + Decode(1, ConvertType.Integer, HexState.FromHex, "Failed to decode disc in set", "Decoded disc in set in");
                // Reading save game id
                saveGameIDLabel.Text = saveGameIDLabel.Text + Decode(4, ConvertType.Integer, HexState.AsHex, "Failed to decode save game id", "Decoded save game id in");
                // Reading console id
                consoleIDLabel.Text = consoleIDLabel.Text + Decode(5, ConvertType.Integer, HexState.AsHex, "Failed to decode console id", "Decoded console id in");
                // Reading profile id
                profileIDLabel.Text = profileIDLabel.Text + Decode(8, ConvertType.Integer, HexState.AsHex, "Failed to decode profile id", "Decoded profile id in");
                // Reading volume decsriptor
                data.vd = Decode(36, ConvertType.String, HexState.AsHex, "Failed to decode volume descriptor", "Decoded volume descriptor in");
                // Reading data file count
                temp = Decode(4, ConvertType.Integer, HexState.FromHex, "Failed to decode data file count", "Decoded data file count in");
                dataFileCountLabel.Text = dataFileCountLabel.Text + temp;
                data.dataFileCount = Convert.ToInt32(temp);
                // Reading data file combined size
                temp = Decode(8, ConvertType.Long, HexState.FromHex, "Failed to decode data file combined size", "Decoded data file combined size in");
                dataFileSizeLabel.Text = dataFileSizeLabel.Text + ConvertDataSize(temp);
                data.dataFileSize = Convert.ToInt64(temp);
                // Reading descriptor
                temp = Decode(4, ConvertType.Integer, HexState.FromHex, "Failed to decode descriptor", "Decoded descriptor in");
                if (temp == "0")
                {
                    descTypeLabel.Text = descTypeLabel.Text + temp + " (STFS)";
                }
                else if (temp == "1")
                {
                    descTypeLabel.Text = descTypeLabel.Text + temp + " (SVOD)";
                    data.svod = true;
                }
                else
                {
                    descTypeLabel.Text = descTypeLabel.Text + temp;
                }
                SkipDecode(80);
                // Reading device id
                deviceIDLabel.Text = deviceIDLabel.Text + Decode(14, ConvertType.Integer, HexState.AsHex, "Failed to decode device id", "Decoded device id in");
                // Reading display name
                temp = Decode(80, ConvertType.String, HexState.FromHex, "Failed to decode display name", "Decoded display name in");
                //data.displayName = temp;
                displayNameLabel.Text = displayNameLabel.Text + temp;
                SkipDecode(2176);
                // Reading display description (Buggy)
                data.desc = Decode(308, ConvertType.String, HexState.FromHex, "Failed to decode display description", "Decoded display description in");
                descLabel.Text = descLabel.Text + data.desc;
                SkipDecode(2050);
                // Reading publisher name
                pubNameLabel.Text = pubNameLabel.Text + Decode(80, ConvertType.String, HexState.FromHex, "Failed to decode publisher name", "Decoded publisher name in");
                */

                SkipDecode(4861);

                // Reading title name
                data.displayName = Decode(176, ConvertType.String, HexState.FromHex, "Failed to decode title name", "Decoded title name in");
                SkipDecode(1);
                // Reading thumbnail size
                thumbSize = Convert.ToInt32(Decode(4, ConvertType.Integer, HexState.FromHex, "Failed to decode thumbnail image size", "Decoded thumbnail image size in"));
                SkipDecode(4);
                //data.ProcessDescriptor();
                icon = FromHexImage(thumbSize * thumbSize);
                stfsFile.Close();
            }
            catch (FileNotFoundException)
            {

            }
            catch (IOException)
            {

            }
        }
        // Converts hex values to string
        public static string FromHexString(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            // Eliminating leading spaces
            int leads = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                if (bytes[i] == 0)
                {
                    leads++;
                }
                else
                {
                    break;
                }
            }
            if (leads > 0)
            {
                var newBytes = new byte[bytes.Length - leads];
                for (int i = bytes.Length - 1; i >= leads; i--)
                {
                    newBytes[i - leads] = bytes[i];
                }
                bytes = newBytes;
            }
            // Getting rid of weird characters
            List<byte> tempList = bytes.ToList();
            for (int i = 0; i < tempList.Count; i++)
            {
                if (tempList[i] == 0)
                {
                    tempList.RemoveAt(i);
                    i--;
                }
            }
            bytes = tempList.ToArray();
            return Encoding.ASCII.GetString(bytes);
        }
        // Converts hex values to long
        public static long FromHexLong(string hexString)
        {
            return long.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
        }
        // Converts hex values to int
        public static int FromHexInt(string hexString)
        {
            return int.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
        }
        public static short FromHexShort(string hexString)
        {
            return short.Parse(hexString, System.Globalization.NumberStyles.HexNumber);
        }
        // Converts hex values to image
        public Image FromHexImage(int byteNum)
        {
            try
            {
                var bytes = new byte[byteNum];
                stfsFile.Read(bytes, 0, byteNum);
                MemoryStream stream = new MemoryStream(bytes);
                var img = Image.FromStream(stream);
                stream.Dispose();
                stream.Close();
                GC.Collect();
                return img;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return null;
        }
        // Converts to a data size
        public static string ConvertDataSize(string size)
        {
            double num = Convert.ToDouble(size);
            // Bytes
            if (num < 1024)
            {
                return "" + num + " B";
            }
            // Kilobytes
            else if (num < (1024 * 1024))
            {
                return "" + (num / 1024) + " KB";
            }
            // Megabytes
            else if (num < (1024 * 1024 * 1024))
            {
                return "" + Math.Round((num / 1024 / 1024), 3) + " MB";
            }
            // Gigabytes
            else if (num < (long)(1024 * 1024 * 1024) * 1024)
            {
                return "" + Math.Round((num / 1024 / 1024 / 1024), 3) + " GB";
            }
            // Terabytes
            else if (num < (long)(1024 * 1024 * 1024) * 1024 * 1024)
            {
                return "" + Math.Round((num / 1024 / 1024 / 1024 / 1024), 3) + " TB";
            }
            return size;
        }
        // Function to read bytes from the file and return them in a string,
        // converting from hex if asked
        public string Decode(uint bytes, ConvertType cType, HexState asHex, string errorMessage, string successMessage)
        {
            try
            {
                string decode = "";
                for (int i = 0; i < bytes; i++)
                {
                    decode = decode + string.Format("{0:X2}", stfsFile.ReadByte());
                    fileIndex++;
                }
                if (asHex == HexState.FromHex)
                {
                    if (cType == ConvertType.String)
                    {
                        return FromHexString(decode);
                    }
                    else if (cType == ConvertType.Long)
                    {
                        return "" + FromHexLong(decode);
                    }
                    else if (cType == ConvertType.Short)
                    {
                        return "" + FromHexShort(decode);
                    }
                    return "" + FromHexInt(decode);
                }
                else if (asHex == HexState.BothHexFirst)
                {
                    if (cType == ConvertType.String)
                    {
                        return decode + " (" + FromHexString(decode) + ")";
                    }
                    else if (cType == ConvertType.Long)
                    {
                        return decode + " (" + FromHexLong(decode) + ")";
                    }
                    else if (cType == ConvertType.Short)
                    {
                        return decode + " (" + FromHexShort(decode) + ")";
                    }
                    return decode + " (" + FromHexInt(decode) + ")";
                }
                else if (asHex == HexState.BothHexLast)
                {
                    if (cType == ConvertType.String)
                    {
                        return FromHexString(decode) + " (" + decode + ")";
                    }
                    else if (cType == ConvertType.Long)
                    {
                        return FromHexLong(decode) + " (" + decode + ")";
                    }
                    else if (cType == ConvertType.Short)
                    {
                        return FromHexShort(decode) + " (" + decode + ")";
                    }
                    return FromHexInt(decode) + " (" + decode + ")";
                }
                return decode;
            }
            catch (Exception e)
            {

            }
            return "";
        }
        // Function used to 'skip' data in the file not immedietely needed
        public void SkipDecode(int bytes)
        {
            stfsFile.Seek(bytes, SeekOrigin.Current);
            fileIndex += bytes;
        }
        public void SeekDecode(int offset)
        {
            stfsFile.Seek(offset, SeekOrigin.Begin);
            fileIndex = offset;
        }
        // Returns a string of the Content Type provided from hex
        public string ContentTypeAsString(string ctHex)
        {
            switch (ctHex)
            {
                case ("00000001"):
                    return "Saved Game";
                case ("00000002"):
                    return "Marketplace Content";
                case ("00000003"):
                    return "Publisher";
                case ("00001000"):
                    return "Xbox 360 Title";
                case ("00002000"):
                    return "IPTV Pause Buffer";
                case ("00004000"):
                    return "Installed Game";
                case ("00005000"):
                    return "Xbox Original Game/Xbox Title";
                case ("00007000"):
                    return "Game on Demand";
                case ("00009000"):
                    return "Avatar Item";
                case ("00010000"):
                    return "Profile";
                case ("00020000"):
                    return "Gamer Picture";
                case ("00030000"):
                    return "Theme";
                case ("00040000"):
                    return "Cache File";
                case ("00050000"):
                    return "Storage Download";
                case ("00060000"):
                    return "Xbox Saved Game";
                case ("00070000"):
                    return "Xbox Download";
                case ("00080000"):
                    return "Game Demo";
                case ("00090000"):
                    return "Video";
                case ("000A0000"):
                    return "Game Title";
                case ("000B0000"):
                    return "Installer";
                case ("000C0000"):
                    return "Game Trailer";
                case ("000D0000"):
                    return "Arcade Title";
                case ("000E0000"):
                    return "XNA";
                case ("000F0000"):
                    return "License Store";
                case ("00100000"):
                    return "Movie";
                case ("00200000"):
                    return "TV";
                case ("00300000"):
                    return "Music Video";
                case ("00400000"):
                    return "Game Video";
                case ("00500000"):
                    return "Podcast Video";
                case ("00600000"):
                    return "Viral Video";
                case ("02000000"):
                    return "Community Game";
            }
            return "";
        }
    }
}
