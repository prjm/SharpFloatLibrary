using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using SharpFloat.ExtF80;

namespace SharpFloatTestFloatRunner.Common {

    /// <summary>
    ///     base class for test cases
    /// </summary>
    public abstract class TestEntry {

        /// <summary>
        ///     name of test case
        /// </summary>
        /// <remarks>corresponds to the test case file name</remarks>
        public abstract string Name { get; }

        /// <summary>
        ///     run the test case
        /// </summary>
        /// <param name="path">path of the test case file</param>
        /// <remarks>path has to be a zip file</remarks>
        public void Run(string path) {
            using (var zip = ZipFile.Open(path, ZipArchiveMode.Read)) {
                foreach (var entry in zip.Entries) {
                    using (var input = entry.Open()) {
                        ProcessTestFile(input);
                    }

                }
            }
        }

        /// <summary>
        ///     process a test file
        /// </summary>
        /// <param name="input">input file stream</param>
        private void ProcessTestFile(Stream input) {
            var lineCount = 0UL;
            using (var stream = new StreamReader(input, Encoding.ASCII)) {
                while (!stream.EndOfStream) {
                    var line = stream.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    try {
                        ProcessLine(line);
                        lineCount++;
                    }
                    catch (Exception e) {
                        ProcessLine(line);
                        throw new InvalidComputationException(lineCount, line, e);
                    }
                }
            }
        }

        private void ProcessLine(string line) {
            var data = line.Split(' ');
            ProcessLineInTest(data);
        }

        /// <summary>
        ///     parse a boolean value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ToBool(string value) {
            if (uint.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var parsedResult))
                return parsedResult != 0;
            return false;
        }

        /// <summary>
        ///     convert a hex string to an ExtF80 value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected ExtF80 ToExtF80(string value) {
            ushort signedExponent = 0;
            ulong significant = 0;

            if (uint.TryParse(value.Substring(0, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var parsedResult))
                signedExponent = (ushort)parsedResult;

            if (ulong.TryParse(value.Substring(4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var parsedResult2))
                significant = parsedResult2;

            return new ExtF80(signedExponent, significant);
        }

        /// <summary>
        ///     assert two equal ExtF80 values
        /// </summary>
        /// <param name="expected">expected value</param>
        /// <param name="actual">actual value</param>
        protected static void AssertEqual(ExtF80 expected, ExtF80 actual) {
            if (expected.signExp != actual.signExp)
                throw new NumbersNotEqualException(expected.signExp, actual.signExp);
            if (expected.signif != actual.signif)
                throw new NumbersNotEqualException(expected.signif, actual.signif);
        }


        /// <summary>
        ///     assert two equal ExtF80 values
        /// </summary>
        /// <param name="expected">expected value</param>
        /// <param name="actual">actual value</param>
        protected static void AssertEqual(bool expected, bool actual) {
            if (expected != actual)
                throw new NumbersNotEqualException();
        }

        /// <summary>
        ///     process one line of the test case
        /// </summary>
        /// <param name="data"></param>
        protected abstract void ProcessLineInTest(string[] data);
    }
}
