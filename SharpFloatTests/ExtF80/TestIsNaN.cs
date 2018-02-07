using SharpFloatTests.Common;
using static SharpFloat.ExtF80.ExtF80;

namespace SharpFloatTests.ExtF80 {

    public class TestIsNaN {

        [TestCase]
        public void TestIsNaNValue() {
            Assert.AreEqualBool(false, IsNaNExtF80UI(0, 0));
            Assert.AreEqualBool(false, IsNaNExtF80UI(0x7FF0, 0x7FFFFFFFFFFFFFFF));
            Assert.AreEqualBool(true, IsNaNExtF80UI(0x7FFF, 0x7FFFFFFFFFFFFFFF));
            Assert.AreEqualBool(true, IsNaNExtF80UI(0x7FFF, 0x8FFFFFFFFFFFFFFF));
        }

        [TestCase]
        public void TestIsSignalingNaNValue() {
            Assert.AreEqualBool(false, IsSigNaNExtF80UI(0, 0));
            Assert.AreEqualBool(false, IsSigNaNExtF80UI(0, 0));
            Assert.AreEqualBool(false, IsSigNaNExtF80UI(0x7FFF, 0x4000000000000000));
            Assert.AreEqualBool(false, IsSigNaNExtF80UI(0x7FF0, 0x3FFFFFFFFFFFFFFF));
            Assert.AreEqualBool(true, IsSigNaNExtF80UI(0x7FFF, 0x3FFFFFFFFFFFFFFF));
            Assert.AreEqualBool(true, IsSigNaNExtF80UI(0x7FFF, 0x3FFFFFFFFFFFFFF0));
        }
    }
}
