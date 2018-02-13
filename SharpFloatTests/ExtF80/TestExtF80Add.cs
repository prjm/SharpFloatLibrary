using SharpFloatTests.Common;

using E = SharpFloat.ExtF80.ExtF80;

namespace SharpFloatTests.ExtF80 {

    public class TestExtF80Add {

        [TestCase]
        public void TestAdditions() {
            var zero = new E(0, 0) + new E(0, 0);
            Assert.EqualUShort(0, zero.signExp);
            Assert.EqualULong(0, zero.signif);

            var caseOne = new E(0x3F80, 0xE00000000000010) + new E(0xBBFF, 0xFFFEFFFFBFFFFFFF);
            Assert.EqualUShort(0x3F80, caseOne.signExp);
            Assert.EqualULong(0xE000000000000010, caseOne.signif);
        }

    }
}
