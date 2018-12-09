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
        ///     test entries
        /// </summary>
        private readonly TestEntry[] entries = new TestEntry[] {
            new F32ToExtF80Test(),
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