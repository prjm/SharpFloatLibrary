using SharpFloat.FloatingPoint;
using SharpFloatTests.Common;

namespace SharpFloatTests.FloatingPoint {
    public class ExtF80Misc {

        [TestCase]
        public void TestLicense() {
            Assert.EqualBool(string.IsNullOrEmpty(ExtF80.License), false);
        }

        [TestCase]
        public void TestFromString() {
            Assert.EqualExtF80(ExtF80.Zero, "0");
            Assert.EqualExtF80(ExtF80.Zero, "0.0000");
            Assert.EqualExtF80(ExtF80.Zero, "0.0000E88888");
        }

        [TestCase]
        public void TestFromInt() {
            Assert.EqualExtF80(ExtF80.Zero, 0U);
            Assert.EqualExtF80(new ExtF80(0x3FFF, 0x8000000000000000), 1U);
            Assert.EqualExtF80(ExtF80.Zero, 0);
            Assert.EqualExtF80(new ExtF80(0x3FFF, 0x8000000000000000), 1);
            Assert.EqualExtF80(new ExtF80(0xBFFF, 0x8000000000000000), -1);
        }

        [TestCase]
        public void TestFromLong() {
            Assert.EqualExtF80(ExtF80.Zero, 0UL);
            Assert.EqualExtF80(new ExtF80(0x3FFF, 0x8000000000000000), 1UL);
            Assert.EqualExtF80(ExtF80.Zero, 0L);
            Assert.EqualExtF80(new ExtF80(0x3FFF, 0x8000000000000000), 1L);
            Assert.EqualExtF80(new ExtF80(0xBFFF, 0x8000000000000000), -1L);
        }

        [TestCase]
        public void TestNegate() {
            ExtF80 a = 5;
            ExtF80 b = -5;
            Assert.EqualExtF80(a, -b, false);
            Assert.EqualExtF80(ExtF80.Zero, -ExtF80.NegativeZero, false);
        }

        [TestCase]
        public void TestRoundtripFormatting() {
            Assert.EqualAfterRoundTripFormatting(ExtF80.Zero);
            Assert.EqualAfterRoundTripFormatting(new ExtF80(0x3FFF, 0x8000000000000000));
            Assert.EqualAfterRoundTripFormatting(new ExtF80(0x4000, 0x8000000000000000));
            Assert.EqualAfterRoundTripFormatting(new ExtF80(0x3687, 0x801003FFFFFFFFFE));
            Assert.EqualAfterRoundTripFormatting(new ExtF80(0xB687, 0x801003FFFFFFFFFE));
            Assert.EqualAfterRoundTripFormatting(new ExtF80(0xC04C, 0xFFFFFFFFFFFFBFF7));
            Assert.EqualAfterRoundTripFormatting(new ExtF80(0x3FE3, 0xFFFDFFFFFFFFFFF0));
            Assert.EqualAfterRoundTripFormatting(new ExtF80(0x0000, 0x7FFFFFBFFFFFFFFB));
            Assert.EqualAfterRoundTripFormatting(new ExtF80(0x0001, 0x80000000003FFFFD));
            Assert.EqualAfterRoundTripFormatting(ExtF80.MinValue);
        }

    }
}
