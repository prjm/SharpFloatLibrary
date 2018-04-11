using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
