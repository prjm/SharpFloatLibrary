using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XAssert = Xunit.Assert;

namespace SharpFloatTests.Common {
    public static class Assert {

        public static void AreEqual(bool expected, bool value) {
            XAssert.Equal(expected, value);
        }

        public static void AreEqual(ushort expected, ushort value) {
            XAssert.Equal(expected, value);
        }



    }
}
