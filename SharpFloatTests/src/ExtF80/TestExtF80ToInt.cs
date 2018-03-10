using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFloat.FloatingPoint;
using SharpFloat.Globals;
using SharpFloatTests.Common;

namespace SharpFloatTests.FloatingPoint {
    public class TestExtF80ToInt {

        [TestCase]
        public void TestFloatToInt() {
            Settings.RoundingMode = RoundingMode.MinimumMagnitude;
            var a = new ExtF80(0x4000, 0xB000000000000000);
            var v = (int)a;
            Assert.EqualInt(2, v);

            Settings.RoundingMode = RoundingMode.NearEven;
            a = new ExtF80(0x0000, 0x7FFFFBFFFFFFFFFB);
            v = a.ToInt(RoundingMode.NearEven, true);
            Assert.EqualInt(0, v);

            Settings.RoundingMode = RoundingMode.NearEven;
            a = new ExtF80(0xC01E, 0x8000000000008007);
            v = a.ToInt(RoundingMode.NearEven, true);
            Assert.EqualInt(-2147483648, v);
        }

        [TestCase]
        public void TestFloatToLong() {
            Settings.RoundingMode = RoundingMode.NearEven;
            var a = new ExtF80(0xB687, 0x801003FFFFFFFFFE);
            var v = a.ToLong(RoundingMode.NearEven, true);
            Assert.EqualLong(0L, v);
        }
    }
}
