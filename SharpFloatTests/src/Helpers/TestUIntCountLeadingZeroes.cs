using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {

    public class TestUIntCountLeadingZeroes {

        [TestCase]
        public void TestUintCountLeadZeroes() {
            Assert.EqualByte(32, ((uint)0).CountLeadingZeroes());
            Assert.EqualByte(31, ((uint)1).CountLeadingZeroes());
            Assert.EqualByte(30, ((uint)2).CountLeadingZeroes());
            Assert.EqualByte(29, ((uint)4).CountLeadingZeroes());
            Assert.EqualByte(28, ((uint)8).CountLeadingZeroes());
            Assert.EqualByte(27, ((uint)16).CountLeadingZeroes());
            Assert.EqualByte(26, ((uint)32).CountLeadingZeroes());
            Assert.EqualByte(25, ((uint)64).CountLeadingZeroes());

            Assert.EqualByte(24, ((uint)128).CountLeadingZeroes());
            Assert.EqualByte(23, ((uint)256).CountLeadingZeroes());
            Assert.EqualByte(22, ((uint)512).CountLeadingZeroes());
            Assert.EqualByte(21, ((uint)1024).CountLeadingZeroes());
            Assert.EqualByte(20, ((uint)2048).CountLeadingZeroes());
            Assert.EqualByte(19, ((uint)4096).CountLeadingZeroes());
            Assert.EqualByte(18, ((uint)8192).CountLeadingZeroes());
            Assert.EqualByte(17, ((uint)16384).CountLeadingZeroes());
            Assert.EqualByte(16, ((uint)32768).CountLeadingZeroes());

            Assert.EqualByte(15, ((uint)65536).CountLeadingZeroes());
            Assert.EqualByte(14, ((uint)131072).CountLeadingZeroes());
            Assert.EqualByte(13, ((uint)262144).CountLeadingZeroes());
            Assert.EqualByte(12, ((uint)524288).CountLeadingZeroes());
            Assert.EqualByte(11, ((uint)1048576).CountLeadingZeroes());
            Assert.EqualByte(10, ((uint)2097152).CountLeadingZeroes());
            Assert.EqualByte(09, ((uint)4194304).CountLeadingZeroes());
            Assert.EqualByte(08, ((uint)8388608).CountLeadingZeroes());

            Assert.EqualByte(07, ((uint)16777216).CountLeadingZeroes());
            Assert.EqualByte(06, ((uint)33554432).CountLeadingZeroes());
            Assert.EqualByte(05, ((uint)67108864).CountLeadingZeroes());
            Assert.EqualByte(04, ((uint)134217728).CountLeadingZeroes());
            Assert.EqualByte(03, ((uint)268435456).CountLeadingZeroes());
            Assert.EqualByte(02, ((uint)536870912).CountLeadingZeroes());
            Assert.EqualByte(01, ((uint)1073741824).CountLeadingZeroes());
            Assert.EqualByte(00, ((uint)2147483648).CountLeadingZeroes());
        }

    }
}
