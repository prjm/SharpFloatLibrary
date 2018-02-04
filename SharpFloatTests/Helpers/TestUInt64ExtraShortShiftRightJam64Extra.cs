using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {

    public class TestUInt64ExtraShortShiftRightJam64Extra {

        [TestCase]
        public void TestShortShiftRightJam64Extra() {
            Assert.AreEqual(0xC, UInt64Extra.ShortShiftRightJam64Extra(0xCCC, 3, 8).v);
            Assert.AreEqual(0xCC00000000000001, UInt64Extra.ShortShiftRightJam64Extra(0xCCC, 3, 8).extra);
        }

    }
}
