using System.Collections.Generic;

namespace SharpFloatTestFloatRunner {

    internal class ExtF80TestCase : TestBase {

        public override string Name
            => "ExtF80";

        private TestEntry[] entries = new[] {
            new ExtF80AddTest()
        };

        public override IEnumerable<TestEntry> CreateEntries()
            => entries;
    }
}