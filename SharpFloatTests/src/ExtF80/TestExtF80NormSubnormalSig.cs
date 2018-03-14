using SharpFloatTests.Common;

namespace SharpFloatTests.FloatingPoint {
    public class TestExtF80NormSubnormalSig {

        [TestCase]
        public void TestNormalSubnormalSig() {
            Assert.EqualInt(0, SharpFloat.FloatingPoint.ExtF80.NormalizeSubnormalSignificand(0xFFFFFFFFFFFFFFFFUL).exp);
            Assert.EqualInt(-64, SharpFloat.FloatingPoint.ExtF80.NormalizeSubnormalSignificand(0).exp);
            Assert.EqualInt(-32, SharpFloat.FloatingPoint.ExtF80.NormalizeSubnormalSignificand(0x00000000FFFFFFFFUL).exp);
        }

    }
}
