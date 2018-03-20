using System;
using System.IO;
using System.Text;
using SharpFloatTestFloatRunner.Common;
using SharpFloat.Helpers;

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

            var buffer = new StringBuilder();
            var m = 1UL;
            var e = 2;
            SharpFloat.FloatingPoint.ExtF80.Dragon4(m, e, m.CountLeadingZeroes(), false, SharpFloat.FloatingPoint.FormatCutoffMode.Unique, 99, buffer, 99, out var exp);
            Console.WriteLine(buffer.ToString());
            return;

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
