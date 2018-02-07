using System;
using SharpFloat.Globals;
using SharpFloatTests.Common;

namespace SharpFloatTests.ExtF80 {

    public class TestExtF80RoundPack {

        private SharpFloat.ExtF80.ExtF80 Rp80(bool sign, int exp, ulong sig, ulong sigExtra)
            => SharpFloat.ExtF80.ExtF80.RoundPackToExtF80(sign, exp, sig, sigExtra, 80);


        [TestCase]
        public void TestExtF80RoundPack80() {
            TestExtF80RoundPack80MinimumMagnitude();
            TestExtF80RoundPack80Minimum();
            TestExtF80RoundPack80Maximum();
            TestExtF80RoundPack80NearMaximumMagnitude();
            TestExtF80RoundPack80Odd();
        }

        private void TestExtF80RoundPack80Odd() {
            Settings.RoundingMode = RoundingMode.Odd;
            Assert.EqualULong(0b110, Rp80(true, 0b11, 0b110, 0).signif);
            Assert.EqualULong(0x8003, Rp80(true, 0b11, 0b110, 0).signExp);
            Assert.EqualULong(0b111, Rp80(true, 0b11, 0b110, 0b11).signif);
            Assert.EqualULong(0x3, Rp80(false, 0b11, 0b110, 0).signExp);
        }

        private void TestExtF80RoundPack80NearMaximumMagnitude() {
            Settings.RoundingMode = RoundingMode.NearMaximumMagnitude;
        }

        private void TestExtF80RoundPack80Maximum() {
            Settings.RoundingMode = RoundingMode.Maximum;
        }

        private void TestExtF80RoundPack80Minimum() {
            Settings.RoundingMode = RoundingMode.Minimum;
        }

        private void TestExtF80RoundPack80MinimumMagnitude() {
            Settings.RoundingMode = RoundingMode.MinimumMagnitude;
        }
    }
}
