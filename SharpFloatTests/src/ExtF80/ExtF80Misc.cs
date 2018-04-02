using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFloat.FloatingPoint;
using SharpFloatTests.Common;

namespace SharpFloatTests.FloatingPoint {
    public class ExtF80Misc {

        [TestCase]
        public void TestLicense() {
            Assert.EqualBool(string.IsNullOrEmpty(ExtF80.License), false);
        }

        [TestCase]
        public void TestFromString() {
            Assert.EqualExtF80(ExtF80.Zero, "0");
            Assert.EqualExtF80(ExtF80.Zero, "0.0000");
            Assert.EqualExtF80(ExtF80.Zero, "0.0000E88888");
        }

    }
}
