using SharpFloatTests.Common;

namespace SharpFloatTests.ExtF80 {
    public class TestExtF80NormSubnormalSig {

        [TestCase]
        public void TestNormalSubnormalSig() {
            Assert.EqualInt(0, SharpFloat.FloatingPoint.ExtF80.NormSubnormalSig(0xFFFFFFFFFFFFFFFFUL).exp);
            Assert.EqualInt(-64, SharpFloat.FloatingPoint.ExtF80.NormSubnormalSig(0).exp);
            Assert.EqualInt(-32, SharpFloat.FloatingPoint.ExtF80.NormSubnormalSig(0x00000000FFFFFFFFUL).exp);
        }

    }
}
