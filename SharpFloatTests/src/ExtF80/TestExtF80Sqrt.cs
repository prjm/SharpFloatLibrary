using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFloat.FloatingPoint;
using SharpFloat.Globals;
using SharpFloatTests.Common;

namespace SharpFloatTests.FloatingPoint {
    public class TestExtF80Sqrt {

        [TestCase]
        public void TestSqr() {
            Settings.RoundingMode = RoundingMode.NearEven;
            var r = new ExtF80(0x3FC7, 0xAC857F319EDE38F7);
            var q = r.Sqrt();
            Assert.EqualUShort(0x3FE3, q.signExp);
            Assert.EqualULong(0x949A47748D33595B, q.signif);
        }
    }
}
