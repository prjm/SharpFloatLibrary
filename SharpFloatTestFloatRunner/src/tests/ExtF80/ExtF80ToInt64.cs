﻿using SharpFloat.Globals;
using SharpFloatTestFloatRunner.Common;

namespace SharpFloatTestFloatRunner.Tests.ExtF80 {

    /// <summary>
    ///     test ExtF80 addition
    /// </summary>
    public class ExtF80ToInt64Test : TestEntry {

        /// <summary>
        ///     test name
        /// </summary>
        public override string Name
            => "to_i64";

        /// <summary>
        ///     process a line of the test file: add two ExtF80 and compare
        /// </summary>
        /// <param name="data">test data</param>
        protected override void ProcessLineInTest(string[] data) {
            var value = ToExtF80(data[0]);
            var expected = ToLong(data[1]);
            var actual = value.ToLong(Settings.RoundingMode, true);
            AssertEqual(expected, actual);
        }
    }
}