using SharpFloatTests.Common;
using SharpFloat.Helpers;

namespace SharpFloatTests.Helpers {

    public class TestUShortPackToExtF80UI64 {

        [TestCase]
        public void TestPackToExtF80UI64() {
            Assert.EqualUShort(0, 0.PackToExtF80(false));
            Assert.EqualUShort(0xFF, 0xFF.PackToExtF80(false));
            Assert.EqualUShort(0x7FFF, 0x7FFF.PackToExtF80(false));
            Assert.EqualUShort(0x8000, 0.PackToExtF80(true));
            Assert.EqualUShort(0x80FF, 0xFF.PackToExtF80(true));
            Assert.EqualUShort(0xFFFF, 0x7FFF.PackToExtF80(true));
            Assert.EqualUShort(0, ((ushort)0).PackToExtF80UI64(false));
            Assert.EqualUShort(0xFF, ((ushort)0xFF).PackToExtF80UI64(false));
            Assert.EqualUShort(0x7FFF, ((ushort)0x7FFF).PackToExtF80UI64(false));
            Assert.EqualUShort(0x8000, ((ushort)0).PackToExtF80UI64(true));
            Assert.EqualUShort(0x80FF, ((ushort)0xFF).PackToExtF80UI64(true));
            Assert.EqualUShort(0xFFFF, ((ushort)0x7FFF).PackToExtF80UI64(true));

        }

    }
}

