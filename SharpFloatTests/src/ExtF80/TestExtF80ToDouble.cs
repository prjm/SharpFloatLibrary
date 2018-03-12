using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFloat.FloatingPoint;
using SharpFloat.Globals;
using SharpFloatTests.Common;

namespace SharpFloatTests.FloatingPoint {
    public class TestExtF80ToDouble {

        [TestCase]
        public void TestToDouble() {
            var a = new ExtF80(0x4400, 0xE140D8876452D3DD);
            var v = a.ToDouble(RoundingMode.NearEven);
            var d = unchecked((long)0x7FF0000000000000);
            Assert.EqualDouble(BitConverter.Int64BitsToDouble(d), v);

            Settings.RoundingMode = RoundingMode.NearEven;
            a = new ExtF80(0xC04C, 0xFFFFFFFFFFFFBFF7);
            v = a.ToDouble(RoundingMode.NearEven);
            d = unchecked((long)0xC4CFFFFFFFFFFFF8);
            Assert.EqualDouble(BitConverter.Int64BitsToDouble(d), v);
        }
    }
}
