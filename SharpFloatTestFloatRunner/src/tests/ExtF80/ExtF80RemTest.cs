using SharpFloatTestFloatRunner.Common;

namespace SharpFloatTestFloatRunner.Tests.ExtF80 {

    /// <summary>
    ///     test multiplication
    /// </summary>
    public class ExtF80RemTest : TestEntry {

        /// <summary>
        ///     test name
        /// </summary>
        public override string Name
            => "rem";

        /// <summary>
        ///     process a line of the test file: add two ExtF80 and compare
        /// </summary>
        /// <param name="data">test data</param>
        protected override void ProcessLineInTest(string[] data) {
            var op1 = ToExtF80(data[0]);
            var op2 = ToExtF80(data[1]);
            var expected = ToExtF80(data[2]);
            var actual = SharpFloat.FloatingPoint.ExtF80.Remainder(op1, op2);
            AssertEqual(expected, actual);
        }
    }
}
