using System;
using SharpFloat.Globals;
using SharpFloatTests.Common;

namespace SharpFloatTests.FloatingPoint {

    public class TestExtF80RoundPack {

        private SharpFloat.FloatingPoint.ExtF80 Rp80(bool sign, int exp, ulong sig, ulong sigExtra)
            => SharpFloat.FloatingPoint.ExtF80.RoundPack(sign, exp, sig, sigExtra, 80);


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

            // standard values
            Assert.EqualULong(0b110, Rp80(true, 0b11, 0b110, 0).signif);
            Assert.EqualULong(0x8003, Rp80(true, 0b11, 0b110, 0).signExp);
            Assert.EqualULong(0b111, Rp80(true, 0b11, 0b110, 0b11).signif);
            Assert.EqualULong(0x3, Rp80(false, 0b11, 0b110, 0).signExp);

            // large values
            Settings.ClearFlags();
            Assert.EqualUShort(0x7FFE, Rp80(false, 0x7FFF, 0x123, 0x123).signExp);
            Assert.HasFlag(ExceptionFlags.Overflow);
            Assert.HasFlag(ExceptionFlags.Inexact);

            // small values
            Assert.EqualULong(0, Rp80(false, -32765, 0, 0).signExp);
            Assert.EqualULong(1, Rp80(false, -32765, 0x8000000000000001UL, 0).signif);
            Assert.EqualULong(1, Rp80(false, -32765, 0xFFFFFFFFFFFFFFFFUL, 0).signif);
            Assert.EqualULong(0x3800000000000009, Rp80(false, 0, 0x7000000000000010UL, 0x8000000000000001UL).signif);
        }

        private void TestExtF80RoundPack80NearMaximumMagnitude() {
            Settings.RoundingMode = RoundingMode.NearMaximumMagnitude;

            // standard values
            Assert.EqualULong(0b110, Rp80(true, 0b11, 0b110, 0).signif);
            Assert.EqualULong(0x8003, Rp80(true, 0b11, 0b110, 0).signExp);
            Assert.EqualULong(0b110, Rp80(true, 0b11, 0b110, 0b11).signif);
            Assert.EqualULong(0x3, Rp80(false, 0b11, 0b110, 0).signExp);

            // large values
            Settings.ClearFlags();
            Assert.EqualUShort(0x7FFF, Rp80(false, 0x7FFF, 0x123, 0x123).signExp);
            Assert.HasFlag(ExceptionFlags.Overflow);
            Assert.HasFlag(ExceptionFlags.Inexact);

            // small values
            Assert.EqualULong(0, Rp80(false, -32765, 0, 0).signExp);
            Assert.EqualULong(0, Rp80(false, -32765, 0x8000000000000001UL, 0).signif);
            Assert.EqualULong(0, Rp80(false, -32765, 0xFFFFFFFFFFFFFFFFUL, 0).signif);
            Assert.EqualULong(0x3800000000000008, Rp80(false, 0, 0x7000000000000010UL, 0x8000000000000001UL).signif);
        }

        private void TestExtF80RoundPack80Maximum() {
            Settings.RoundingMode = RoundingMode.Maximum;

            // standard values
            Assert.EqualULong(0b110, Rp80(true, 0b11, 0b110, 0).signif);
            Assert.EqualULong(0x8003, Rp80(true, 0b11, 0b110, 0).signExp);
            Assert.EqualULong(0b110, Rp80(true, 0b11, 0b110, 0b11).signif);
            Assert.EqualULong(0x3, Rp80(false, 0b11, 0b110, 0).signExp);

            // large values
            Settings.ClearFlags();
            Assert.EqualUShort(0x7FFF, Rp80(false, 0x7FFF, 0x123, 0x123).signExp);
            Assert.HasFlag(ExceptionFlags.Overflow);
            Assert.HasFlag(ExceptionFlags.Inexact);

            // small values
            Assert.EqualULong(0, Rp80(false, -32765, 0, 0).signExp);
            Assert.EqualULong(1, Rp80(false, -32765, 0x8000000000000001UL, 0).signif);
            Assert.EqualULong(1, Rp80(false, -32765, 0xFFFFFFFFFFFFFFFFUL, 0).signif);
            Assert.EqualULong(0x3800000000000009, Rp80(false, 0, 0x7000000000000010UL, 0x8000000000000001UL).signif);
        }

        private void TestExtF80RoundPack80Minimum() {
            Settings.RoundingMode = RoundingMode.Minimum;

            // standard values
            Assert.EqualULong(0b110, Rp80(true, 0b11, 0b110, 0).signif);
            Assert.EqualULong(0x8003, Rp80(true, 0b11, 0b110, 0).signExp);
            Assert.EqualULong(0b111, Rp80(true, 0b11, 0b110, 0b11).signif);
            Assert.EqualULong(0x3, Rp80(false, 0b11, 0b110, 0).signExp);

            // large values
            Settings.ClearFlags();
            Assert.EqualUShort(0x7FFE, Rp80(false, 0x7FFF, 0x123, 0x123).signExp);
            Assert.HasFlag(ExceptionFlags.Overflow);
            Assert.HasFlag(ExceptionFlags.Inexact);

            // small values
            Assert.EqualULong(0, Rp80(false, -32765, 0, 0).signExp);
            Assert.EqualULong(0, Rp80(false, -32765, 0x8000000000000001UL, 0).signif);
            Assert.EqualULong(0, Rp80(false, -32765, 0xFFFFFFFFFFFFFFFFUL, 0).signif);
            Assert.EqualULong(0x3800000000000008, Rp80(false, 0, 0x7000000000000010UL, 0x8000000000000001UL).signif);

        }

        private void TestExtF80RoundPack80MinimumMagnitude() {
            Settings.RoundingMode = RoundingMode.MinimumMagnitude;

            // standard values
            Assert.EqualULong(0b110, Rp80(true, 0b11, 0b110, 0).signif);
            Assert.EqualULong(0x8003, Rp80(true, 0b11, 0b110, 0).signExp);
            Assert.EqualULong(0b110, Rp80(true, 0b11, 0b110, 0b11).signif);
            Assert.EqualULong(0x3, Rp80(false, 0b11, 0b110, 0).signExp);

            // large values
            Settings.ClearFlags();
            Assert.EqualUShort(0x7FFE, Rp80(false, 0x7FFF, 0x123, 0x123).signExp);
            Assert.HasFlag(ExceptionFlags.Overflow);
            Assert.HasFlag(ExceptionFlags.Inexact);

            // small values
            Assert.EqualULong(0, Rp80(false, -32765, 0, 0).signExp);
            Assert.EqualULong(0, Rp80(false, -32765, 0x8000000000000001UL, 0).signif);
            Assert.EqualULong(0, Rp80(false, -32765, 0xFFFFFFFFFFFFFFFFUL, 0).signif);
            Assert.EqualULong(0x3800000000000008, Rp80(false, 0, 0x7000000000000010UL, 0x8000000000000001UL).signif);

        }
    }
}
