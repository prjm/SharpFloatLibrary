using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {

    public class TestUIntCountLeadingZeroes {

        [TestCase]
        public void TestUintCountLeadZeroes() {
            Assert.AreEqual(32, ((uint)0).CountLeadingZeroes());
            Assert.AreEqual(31, ((uint)1).CountLeadingZeroes());
            Assert.AreEqual(30, ((uint)2).CountLeadingZeroes());
            Assert.AreEqual(29, ((uint)4).CountLeadingZeroes());
            Assert.AreEqual(28, ((uint)8).CountLeadingZeroes());
            Assert.AreEqual(27, ((uint)16).CountLeadingZeroes());
            Assert.AreEqual(26, ((uint)32).CountLeadingZeroes());
            Assert.AreEqual(25, ((uint)64).CountLeadingZeroes());

            Assert.AreEqual(24, ((uint)128).CountLeadingZeroes());
            Assert.AreEqual(23, ((uint)256).CountLeadingZeroes());
            Assert.AreEqual(22, ((uint)512).CountLeadingZeroes());
            Assert.AreEqual(21, ((uint)1024).CountLeadingZeroes());
            Assert.AreEqual(20, ((uint)2048).CountLeadingZeroes());
            Assert.AreEqual(19, ((uint)4096).CountLeadingZeroes());
            Assert.AreEqual(18, ((uint)8192).CountLeadingZeroes());
            Assert.AreEqual(17, ((uint)16384).CountLeadingZeroes());
            Assert.AreEqual(16, ((uint)32768).CountLeadingZeroes());

            Assert.AreEqual(15, ((uint)65536).CountLeadingZeroes());
            Assert.AreEqual(14, ((uint)131072).CountLeadingZeroes());
            Assert.AreEqual(13, ((uint)262144).CountLeadingZeroes());
            Assert.AreEqual(12, ((uint)524288).CountLeadingZeroes());
            Assert.AreEqual(11, ((uint)1048576).CountLeadingZeroes());
            Assert.AreEqual(10, ((uint)2097152).CountLeadingZeroes());
            Assert.AreEqual(09, ((uint)4194304).CountLeadingZeroes());
            Assert.AreEqual(08, ((uint)8388608).CountLeadingZeroes());

            Assert.AreEqual(07, ((uint)16777216).CountLeadingZeroes());
            Assert.AreEqual(06, ((uint)33554432).CountLeadingZeroes());
            Assert.AreEqual(05, ((uint)67108864).CountLeadingZeroes());
            Assert.AreEqual(04, ((uint)134217728).CountLeadingZeroes());
            Assert.AreEqual(03, ((uint)268435456).CountLeadingZeroes());
            Assert.AreEqual(02, ((uint)536870912).CountLeadingZeroes());
            Assert.AreEqual(01, ((uint)1073741824).CountLeadingZeroes());
            Assert.AreEqual(00, ((uint)2147483648).CountLeadingZeroes());
        }

    }
}
