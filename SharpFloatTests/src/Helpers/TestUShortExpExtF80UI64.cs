using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {

    public class TestUShortExpExtF80UI64 {

        [TestCase]
        public void TestExpExtF80UI64() {
            ushort s(ushort v) => new SharpFloat.FloatingPoint.ExtF80(v, 0).UnsignedExponent;
            Assert.EqualUShort(0x0, s(0));
            Assert.EqualUShort(0xFF, s(0xFF));
            Assert.EqualUShort(0x7FFF, s(0x7FFF));
            Assert.EqualUShort(0x00, s(0x8000));
            Assert.EqualUShort(0x7FFF, s(0xFFFF));
        }

    }
}
