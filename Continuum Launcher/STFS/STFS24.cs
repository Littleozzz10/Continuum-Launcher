using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;

// New STFS code from another project, an Xbox 360 backup tool.
// Much better than the old STFS code Continuum currently uses.
// This code should eventually replace the old STFS code.
namespace STFS
{
    using DataType = HexData.DataType;
    public class HexData
    {
        string name;
        long hexAddress;
        int byteLength;
        private FileStream reader;
        private byte[] data;
        public enum DataType
        {
            UTF7, UTF8, Unicode, ASCII, Int, UInt, Long, Short, Double, Raw, Byte, Image
        }
        public DataType type;
        public HexData(string name, long hexAddress, int byteLength, DataType type, FileStream reader) 
        {
            this.name = name;
            this.hexAddress = hexAddress;
            this.byteLength = byteLength;
            this.type = type;
            this.reader = reader;
        }
        private void Read()
        {
            reader.Seek(hexAddress, SeekOrigin.Begin);
            data = new byte[byteLength];
            reader.ReadExactly(data, 0, byteLength);
            STFS24.Print(name + " (" + String.Format("0x{0:X}", hexAddress) + " + " + String.Format("0x{0:X}", byteLength) + ") interpreted");
        }
        public string AsUTF7()
        {
            Read();
            if (data == null) return "";
            return Encoding.UTF7.GetString(data);
        }
        public string AsUTF8()
        {
            Read();
            if (data == null) return "";
            return Encoding.UTF8.GetString(data);
        }
        public string AsUnicode()
        {
            Read();
            if (data == null) return "";
            return Encoding.Unicode.GetString(data);
        }
        public string AsASCII()
        {
            Read();
            if (data == null) return "";
            return Encoding.ASCII.GetString(data);
        }
        public long AsLong()
        {
            Read();
            return BitConverter.ToInt64(data);
        }
        public long AsLongLE()
        {
            Read();
            data = data.Reverse().ToArray();
            return BitConverter.ToInt64(data);
        }
        public int AsInt()
        {
            Read();
            if (data.Length == 3)
            {
                byte[] newData = [0, data[2], data[1], data[0]];
                data = newData;
            }
            return BitConverter.ToInt32(data);
        }
        public int AsIntLE()
        {
            Read();
            data = data.Reverse().ToArray();
            return BitConverter.ToInt32(data);
        }
        public uint AsUInt()
        {
            Read();
            // Fixing a bug when data equaled {0}
            if (data.Length == 1 && data[0] == 0)
            {
                return 0;
            }
            else if (data.Length == 3)
            {
                byte[] newData = [0, data[2], data[1], data[0]];
                data = newData;
            }
            return BitConverter.ToUInt32(data);
        }
        public uint AsUIntLE()
        {
            Read();
            data = data.Reverse().ToArray();
            // Fixing a bug when data equaled {0}
            if (data.Length == 1 && data[0] == 0)
            {
                return 0;
            }
            else if (data.Length == 3)
            {
                byte[] newData = [data[2], data[1], data[0], 0];
                data = newData;
            }
            return BitConverter.ToUInt32(data);
        }
        public short AsShort()
        {
            Read();
            return BitConverter.ToInt16(data);
        }
        public double AsDouble()
        {
            Read();
            return BitConverter.ToDouble(data);
        }
        public byte[] AsRaw()
        {
            Read();
            return data;
        }
        public byte[] AsRawLE()
        {
            Read();
            data = data.Reverse().ToArray();
            return data;
        }
        public byte AsByte()
        {
            Read();
            return data[0];
        }
    }
    public class XSignature
    {
        public enum SignatureType { CON, LIVE }
        public SignatureType Type;
        private FileStream reader;
        private Dictionary<string, HexData> signatureData;
        // CON signature cache variables
        private byte[]? certSize, ownerConsoleID, publicExponent, publicMod, certSignature, signature;
        private string? ownerConsolePartNum, dateGenerated;
        public enum ConsoleType { Devkit = 1, Retail = 2 }
        private ConsoleType? ownerConsoleType;
        // LIVE/PIRS signature cache values
        private byte[]? packageSignature, padding;
        public XSignature(string? magic, FileStream reader)
        {
            if (magic == "CON ")
            {
                Type = SignatureType.CON;
            }
            else
            {
                Type = SignatureType.LIVE;
            }
            this.reader = reader;
            signatureData = new Dictionary<string, HexData>();
            XSigInit();
        }
        private void XSigInit()
        {
            // CON signature values
            signatureData.Add("certSize", new HexData("Public Key Certificate Size", 0x04, 0x02, DataType.Raw, reader));
            signatureData.Add("ownerConsoleID", new HexData("Certificate Owner Console ID", 0x06, 0x05, DataType.Raw, reader));
            signatureData.Add("ownerConsolePartNum", new HexData("Certificate Owner Console Part Number", 0x0B, 0x14, DataType.ASCII, reader));
            signatureData.Add("ownerConsoleType", new HexData("Certificate Owner Console Type", 0x1F, 0x01, DataType.Byte, reader));
            signatureData.Add("dateGenerated", new HexData("Certificate Date of Generation", 0x20, 0x08, DataType.ASCII, reader));
            signatureData.Add("publicExponent", new HexData("Public Exponent", 0x28, 0x04, DataType.Raw, reader));
            signatureData.Add("publicMod", new HexData("Public Modulus", 0x2C, 0x80, DataType.Raw, reader));
            signatureData.Add("certSignature", new HexData("Certificate Signature", 0xAC, 0x100, DataType.Raw, reader));
            signatureData.Add("signature", new HexData("Signature", 0x01AC, 0x80, DataType.Raw, reader));
            // LIVE/PIRS signature values
            signatureData.Add("packageSignature", new HexData("Package Signature", 0x04, 0x100, DataType.Raw, reader));
            signatureData.Add("padding", new HexData("Signature Padding", 0x0104, 0x128, DataType.Raw, reader));
        }
        // Only for CON signatures (SignatureType = 0)
        public byte[]? GetCertificateSize()
        {
            if (Type == (SignatureType)1) return null;
            if (certSize == null) certSize = signatureData["certSize"].AsRaw();
            return certSize;
        }
        // Only for CON signatures (SignatureType = 0)
        public byte[]? GetOwnerConsoleID()
        {
            if (Type == (SignatureType)1) return null;
            if (ownerConsoleID == null) ownerConsoleID = signatureData["ownerConsoleID"].AsRaw();
            return ownerConsoleID;
        }
        // Only for CON signatures (SignatureType = 0)
        public byte[]? GetPublicExponent()
        {
            if (Type == (SignatureType)1) return null;
            if (publicExponent == null) publicExponent = signatureData["publicExponent"].AsRaw();
            return publicExponent;
        }
        // Only for CON signatures (SignatureType = 0)
        public byte[]? GetPublicModulus()
        {
            if (Type == (SignatureType)1) return null;
            if (publicMod == null) publicMod = signatureData["publicMod"].AsRaw();
            return publicMod;
        }
        // Only for CON signatures (SignatureType = 0)
        public byte[]? GetCertificateSignature()
        {
            if (Type == (SignatureType)1) return null;
            if (certSignature == null) certSignature = signatureData["certSignature"].AsRaw();
            return certSignature;
        }
        // Only for CON signatures (SignatureType = 0)
        public byte[]? GetSignature()
        {
            if (Type == (SignatureType)1) return null;
            if (signature == null) signature = signatureData["signature"].AsRaw();
            return signature;
        }
        // Only for CON signatures (SignatureType = 0)
        public string? GetOwnerConsolePartNum()
        {
            if (Type == (SignatureType)1) return null;
            if (ownerConsolePartNum == null) ownerConsolePartNum = signatureData["ownerConsolePartNum"].AsASCII();
            return ownerConsolePartNum;
        }
        // Only for CON signatures (SignatureType = 0)
        public string? GetDateGenerated()
        {
            if (Type == (SignatureType)1) return null;
            if (dateGenerated == null) dateGenerated = signatureData["dateGenerated"].AsASCII();
            return dateGenerated;
        }
        // Only for CON signatures (SignatureType = 0)
        public ConsoleType? GetOwnerConsoleType()
        {
            if (Type == (SignatureType)1) return null;
            if (ownerConsoleType == null) ownerConsoleType = (ConsoleType?)signatureData["ownerConsoleType"].AsByte();
            return ownerConsoleType;
        }
        // Only for LIVE/PIRS signatures (SignatureType = 1)
        public byte[]? GetPackageSignature()
        {
            if (Type == (SignatureType)0) return null;
            if (packageSignature == null) packageSignature = signatureData["packageSignature"].AsRaw();
            return packageSignature;
        }
        // Only for LIVE/PIRS signatures (SignatureType = 1)
        public byte[]? GetPadding()
        {
            if (Type == (SignatureType)0) return null;
            if (padding == null) padding = signatureData["padding"].AsRaw();
            return padding;
        }
        public void ClearData()
        {
            certSize = null;
            ownerConsoleID = null;
            publicExponent = null;
            publicMod = null;
            certSignature = null;
            signature = null;
            ownerConsolePartNum = null;
            dateGenerated = null;
            ownerConsoleType = null;
            packageSignature = null;
            padding = null;
        }
    }
    public class XMetadata
    {
        public enum XMetadataType { NotSpecified = 0, V1 = 2, V2 = 4 }
        public XMetadataType Type;
        public enum XPlatform { None = 0, Xbox360 = 2, PC = 4 }
        private FileStream reader;
        private Dictionary<string, HexData> metadata;
        // All version cache variables
        private XLicenseEntry[]? licenseData;
        private byte[]? headerHash, mediaID, titleID, saveGameID, consoleID, profileID, reserved, thumbnailImage, titleThumbnailImage;
        private uint? headerSize;
        private int? metadataVersion, version, baseVersion, dataFileCount;
        private long? contentSize, dataFileSize;
        private XPlatform? platform;
        private byte? executableType, discNumber, discInSet;
        private XVolumeDescriptor? vd;
        private XVolumeDescriptor.VDType? descriptorType;
        // Version 1 cache variables
        private byte[]? contentType, padding, deviceID;
        private string[] displayName, displayDescription;
        private string? publisherName, titleName;
        private XTransferFlags? transferFlags;
        private int? thumbnailImageSize, titleThumbnailImageSize;
        // Version 2 cache variables
        private byte[]? seriesID, seasonID, v2Padding;
        private short? seasonNumber, episodeNumber;
        private string? additionalDisplayNames, additionalDisplayDescriptions;
        public XMetadata(FileStream reader)
        {
            this.reader = reader;
            metadata = new Dictionary<string, HexData>();
            MetadataInit();
        }
        private void MetadataInit()
        {
            //metadata.Add("licenseData", new HexData("Licensing Data", 0x022C, 0x0100, DataType.Raw, reader));
            metadata.Add("headerHash", new HexData("Header SHA1 Hash", 0x032C, 0x14, DataType.Raw, reader));
            metadata.Add("headerSize", new HexData("Header Size", 0x0340, 0x04, DataType.UInt, reader));
            metadata.Add("contentType", new HexData("Content Type", 0x0344, 0x04, DataType.Raw, reader));
            metadata.Add("metadataVersion", new HexData("Metadata Version", 0x0348, 0x04, DataType.Int, reader));
            metadata.Add("contentSize", new HexData("Content Size", 0x034C, 0x08, DataType.Long, reader));
            metadata.Add("mediaID", new HexData("Media ID", 0x0354, 0x04, DataType.Raw, reader));
            metadata.Add("version", new HexData("Version", 0x0358, 0x04, DataType.Int, reader));
            metadata.Add("baseVersion", new HexData("Base Version", 0x035C, 0x04, DataType.Int, reader));
            metadata.Add("titleID", new HexData("Title ID", 0x0360, 0x04, DataType.Raw, reader));
            metadata.Add("platform", new HexData("Platform", 0x0364, 0x01, DataType.UInt, reader));
            metadata.Add("executableType", new HexData("Executable Type", 0x0365, 0x01, DataType.Byte, reader));
            metadata.Add("discNumber", new HexData("Disc Number", 0x0366, 0x01, DataType.Byte, reader));
            metadata.Add("discInSet", new HexData("Disc in Set", 0x0367, 0x01, DataType.Byte, reader));
            metadata.Add("saveGameID", new HexData("Save Game ID", 0x0368, 0x04, DataType.Raw, reader));
            metadata.Add("consoleID", new HexData("Console ID", 0x036C, 0x05, DataType.Raw, reader));
            metadata.Add("profileID", new HexData("Profile ID", 0x0371, 0x08, DataType.Raw, reader));
            //metadata.Add("vd", new HexData("Volume Descriptor", 0x0379, 0x24, DataType.Raw, reader));
            metadata.Add("dataFileCount", new HexData("Data File Count", 0x039D, 0x04, DataType.Int, reader));
            metadata.Add("dataFileCombinedSize", new HexData("Data File Combined Size", 0x03A1, 0x08, DataType.Long, reader));
            metadata.Add("descriptorType", new HexData("Volume Descriptor Type", 0x03A9, 0x04, DataType.UInt, reader));
            metadata.Add("reserved", new HexData("Metadata Reserved Data", 0x03AD, 0x04, DataType.Raw, reader));
            // Version 1 values
            metadata.Add("padding", new HexData("Metadata V1 Padding", 0x03B1, 0x4C, DataType.Raw, reader));
            metadata.Add("deviceID", new HexData("Device ID", 0x03FD, 0x14, DataType.Raw, reader));
            metadata.Add("displayName", new HexData("Display Name", 0x0411, 0x0900, DataType.UTF8, reader));
            metadata.Add("displayDescription", new HexData("Display Description", 0x0D11, 0x0900, DataType.UTF8, reader));
            metadata.Add("publisherName", new HexData("Publisher Name", 0x1611, 0x80, DataType.UTF8, reader));
            metadata.Add("titleName", new HexData("Title Name", 0x1691, 0x80, DataType.UTF8, reader));
            metadata.Add("transferFlags", new HexData("Transfer Flags", 0x1711, 0x01, DataType.Byte, reader));
            metadata.Add("thumbnailImageSize", new HexData("Thumbnail Image Size", 0x1712, 0x04, DataType.Int, reader));
            metadata.Add("titleThumbnailImageSize", new HexData("Title Thumbnail Image Size", 0x1716, 0x04, DataType.Int, reader));
            // Version 2 values
            metadata.Add("seriesID", new HexData("Series ID", 0x03B1, 0x10, DataType.Raw, reader));
            metadata.Add("seasonID", new HexData("Season ID", 0x03C1, 0x10, DataType.Raw, reader));
            metadata.Add("seasonNumber", new HexData("Season Number", 0x03D1, 0x02, DataType.Short, reader));
            metadata.Add("episodeNumber", new HexData("Episode Number", 0x03D3, 0x02, DataType.Short, reader));
            metadata.Add("v2padding", new HexData("Metadata V2 Padding", 0x03D5, 0x28, DataType.Raw, reader));
            metadata.Add("additionalDisplayNames", new HexData("Additional Display Names", 0x541A, 0x0300, DataType.UTF8, reader));
            metadata.Add("additionalDisplayDescriptions", new HexData("Additional Display Descriptions", 0x941A, 0x0300, DataType.UTF8, reader));

            // Reading metadata type
            int? ver = GetMetadataVersion();
            if (ver == null)
            {
                ver = 0; // Assuming V1 if null, as it is more common
            }
            Type = (XMetadataType)ver;
        }
        public XVolumeDescriptor? ReturnVolumeDescriptor()
        {
            if (vd == null)
            {
                long address = 0x0379;
                if (descriptorType == null)
                {
                    GetDescriptorType();
                }
                vd = new XVolumeDescriptor(reader, address, descriptorType);
            }
            return vd;
        }
        public XLicenseEntry[]? GetLicenseData()
        {
            if (licenseData == null)
            {
                long address = 0x022C;
                licenseData = new XLicenseEntry[256];
                for (int i = 0; i < licenseData.Length; i++)
                {
                    licenseData[i] = new XLicenseEntry(reader, address);
                    address += 0x10;
                }
            }
            return licenseData;
        }
        public byte[]? GetHeaderHash()
        {
            if (headerHash == null) headerHash = metadata["headerHash"].AsRaw();
            return headerHash;
        }
        public uint? GetHeaderSize()
        {
            if (headerSize == null) headerSize = metadata["headerSize"].AsUIntLE();
            return headerSize;
        }
        public XContentType? GetContentType()
        {
            if (contentType == null) contentType = metadata["contentType"].AsRawLE();
            return (XContentType)BitConverter.ToInt32(contentType);
        }
        public byte[]? GetContentTypeAsBytes()
        {
            if (contentType == null) contentType = metadata["contentType"].AsRawLE();
            return contentType.Reverse().ToArray();
        }
        public int? GetMetadataVersion()
        {
            if (metadataVersion == null) metadataVersion = metadata["metadataVersion"].AsIntLE();
            return metadataVersion;
        }
        public long? GetContentSize()
        {
            if (contentSize == null) contentSize = metadata["contentSize"].AsLongLE();
            return contentSize;
        }
        public byte[]? GetMediaID()
        {
            if (mediaID == null) mediaID = metadata["mediaID"].AsRaw();
            return mediaID;
        }
        public int? GetVersion()
        {
            if (version == null) version = metadata["version"].AsIntLE();
            return version;
        }
        public int? GetBaseVersion()
        {
            if (baseVersion == null) baseVersion = metadata["baseVersion"].AsIntLE();
            return baseVersion;
        }
        public byte[]? GetTitleID()
        {
            if (titleID == null) titleID = metadata["titleID"].AsRaw();
            return titleID;
        }
        public XPlatform? GetPlatform()
        {
            if (platform == null) platform = (XPlatform)metadata["platform"].AsUInt();
            return platform;
        }
        public byte? GetExecutableType()
        {
            if (executableType == null) executableType = metadata["executableType"].AsByte();
            return executableType;
        }
        public byte? GetDiscNumber()
        {
            if (discNumber == null) discNumber = metadata["discNumber"].AsByte();
            return discNumber;
        }
        public byte? GetDiscInSet()
        {
            if (discInSet == null) discInSet = metadata["discInSet"].AsByte();
            return discInSet;
        }
        public byte[]? GetSaveGameID()
        {
            if (saveGameID == null) saveGameID = metadata["saveGameID"].AsRaw();
            return saveGameID;
        }
        public byte[]? GetConsoleID()
        {
            if (consoleID == null) consoleID = metadata["consoleID"].AsRaw();
            return consoleID;
        }
        public byte[]? GetProfileID()
        {
            if (profileID == null) profileID = metadata["profileID"].AsRaw();
            return profileID;
        }
        public XVolumeDescriptor.VDType? GetDescriptorType()
        {
            if (descriptorType == null) descriptorType = (XVolumeDescriptor.VDType)metadata["descriptorType"].AsUIntLE();
            return descriptorType;
        }
        public int? GetDataFileCount()
        {
            if (dataFileCount == null) dataFileCount = metadata["dataFileCount"].AsIntLE();
            return dataFileCount;
        }
        public long? GetDataFileSize()
        {
            if (dataFileSize == null) dataFileSize = metadata["dataFileCombinedSize"].AsLongLE();
            return dataFileSize;
        }
        public byte[]? GetReserved()
        {
            if (reserved == null) reserved = metadata["reserved"].AsRaw();
            return reserved;
        }
        // Only for version 1 metadata
        public byte[]? GetPaddingV1()
        {
            if (Type == XMetadataType.V2) return null;
            if (padding == null) padding = metadata["padding"].AsRaw();
            return padding;
        }
        // Only for version 1 metadata
        public byte[]? GetDeviceID()
        {
            if (Type == XMetadataType.V2) return null;
            if (deviceID == null) deviceID = metadata["deviceID"].AsRaw();
            return deviceID;
        }
        // Only for version 1 metadata
        public string[]? GetDisplayName()
        {
            if (Type == XMetadataType.V2) return null;
            if (displayName == null)
            {
                string names = metadata["displayName"].AsUTF8();
                if (names != null) 
                {
                    displayName = new string[9];
                    for (int i = 0; i < 9; i++)
                    {
                        int length = names.Length / 9;
                        displayName[i] = names.Substring(i * length, length);
                    }
                }
            }
            return displayName;
        }
        // Only for version 1 metadata
        public string[]? GetDisplayDescription()
        {
            if (Type == XMetadataType.V2) return null;
            if (displayDescription == null)
            {
                string names = metadata["displayDescription"].AsUTF8();
                if (names != null)
                {
                    displayDescription = new string[9];
                    for (int i = 0; i < 9; i++)
                    {
                        int length = names.Length / 9;
                        displayDescription[i] = names.Substring(i * length, length);
                    }
                }
            }
            return displayDescription;
        }
        // Only for version 1 metadata
        public string? GetPublisherName()
        {
            if (Type == XMetadataType.V2) return null;
            if (publisherName == null) publisherName = metadata["publisherName"].AsUTF8();
            return publisherName;
        }
        // Only for version 1 metadata
        public string? GetTitleName()
        {
            if (Type == XMetadataType.V2) return null;
            if (titleName == null) titleName = metadata["titleName"].AsUTF8();
            return titleName;
        }
        // Only for version 1 metadata
        public XTransferFlags? GetTransferFlags()
        {
            if (Type == XMetadataType.V2) return null;
            if (transferFlags == null) transferFlags = (XTransferFlags)metadata["transferFlags"].AsByte();
            return transferFlags;
        }
        // Only for version 1 metadata
        public int? GetThumbnailImageSize()
        {
            if (Type == XMetadataType.V2) return null;
            if (thumbnailImageSize == null) thumbnailImageSize = metadata["thumbnailImageSize"].AsIntLE();
            return thumbnailImageSize;
        }
        // Only for version 1 metadata
        public int? GetTitleThumbnailImageSize()
        {
            if (Type == XMetadataType.V2) return null;
            if (titleThumbnailImageSize == null) titleThumbnailImageSize = metadata["titleThumbnailImageSize"].AsIntLE();
            return titleThumbnailImageSize;
        }
        // Only for version 2 metadata
        public byte[]? GetSeriesID()
        {
            if (Type == XMetadataType.V1) return null;
            if (seriesID == null) seriesID = metadata["seriesID"].AsRaw();
            return seriesID;
        }
        // Only for version 2 metadata
        public byte[]? GetSeasonID()
        {
            if (Type == XMetadataType.V1) return null;
            if (seasonID == null) seasonID = metadata["seasonID"].AsRaw();
            return seasonID;
        }
        // Only for version 2 metadata
        public short? GetSeasonNumber()
        {
            if (Type == XMetadataType.V1) return null;
            if (seasonNumber == null) seasonNumber = metadata["seasonNumber"].AsShort();
            return seasonNumber;
        }
        // Only for version 2 metadata
        public short? GetEpisodeNumber()
        {
            if (Type == XMetadataType.V1) return null;
            if (episodeNumber == null) episodeNumber = metadata["episodeNumber"].AsShort();
            return episodeNumber;
        }
        // Only for version 2 metadata
        public byte[]? GetPaddingV2()
        {
            if (Type == XMetadataType.V1) return null;
            if (v2Padding == null) v2Padding = metadata["v2padding"].AsRaw();
            return v2Padding;
        }
        // Only for version 2 metadata
        public string? GetAdditionalDisplayNames()
        {
            if (Type == XMetadataType.V1) return null;
            if (additionalDisplayNames == null) additionalDisplayNames = metadata["additionalDisplayNames"].AsUTF8();
            return additionalDisplayNames;
        }
        // Only for version 2 metadata
        public string? GetAdditionalDisplayDescriptions()
        {
            if (Type == XMetadataType.V1) return null;
            if (additionalDisplayDescriptions == null) additionalDisplayDescriptions = metadata["additionalDisplayDescriptions"].AsUTF8();
            return additionalDisplayDescriptions;
        }
        public void ClearData()
        {

        }
    }
    public class XLicenseEntry
    {
        private FileStream reader;
        private Dictionary<string, HexData> licenseData;
        private long address;
        // Cache variables
        private long? licenseID;
        private int? licenseBits, licenseFlags;
        public XLicenseEntry(FileStream reader, long address)
        {
            this.reader = reader;
            this.address = address;
            licenseData = new Dictionary<string, HexData>();
            LicenseInit();
        }
        private void LicenseInit()
        {
            licenseData.Add("licenseID", new HexData("License ID", address + 0x00, 0x08, DataType.Long, reader));
            licenseData.Add("licenseBits", new HexData("License Bits", address + 0x08, 0x04, DataType.Int, reader));
            licenseData.Add("licenseFlags", new HexData("License Flags", address + 0x0C, 0x04, DataType.Int, reader));
        }
        public long? GetLicenseID()
        {
            if (licenseID == null) licenseID = licenseData["licenseID"].AsLong();
            return licenseID;
        }
        public int? GetLicenseBits()
        {
            if (licenseBits == null) licenseBits = licenseData["licenseBits"].AsInt();
            return licenseBits;
        }
        public int? GetLicenseFlags()
        {
            if (licenseFlags == null) licenseFlags = licenseData["licenseFlags"].AsInt();
            return licenseFlags;
        }
        public void ClearData()
        {
            licenseID = null;
            licenseBits = null;
            licenseFlags = null;
        }
    }
    public class XVolumeDescriptor
    {
        public enum VDType { STFS, SVOD }
        public VDType Type;
        private FileStream reader;
        private Dictionary<string, HexData> descriptorData;
        private long address;
        // Cache variables
        private byte? size;
        // STFS specific cache variables
        private byte? reserved, blockSeparation;
        private short? fileTableBlockCount;
        private int? fileTableBlockNumber, totalAllocatedBlockCount, totalUnallocatedBlockCount;
        private byte[]? topHashTableHash;
        // SVOD specific cache variables
        private byte? blockCacheElementCount, workerThreadProcessor, workerThreadPriority, deviceFeatures;
        private byte[]? hash, padding;
        private uint? dataBlockCount, dataBlockOffset;
        public XVolumeDescriptor(FileStream reader, long address, VDType? type)
        {
            this.reader = reader;
            this.address = address;
            descriptorData = new Dictionary<string, HexData>();
            // Handling null type (Assumes STFS)
            if (type == null)
            {
                type = VDType.STFS;
            }
            Type = (VDType)type;
            DescriptorInit();
        }
        private void DescriptorInit()
        {
            descriptorData.Add("size", new HexData("Volume Descriptor Size", address + 0x00, 0x01, DataType.Byte, reader));
            // STFS specific data
            descriptorData.Add("reserved", new HexData("Volume Descriptor Reserved", address + 0x01, 0x01, DataType.Byte, reader));
            descriptorData.Add("blockSeparation", new HexData("Block Separation", address + 0x02, 0x01, DataType.Byte, reader));
            descriptorData.Add("fileTableBlockCount", new HexData("File Table Block Count", address + 0x03, 0x02, DataType.Short, reader));
            descriptorData.Add("fileTableBlockNumber", new HexData("File Table Block Number", address + 0x05, 0x03, DataType.Int, reader));
            descriptorData.Add("topHashTableHash", new HexData("Top Hash Table Hash", address + 0x08, 0x14, DataType.Raw, reader));
            descriptorData.Add("totalAllocatedBlockCount", new HexData("Total Allocated Block Count", address + 0x1C, 0x04, DataType.Int, reader));
            descriptorData.Add("totalUnallocatedBlockCount", new HexData("Total Unallocated Block Count", address + 0x20, 0x04, DataType.Int, reader));
            // SVOD specific data
            descriptorData.Add("blockCacheElementCount", new HexData("BlockCacheElementCount", address + 0x01, 0x01, DataType.Byte, reader));
            descriptorData.Add("workerThreadProcessor", new HexData("Worked Thread Processor", address + 0x02, 0x01, DataType.Byte, reader));
            descriptorData.Add("workerThreadPriority", new HexData("Worker Thread Priority", address + 0x03, 0x01, DataType.Byte, reader));
            descriptorData.Add("hash", new HexData("Volume Descriptor Hash", address + 0x04, 0x14, DataType.Raw, reader));
            descriptorData.Add("deviceFeatures", new HexData("Device Features", address + 0x18, 0x01, DataType.Byte, reader));
            descriptorData.Add("dataBlockCount", new HexData("Data Block Count", address + 0x19, 0x03, DataType.UInt, reader));
            descriptorData.Add("dataBlockOffset", new HexData("Data Block Offset", address + 0x1C, 0x03, DataType.UInt, reader));
            descriptorData.Add("padding", new HexData("Volume Descriptor Padding/Reserved", address + 0x1F, 0x05, DataType.Raw, reader));
        }
        public byte? GetSize()
        {
            if (size == null) size = descriptorData["size"].AsByte();
            return size;
        }
        // Only for STFS VD's
        public byte? GetReserved()
        {
            if (Type == VDType.SVOD) return null;
            if (reserved == null) reserved = descriptorData["reserved"].AsByte();
            return reserved;
        }
        // Only for STFS VD's
        public byte? GetBlockSeparation()
        {
            if (Type == VDType.SVOD) return null;
            if (blockSeparation == null) blockSeparation = descriptorData["blockSeparation"].AsByte();
            return blockSeparation;
        }
        // Only for STFS VD's
        public short? GetFileTableBlockCount()
        {
            if (Type == VDType.SVOD) return null;
            if (fileTableBlockCount == null) fileTableBlockCount = descriptorData["fileTableBlockCount"].AsShort();
            return fileTableBlockCount;
        }
        // Only for STFS VD's
        public int? GetFileTableBlockNumber()
        {
            if (Type == VDType.SVOD) return null;
            if (fileTableBlockNumber == null) fileTableBlockNumber = descriptorData["fileTableBlockNumber"].AsInt();
            return fileTableBlockNumber;
        }
        // Only for STFS VD's
        public byte[]? GetTopHashTableHash()
        {
            if (Type == VDType.SVOD) return null;
            if (topHashTableHash == null) topHashTableHash = descriptorData["topHashTableHash"].AsRaw();
            return topHashTableHash;
        }
        // Only for STFS VD's
        public int? GetTotalAllocatedBlockCount()
        {
            if (Type == VDType.SVOD) return null;
            if (totalAllocatedBlockCount == null) totalAllocatedBlockCount = descriptorData["totalAllocatedBlockCount"].AsIntLE();
            return totalAllocatedBlockCount;
        }
        // Only for STFS VD's
        public int? GetTotalUnallocatedBlockCount()
        {
            if (Type == VDType.SVOD) return null;
            if (totalUnallocatedBlockCount == null) totalUnallocatedBlockCount = descriptorData["totalUnallocatedBlockCount"].AsIntLE();
            return totalUnallocatedBlockCount;
        }
        // Only for SVOD VD's
        public byte? GetBlockCacheElementCount()
        {
            if (Type == VDType.STFS) return null;
            if (blockCacheElementCount == null) blockCacheElementCount = descriptorData["blockCacheElementCount"].AsByte();
            return blockCacheElementCount;
        }
        // Only for SVOD VD's
        public byte? GetWorkerThreadProcessor()
        {
            if (Type == VDType.STFS) return null;
            if (workerThreadProcessor == null) workerThreadProcessor = descriptorData["workerThreadProcessor"].AsByte();
            return workerThreadProcessor;
        }
        // Only for SVOD VD's
        public byte? GetWorkerThreadPriority()
        {
            if (Type == VDType.STFS) return null;
            if (workerThreadPriority == null) workerThreadPriority = descriptorData["workerThreadPriority"].AsByte();
            return workerThreadPriority;
        }
        // Only for SVOD VD's
        public byte[]? GetHash()
        {
            if (Type == VDType.STFS) return null;
            if (hash == null) hash = descriptorData["hash"].AsRaw();
            return hash;
        }
        // Only for SVOD VD's
        public byte? GetDeviceFeatures()
        {
            if (Type == VDType.STFS) return null;
            if (deviceFeatures == null) deviceFeatures = descriptorData["deviceFeatures"].AsByte();
            return deviceFeatures;
        }
        // Only for SVOD VD's
        public uint? GetDataBlockCount()
        {
            if (Type == VDType.STFS) return null;
            if (dataBlockCount == null) dataBlockCount = descriptorData["dataBlockCount"].AsUIntLE();
            return dataBlockCount;
        }
        // Only for SVOD VD's
        public uint? GetDataBlockOffset()
        {
            if (Type == VDType.STFS) return null;
            if (dataBlockOffset == null) dataBlockOffset = descriptorData["dataBlockOffset"].AsUIntLE();
            return dataBlockOffset;
        }
        // Only for SVOD VD's
        public byte[]? GetPadding()
        {
            if (Type == VDType.STFS) return null;
            if (padding == null) padding = descriptorData["padding"].AsRaw();
            return padding;
        }
        public void ClearData()
        {
            size = null;
            reserved = null;
            blockSeparation = null;
            fileTableBlockCount = null;
            fileTableBlockNumber = null;
            totalAllocatedBlockCount = null;
            totalUnallocatedBlockCount = null;
            topHashTableHash = null;
            blockCacheElementCount = null;
            workerThreadProcessor = null;
            workerThreadPriority = null;
            hash = null;
            deviceFeatures = null;
            dataBlockCount = null;
            dataBlockOffset = null;
            padding = null;
        }
    }
    public enum XContentType
    { 
        SavedGame = 0x0000001,
        MarketplaceContent = 0x0000002,
        Publisher = 0x0000003,
        Xbox360Title = 0x0001000,
        IPTVPauseBuffer = 0x0002000,
        InstalledGame = 0x0004000,
        XboxTitle = 0x0005000,
        GameOnDemand = 0x0007000,
        AvatarItem = 0x0009000,
        Profile = 0x0010000,
        GamerPicture = 0x0020000,
        Theme = 0x0030000,
        CacheFile = 0x0040000,
        StorageDownload = 0x0050000,
        XboxSavedGame = 0x0060000,
        XboxDownload = 0x0070000,
        GameDemo = 0x0080000,
        Video = 0x0090000,
        GameTitle = 0x00A0000,
        Installer = 0x00B0000,
        GameTrailer = 0x00C0000,
        ArcadeTitle = 0x00D0000,
        XNA = 0x00E0000,
        LicenseStore = 0x00F0000,
        Movie = 0x0100000,
        TV = 0x0200000,
        MusicVideo = 0x0300000,
        GameVideo = 0x0400000,
        PodcastVideo = 0x0500000,
        ViralVideo = 0x0600000,
        CommunityGame = 0x2000000
    }
    [Flags] public enum XTransferFlags
    {
        None0 = 0b00000001,
        None1 = 0b00000010,
        DeepLinkSupported = 0b00000100,
        DisableNetworkStorage = 0b00001000,
        KinectEnabled = 0b00010000,
        MoveOnlyTransfer = 0b00100000,
        DeviceIDTransfer = 0b01000000,
        ProfileIDTransfer = 0b10000000
    }
    public class STFS24
    {
        public static void Print(string message)
        {
            //if (X360_Backup_Tool.Program.ConsoleLogging)
            //{
            //    Console.WriteLine(message);
            //}
        }
        public static void PrintByteArray(byte[] bytes)
        {
            foreach (byte b in bytes)
            {
                Console.Write(b + ",");
            }
        }
        public static void PrintByteArrayAsHex(byte[] bytes)
        {
            foreach (byte b in bytes)
            {
                if (b < 0x10)
                {
                    // Writing in leading zeroes on bytes that need it
                    Console.Write(0);
                }
                Console.Write(String.Format("{0:X}", b));
            }
        }
        public static string GetByteArrayAsHex(byte[] bytes)
        {
            string toReturn = "";
            foreach (byte b in bytes)
            {
                if (b < 0x10)
                {
                    // Writing in leading zeroes on bytes that need it
                    toReturn = toReturn + "0";
                }
                toReturn = toReturn + String.Format("{0:X}", b);
            }
            return toReturn;
        }
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
        public static string TransferFlagsToString(XTransferFlags flags)
        {
            string toReturn = "";
            if (flags.HasFlag(XTransferFlags.None0))
            {
                toReturn += "None0 | ";
            }
            if (flags.HasFlag(XTransferFlags.None1))
            {
                toReturn += "None1 | ";
            }
            if (flags.HasFlag(XTransferFlags.DeepLinkSupported))
            {
                toReturn += "DeepLinkSupported | ";
            }
            if (flags.HasFlag(XTransferFlags.DisableNetworkStorage))
            {
                toReturn += "DisableNetworkStorage | ";
            }
            if (flags.HasFlag(XTransferFlags.KinectEnabled))
            {
                toReturn += "KinectEnabled | ";
            }
            if (flags.HasFlag(XTransferFlags.MoveOnlyTransfer))
            {
                toReturn += "MoveOnlyTransfer | ";
            }
            if (flags.HasFlag(XTransferFlags.DeviceIDTransfer))
            {
                toReturn += "DeviceIDTransfer | ";
            }
            if (flags.HasFlag(XTransferFlags.ProfileIDTransfer))
            {
                toReturn += "ProfileIDTransfer | ";
            }
            if (toReturn != "")
            {
                toReturn = toReturn.Substring(0, toReturn.Length - 3);
            }
            return toReturn;
        }

