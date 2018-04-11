using SharpFloat.Globals;
using SharpFloatTestFloatRunner.Common;

namespace SharpFloatTestFloatRunner.Tests.I64 {

    /// <summary>
    ///     test ExtF80 addition
    /// </summary>
    public class I64ToExtF80Test : TestEntry {

        /// <summary>
        ///     test name
        /// </summary>
        public override string Name
            => "to_extF80";

        /// <summary>
        ///     process a line of the test file: add two ExtF80 and compare
        /// </summary>
        /// <param name="data">test data</param>
        protected override void ProcessLineInTest(string[] data) {
            var value = ToLong(data[0]);
            var expected = ToExtF80(data[1]);
            var actual = (SharpFloat.FloatingPoint.ExtF80)value;
            AssertEqual(expected, actual);
        }
    }
}