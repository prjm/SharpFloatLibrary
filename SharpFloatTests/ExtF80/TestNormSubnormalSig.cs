using SharpFloatTests.Common;
using SharpFloat;

namespace SharpFloatTests.ExtF80 {
    public class TestNormSubnormalSig {

        [TestCase]
        public void TestNormalSubnormalSig() {
            Assert.EqualInt(0, SharpFloat.ExtF80.ExtF80.NormSubnormalSig(0xFFFFFFFFFFFFFFFFUL).exp);
            Assert.EqualInt(-64, SharpFloat.ExtF80.ExtF80.NormSubnormalSig(0).exp);
            Assert.EqualInt(-32, SharpFloat.ExtF80.ExtF80.NormSubnormalSig(0x00000000FFFFFFFFUL).exp);
        }

    }
}
