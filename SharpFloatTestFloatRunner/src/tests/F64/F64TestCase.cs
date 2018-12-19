using System.Collections.Generic;
using SharpFloatTestFloatRunner.Common;

namespace SharpFloatTestFloatRunner.Tests.F64 {

    /// <summary>
    ///     test ExtF80 operations
    /// </summary>
    public class F64TestCase : TestBase {

        /// <summary>
        ///     test case name
        /// </summary>
        public override string Name
            => "F64";

        /// <summary>
        ///     get test entries
        /// </summary>
        /// <returns></returns>
        public override void GetTestEntries(List<TestEntry> entries) {
            entries.Add(new F64ToExtF80Test());
        }
    }
}