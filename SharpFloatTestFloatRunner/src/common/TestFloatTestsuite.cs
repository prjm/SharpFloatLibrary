using System;
using System.IO;
using System.Linq;
using SharpFloatTestFloatRunner.Tests.ExtF80;
using SharpFloatTestFloatRunner.Tests.F32;
using SharpFloatTestFloatRunner.Tests.F64;
using SharpFloatTestFloatRunner.Tests.I32;
using SharpFloatTestFloatRunner.Tests.I64;
using SharpFloatTestFloatRunner.Tests.UI32;
using SharpFloatTestFloatRunner.Tests.UI64;

namespace SharpFloatTestFloatRunner.Common {

    internal class TestFloatTestsuite {

        private readonly string dataDir;
        private readonly TestBase[] tests;

        public TestFloatTestsuite(string dataPath) {
            dataDir = dataPath;
            tests = new TestBase[] {
                new ExtF80TestCase(),
                new I32TestCase(),
                new UI32TestCase(),
                new I64TestCase(),
                new UI64TestCase(),
                new F64TestCase(),
                new F32TestCase(),
            };
        }

        public void Run() {

            foreach (var dir in Directory.EnumerateDirectories(dataDir).OrderBy(t => t)) {
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