using SharpFloatTests.Common;

using E = SharpFloat.ExtF80.ExtF80;

namespace SharpFloatTests.ExtF80 {

    public class TestExtF80Add {

        [TestCase]
        public void TestAddSameSignSameExponent() {
            var zero = ((new E(0, 0)) + (new E(0, 0)));
            Assert.EqualUShort(0, zero.signExp);
            Assert.EqualULong(0, zero.signif);
        }

    }
}
