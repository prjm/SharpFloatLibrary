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
        ///     test entries
        /// </summary>
        private readonly TestEntry[] entries = new TestEntry[] {
            new UI32ToExtF80Test(),
        };

        /// <summary>
        ///     get test entries
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<TestEntry> GetTestEntries() {
            return entries;
        }
    }
}