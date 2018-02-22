using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {

    public class TestUInt128 {

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

        [TestCase]
        public void Test128Sub() {
            Assert.EqualULong(220, UInt128.Sub128(0, 320, 0, 100).v0);
            Assert.EqualULong(18446744073709551516, UInt128.Sub128(0, 0, 0, 100).v0);
            Assert.EqualULong(98, UInt128.Sub128(99, 320, 1, 100).v64);
        }

        [TestCase]
        public void Test128Le() {
            var v1 = new UInt128(0, 1);
            var v2 = new UInt128(0, 2);
            var v3 = new UInt128(1, 0);
            var v4 = new UInt128(1, 2);

#pragma warning disable CS1718 // Comparison made to same variable
            Assert.IsTrue(value: v1 <= v1);
            Assert.IsTrue(value: v1 >= v1);
#pragma warning restore CS1718 // Comparison made to same variable

            Assert.IsTrue(v1 <= v2);
            Assert.IsTrue(v1 < v2);
            Assert.IsTrue(v1 < v3);
            Assert.IsTrue(v1 < v4);

            Assert.IsTrue(v2 >= v1);
            Assert.IsTrue(v2 > v1);
            Assert.IsTrue(v3 > v1);
            Assert.IsTrue(v4 > v1);
        }


    }
}
