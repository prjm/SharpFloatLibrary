using SharpFloatTests.Common;
using SharpFloat.Helpers;

namespace SharpFloatTests.Helpers {

    public class TestULongCountLeadingZeroes {

        [TestCase]
        public void TestULongCountLeadZeros() {
            Assert.AreEqual(32 + 32, ((ulong)0).CountLeadingZeroes());
            Assert.AreEqual(32 + 31, ((ulong)1).CountLeadingZeroes());
            Assert.AreEqual(32 + 30, ((ulong)2).CountLeadingZeroes());
            Assert.AreEqual(32 + 29, ((ulong)4).CountLeadingZeroes());
            Assert.AreEqual(32 + 28, ((ulong)8).CountLeadingZeroes());
            Assert.AreEqual(32 + 27, ((ulong)16).CountLeadingZeroes());
            Assert.AreEqual(32 + 26, ((ulong)32).CountLeadingZeroes());
            Assert.AreEqual(32 + 25, ((ulong)64).CountLeadingZeroes());

            Assert.AreEqual(32 + 24, ((ulong)128).CountLeadingZeroes());
            Assert.AreEqual(32 + 23, ((ulong)256).CountLeadingZeroes());
            Assert.AreEqual(32 + 22, ((ulong)512).CountLeadingZeroes());
            Assert.AreEqual(32 + 21, ((ulong)1024).CountLeadingZeroes());
            Assert.AreEqual(32 + 20, ((ulong)2048).CountLeadingZeroes());
            Assert.AreEqual(32 + 19, ((ulong)4096).CountLeadingZeroes());
            Assert.AreEqual(32 + 18, ((ulong)8192).CountLeadingZeroes());
            Assert.AreEqual(32 + 17, ((ulong)16384).CountLeadingZeroes());
            Assert.AreEqual(32 + 16, ((ulong)32768).CountLeadingZeroes());

            Assert.AreEqual(32 + 15, ((ulong)65536).CountLeadingZeroes());
            Assert.AreEqual(32 + 14, ((ulong)131072).CountLeadingZeroes());
            Assert.AreEqual(32 + 13, ((ulong)262144).CountLeadingZeroes());
            Assert.AreEqual(32 + 12, ((ulong)524288).CountLeadingZeroes());
            Assert.AreEqual(32 + 11, ((ulong)1048576).CountLeadingZeroes());
            Assert.AreEqual(32 + 10, ((ulong)2097152).CountLeadingZeroes());
            Assert.AreEqual(32 + 09, ((ulong)4194304).CountLeadingZeroes());
            Assert.AreEqual(32 + 08, ((ulong)8388608).CountLeadingZeroes());

            Assert.AreEqual(32 + 07, ((ulong)16777216).CountLeadingZeroes());
            Assert.AreEqual(32 + 06, ((ulong)33554432).CountLeadingZeroes());
            Assert.AreEqual(32 + 05, ((ulong)67108864).CountLeadingZeroes());
            Assert.AreEqual(32 + 04, ((ulong)134217728).CountLeadingZeroes());
            Assert.AreEqual(32 + 03, ((ulong)268435456).CountLeadingZeroes());
            Assert.AreEqual(32 + 02, ((ulong)536870912).CountLeadingZeroes());
            Assert.AreEqual(32 + 01, ((ulong)1073741824).CountLeadingZeroes());
            Assert.AreEqual(32 + 00, ((ulong)2147483648).CountLeadingZeroes());

            Assert.AreEqual(31, ((ulong)4294967296).CountLeadingZeroes());
            Assert.AreEqual(30, ((ulong)8589934592).CountLeadingZeroes());
            Assert.AreEqual(29, ((ulong)17179869184).CountLeadingZeroes());
            Assert.AreEqual(28, ((ulong)34359738368).CountLeadingZeroes());
            Assert.AreEqual(27, ((ulong)68719476736).CountLeadingZeroes());
            Assert.AreEqual(26, ((ulong)137438953472).CountLeadingZeroes());
            Assert.AreEqual(25, ((ulong)274877906944).CountLeadingZeroes());

            Assert.AreEqual(24, ((ulong)549755813888).CountLeadingZeroes());
            Assert.AreEqual(23, ((ulong)1099511627776).CountLeadingZeroes());
            Assert.AreEqual(22, ((ulong)2199023255552).CountLeadingZeroes());
            Assert.AreEqual(21, ((ulong)4398046511104).CountLeadingZeroes());
            Assert.AreEqual(20, ((ulong)8796093022208).CountLeadingZeroes());
            Assert.AreEqual(19, ((ulong)17592186044416).CountLeadingZeroes());
            Assert.AreEqual(18, ((ulong)35184372088832).CountLeadingZeroes());
            Assert.AreEqual(17, ((ulong)70368744177664).CountLeadingZeroes());
            Assert.AreEqual(16, ((ulong)140737488355328).CountLeadingZeroes());

            Assert.AreEqual(15, ((ulong)281474976710656).CountLeadingZeroes());
            Assert.AreEqual(14, ((ulong)562949953421312).CountLeadingZeroes());
            Assert.AreEqual(13, ((ulong)1125899906842624).CountLeadingZeroes());
            Assert.AreEqual(12, ((ulong)2251799813685248).CountLeadingZeroes());
            Assert.AreEqual(11, ((ulong)4503599627370496).CountLeadingZeroes());
            Assert.AreEqual(10, ((ulong)9007199254740992).CountLeadingZeroes());
            Assert.AreEqual(09, ((ulong)18014398509481984).CountLeadingZeroes());
            Assert.AreEqual(08, ((ulong)36028797018963968).CountLeadingZeroes());

            Assert.AreEqual(07, ((ulong)72057594037927936).CountLeadingZeroes());
            Assert.AreEqual(06, ((ulong)144115188075855872).CountLeadingZeroes());
            Assert.AreEqual(05, ((ulong)288230376151711744).CountLeadingZeroes());
            Assert.AreEqual(04, ((ulong)576460752303423488).CountLeadingZeroes());
            Assert.AreEqual(03, ((ulong)1152921504606846976).CountLeadingZeroes());
            Assert.AreEqual(02, ((ulong)2305843009213693952).CountLeadingZeroes());
            Assert.AreEqual(01, ((ulong)4611686018427387904).CountLeadingZeroes());
            Assert.AreEqual(00, ((ulong)0x8000000000000000).CountLeadingZeroes());

        }
    }
}
