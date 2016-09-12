# Least-Significant-Bit-Steganography

This project was created to demonstrate my theoretical knowledge of the "least significant bit" method of image steganography.

LSB Steganography is a method to hide data within an image by utilizing the bits of each pixel color.

A pixel contains 3 colors (RGB) and this allows us to hide 3 bits of data within each color spectrum with minimal change in the final output of the image. The more data that is added to each pixel, the more noticable the change.

In this example, I will only be modifying 1 bit per pixel for demonstration purposes.

## How to use:
1. You will need to compile this using Visual Studio
2. You will then need to move the "img1.png" and "img2.png" to where the binary is.
3. Run the binary and you will get an "output.png" which is the carrier image.
4. The resulting bin file is the binary file that was retreived from the carrier image.
