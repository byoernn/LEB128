using NUnit.Framework;

namespace LEB128.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test_UnsingedInteger_Positive_ToLEB128ByteArray()
        {
            //0b0000110_0111101_0001111_1111000_0001101
            byte[] actual = LEBConverter.ToLEB128ByteArray((uint)1738800141);
            byte[] expected = new byte[] { 0x8D, 0xF8, 0x8F, 0xBD, 0x06 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_SignedInteger_Positive_ToLEB128ByteArray()
        {
            byte[] actual = LEBConverter.ToLEB128ByteArray((int)1738800141);
            byte[] expected = new byte[] { 0x8D, 0xF8, 0x8F, 0xBD, 0x06 };
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_SignedInteger_Negative_ToLEB128ByteArray()
        {
            //0b111_1110101_0110001_1101111
            byte[] actual = LEBConverter.ToLEB128ByteArray((int)-173841);
            byte[] expected = new byte[] { 0xEF, 0xB1, 0x75};
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Test_LEB128ByteArray_ToUnsingedInteger_Positive()
        {
            //0b0000110_0111101_0001111_1111000_0001101
            var bytes = new byte[] { 0x8D, 0xF8, 0x8F, 0xBD, 0x06 };
            uint actual = LEBConverter.ToUInt(bytes);
            const uint expected = 1738800141;
            Assert.AreEqual(expected, actual);
        }
    }
}