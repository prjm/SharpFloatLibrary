using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using SharpFloat.Globals;

[assembly: CLSCompliant(false)]

namespace SharpFloatTestFloatRunner.Common {

    /// <summary>
    ///     base class for a test suite
    /// </summary>
    public abstract class TestBase {

        /// <summary>
        ///     test case name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     enumerate test case entries
        /// </summary>
        /// <returns>test cases</returns>
        public abstract void GetTestEntries(List<TestEntry> entries);

        /// <summary>
        ///     convert a rounding mode value to a string constant
        /// </summary>
        /// <param name="mode">rounding mode to convert</param>
        /// <returns>string constants</returns>
        public static string GetRoundingModeAsString(RoundingMode mode) {
            switch (mode) {
                case RoundingMode.Maximum:
                    return "max";
                case RoundingMode.Minimum:
                    return "min";
                case RoundingMode.MinimumMagnitude:
                    return "min_mag";
                case RoundingMode.NearEven:
                    return "near_even";
                case RoundingMode.NearMaximumMagnitude:
                    return "near_max_mag";
                case RoundingMode.Odd:
                    return "odd";
            }

            return "un0defined";
        }

        /// <summary>
        ///     run this test suite
        /// </summary>
        /// <param name="dir">data files directory</param>
        public void Run(string dir) {
            var entries = new List<TestEntry>();
            GetTestEntries(entries);
            foreach (var entry in entries) {
                Console.Write(Name.ToUpperInvariant());
                Console.Write("_");
                Console.WriteLine(entry.Name.ToUpperInvariant());
                var w = new Stopwatch();

                var values = Enum.GetValues(typeof(RoundingMode)).Cast<RoundingMode>();
                foreach (var roundingMode in values) {
                    var testFileName = entry.Name + "_" + GetRoundingModeAsString(roundingMode) + ".zip";
                    var path = Path.Combine(dir, testFileName);
                    if (!File.Exists(path))
                        continue;

                    Console.Write($"\t{testFileName}");
                    Settings.RoundingMode = roundingMode;
                    w.Start();
                    entry.Run(path);
                    w.Stop();
                    Console.WriteLine($" [OK] {w.ElapsedTicks.ToString(CultureInfo.InvariantCulture)}");
                }
            }
        }
    }
}
