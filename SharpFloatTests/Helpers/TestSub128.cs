using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {
    public class TestSub128 {

        [TestCase]
        public void Test128Sub() {
            Assert.AreEqual(220, UInt128.Sub128(0, 320, 0, 100).v0);
            Assert.AreEqual(18446744073709551516, UInt128.Sub128(0, 0, 0, 100).v0);
            Assert.AreEqual(98, UInt128.Sub128(99, 320, 1, 100).v64);
        }

    }
}
