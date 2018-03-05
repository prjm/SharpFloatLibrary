using SharpFloat.FloatingPoint;
using SharpFloat.Globals;
using SharpFloatTests.Common;

namespace SharpFloatTests.FloatingPoint {
    public class ExtF80RoundToInt {

        [TestCase]
        public void TestRoundToInt() {
            var r = new ExtF80(0xB687, 0x801003FFFFFFFFFE);
            var q = r.RoundToInt(RoundingMode.NearEven, true);
            Assert.EqualUShort(0x8000, q.signExp);
            Assert.EqualULong(0, q.signif);
        }
    }
}
