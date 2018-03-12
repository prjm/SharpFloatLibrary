using System;
using SharpFloat.Globals;
using SharpFloat.Helpers;
using SharpFloatTestFloatRunner.Common;

namespace SharpFloatTestFloatRunner.Tests.ExtF80 {

    /// <summary>
    ///     test ExtF80 to double
    /// </summary>
    public class ExtF80ToFloatTest : TestEntry {

        /// <summary>
        ///     test name
        /// </summary>
        public override string Name
            => "to_f32";

        /// <summary>
        ///     process a line of the test file: add two ExtF80 and compare
        /// </summary>
        /// <param name="data">test data</param>
        protected override void ProcessLineInTest(string[] data) {
            var value = ToExtF80(data[0]);
            var expected = ToFloat(data[1]);
            var actual = value.ToFloat(Settings.RoundingMode);
            AssertEqual(FloatHelpers.SingleToInt32Bits(expected), FloatHelpers.SingleToInt32Bits(actual));
        }
    }
}