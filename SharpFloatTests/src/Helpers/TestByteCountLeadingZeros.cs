using SharpFloatTests.Common;
using SharpFloat.Helpers;

namespace SharpFloatTests.Helpers {
    public class TestByteCountLeadingZeros {

        [TestCase]
        public void TestByteCoundLeadZeros() {
            Assert.EqualByte(8, ((byte)0).CountLeadingZeros());
            Assert.EqualByte(7, ((byte)1).CountLeadingZeros());
            Assert.EqualByte(6, ((byte)2).CountLeadingZeros());
            Assert.EqualByte(5, ((byte)4).CountLeadingZeros());
            Assert.EqualByte(4, ((byte)8).CountLeadingZeros());
            Assert.EqualByte(3, ((byte)16).CountLeadingZeros());
            Assert.EqualByte(2, ((byte)32).CountLeadingZeros());
            Assert.EqualByte(1, ((byte)64).CountLeadingZeros());
            Assert.EqualByte(0, ((byte)128).CountLeadingZeros());
        }
    }
}

