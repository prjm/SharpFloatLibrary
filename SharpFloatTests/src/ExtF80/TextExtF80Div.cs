using SharpFloat.Globals;
using SharpFloatTests.Common;

namespace SharpFloatTests.FloatingPoint {

    public class TextExtF80Div {

        [TestCase]
        public void TestDivisions() {
            Settings.RoundingMode = RoundingMode.NearEven;
            var v1 = new SharpFloat.FloatingPoint.ExtF80(0xB687, 0x801003FFFFFFFFFE);
            var v2 = new SharpFloat.FloatingPoint.ExtF80(0xC04C, 0xFFFFFFFFFFFFBFF7);
            var v3 = v1 / v2;
            Assert.EqualUShort(0x3639, v3.signExp);
            Assert.EqualULong(0x8010040000002007, v3.signif);

            Settings.RoundingMode = RoundingMode.Minimum;
            v1 = new SharpFloat.FloatingPoint.ExtF80(0x0000, 0x0000000000000001UL);
            v2 = new SharpFloat.FloatingPoint.ExtF80(0xC000, 0x8000000000000000UL);
            v3 = v1 / v2;
            Assert.EqualUShort(0x8000, v3.signExp);
            Assert.EqualULong(0x0000000000000001UL, v3.signif);
        }
    }
}
