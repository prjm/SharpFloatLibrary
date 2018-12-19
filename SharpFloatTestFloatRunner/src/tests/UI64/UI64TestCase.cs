using System.Collections.Generic;
using SharpFloatTestFloatRunner.Common;

namespace SharpFloatTestFloatRunner.Tests.UI64 {

    /// <summary>
    ///     test ExtF80 operations
    /// </summary>
    public class UI64TestCase : TestBase {

        /// <summary>
        ///     test case name
        /// </summary>
        public override string Name
            => "UI64";

        /// <summary>
        ///     get test entries
        /// </summary>
        /// <returns></returns>
        public override void GetTestEntries(List<TestEntry> entries) {
            entries.Add(new UI64ToExtF80Test());
        }
    }
}