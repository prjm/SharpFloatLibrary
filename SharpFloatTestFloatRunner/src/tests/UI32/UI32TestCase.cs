using System.Collections.Generic;
using SharpFloatTestFloatRunner.Common;

namespace SharpFloatTestFloatRunner.Tests.UI32 {

    /// <summary>
    ///     test ExtF80 operations
    /// </summary>
    public class UI32TestCase : TestBase {

        /// <summary>
        ///     test case name
        /// </summary>
        public override string Name
            => "UI32";

        /// <summary>
        ///     get test entries
        /// </summary>
        public override void GetTestEntries(List<TestEntry> entries) {
            entries.Add(new UI32ToExtF80Test());
        }
    }
}