namespace Least_Significant_Bit_Steganography
{
    static class BitModifier
    {
        public static int SetBit(int input, int index, bool value)
        {
            int retVal;

            if (value)
            {
                retVal = (input | (1 << index));
            }
            else
            {
                retVal = (input & ~(1 << index));
            }

            return retVal;
        }

        public static byte SetBit(byte input, int index, bool value)
        {
            byte retVal;

            if (value)
            {
                retVal = (byte)(input | (1 << index));
            }
            else
            {
                retVal = (byte)(input & ~(1 << index));
            }

            return retVal;
        }

        public static bool GetBit(this byte b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }

        public static bool GetBit(this int b, int bitNumber)
        {
            return (b & (1 << bitNumber)) != 0;
        }
    }
}
