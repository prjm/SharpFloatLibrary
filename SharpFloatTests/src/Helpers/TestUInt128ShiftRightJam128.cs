using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {
    public class TestUInt128ShiftRightJam128 {

        [TestCase]
        public void TestUInt128ShiftRightJam() {
            UInt128 s(ulong a64, ulong a0, int d)
                => new UInt128(a64, a0).ShiftRightJam128(d);

            Assert.EqualULong(1, s(4, 0xFFFF00, 2).v64);
            Assert.EqualULong(0x3FFFC0, s(4, 0xFFFF00, 2).v0);
            Assert.EqualULong(0x3FFFC1, s(4, 0xFFFF01, 2).v0);
            Assert.EqualULong(1, s(0, 1, 127).v0);
            Assert.EqualULong(1, s(1, 0, 127).v0);
            Assert.EqualULong(0, s(0, 1, 127).v64);
            Assert.EqualULong(0, s(1, 0, 127).v64);
            Assert.EqualULong(1, s(0xFFFFFF, 0x100, 100).v0);
            Assert.EqualULong(0xF, s(0xFFFFFFFFFF, 0x100, 100).v0);
            Assert.EqualULong(0, s(0xFFFFFF, 0x100, 100).v64);
            Assert.EqualULong(0, s(0xFFFFFFFFFF, 0x100, 100).v64);
        }
    }
}
