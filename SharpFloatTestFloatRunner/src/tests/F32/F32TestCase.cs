using System.Collections.Generic;
using SharpFloatTestFloatRunner.Common;

namespace SharpFloatTestFloatRunner.Tests.F32 {

    /// <summary>
    ///     test ExtF80 operations
    /// </summary>
    public class F32TestCase : TestBase {

        /// <summary>
        ///     test case name
        /// </summary>
        public override string Name
            => "F32";

        /// <summary>
        ///     get test entries
        /// </summary>
        /// <returns></returns>
        public override void GetTestEntries(List<TestEntry> entries) {
            entries.Add(new F32ToExtF80Test());
        }
    }
}