using System;

using System.Drawing;
using System.IO;

namespace Least_Significant_Bit_Steganography
{
    class Steganography
    {
        private string carrierImageLocation;
        private string hiddenFileLocation;
        private string outputImageLocation;

        /// <summary>
        /// Sets the image to hide the "hidden file" into
        /// </summary>
        /// <param name="fileName"></param>
        public void SetCarrierImage(string fileName)
        {
            this.carrierImageLocation = fileName;
        }

        /// <summary>
        /// Which file to hide into our carrier image
        /// </summary>
        /// <param name="fileName"></param>
        public void SetHiddenFile(string fileName)
        {
            this.hiddenFileLocation = fileName;
        }

        /// <summary>
        /// Where to output the combined image
        /// </summary>
        /// <param name="fileName"></param>
        public void SetOutputImage(string fileName)
        {
            this.outputImageLocation = fileName;
        }

        /// <summary>
        /// Get the hidden file
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public byte[] GetHiddenFile(string fileName)
        {
            int fileLength = 0;

            using (Bitmap carrierImage = new Bitmap(fileName))
            {
                for (int x = 0; x < 32; x++)
                {
                    Color c = carrierImage.GetPixel(x, 0);

                    bool bit = BitModifier.GetBit(c.ToArgb(), 0);

                    fileLength = BitModifier.SetBit(fileLength, x, bit);

                    carrierImage.SetPixel(x, 0, c);
                }

                Console.WriteLine("Extracting {0} bytes", fileLength);
            }

            byte[] fileBytes = new byte[fileLength];

            using (Bitmap carrierImage = new Bitmap(fileName))
            {
                bool finished = false;
                int bitIndex = 0;
                int byteIndex = 0;
                int bitCounter = 0;

                // Retreive file from image
                for (int y = 0; y < carrierImage.Height && finished == false; y++)
                {
                    int start = 0;
                    if (y == 0)
                    {
                        start = 32;
                    }
                    for (int x = start; x < carrierImage.Width && finished == false; x++)
                    {
                        if (bitIndex == 8)
                        {
                            bitCounter += 1;
                            bitIndex = 0;
                            byteIndex += 1;
                        }

                        if (byteIndex == fileLength)
                        {
                            finished = true;
                            break;
                        }

                        Color c = carrierImage.GetPixel(x, y);

                        bool value = BitModifier.GetBit(c.ToArgb(), 0);

                        fileBytes[byteIndex] = BitModifier.SetBit(fileBytes[byteIndex], bitIndex, value);

                        bitIndex += 1;
                    }
                }

                Console.WriteLine("Read {0} bytes", bitCounter);
            }

            return fileBytes;
        }

        /// <summary>
        /// Combine the specified hidden file into our carrier image
        /// </summary>
        public void ProcessImage()
        {
            using (Bitmap carrierImage = new Bitmap(this.carrierImageLocation))
            {
                byte[] hiddenFile = File.ReadAllBytes(this.hiddenFileLocation);
                Console.WriteLine("Hiding {0} bytes", hiddenFile.Length);

                short bitIndex = 0;
                int byteIndex = 0;
                bool finished = false;

                // Save file length
                for (int x = 0; x < 32; x++)
                {
                    Color oldColor = carrierImage.GetPixel(x, 0);
                    
                    bool value = BitModifier.GetBit(hiddenFile.Length, x);

                    int color = BitModifier.SetBit(oldColor.ToArgb(), 0, value);

                    Color newColor = Color.FromArgb(color);
                    
                    carrierImage.SetPixel(x, 0, newColor);
                }

                // 32 is the lengh of our "file length" starter variable.
                // Since each byte is 8 bits, we will be ensuring that we have enough storage 
                // space in our image (width * height) to store the file.
                if (32 + (hiddenFile.Length * 8) > (carrierImage.Width * carrierImage.Height))
                {
                    throw new Exception("The file we are hiding is too large for this image.");
                }

                int bitCounter = 0;
                // Hide file into image
                for (int y = 0; y < carrierImage.Height && finished == false; y++)
                {
                    int start = 32;
                    if(y != 0)
                    {
                        start = 0;
                    }
                    for (int x = start; x < carrierImage.Width && finished == false; x++)
                    {
                        if (bitIndex == 8)
                        {
                            bitCounter += 1;
                            bitIndex = 0;
                            byteIndex += 1;
                        }

                        if (byteIndex == hiddenFile.Length)
                        {
                            finished = true;
                            break;
                        }

                        Color c = carrierImage.GetPixel(x, y);

                        byte hiddenFileBit = hiddenFile[byteIndex];
                        bool value = BitModifier.GetBit(hiddenFileBit, bitIndex);

                        int color = BitModifier.SetBit(c.ToArgb(), 0, value);

                        c = Color.FromArgb(color);

                        carrierImage.SetPixel(x, y, c);

                        bitIndex += 1;
                    }
                }

                Console.WriteLine("Wrote {0} bytes", bitCounter);
                carrierImage.Save(this.outputImageLocation);
            }
        }
        
        /// <summary>
        /// Determines how many bytes each pixel color holds
        /// </summary>
        /// <param name="pf"></param>
        /// <returns></returns>
        private int getBytesPerPixel(System.Drawing.Imaging.PixelFormat pf)
        {
            int retVal = 0;

            switch (pf)
            {
                case System.Drawing.Imaging.PixelFormat.Format16bppArgb1555:
                case System.Drawing.Imaging.PixelFormat.Format16bppRgb555:
                case System.Drawing.Imaging.PixelFormat.Format16bppRgb565:
                    retVal = 16;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format24bppRgb:
                    retVal = 24;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
                case System.Drawing.Imaging.PixelFormat.Format32bppPArgb:
                case System.Drawing.Imaging.PixelFormat.Format32bppRgb:
                    retVal = 32;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format48bppRgb:
                    retVal = 48;
                    break;
                case System.Drawing.Imaging.PixelFormat.Format64bppArgb:
                case System.Drawing.Imaging.PixelFormat.Format64bppPArgb:
                    retVal = 64;
                    break;
                default:
                    retVal = 8;
                    break;
            }

            return retVal;
        }
    }
}