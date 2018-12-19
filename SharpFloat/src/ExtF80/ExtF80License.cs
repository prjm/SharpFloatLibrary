using System;
using System.IO;
using System.Reflection;

namespace SharpFloat.FloatingPoint {

    public partial struct ExtF80 {

        private static Lazy<string> license
            = new Lazy<string>(GetLicenseText, true);

        private static string GetLicenseText() {
            var licenses = new string[] {
                GetLicenseText("LICENSE.txt"), //
                GetLicenseText("LICENSE_Softfloat_Library.txt"), //
                GetLicenseText("LICENSE_Dragon4.txt"), //
                GetLicenseText("LICENSE_Roslyn.txt"), //
                GetLicenseText("LICENSE_Default.txt")
            };
            return string.Concat(licenses);
        }

        private static string GetLicenseText(string textFileName) {
            var assembly = typeof(ExtF80).GetTypeInfo().Assembly;
            using (var resource = assembly.GetManifestResourceStream("SharpFloat." + textFileName)) {
                using (var reader = new StreamReader(resource)) {
                    var license = new string[] {
                        "License file: ",
                        textFileName,
                        "\r\n",
                        reader.ReadToEnd(),
                        new string('-', 80),
                        "\r\n" };
                    return string.Concat(license);
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
