using System.Collections.Generic;
using SharpFloatTestFloatRunner.Common;

namespace SharpFloatTestFloatRunner.Tests.I32 {

    /// <summary>
    ///     test ExtF80 operations
    /// </summary>
    public class I32TestCase : TestBase {

        /// <summary>
        ///     test case name
        /// </summary>
        public override string Name
            => "I32";

        /// <summary>
        ///     get test entries
        /// </summary>
        /// <returns></returns>
        public override void GetTestEntries(List<TestEntry> entries) {
            entries.Add(new I32ToExtF80Test());
        }
    }
}