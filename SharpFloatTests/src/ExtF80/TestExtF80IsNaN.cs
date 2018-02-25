using SharpFloatTests.Common;
using static SharpFloat.FloatingPoint.ExtF80;

namespace SharpFloatTests.FloatingPoint {

    public class TestExtF80IsNaN {

        [TestCase]
        public void TestIsNaNValue() {
            Assert.EqualBool(false, IsNaNExtF80UI(0, 0));
            Assert.EqualBool(false, IsNaNExtF80UI(0x7FF0, 0x7FFFFFFFFFFFFFFF));
            Assert.EqualBool(true, IsNaNExtF80UI(0x7FFF, 0x7FFFFFFFFFFFFFFF));
            Assert.EqualBool(true, IsNaNExtF80UI(0x7FFF, 0x8FFFFFFFFFFFFFFF));
        }

        [TestCase]
        public void TestIsSignalingNaNValue() {
            Assert.EqualBool(false, IsSigNaNExtF80UI(new SharpFloat.FloatingPoint.ExtF80(0, 0)));
            Assert.EqualBool(false, IsSigNaNExtF80UI(new SharpFloat.FloatingPoint.ExtF80(0, 0)));
            Assert.EqualBool(false, IsSigNaNExtF80UI(new SharpFloat.FloatingPoint.ExtF80(0x7FFF, 0x4000000000000000)));
            Assert.EqualBool(false, IsSigNaNExtF80UI(new SharpFloat.FloatingPoint.ExtF80(0x7FF0, 0x3FFFFFFFFFFFFFFF)));
            Assert.EqualBool(true, IsSigNaNExtF80UI(new SharpFloat.FloatingPoint.ExtF80(0x7FFF, 0x3FFFFFFFFFFFFFFF)));
            Assert.EqualBool(true, IsSigNaNExtF80UI(new SharpFloat.FloatingPoint.ExtF80(0x7FFF, 0x3FFFFFFFFFFFFFF0)));
        }
    }
}
