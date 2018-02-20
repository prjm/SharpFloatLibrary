using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {

    public class TestAdd128 {

        [TestCase]
        public void Test128Add() {
            var v1 = new UInt128(0, 3);
            var v2 = new UInt128(0, 4);
            var v3 = v1 + v2;
            Assert.EqualULong(7, v3.v0);
            Assert.EqualULong(0, v3.v64);

            v1 = new UInt128(1, 3);
            v2 = new UInt128(1, 4);
            v3 = v1 + v2;
            Assert.EqualULong(7, v3.v0);
            Assert.EqualULong(2, v3.v64);

            v1 = new UInt128(1, 0xCCCCCCCCCCCCCCCC);
            v2 = new UInt128(1, 0xDDDDDDDDDDDDDDDD);
            v3 = v1 + v2;
            Assert.EqualULong(0xAAAAAAAAAAAAAAA9, v3.v0);
            Assert.EqualULong(3, v3.v64);
        }

    }
}
