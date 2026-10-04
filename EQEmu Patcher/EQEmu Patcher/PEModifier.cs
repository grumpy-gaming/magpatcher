using System;
using System.IO;

namespace EQEmu_Patcher
{
    /// <summary>
    /// Applies the "4GB patch" (sets the Large Address Aware flag in the PE header of eqgame.exe).
    /// This is the same single-bit change the NTCore 4GB Patch tool makes. It touches exactly one
    /// byte of the file, and a backup (eqgame.exe.bak) is kept the first time it is applied.
    ///
    /// Idea borrowed from THJPatcher (GPLv2).
    /// </summary>
    public static class PEModifier
    {
        private const ushort ImageDosSignature = 0x5A4D;   // "MZ"
        private const uint ImageNtSignature = 0x00004550;  // "PE\0\0"
        private const byte LargeAddressAwareLowByte = 0x20; // IMAGE_FILE_LARGE_ADDRESS_AWARE = 0x0020
        private const int CharacteristicsOffsetInFileHeader = 18;

        // Returns the file offset of the low byte of the "Characteristics" field, or -1 if this isn't a PE file.
        private static int FindCharacteristicsOffset(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 0x40) return -1;
            if (BitConverter.ToUInt16(bytes, 0) != ImageDosSignature) return -1;

            int peOffset = BitConverter.ToInt32(bytes, 0x3C);
            if (peOffset < 0 || peOffset > bytes.Length - 24) return -1;
            if (BitConverter.ToUInt32(bytes, peOffset) != ImageNtSignature) return -1;

            int offset = peOffset + 4 + CharacteristicsOffsetInFileHeader;
            if (offset + 1 >= bytes.Length) return -1;
            return offset;
        }

        public static bool IsApplied(string exePath)
        {
            try
            {
                byte[] bytes = File.ReadAllBytes(exePath);
                int offset = FindCharacteristicsOffset(bytes);
                if (offset < 0) return false;
                return (bytes[offset] & LargeAddressAwareLowByte) != 0;
            }
            catch
            {
                return false;
            }
        }

        // Returns "" on success (or if it was already applied), otherwise a message describing the problem.
        public static string Apply(string exePath)
        {
            try
            {
                if (!File.Exists(exePath)) return "eqgame.exe was not found.";

                byte[] bytes = File.ReadAllBytes(exePath);
                int offset = FindCharacteristicsOffset(bytes);
                if (offset < 0) return "eqgame.exe does not look like a valid Windows program, so it was left alone.";

                if ((bytes[offset] & LargeAddressAwareLowByte) != 0) return ""; // already patched

                string backupPath = exePath + ".bak";
                if (!File.Exists(backupPath))
                {
                    File.Copy(exePath, backupPath, false);
                }

                bytes[offset] = (byte)(bytes[offset] | LargeAddressAwareLowByte);

                // Write to a temp file first, then swap, so a failure can't leave a half-written eqgame.exe.
                string tempPath = exePath + ".mpatch.tmp";
                File.WriteAllBytes(tempPath, bytes);
                File.Replace(tempPath, exePath, null);
                return "";
            }
            catch (UnauthorizedAccessException ex)
            {
                return "Windows would not let the patcher change eqgame.exe. Close EverQuest and try again, "
                    + "or run the patcher as administrator if your game is in Program Files. (" + ex.Message + ")";
            }
            catch (IOException ex)
            {
                return "eqgame.exe is in use. Close EverQuest completely and try again. (" + ex.Message + ")";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
