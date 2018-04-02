using System;
using System.IO;
using System.Reflection;

namespace SharpFloat.FloatingPoint {

    public partial struct ExtF80 {

        private static Lazy<string> license
            = new Lazy<string>(GetLicenseText, true);

        private static string GetLicenseText() {
            return string.Concat(//
                GetLicenseText("LICENSE.txt"), //
                GetLicenseText("LICENSE_Softfloat_Library.txt"), //
                GetLicenseText("LICENSE_Dragon4.txt"), //
                GetLicenseText("LICENSE_Roslyn.txt"), //
                GetLicenseText("LICENSE_Default.txt"));
        }

        private static string GetLicenseText(string textFileName) {
            var assembly = typeof(ExtF80).GetTypeInfo().Assembly;
            using (var resource = assembly.GetManifestResourceStream("SharpFloat." + textFileName)) {
                using (var reader = new StreamReader(resource)) {
                    return string.Concat(
                        "License file: ",
                        textFileName,
                        "\r\n",
                        reader.ReadToEnd(),
                        new string('-', 80),
                        "\r\n");
                }
            }
        }

        /// <summary>
        ///     license text
        /// </summary>
        public static string License
            => license.Value;

    }
}
