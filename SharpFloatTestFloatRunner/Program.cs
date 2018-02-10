using System;
using System.IO;

namespace SharpFloatTestFloatRunner {

    class Program {

        static void Main(string[] args) {
            var dataDir = SearchDataDir(Environment.CurrentDirectory);

            if (dataDir == null)
                throw new InvalidOperationException();

            var suite = new TestFloatTestsuite(dataDir);
            suite.Run();
        }

        private static string SearchDataDir(string basePath) {
            while (true) {
                var path = Path.Combine(basePath, "data");
                if (Directory.Exists(path))
                    return path;

                var parentPath = Directory.GetParent(basePath);

                if (parentPath == null)
                    return null;

                basePath = parentPath.FullName;
            }
        }
    }
}
