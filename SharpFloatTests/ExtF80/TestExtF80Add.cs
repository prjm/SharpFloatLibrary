using SharpFloatTests.Common;

using E = SharpFloat.ExtF80.ExtF80;

namespace SharpFloatTests.ExtF80 {

    public class TestExtF80Add {

        [TestCase]
        public void TestAdditions() {
            var zero = new E(0, 0) + new E(0, 0);
            Assert.EqualUShort(0, zero.signExp);
            Assert.EqualULong(0, zero.signif);

            var caseOne = new E(0x4002, 0x8000400010000000) + new E(0xC002, 0x8040000FFFFFFFFF);
            Assert.EqualUShort(0xBFF8, caseOne.signExp);
            Assert.EqualULong(0xFF003FBFFFFFFC00, caseOne.signif);
        }

    }
}
