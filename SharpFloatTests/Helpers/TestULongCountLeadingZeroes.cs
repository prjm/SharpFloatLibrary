using SharpFloatTests.Common;
using SharpFloat.Helpers;

namespace SharpFloatTests.Helpers {

    public class TestULongCountLeadingZeroes {

        [TestCase]
        public void TestULongCountLeadZeros() {
            Assert.EqualByte(32 + 32, ((ulong)0).CountLeadingZeroes());
            Assert.EqualByte(32 + 31, ((ulong)1).CountLeadingZeroes());
            Assert.EqualByte(32 + 30, ((ulong)2).CountLeadingZeroes());
            Assert.EqualByte(32 + 29, ((ulong)4).CountLeadingZeroes());
            Assert.EqualByte(32 + 28, ((ulong)8).CountLeadingZeroes());
            Assert.EqualByte(32 + 27, ((ulong)16).CountLeadingZeroes());
            Assert.EqualByte(32 + 26, ((ulong)32).CountLeadingZeroes());
            Assert.EqualByte(32 + 25, ((ulong)64).CountLeadingZeroes());

            Assert.EqualByte(32 + 24, ((ulong)128).CountLeadingZeroes());
            Assert.EqualByte(32 + 23, ((ulong)256).CountLeadingZeroes());
            Assert.EqualByte(32 + 22, ((ulong)512).CountLeadingZeroes());
            Assert.EqualByte(32 + 21, ((ulong)1024).CountLeadingZeroes());
            Assert.EqualByte(32 + 20, ((ulong)2048).CountLeadingZeroes());
            Assert.EqualByte(32 + 19, ((ulong)4096).CountLeadingZeroes());
            Assert.EqualByte(32 + 18, ((ulong)8192).CountLeadingZeroes());
            Assert.EqualByte(32 + 17, ((ulong)16384).CountLeadingZeroes());
            Assert.EqualByte(32 + 16, ((ulong)32768).CountLeadingZeroes());

            Assert.EqualByte(32 + 15, ((ulong)65536).CountLeadingZeroes());
            Assert.EqualByte(32 + 14, ((ulong)131072).CountLeadingZeroes());
            Assert.EqualByte(32 + 13, ((ulong)262144).CountLeadingZeroes());
            Assert.EqualByte(32 + 12, ((ulong)524288).CountLeadingZeroes());
            Assert.EqualByte(32 + 11, ((ulong)1048576).CountLeadingZeroes());
            Assert.EqualByte(32 + 10, ((ulong)2097152).CountLeadingZeroes());
            Assert.EqualByte(32 + 09, ((ulong)4194304).CountLeadingZeroes());
            Assert.EqualByte(32 + 08, ((ulong)8388608).CountLeadingZeroes());

            Assert.EqualByte(32 + 07, ((ulong)16777216).CountLeadingZeroes());
            Assert.EqualByte(32 + 06, ((ulong)33554432).CountLeadingZeroes());
            Assert.EqualByte(32 + 05, ((ulong)67108864).CountLeadingZeroes());
            Assert.EqualByte(32 + 04, ((ulong)134217728).CountLeadingZeroes());
            Assert.EqualByte(32 + 03, ((ulong)268435456).CountLeadingZeroes());
            Assert.EqualByte(32 + 02, ((ulong)536870912).CountLeadingZeroes());
            Assert.EqualByte(32 + 01, ((ulong)1073741824).CountLeadingZeroes());
            Assert.EqualByte(32 + 00, ((ulong)2147483648).CountLeadingZeroes());

            Assert.EqualByte(31, ((ulong)4294967296).CountLeadingZeroes());
            Assert.EqualByte(30, ((ulong)8589934592).CountLeadingZeroes());
            Assert.EqualByte(29, ((ulong)17179869184).CountLeadingZeroes());
            Assert.EqualByte(28, ((ulong)34359738368).CountLeadingZeroes());
            Assert.EqualByte(27, ((ulong)68719476736).CountLeadingZeroes());
            Assert.EqualByte(26, ((ulong)137438953472).CountLeadingZeroes());
            Assert.EqualByte(25, ((ulong)274877906944).CountLeadingZeroes());

            Assert.EqualByte(24, ((ulong)549755813888).CountLeadingZeroes());
            Assert.EqualByte(23, ((ulong)1099511627776).CountLeadingZeroes());
            Assert.EqualByte(22, ((ulong)2199023255552).CountLeadingZeroes());
            Assert.EqualByte(21, ((ulong)4398046511104).CountLeadingZeroes());
            Assert.EqualByte(20, ((ulong)8796093022208).CountLeadingZeroes());
            Assert.EqualByte(19, ((ulong)17592186044416).CountLeadingZeroes());
            Assert.EqualByte(18, ((ulong)35184372088832).CountLeadingZeroes());
            Assert.EqualByte(17, ((ulong)70368744177664).CountLeadingZeroes());
            Assert.EqualByte(16, ((ulong)140737488355328).CountLeadingZeroes());

            Assert.EqualByte(15, ((ulong)281474976710656).CountLeadingZeroes());
            Assert.EqualByte(14, ((ulong)562949953421312).CountLeadingZeroes());
            Assert.EqualByte(13, ((ulong)1125899906842624).CountLeadingZeroes());
            Assert.EqualByte(12, ((ulong)2251799813685248).CountLeadingZeroes());
            Assert.EqualByte(11, ((ulong)4503599627370496).CountLeadingZeroes());
            Assert.EqualByte(10, ((ulong)9007199254740992).CountLeadingZeroes());
            Assert.EqualByte(09, ((ulong)18014398509481984).CountLeadingZeroes());
            Assert.EqualByte(08, ((ulong)36028797018963968).CountLeadingZeroes());

            Assert.EqualByte(07, ((ulong)72057594037927936).CountLeadingZeroes());
            Assert.EqualByte(06, ((ulong)144115188075855872).CountLeadingZeroes());
            Assert.EqualByte(05, ((ulong)288230376151711744).CountLeadingZeroes());
            Assert.EqualByte(04, ((ulong)576460752303423488).CountLeadingZeroes());
            Assert.EqualByte(03, ((ulong)1152921504606846976).CountLeadingZeroes());
            Assert.EqualByte(02, ((ulong)2305843009213693952).CountLeadingZeroes());
            Assert.EqualByte(01, ((ulong)4611686018427387904).CountLeadingZeroes());
            Assert.EqualByte(00, ((ulong)0x8000000000000000).CountLeadingZeroes());

        }
    }
}
