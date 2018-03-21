using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpFloat.FloatingPoint;
using SharpFloatTests.Common;

namespace SharpFloatTests.Helpers {
    public class TestToString {
        void TestFormat_F32(StringBuilder pBuffer, uint bufferSize, PrintFloatFormat format, int precision, float value, string pValueStr) {

            var printLen = ExtF80.PrintFloat32(pBuffer, bufferSize, value, format, precision);
            Assert.EqualString(pBuffer.ToString(), pValueStr);
        }

        //******************************************************************************
        //******************************************************************************
        void TestFormat_F64(StringBuilder pBuffer, uint bufferSize, PrintFloatFormat format, int precision, double value, string pValueStr) {
            var printLen = ExtF80.PrintFloat64(pBuffer, bufferSize, value, format, precision);
            Assert.EqualString(pBuffer.ToString(), pValueStr);
        }

        [TestCase]
        public void TestStringFormat() {
            var valBuff = new StringBuilder();
            var valBuffSize = 999U;
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, -1, 1.0f, "1");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, -1, 1.0f, "1e+000");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, -1, 10.234f, "10.234");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, -1, -10.234f, "-10.234");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, -1, 10.234f, "1.0234e+001");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, -1, -10.234f, "-1.0234e+001");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, -1, 1000.0f, "1000");

            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 0, 1.0f, "1");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 0, 1.0f, "1e+000");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 0, 10.234f, "10");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 0, -10.234f, "-10");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 0, 10.234f, "1e+001");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 0, -10.234f, "-1e+001");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 2, 10.234f, "10.23");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 2, 10.234f, "1.02e+001");
            TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 16, 9.9999999999999995e-008, "9.9999999999999995e-008");
            TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 16, 9.8813129168249309e-324, "9.8813129168249309e-324");
            TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 16, 9.9999999999999694e-311, "9.9999999999999694e-311");

            // test rounding
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 10, 3.14159265358979323846f, "3.1415927410"); // 3.1415927410 is closest tF32 to PI
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 10, 3.14159265358979323846f, "3.1415927410e+000");
            TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 10, 3.14159265358979323846, "3.1415926536");
            TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 10, 3.14159265358979323846, "3.1415926536e+000");

            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 5, 299792458.0f, "299792448.00000"); // 299792448 is closest tF32 to 299792458
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 5, 299792458.0f, "2.99792e+008");
            TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 5, 299792458.0, "299792458.00000");
            TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 5, 299792458.0, "2.99792e+008");

            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 25, 3.14159265358979323846f, "3.1415927410125732421875000");
            TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 50, 3.14159265358979323846, "3.14159265358979311599796346854418516159057617187500");
            TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, -1, 3.14159265358979323846, "3.141592653589793");

            // smallest numbers
            //TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 149, Math.Pow(0.5, 126 + 23), "0.00000000000000000000000000000000000000000000140129846432481707092372958328991613128026194187651577175706828388979108268586060148663818836212158203125");
            //TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 1074, Math.pow((0.5, 1022 + 52), "0.000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000004940656458412465441765687928682213723650598026143247644255856825006755072702087518652998363616359923797965646954457177309266567103559397963987747960107818781263007131903114045278458171678489821036887186360569987307230500063874091535649843873124733972731696151400317153853980741262385655911710266585566867681870395603106249319452715914924553293054565444011274801297099995419319894090804165633245247571478690147267801593552386115501348035264934720193790268107107491703332226844753335720832431936092382893458368060106011506169809753078342277318329247904982524730776375927247874656084778203734469699533647017972677717585125660551199131504891101451037862738167250955837389733598993664809941164205702637090279242767544565229087538682506419718265533447265625");

            // largest numbers                   .
            //          TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 0, FLT_MAX, "340282346638528859811704183484516925440");
            //            TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 0, DBL_MAX, "179769313486231570814527423731704356798070567525844996598917476803157260780028538760589558632766878171540458953514382464234321326889464182768467546703537516986049910576551282076245490090389328944075868508455133942304583236903222948165808559332123348274797826204144723168738177180919299881250404026184124858368");

            // test trailing zeros               .
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 3, 1.0f, "1.000");
            TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 3, 1.0f, "1.000");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 3, 1.0f, "1.000e+000");
            TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 3, 1.0f, "1.000e+000");

            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 3, 1.5f, "1.500");
            TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Positional, 3, 1.5f, "1.500");
            TestFormat_F32(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 3, 1.5f, "1.500e+000");
            TestFormat_F64(valBuff, valBuffSize, PrintFloatFormat.PrintFloatFormat_Scientific, 3, 1.5f, "1.500e+000");

            // test buffer overflow
            TestFormat_F32(valBuff, 2, PrintFloatFormat.PrintFloatFormat_Scientific, -1, 1234565789f, "1");
            TestFormat_F32(valBuff, 3, PrintFloatFormat.PrintFloatFormat_Scientific, -1, 1234565789f, "1.");
            TestFormat_F32(valBuff, 4, PrintFloatFormat.PrintFloatFormat_Scientific, -1, 12, "1.2");
            TestFormat_F32(valBuff, 5, PrintFloatFormat.PrintFloatFormat_Scientific, -1, 12, "1.2e");
            TestFormat_F32(valBuff, 6, PrintFloatFormat.PrintFloatFormat_Scientific, -1, 12, "1.2e+");
            TestFormat_F32(valBuff, 5, PrintFloatFormat.PrintFloatFormat_Positional, -1, 1234565789f, "1235");
            TestFormat_F32(valBuff, 5, PrintFloatFormat.PrintFloatFormat_Positional, -1, 123.4565789f, "123.");
            TestFormat_F32(valBuff, 5, PrintFloatFormat.PrintFloatFormat_Positional, -1, 12.34565789f, "12.3");
            TestFormat_F32(valBuff, 5, PrintFloatFormat.PrintFloatFormat_Positional, -1, 0.1234565789f, "0.12");
            TestFormat_F32(valBuff, 5, PrintFloatFormat.PrintFloatFormat_Positional, -1, -0.1234565789f, "-0.1");

            TestFormat_F32(valBuff, 5, PrintFloatFormat.PrintFloatFormat_Scientific, -1, 1234565789f, "1.23");
            TestFormat_F32(valBuff, 1, PrintFloatFormat.PrintFloatFormat_Scientific, -1, 1234565789f, "");
            TestFormat_F32(valBuff, 8, PrintFloatFormat.PrintFloatFormat_Scientific, -1, 12f, "1.2e+00");
            TestFormat_F32(valBuff, 9, PrintFloatFormat.PrintFloatFormat_Scientific, -1, 12f, "1.2e+001");
            TestFormat_F32(valBuff, 9, PrintFloatFormat.PrintFloatFormat_Scientific, -1, -12f, "-1.2e+00");

            // test rounding up trailinPrintFloatFormat.g 9s
            TestFormat_F32(valBuff, 7, PrintFloatFormat.PrintFloatFormat_Positional, -1, 1000.4999f, "1000.5");
            TestFormat_F32(valBuff, 7, PrintFloatFormat.PrintFloatFormat_Positional, -1, 1000.9999f, "1001");
            TestFormat_F32(valBuff, 7, PrintFloatFormat.PrintFloatFormat_Positional, -1, 999.9999f, "1000");

        }
    }
}
