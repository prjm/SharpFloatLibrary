using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFloat.Globals;

namespace SharpFloatTestFloatRunner {

    public abstract class TestBase {

        public abstract string Name { get; }

        public abstract IEnumerable<TestEntry> CreateEntries();

        private static string GetRoundingModeAsString(RoundingMode mode) {
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
            return "undef";
        }

        internal void Run(string dir) {
            foreach (var entry in CreateEntries()) {
                foreach (var roundingMode in Enum.GetValues(typeof(RoundingMode)).Cast<RoundingMode>()) {
                    var testFileName = entry.Name + "_" + GetRoundingModeAsString(roundingMode) + ".zip";
                    var path = Path.Combine(dir, testFileName);
                    if (!File.Exists(path))
                        continue;

                    entry.Run(path);
                }
            }
        }
    }
}
