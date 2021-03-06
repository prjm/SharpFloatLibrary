﻿using System.Collections.Generic;
using SharpFloatTestFloatRunner.Common;

namespace SharpFloatTestFloatRunner.Tests.ExtF80 {

    /// <summary>
    ///     test ExtF80 operations
    /// </summary>
    public class ExtF80TestCase : TestBase {

        /// <summary>
        ///     test case name
        /// </summary>
        public override string Name
            => "ExtF80";

        /// <summary>
        ///     get test entries
        /// </summary>
        /// <returns></returns>
        public override void GetTestEntries(List<TestEntry> entries) {
            entries.AddRange(new TestEntry[] {
                new ExtF80AddTest(),
                new ExtF80SubTest(),
                new ExtF80MulTest(),
                new ExtF80DivTest(),
                new ExtF80RemTest(),
                new ExtF80EqTest(),
                new ExtF80LtTest(),
                new ExtF80LeTest(),
                new ExtF80SqrtTest(),
                new ExtF80RoundToIntTest(),
                new ExtF80ToInt32Test(),
                new ExtF80ToUInt32Test(),
                new ExtF80ToInt64Test(),
                new ExtF80ToUInt64Test(),
                new ExtF80ToDoubleTest(),
                new ExtF80ToFloatTest(),
            });
        }
    }
}