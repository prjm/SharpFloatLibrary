using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {
    public class TestUInt128ShiftRightJam128 {

        [TestCase]
        public void TestUInt128ShiftRightJam() {
            Assert.EqualULong(1, UInt128.ShiftRightJam128(4, 0xFFFF00, 2).v64);
            Assert.EqualULong(0x3FFFC0, UInt128.ShiftRightJam128(4, 0xFFFF00, 2).v0);
            Assert.EqualULong(0x3FFFC1, UInt128.ShiftRightJam128(4, 0xFFFF01, 2).v0);
            Assert.EqualULong(1, UInt128.ShiftRightJam128(0, 1, 127).v0);
            Assert.EqualULong(1, UInt128.ShiftRightJam128(1, 0, 127).v0);
            Assert.EqualULong(0, UInt128.ShiftRightJam128(0, 1, 127).v64);
            Assert.EqualULong(0, UInt128.ShiftRightJam128(1, 0, 127).v64);
            Assert.EqualULong(1, UInt128.ShiftRightJam128(0xFFFFFF, 0x100, 100).v0);
            Assert.EqualULong(0xF, UInt128.ShiftRightJam128(0xFFFFFFFFFF, 0x100, 100).v0);
            Assert.EqualULong(0, UInt128.ShiftRightJam128(0xFFFFFF, 0x100, 100).v64);
            Assert.EqualULong(0, UInt128.ShiftRightJam128(0xFFFFFFFFFF, 0x100, 100).v64);
        }
    }
}
