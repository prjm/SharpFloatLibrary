using SharpFloatTests.Common;
using E = SharpFloat.FloatingPoint.ExtF80;

namespace SharpFloatTests.ExtF80 {
    public class ExtF80Eq {

        [TestCase]
        public void TestEquals() {
            var e1 = new E(1, 2);
            var e2 = new E(1, 2);
            var e3 = new E(1, 3);
            var e4 = new E(2, 2);

            Assert.IsTrue(e1 == e2);
            Assert.IsTrue(e2 == e1);
            Assert.IsTrue(e1 != e3);
            Assert.IsTrue(e1 != e4);
            Assert.IsTrue(e3 != e1);
            Assert.IsTrue(e4 != e1);

            var nan1 = new E((ushort)0x7FFFU, 0x8888888888888888UL);
            Assert.IsTrue(e1 != nan1);
            Assert.IsTrue(!(e1 == nan1));
        }

    }
}
