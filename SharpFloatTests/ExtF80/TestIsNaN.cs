using SharpFloatTests.Common;
using static SharpFloat.ExtF80.ExtF80;

namespace SharpFloatTests.ExtF80 {

    public class TestIsNaN {

        [TestCase]
        public void TestIsNaNValue() {
            Assert.AreEqual(false, IsNaNExtF80UI(0, 0));
            Assert.AreEqual(false, IsNaNExtF80UI(0x7FF0, 0x7FFFFFFFFFFFFFFF));
            Assert.AreEqual(true, IsNaNExtF80UI(0x7FFF, 0x7FFFFFFFFFFFFFFF));
            Assert.AreEqual(true, IsNaNExtF80UI(0x7FFF, 0x8FFFFFFFFFFFFFFF));
        }

        [TestCase]
        public void TestIsSignalingNaNValue() {
            Assert.AreEqual(false, IsSigNaNExtF80UI(0, 0));
            Assert.AreEqual(false, IsSigNaNExtF80UI(0, 0));
            Assert.AreEqual(false, IsSigNaNExtF80UI(0x7FFF, 0x4000000000000000));
            Assert.AreEqual(false, IsSigNaNExtF80UI(0x7FF0, 0x3FFFFFFFFFFFFFFF));
            Assert.AreEqual(true, IsSigNaNExtF80UI(0x7FFF, 0x3FFFFFFFFFFFFFFF));
            Assert.AreEqual(true, IsSigNaNExtF80UI(0x7FFF, 0x3FFFFFFFFFFFFFF0));
        }
    }
}
