using SharpFloatTests.Common;
using SharpFloat.Helpers;

namespace SharpFloatTests.Helpers {
    public class TestByteCountLeadingZeroes {

        [TestCase]
        public void TestByteCoundLeadZeroes() {
            Assert.AreEqual(8, ((byte)0).CountLeadingZeroes());
            Assert.AreEqual(7, ((byte)1).CountLeadingZeroes());
            Assert.AreEqual(6, ((byte)2).CountLeadingZeroes());
            Assert.AreEqual(5, ((byte)4).CountLeadingZeroes());
            Assert.AreEqual(4, ((byte)8).CountLeadingZeroes());
            Assert.AreEqual(3, ((byte)16).CountLeadingZeroes());
            Assert.AreEqual(2, ((byte)32).CountLeadingZeroes());
            Assert.AreEqual(1, ((byte)64).CountLeadingZeroes());
            Assert.AreEqual(0, ((byte)128).CountLeadingZeroes());
        }
    }
}

