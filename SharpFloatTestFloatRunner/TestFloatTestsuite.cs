using System;
using System.IO;

namespace SharpFloatTestFloatRunner {
    internal class TestFloatTestsuite {

        private readonly string dataDir;
        private readonly TestBase[] tests;

        public TestFloatTestsuite(string dataPath) {
            dataDir = dataPath;
            tests = new[] {
                new ExtF80TestCase()
            };
        }

        public void Run() {

            foreach (var dir in Directory.EnumerateDirectories(dataDir)) {
                var dirName = Path.GetFileName(dir);
                var testCase = FindTestCase(dirName);
                if (testCase != null)
                    testCase.Run(dir);
            }

        }

        private TestBase FindTestCase(string dirName) {
            foreach (var testCase in tests)
                if (string.Equals(dirName, testCase.Name, StringComparison.OrdinalIgnoreCase))
                    return testCase;
            return null;
        }
    }
}