using System;
using System.IO;
using SharpFloatTestFloatRunner.Common;

namespace SharpFloatTestFloatRunner {

    /// <summary>
    ///     helper program to compare the output of the <c>testfloat_get</c> program with the internal implementation
    /// </summary>
    class Program {

        /// <summary>
        ///     search the data files, compute own results and compare
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args) {
            var dataDir = SearchDataDir(Environment.CurrentDirectory);

            if (dataDir == null)
                throw new InvalidOperationException("No data files found.");

            var suite = new TestFloatTestsuite(dataDir);
            suite.Run();
        }

        /// <summary>
        ///     search the data files directory
        /// </summary>
        /// <param name="basePath"></param>
        /// <returns></returns>
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
