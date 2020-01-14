using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace LEB128
{
    public static class LEBConverter
    {
        /// <summary>
        /// Returns LEB128 representation of an unsigned Integer
        /// </summary>
        /// <param name="value">uint</param>
        /// <returns>byte[]</returns>
        public static byte[] ToLEB128ByteArray(uint value)
        {
            List<byte> bytes = new List<byte>();
            do
            {
                byte chunk = (byte) (value & 0x7F); //0b01111111
                value >>= 7;
                if (value != 0)
                    chunk |= 0x80; //0b10000000;
                bytes.Add(chunk);
            } while (value != 0);
            return bytes.ToArray();
        }

        /// <summary>
        /// Returns LEB128 representation of a signed Integer
        /// </summary>
        /// <param name="value">int</param>
        /// <returns>byte[]</returns>
        public static byte[] ToLEB128ByteArray(int value)
        {
            List<byte> bytes = new List<byte>();
            bool more = true;
            while (more)
            {
                byte chunk = (byte)(value & 0x7F); //0b01111111
                value >>= 7;
                if ((value == 0 && (chunk & 0x40) == 0)
                    || (value == -1 && (chunk & 0x40) != 0))
                {
                    more = false;
                }
                else
                {
                    chunk |= 0x80;
                }
                bytes.Add(chunk);
            }
            return bytes.ToArray();
        }

        /// <summary>
        /// Returns an unsigned Integer from LEB128 byte[]
        /// </summary>
        /// <param name="value">byte[]</param>
        /// <returns>uint</returns>
        public static uint ToUInt(byte[] value)
        {
            uint result = 0;
            int shift = 0;
            foreach (var _byte in value)
            {
                byte lowBits = (byte)(_byte & 0x7F);
                byte hightBit = (byte)(_byte & 0x80);
                result |= (uint)(lowBits << shift);
                if (hightBit == 0)
                    break;
                shift += 7;
            }
            return result;
        }

        /// <summary>
        /// Returns a signed Integer from LEB128 byte[]
        /// </summary>
        /// <param name="value">byte[]</param>
        /// <returns>uint</returns>
        public static int ToInt(byte[] value)
        {
            int result = 0;
            int shift = 0;
            //32 bit Integer
            const int size = 32;
            byte _byte = 0;
            for(int i = 0, hightBit=1; hightBit != 0; i++)
            {
                _byte = value[i];
                byte lowBits = (byte)(_byte & 0x7F);
                hightBit = (_byte & 0x80);
                result |= (lowBits << shift);
                shift += 7;
                //may be a redundant exit condition
                if (hightBit == 0)
                    break;
            }
            if ((shift < size) && (_byte & 0x40) == 0)
              result |= (~0 << shift);
            return result;
        }
    }
}
