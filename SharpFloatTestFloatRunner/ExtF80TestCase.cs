using System.Collections.Generic;

namespace SharpFloatTestFloatRunner {

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
        ///     test entries
        /// </summary>
        private TestEntry[] entries = new[] {
            new ExtF80AddTest()
        };

        /// <summary>
        ///     get test entries
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<TestEntry> GetTestEntries()
            => entries;
    }
}