using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFloat.ExtF80;

namespace SharpFloatTestFloatRunner {
    public abstract class TestEntry {
        public abstract string Name { get; }

        internal void Run(string path) {
            var lineCount = 0UL;
            using (var zip = ZipFile.Open(path, ZipArchiveMode.Read)) {
                foreach (var entry in zip.Entries) {

                    using (var input = entry.Open()) {
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
                                    Console.WriteLine("Line " + lineCount + ": " + line + " / " + e.Message);
                                    //throw new Exception("Line " + lineCount + ": " + line, e);
                                }
                            }
                        }
                    }

                }
            }
        }

        private void ProcessLine(string line) {
            var data = line.Split(' ');
            ProcessLineInTest(data);
        }

        protected ExtF80 TryStrToFloat(string value) {
            ushort signedExponent = 0;
            ulong significant = 0;

            if (uint.TryParse(value.Substring(0, 4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var parsedResult))
                signedExponent = (ushort)parsedResult;

            if (ulong.TryParse(value.Substring(4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var parsedResult2))
                significant = parsedResult2;

            return new ExtF80(signedExponent, significant);
        }

        internal abstract void ProcessLineInTest(string[] data);
    }
}
