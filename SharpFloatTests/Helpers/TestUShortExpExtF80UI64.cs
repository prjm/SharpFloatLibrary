using SharpFloatTests.Common;
using SharpFloat.Helpers;

namespace SharpFloatTests.Helpers {

    public class TestUShortExpExtF80UI64 {

        [TestCaseAttribute]
        public void TestExpExtF80UI64() {
            Assert.AreEqual(0x0, ((ushort)0).ExpExtF80UI64());
            Assert.AreEqual(0xFF, ((ushort)0xFF).ExpExtF80UI64());
            Assert.AreEqual(0x7FFF, ((ushort)0x7FFF).ExpExtF80UI64());
            Assert.AreEqual(0x00, ((ushort)0x8000).ExpExtF80UI64());
            Assert.AreEqual(0x7FFF, ((ushort)0xFFFF).ExpExtF80UI64());
        }

    }
}
