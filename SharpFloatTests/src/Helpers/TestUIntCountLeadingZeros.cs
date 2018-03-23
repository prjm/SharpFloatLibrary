using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {

    public class TestUIntCountLeadingZeros {

        [TestCase]
        public void TestUintCountLeadZeros() {
            Assert.EqualByte(32, ((uint)0).CountLeadingZeros());
            Assert.EqualByte(31, ((uint)1).CountLeadingZeros());
            Assert.EqualByte(30, ((uint)2).CountLeadingZeros());
            Assert.EqualByte(29, ((uint)4).CountLeadingZeros());
            Assert.EqualByte(28, ((uint)8).CountLeadingZeros());
            Assert.EqualByte(27, ((uint)16).CountLeadingZeros());
            Assert.EqualByte(26, ((uint)32).CountLeadingZeros());
            Assert.EqualByte(25, ((uint)64).CountLeadingZeros());

            Assert.EqualByte(24, ((uint)128).CountLeadingZeros());
            Assert.EqualByte(23, ((uint)256).CountLeadingZeros());
            Assert.EqualByte(22, ((uint)512).CountLeadingZeros());
            Assert.EqualByte(21, ((uint)1024).CountLeadingZeros());
            Assert.EqualByte(20, ((uint)2048).CountLeadingZeros());
            Assert.EqualByte(19, ((uint)4096).CountLeadingZeros());
            Assert.EqualByte(18, ((uint)8192).CountLeadingZeros());
            Assert.EqualByte(17, ((uint)16384).CountLeadingZeros());
            Assert.EqualByte(16, ((uint)32768).CountLeadingZeros());

            Assert.EqualByte(15, ((uint)65536).CountLeadingZeros());
            Assert.EqualByte(14, ((uint)131072).CountLeadingZeros());
            Assert.EqualByte(13, ((uint)262144).CountLeadingZeros());
            Assert.EqualByte(12, ((uint)524288).CountLeadingZeros());
            Assert.EqualByte(11, ((uint)1048576).CountLeadingZeros());
            Assert.EqualByte(10, ((uint)2097152).CountLeadingZeros());
            Assert.EqualByte(09, ((uint)4194304).CountLeadingZeros());
            Assert.EqualByte(08, ((uint)8388608).CountLeadingZeros());

            Assert.EqualByte(07, ((uint)16777216).CountLeadingZeros());
            Assert.EqualByte(06, ((uint)33554432).CountLeadingZeros());
            Assert.EqualByte(05, ((uint)67108864).CountLeadingZeros());
            Assert.EqualByte(04, ((uint)134217728).CountLeadingZeros());
            Assert.EqualByte(03, ((uint)268435456).CountLeadingZeros());
            Assert.EqualByte(02, ((uint)536870912).CountLeadingZeros());
            Assert.EqualByte(01, ((uint)1073741824).CountLeadingZeros());
            Assert.EqualByte(00, ((uint)2147483648).CountLeadingZeros());
        }

    }
}
