using SharpFloat.Helpers;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {

    public class TestBigInt {

        [TestCase]
        public void TestBasics() {
            var b = new BigInt();
            Assert.IsTrue(b.Zero);

            b = new BigInt(5);
            Assert.IsTrue(!b.Zero);
            Assert.EqualUInt(5, b.AsUInt);

            b = new BigInt(0xFFFFFFFFFFFFFFFFUL);
            Assert.IsTrue(!b.Zero);
            Assert.EqualULong(0xFFFFFFFFFFFFFFFFUL, b.AsULong);

            b.Length = 1;
            Assert.IsTrue(!b.Zero);
            Assert.EqualUInt(0xFFFF_FFFF, b.AsUInt);

            b.Length = 2;
            Assert.IsTrue(!b.Zero);
            Assert.EqualULong(0x0000_0000_FFFF_FFFF, b.AsULong);

        }

        [TestCase]
        public void TestAdd() {
            var a = new BigInt(5);
            var b = new BigInt(4);
            var c = new BigInt();
            BigInt.Add(c, a, b);
            Assert.EqualUInt(9, c.AsUInt);

            a = new BigInt(0xFFFF_FFFF);
            b = new BigInt(1);
            c = new BigInt();
            BigInt.Add(c, a, b);
            Assert.EqualULong(0xFFFF_FFFFUL + 1, c.AsULong);
        }

        [TestCase]
        public void TestMultiply() {
            var a = new BigInt(5);
            var b = new BigInt(4);
            var c = new BigInt();
            BigInt.Multiply(c, a, b);
            Assert.EqualUInt(20, c.AsUInt);

            a = new BigInt(0xFFFF_FFFF);
            b = new BigInt(3);
            c = new BigInt();
            BigInt.Multiply(c, a, b);
            Assert.EqualULong(0xFFFF_FFFFUL * 3, c.AsULong);

            a = new BigInt(5);
            c = new BigInt();
            BigInt.Multiply(c, a, 4);
            Assert.EqualUInt(20, c.AsUInt);

            a = new BigInt(0xFFFF_FFFF);
            c = new BigInt();
            BigInt.Multiply(c, a, 3);
            Assert.EqualULong(0xFFFF_FFFFUL * 3, c.AsULong);

            a = new BigInt(5);
            c = new BigInt();
            BigInt.Multiply2(c, a);
            Assert.EqualUInt(10, c.AsUInt);

            a = new BigInt(0xFFFF_FFFF);
            c = new BigInt();
            BigInt.Multiply2(c, a);
            Assert.EqualULong(0xFFFF_FFFFUL << 1, c.AsULong);

            a = new BigInt(5);
            BigInt.Multiply2(a);
            Assert.EqualUInt(10, a.AsUInt);

            a = new BigInt(0xFFFF_FFFF);
            BigInt.Multiply2(a);
            Assert.EqualULong(0xFFFF_FFFFUL << 1, c.AsULong);

            a = new BigInt(5);
            BigInt.Multiply10(a);
            Assert.EqualUInt(50, a.AsUInt);

            a = new BigInt(0xFFFF_FFFF);
            BigInt.Multiply10(a);
            Assert.EqualULong(0xFFFF_FFFFUL * 10UL, a.AsULong);
        }

        [TestCase]
        public void TestPower() {
            var a = new BigInt(5);
            BigInt.Pow10(a, 3);
            Assert.EqualUInt(1000, a.AsUInt);

            a = new BigInt(5);
            var b = new BigInt();
            BigInt.MultiplyPow10(b, a, 3);
            Assert.EqualUInt(5000, b.AsUInt);

            a = new BigInt();
            BigInt.Pow2(a, 32);
            Assert.EqualULong(4294967296UL, a.AsULong);

        }

        [TestCase]
        public void TestShift() {
            var a = new BigInt(0xFFFF_FFFF);
            BigInt.ShiftLeft(a, 32);
            Assert.EqualULong(0xFFFF_FFFF_0000_0000, a.AsULong);

            a = new BigInt(0x0000_FFFF);
            BigInt.ShiftLeft(a, 4);
            Assert.EqualULong(0x0000_0000_000F_FFF0, a.AsULong);
        }
    }
}
