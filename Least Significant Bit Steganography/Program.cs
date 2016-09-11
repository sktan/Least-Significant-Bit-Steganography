using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace Least_Significant_Bit_Steganography
{
    class Program
    {
        static void Main(string[] args)
        {
            Steganography steg = new Steganography();

            steg.SetCarrierImage(Path.Combine(Environment.CurrentDirectory, "img1.png"));
            steg.SetHiddenFile(Path.Combine(Environment.CurrentDirectory, "img2.png"));

            steg.SetOutputImage(Path.Combine(Environment.CurrentDirectory, "output.png"));

            steg.ProcessImage();


            File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, "retreived.bin"),
                steg.GetHiddenFile(Path.Combine(Environment.CurrentDirectory, "output.png")));

            Console.ReadLine();
        }
    }
}
