using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {

    public class TestUShortSignExtF80UI64 {

        [TestCaseAttribute]
        public void TestSignExtF80UI64() {
            Assert.EqualBool(false, ((ushort)0).SignExtF80UI64());
            Assert.EqualBool(false, ((ushort)0xFF).SignExtF80UI64());
            Assert.EqualBool(false, ((ushort)0x7FFF).SignExtF80UI64());
            Assert.EqualBool(true, ((ushort)0x8000).SignExtF80UI64());
            Assert.EqualBool(true, ((ushort)0xFFFF).SignExtF80UI64());
        }

    }
}
