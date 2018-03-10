using SharpFloat.FloatingPoint;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {

    public class TestUShortExpExtF80UI64 {

        [TestCase]
        public void TestExpExtF80UI64() {
            int s(ushort v) => new ExtF80(v, 0).UnsignedExponent;
            Assert.EqualInt(0x0, s(0));
            Assert.EqualInt(0xFF, s(0xFF));
            Assert.EqualInt(0x7FFF, s(0x7FFF));
            Assert.EqualInt(0x00, s(0x8000));
            Assert.EqualInt(0x7FFF, s(0xFFFF));
        }

    }
}
