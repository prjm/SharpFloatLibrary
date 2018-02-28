using System.Collections.Generic;
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
        ///     test entries
        /// </summary>
        private TestEntry[] entries = new TestEntry[] {
            new ExtF80AddTest(),
            new ExtF80SubTest(),
            new ExtF80MulTest(),
            new ExtF80DivTest(),
            new ExtF80EqTest(),
            new ExtF80LtTest(),
            new ExtF80LeTest(),
        };

        /// <summary>
        ///     get test entries
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<TestEntry> GetTestEntries()
            => entries;
    }
}