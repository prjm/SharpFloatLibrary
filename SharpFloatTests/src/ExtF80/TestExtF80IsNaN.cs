using SharpFloat.FloatingPoint;
using SharpFloatTests.Common;
using static SharpFloat.FloatingPoint.ExtF80;

namespace SharpFloatTests.FloatingPoint {

    public class TestExtF80IsNaN {

        [TestCase]
        public void TestIsNaNValue() {
            bool s(ushort a, ulong b) => new ExtF80(a, b).IsNaN;
            Assert.EqualBool(false, s(0, 0));
            Assert.EqualBool(false, s(0x7FF0, 0x7FFFFFFFFFFFFFFF));
            Assert.EqualBool(true, s(0x7FFF, 0x7FFFFFFFFFFFFFFF));
            Assert.EqualBool(true, s(0x7FFF, 0x8FFFFFFFFFFFFFFF));
        }

        [TestCase]
        public void TestIsSignalingNaNValue() {
            bool s(ushort a, ulong b) => new ExtF80(a, b).IsSignalingNaN;
            Assert.EqualBool(false, s(0, 0));
            Assert.EqualBool(false, s(0, 0));
            Assert.EqualBool(false, s(0x7FFF, 0x4000000000000000));
            Assert.EqualBool(false, s(0x7FF0, 0x3FFFFFFFFFFFFFFF));
            Assert.EqualBool(true, s(0x7FFF, 0x3FFFFFFFFFFFFFFF));
            Assert.EqualBool(true, s(0x7FFF, 0x3FFFFFFFFFFFFFF0));
        }
    }
}