        public string filepath;
        public bool fileOpen = false;
        private FileStream reader;
        private Dictionary<string, HexData> stfsData;
        // Variables to cache STFS data
        private string? magic;
        private XSignature signature;
        private XMetadata metadata;
        public STFS24(string filepath)
        {
            this.filepath = filepath;
            reader = new FileStream(filepath, FileMode.Open);
            fileOpen = true;
            stfsData = new Dictionary<string, HexData>();
            STFSInit();
        }
        private void STFSInit()
        {
            stfsData.Add("magic", new HexData("Magic", 0x00, 0x04, DataType.ASCII, reader));
            GetMagic();
            signature = new XSignature(magic, reader);
            metadata = new XMetadata(reader);

            Print("STFS file opened");
        }
        public XSignature ReturnSignature()
        {
            return signature;
        }
        public XMetadata ReturnMetadata()
        {
            return metadata;
        }
        public string GetMagic()
        {
            if (magic == null) magic = stfsData["magic"].AsASCII();
            return magic;
        }
        public void ClearData()
        {
            magic = null;
            signature.ClearData();
            
            Print("STFS data cleared");
        }
        public void CloseStream()
        {
            Print("Closing STFS stream");
            reader.Close();
        }
        public bool IsStreamOpen()
        {
            return reader.CanRead;
        }
    }
}
