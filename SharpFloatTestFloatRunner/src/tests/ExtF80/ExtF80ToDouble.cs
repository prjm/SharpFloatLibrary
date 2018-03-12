using System;
using SharpFloat.Globals;
using SharpFloatTestFloatRunner.Common;

namespace SharpFloatTestFloatRunner.Tests.ExtF80 {

    /// <summary>
    ///     test ExtF80 to double
    /// </summary>
    public class ExtF80ToDoubleTest : TestEntry {

        /// <summary>
        ///     test name
        /// </summary>
        public override string Name
            => "to_f64";

        /// <summary>
        ///     process a line of the test file: add two ExtF80 and compare
        /// </summary>
        /// <param name="data">test data</param>
        protected override void ProcessLineInTest(string[] data) {
            var value = ToExtF80(data[0]);
            var expected = ToDouble(data[1]);
            var actual = value.ToDouble(Settings.RoundingMode);
            AssertEqual(BitConverter.DoubleToInt64Bits(expected), BitConverter.DoubleToInt64Bits(actual));
        }
    }
}