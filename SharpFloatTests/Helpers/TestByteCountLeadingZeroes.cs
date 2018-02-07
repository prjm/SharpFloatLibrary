using SharpFloatTests.Common;
using SharpFloat.Helpers;

namespace SharpFloatTests.Helpers {
    public class TestByteCountLeadingZeroes {

        [TestCase]
        public void TestByteCoundLeadZeroes() {
            Assert.EqualByte(8, ((byte)0).CountLeadingZeroes());
            Assert.EqualByte(7, ((byte)1).CountLeadingZeroes());
            Assert.EqualByte(6, ((byte)2).CountLeadingZeroes());
            Assert.EqualByte(5, ((byte)4).CountLeadingZeroes());
            Assert.EqualByte(4, ((byte)8).CountLeadingZeroes());
            Assert.EqualByte(3, ((byte)16).CountLeadingZeroes());
            Assert.EqualByte(2, ((byte)32).CountLeadingZeroes());
            Assert.EqualByte(1, ((byte)64).CountLeadingZeroes());
            Assert.EqualByte(0, ((byte)128).CountLeadingZeroes());
        }
    }
}

