using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {
    public class TestUInt128Mul64ByShifted32To128 {

        [TestCase]
        public void TestMul64ByShifted32To128() {
            var a = 0x3333333333333333UL;
            var b = 0x111111U;
            var r = UInt128.Mul64ByShifted32To128(a, b);
            Assert.EqualULong(0x332FC96300000000, r.v0);
            Assert.EqualULong(0x000369d033333333, r.v64);

            a = 0x333333311111111UL;
            b = 0x111222U;
            r = UInt128.Mul64ByShifted32To128(a, b);
            Assert.EqualULong(0x9998764200000000, r.v0);
            Assert.EqualULong(0x000036a06cca861d, r.v64);
        }
    }
}
