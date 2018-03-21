using System;
using SharpFloat.Globals;
using XAssert = Xunit.Assert;

namespace SharpFloatTests.Common {

    public static class Assert {

        public static void EqualBool(bool expected, bool value)
            => XAssert.Equal(expected, value);

        public static void EqualUShort(ushort expected, ushort value)
            => XAssert.Equal(expected, value);

        public static void EqualULong(ulong expected, ulong value)
            => XAssert.Equal(expected, value);

        public static void IsTrue(bool value)
            => XAssert.True(value);

        public static void EqualByte(byte expected, byte value)
            => XAssert.Equal(expected, value);

        public static void EqualInt(int expected, int value)
            => XAssert.Equal(expected, value);

        public static void EqualUInt(uint expected, uint value)
            => XAssert.Equal(expected, value);

        public static void EqualLong(long expected, long value)
            => XAssert.Equal(expected, value);

        public static void EqualDouble(double expected, double value)
            => XAssert.Equal(expected, value);

        public static void HasFlag(ExceptionFlags flag)
            => EqualByte((byte)flag, (byte)(Settings.Flags & flag));

        public static void EqualString(string expected, string value)
            => XAssert.Equal(expected, value);


    }
}
