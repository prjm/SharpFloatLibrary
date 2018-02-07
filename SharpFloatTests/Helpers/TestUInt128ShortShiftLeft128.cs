using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {

    public class TestUInt128ShortShiftLeft128 {


        [TestCase]
        public void TestShortShiftLeft128() {
            Assert.EqualULong(0x13, UInt128.ShortShiftLeft128(4, 0xC000000000000000UL, 2).v64);
            Assert.EqualULong(0, UInt128.ShortShiftLeft128(4, 0xC000000000000000UL, 2).v0);
        }

    }

}
