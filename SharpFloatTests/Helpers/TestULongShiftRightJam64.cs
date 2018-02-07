using SharpFloatTests.Common;
using SharpFloat.Helpers;

namespace SharpFloatTests.Helpers {
    public class TestULongShiftRightJam64 {

        [TestCase]
        public void TestShiftRightJam64() {
            Assert.EqualULong(0, 0UL.ShiftRightJam64(63));
            Assert.EqualULong(1, 0x23423423423UL.ShiftRightJam64(63));
            Assert.EqualULong(3, 5UL.ShiftRightJam64(1));
        }

    }
}
