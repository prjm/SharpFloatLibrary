using SharpFloat.Globals;
using SharpFloatTests.Common;

using E = SharpFloat.FloatingPoint.ExtF80;

namespace SharpFloatTests.FloatingPoint {

    public class TestExtF80Add {

        [TestCase]
        public void TestAdditions() {
            var zero = new E(0, 0) + new E(0, 0);
            Assert.EqualUShort(0, zero.signExp);
            Assert.EqualULong(0, zero.signif);

            Settings.RoundingMode = RoundingMode.NearEven;
            var caseOne = new E(0x4002, 0x8000400010000000) + new E(0xC002, 0x8040000FFFFFFFFF);
            Assert.EqualUShort(0xBFF8, caseOne.signExp);
            Assert.EqualULong(0xFF003FBFFFFFFC00, caseOne.signif);

            Settings.RoundingMode = RoundingMode.NearEven;
            var caseTwo = new E(0x403E, 0xFFFFFFFFFFFFFFD0) + new E(0xC03F, 0xE22ECB436FA3CAD3);
            Assert.EqualUShort(0xC03E, caseTwo.signExp);
            Assert.EqualULong(0xC45D9686DF4795D6, caseTwo.signif);

            Settings.RoundingMode = RoundingMode.MinimumMagnitude;
            var caseThree = new E(0x403E, 0xFFFFFFFFFFFFFFD0) + new E(0xC03F, 0xE22ECB436FA3CAD3);
            Assert.EqualUShort(0xC03E, caseThree.signExp);
            Assert.EqualULong(0xC45D9686DF4795D6, caseThree.signif);
        }

    }
}
