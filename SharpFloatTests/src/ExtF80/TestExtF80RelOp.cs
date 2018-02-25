using SharpFloatTests.Common;
using E = SharpFloat.FloatingPoint.ExtF80;

namespace SharpFloatTests.src.ExtF80 {
    public class TestExtF80RelOp {


        [TestCase]
        public void TestExtF80Lt() {
            var v1 = new E(0, 0);
            var v2 = new E(0, 0);
            Assert.EqualBool(false, v1 < v2);
        }
    }
}
