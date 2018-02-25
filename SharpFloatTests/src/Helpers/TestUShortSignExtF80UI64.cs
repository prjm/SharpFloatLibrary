using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {

    public class TestUShortSignExtF80UI64 {

        [TestCaseAttribute]
        public void TestSignExtF80UI64() {
            bool s(ushort v) => new SharpFloat.FloatingPoint.ExtF80(v, 0).IsNegative;
            Assert.EqualBool(false, s(0));
            Assert.EqualBool(false, s(0xFF));
            Assert.EqualBool(false, s(0x7FFF));
            Assert.EqualBool(true, s(0x8000));
            Assert.EqualBool(true, s(0xFFFF));
        }

    }
}
