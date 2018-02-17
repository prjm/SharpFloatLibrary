using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {
    public class TestUInt64ExtraShiftRightJam64Extra {

        [TestCase]
        public void TestShiftRightJam64Extra() {
            Assert.EqualULong(0xCC, UInt64Extra.ShiftRightJam64Extra(0xCC00, 1, 8).v);
            Assert.EqualULong(1, UInt64Extra.ShiftRightJam64Extra(0xCC00, 1, 8).extra);
            Assert.EqualULong(1, UInt64Extra.ShiftRightJam64Extra(0xCC00, 1, 65).extra);
            Assert.EqualULong(0, UInt64Extra.ShiftRightJam64Extra(0, 0, 65).extra);
            Assert.EqualULong(0, UInt64Extra.ShiftRightJam64Extra(0xCC00, 1, 64).v);
            Assert.EqualULong(0, UInt64Extra.ShiftRightJam64Extra(0xCC00, 0, 64).v);
            Assert.EqualULong(0xCC01, UInt64Extra.ShiftRightJam64Extra(0xCC00, 4, 64).extra);
            Assert.EqualULong(0xCC00, UInt64Extra.ShiftRightJam64Extra(0xCC00, 0, 64).extra);

        }
    }
}
