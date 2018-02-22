using SharpFloat.Globals;
using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {

    public class TestUInt128PropagateNaNExtF80UI {

        [TestCase]
        public void TestUInt128PropagateNaNExt() {
            ushort s64 = 0x7FFF; // signaling nan
            ulong s0 = 0x3FFFFFFFFFFFFFFF;
            ushort x64 = 0x10; // other value 1
            ulong x0 = 0x9999;
            ushort y64 = 0x09; // other value 2
            ulong y0 = 0x9998;
            ushort ns64 = 0x7FFF; // non signaling nan
            ulong ns0 = 0x4000000000000000;

            SharpFloat.ExtF80.ExtF80 f(ushort v64, ulong v0) {
                return new SharpFloat.ExtF80.ExtF80(v64, v0);
            };

            Settings.ClearFlags();
            Assert.EqualULong(0x7FFF, UInt128.PropagateNaNExtF80UI(f(s64, s0), f(x64, x0)).signExp);
            Assert.EqualByte((byte)ExceptionFlags.Invalid, (byte)(Settings.Flags & ExceptionFlags.Invalid));
            Settings.ClearFlags();

            Settings.ClearFlags();
            Assert.EqualULong(0xC000000000000000, UInt128.PropagateNaNExtF80UI(f(s64, s0), f(ns64, ns0)).signif);
            Assert.EqualByte((byte)ExceptionFlags.Invalid, (byte)(Settings.Flags & ExceptionFlags.Invalid));
            Settings.ClearFlags();

            Settings.ClearFlags();
            Assert.EqualULong(0x7FFF, UInt128.PropagateNaNExtF80UI(f(x64, x0), f(s64, s0)).signExp);
            Assert.EqualByte((byte)ExceptionFlags.Invalid, (byte)(Settings.Flags & ExceptionFlags.Invalid));
            Settings.ClearFlags();

            Settings.ClearFlags();
            Assert.EqualULong(0xC000000000000000, UInt128.PropagateNaNExtF80UI(f(ns64, ns0), f(s64, s0)).signif);
            Assert.EqualByte((byte)ExceptionFlags.Invalid, (byte)(Settings.Flags & ExceptionFlags.Invalid));
            Settings.ClearFlags();

            Settings.ClearFlags();
            Assert.EqualULong(0x10, UInt128.PropagateNaNExtF80UI(f(x64, x0), f(y64, y0)).signExp);
            Assert.EqualByte((byte)ExceptionFlags.None, (byte)(Settings.Flags & ExceptionFlags.Invalid));
            Settings.ClearFlags();

            Settings.ClearFlags();
            Assert.EqualULong(0x10, UInt128.PropagateNaNExtF80UI(f(y64, y0), f(x64, x0)).signExp);
            Assert.EqualByte((byte)ExceptionFlags.None, (byte)(Settings.Flags & ExceptionFlags.Invalid));
            Settings.ClearFlags();

            Settings.ClearFlags();
            Assert.EqualULong(0xC000000000009999UL, UInt128.PropagateNaNExtF80UI(f(x64, x0), f(x64, y0)).signif);
            Assert.EqualByte((byte)ExceptionFlags.None, (byte)(Settings.Flags & ExceptionFlags.Invalid));
            Settings.ClearFlags();


            Settings.ClearFlags();
            Assert.EqualULong(0xC000000000009999UL, UInt128.PropagateNaNExtF80UI(f(x64, y0), f(x64, x0)).signif);
            Assert.EqualByte((byte)ExceptionFlags.None, (byte)(Settings.Flags & ExceptionFlags.Invalid));
            Settings.ClearFlags();
        }

    }
}
