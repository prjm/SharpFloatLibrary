using XAssert = Xunit.Assert;

namespace SharpFloatTests.Common {
    public static class Assert {

        public static void AreEqual(bool expected, bool value) {
            XAssert.Equal(expected, value);
        }

        public static void AreEqual(ushort expected, ushort value) {
            XAssert.Equal(expected, value);
        }

        public static void AreEqual(ulong expected, ulong value) {
            XAssert.Equal(expected, value);
        }


    }
}
