using SharpFloatTests.Common;

namespace SharpFloatTests.src.ExtF80 {

    public class TextExtF80Div {

        [TestCase]
        public void TestDivisions() {
            var v1 = new SharpFloat.ExtF80.ExtF80(0xB687, 0x801003FFFFFFFFFE);
            var v2 = new SharpFloat.ExtF80.ExtF80(0xC04C, 0xFFFFFFFFFFFFBFF7);
            var v3 = v1 / v2;
            Assert.EqualUShort(0x3639, v3.signExp);
            Assert.EqualULong(0x8010040000002007, v3.signif);
        }
    }
}
