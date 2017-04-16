namespace SciAdvNet.Vfs.Criware
{
    internal static class CpkTableEncryption
    {
        public static byte[] Decrypt(byte[] encryptedTable)
        {
            int c = 0x5f;
            byte m = 0x15;

            byte[] decrypted = new byte[encryptedTable.Length];
            for (int i = 0; i < encryptedTable.Length; i++)
            {
                decrypted[i] = (byte)((encryptedTable[i] ^ c) & 0xff);
                c *= (m & 0xff);
            }

            return decrypted;
        }
    }
}
