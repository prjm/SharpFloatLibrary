/*
 *    This library is a port of the Dragon4 algorithm of Ryan Juckett.
 *    This is not the original software library -- it is an adapted version for C#.
 *
 *    Copyright 2018 Bastian Turcs. All rights reserved.
 *
 */
/******************************************************************************
 Copyright (c) 2014 Ryan Juckett
 http://www.ryanjuckett.com/

 This software is provided 'as-is', without any express or implied
 warranty. In no event will the authors be held liable for any damages
 arising from the use of this software.

 Permission is granted to anyone to use this software for any purpose,
 including commercial applications, and to alter it and redistribute it
 freely, subject to the following restrictions:

 1. The origin of this software must not be misrepresented; you must not
    claim that you wrote the original software. If you use this software
    in a product, an acknowledgment in the product documentation would be
    appreciated but is not required.

 2. Altered source versions must be plainly marked as such, and must not be
    misrepresented as being the original software.

 3. This notice may not be removed or altered from any source
    distribution.
******************************************************************************/

using System;
using System.Text;
using SharpFloat.Helpers;

namespace SharpFloat.FloatingPoint {

    /// <summary>
    ///     floating point format to use
    /// </summary>
    public enum PrintFloatFormat : byte {

        /// <summary>
        ///     undefined format
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     positional format, example <c>[-]ddddd.dddd</c>
        /// </summary>
        PositionalFormat = 1,

        /// <summary>
        ///     scientific format, example <c>[-]d.dddde[sign]ddd</c>
        /// </summary>
        ScientificFormat = 2
    };

    public partial struct ExtF80 {

        /// <summary>
        ///     outputs the positive number with positional notation: ddddd.dddd
        /// </summary>
        /// <param name="outputBuffer">output buffer</param>
        /// <param name="mantissa">value significand</param>
        /// <param name="exponent">value exponent in base 2</param>
        /// <param name="mantissaHighBitIdx">index of the highest set mantissa bit</param>
        /// <param name="hasUnequalMargins">is the high margin twice as large as the low margin</param>
        /// <param name="precision">
        ///     Negative prints as many digits as are needed for a unique
        ///     number. Positive specifies the maximum number of
        ///     significant digits to print past the decimal point.</param>
        /// <returns></returns>
        private static uint FormatPositional(StringBuilder outputBuffer, ulong mantissa, int exponent, uint mantissaHighBitIdx, bool hasUnequalMargins, int precision) {
            var bufferOffset = outputBuffer.Length;
            uint numPrintDigits;
            int printExponent;

            if (precision < 0) {
                numPrintDigits = Dragon4(mantissa, exponent, mantissaHighBitIdx, hasUnequalMargins, FormatCutoffMode.Unique, 0, outputBuffer, out printExponent);
            }
            else {
                numPrintDigits = Dragon4(mantissa, exponent, mantissaHighBitIdx, hasUnequalMargins, FormatCutoffMode.FractionLength, (uint)precision, outputBuffer, out printExponent);
            }

            if (numPrintDigits <= 0)
                throw new InvalidOperationException();

            // track the number of digits past the decimal point that have been printed
            var numFractionDigits = 0U;

            // if output has a whole number
            if (printExponent >= 0) {
                // leave the whole number at the start of the buffer
                var numWholeDigits = (uint)(printExponent + 1);
                if (numPrintDigits < numWholeDigits) {

                    // add trailing zeros up to the decimal point
                    for (; numPrintDigits < numWholeDigits; ++numPrintDigits)
                        outputBuffer.Append('0');
                }
                // insert the decimal point prior to the fraction
                else if (numPrintDigits > numWholeDigits) {
                    numFractionDigits = numPrintDigits - numWholeDigits;
                    outputBuffer.Insert(bufferOffset + (int)numWholeDigits, '.');
                    numPrintDigits = numWholeDigits + 1 + numFractionDigits;
                }
            }
            else {
                // shift out the fraction to make room for the leading zeros
                var numFractionZeros = (uint)-printExponent - 1;
                var digitsStartIdx = 2 + numFractionZeros;

                // shift the significant digits right such that there is room for leading zeros
                numFractionDigits = numPrintDigits;

                // insert the leading zeros
                for (uint i = 2; i < digitsStartIdx; ++i)
                    outputBuffer.Insert(bufferOffset, '0');

                // update the counts
                numFractionDigits += numFractionZeros;
                numPrintDigits = numFractionDigits;

                // add the decimal point
                outputBuffer[1] = '.';
                numPrintDigits += 1;

                // add the initial zero
                outputBuffer[0] = '0';
                numPrintDigits += 1;
            }

            // add trailing zeros up to precision length
            if (precision > (int)numFractionDigits) {
                // add a decimal point if this is the first fractional digit we are printing
                if (numFractionDigits == 0) {
                    outputBuffer.Append('.');
                }

                // compute the number of trailing zeros needed
                var totalDigits = (uint)(numPrintDigits + (precision - numFractionDigits));
                for (; numPrintDigits < totalDigits; ++numPrintDigits)
                    outputBuffer.Append('0');
            }

            return numPrintDigits;
        }

        /// <summary>
        ///     output a number using scientific notation
        /// </summary>
        /// <param name="outputBuffer">output buffer</param>
        /// <param name="mantissa">value significand</param>
        /// <param name="exponent">value exponent in base 2</param>
        /// <param name="mantissaHighBitIdx">index of the highest set mantissa bit</param>
        /// <param name="hasUnequalMargins">is the high margin twice as large as the low margin</param>
        /// <param name="precision">
        ///     Negative prints as many digits as are needed for a unique
        ///     number. Positive specifies the maximum number of
        ///     significant digits to print past the decimal point.
        /// </param>
        /// <returns></returns>
        private static uint FormatScientific(StringBuilder outputBuffer, ulong mantissa, int exponent, uint mantissaHighBitIdx, bool hasUnequalMargins, int precision) {
            var bufferOffset = outputBuffer.Length;
            int printExponent;
            uint numPrintDigits;

            if (precision < 0) {
                numPrintDigits = Dragon4(mantissa, exponent, mantissaHighBitIdx, hasUnequalMargins, FormatCutoffMode.Unique, 0, outputBuffer, out printExponent);
            }
            else {
                numPrintDigits = Dragon4(mantissa, exponent, mantissaHighBitIdx, hasUnequalMargins, FormatCutoffMode.TotalLength, (uint)(precision + 1), outputBuffer, out printExponent);
            }

            if (numPrintDigits <= 0)
                throw new InvalidOperationException();

            // keep the whole number as the first digit
            /*
            if (bufferSize > 1) {
                pCurOut += 1;
                bufferSize -= 1;
            }
            */

            // insert the decimal point prior to the fractional number
            var numFractionDigits = numPrintDigits - 1;
            if (numFractionDigits > 0) {
                outputBuffer.Insert(1 + bufferOffset, '.');
            }

            // add trailing zeros up to precision length
            if (precision > (int)numFractionDigits) {
                // add a decimal point if this is the first fractional digit we are printing
                if (numFractionDigits == 0) {
                    outputBuffer.Append('.');
                }

                // compute the number of trailing zeros needed
                var numZeros = (uint)(precision - numFractionDigits);
                for (var pEnd = 0; pEnd < numZeros; ++pEnd)
                    outputBuffer.Append('0');
            }

            // print the exponent into a local buffer and copy into output buffer
            var exponentBuffer = new char[6];
            exponentBuffer[0] = 'e';
            if (printExponent >= 0) {
                exponentBuffer[1] = '+';
            }
            else {
                exponentBuffer[1] = '-';
                printExponent = -printExponent;
            }

            if (printExponent >= 10000)
                throw new InvalidOperationException();

            var thousandsPlace = printExponent / 1000;
            var hundredsPlace = (printExponent - thousandsPlace * 1000) / 100;
            var tensPlace = (printExponent - thousandsPlace * 1000 - hundredsPlace * 100) / 10;
            var onesPlace = (printExponent - thousandsPlace * 1000 - hundredsPlace * 100 - tensPlace * 10);

            exponentBuffer[2] = (char)('0' + thousandsPlace);
            exponentBuffer[3] = (char)('0' + hundredsPlace);
            exponentBuffer[4] = (char)('0' + tensPlace);
            exponentBuffer[5] = (char)('0' + onesPlace);

            // copy the exponent buffer into the output
            outputBuffer.Append(new string(exponentBuffer));

            return (uint)outputBuffer.Length;
        }

        //******************************************************************************
        // Print a hexadecimal value with a given width.
        // The output string is always NUL terminated and the string length (not
        // including the NUL) is returned.
        //******************************************************************************
        private static uint PrintHex(StringBuilder pOutBuffer, ulong value, uint width) {
            var digits = "0123456789abcdef";

            while (width > 0) {
                --width;

                var digit = (byte)((value >> 4 * (int)width) & 0xF);
                pOutBuffer.Append(digits[digit]);
            }

            return (uint)pOutBuffer.Length;
        }

        //******************************************************************************
        // Print special case values for infinities and NaNs.
        // The output string is always NUL terminated and the string length (not
        // including the NUL) is returned.
        //******************************************************************************
        private static uint PrintInfNan(StringBuilder pOutBuffer, ulong mantissa, uint mantissaHexWidth) {

            // Check for infinity
            if (mantissa == 0) {
                // copy and make sure the buffer is terminated
                pOutBuffer.Append("Inf");
                return (uint)pOutBuffer.Length;
                ;
            }
            else {
                pOutBuffer.Append("NaN");
                PrintHex(pOutBuffer, mantissa, mantissaHexWidth);
                return (uint)pOutBuffer.Length;
            }
        }


        /// <summary>
        ///     Print a 32-bit floating-point number as a decimal string.
        /// </summary>
        /// <param name="outputBffer">output buffer</param>
        /// <param name="value">value to print</param>
        /// <param name="format">output format</param>
        /// <param name="precision">
        ///     If negative, the minimum number of digits to represent a
        ///     unique 32-bit floating point value is output. Otherwise,
        ///     this is the number of digits to print past the decimal point.
        /// </param>
        /// <returns></returns>
        public static uint PrintFloat32(StringBuilder outputBffer, float value, PrintFloatFormat format, int precision) {
            var bits = FloatHelpers.SingleToInt32Bits(value);
            var floatExponent = (bits >> 23) & 0xFF;
            var floatMantissa = bits & 0x7FFFFF;
            var prefixLength = 0;

            // output the sign
            if ((bits >> 63) != 0) {
                outputBffer.Append('-');
                ++prefixLength;
            }

            // if this is a special value
            if (floatExponent == 0xFF) {
                return (uint)(PrintInfNan(outputBffer, floatMantissa, 6) + prefixLength);
            }
            else {
                // factor the value into its parts
                uint mantissa;
                int exponent;
                uint mantissaHighBitIdx;
                bool hasUnequalMargins;
                if (floatExponent != 0) {
                    // normalized
                    // The floating point equation is:
                    //  value = (1 + mantissa/2^23) * 2 ^ (exponent-127)
                    // We convert the integer equation by factoring a 2^23 out of the exponent
                    //  value = (1 + mantissa/2^23) * 2^23 * 2 ^ (exponent-127-23)
                    //  value = (2^23 + mantissa) * 2 ^ (exponent-127-23)
                    // Because of the implied 1 in front of the mantissa we have 24 bits of precision.
                    //   m = (2^23 + mantissa)
                    //   e = (exponent-127-23)
                    mantissa = (uint)((1L << 23) | floatMantissa);
                    exponent = (int)(floatExponent - 127 - 23);
                    mantissaHighBitIdx = 23;
                    hasUnequalMargins = (floatExponent != 1) && (floatMantissa == 0);
                }
                else {
                    // denormalized
                    // The floating point equation is:
                    //  value = (mantissa/2^23) * 2 ^ (1-127)
                    // We convert the integer equation by factoring a 2^23 out of the exponent
                    //  value = (mantissa/2^23) * 2^23 * 2 ^ (1-127-23)
                    //  value = mantissa * 2 ^ (1-127-23)
                    // We have up to 23 bits of precision.
                    //   m = (mantissa)
                    //   e = (1-127-23)
                    mantissa = floatMantissa;
                    exponent = 1 - 127 - 23;
                    mantissaHighBitIdx = mantissa.LogBase2();
                    hasUnequalMargins = false;
                }

                // format the value
                switch (format) {
                    case PrintFloatFormat.PositionalFormat:
                        return (uint)(FormatPositional(outputBffer,
                                                    mantissa,
                                                    exponent,
                                                    mantissaHighBitIdx,
                                                    hasUnequalMargins,
                                                    precision) + prefixLength);

                    case PrintFloatFormat.ScientificFormat:
                        return (uint)(FormatScientific(outputBffer,
                                                    mantissa,
                                                    exponent,
                                                    mantissaHighBitIdx,
                                                    hasUnequalMargins,
                                                    precision) + prefixLength);

                    default:
                        throw new ArgumentOutOfRangeException(nameof(format));
                }
            }
        }

        /// <summary>
        ///     Print a 64-bit floating-point number as a decimal string.
        /// </summary>
        /// <param name="outputBuffer">output buffer</param>
        /// <param name="value">value to print</param>
        /// <param name="format">print format</param>
        /// <param name="precision">
        ///     If negative, the minimum number of digits to represent a
        ///     unique 64-bit floating point value is output. Otherwise,
        ///     this is the number of digits to print past the decimal point.
        /// </param>
        /// <returns></returns>
        public static uint PrintFloat64(StringBuilder outputBuffer, double value, PrintFloatFormat format, int precision) {
            var bits = (ulong)BitConverter.DoubleToInt64Bits(value);
            var floatExponent = (uint)((bits >> 52) & 0x7ffU);
            var floatMantissa = bits & 0xFFFFFFFFFFFFFUL;
            var prefixLength = 0U;

            // output the sign
            if ((bits >> 63) != 0) {
                outputBuffer.Append('-');
                ++prefixLength;
            }

            // if this is a special value
            if (floatExponent == 0x7FF) {
                return PrintInfNan(outputBuffer, floatMantissa, 13) + prefixLength;
            }
            // else this is a number
            else {
                // factor the value into its parts
                ulong mantissa;
                int exponent;
                uint mantissaHighBitIdx;
                bool hasUnequalMargins;

                if (floatExponent != 0) {
                    // normal
                    // The floating point equation is:
                    //  value = (1 + mantissa/2^52) * 2 ^ (exponent-1023)
                    // We convert the integer equation by factoring a 2^52 out of the exponent
                    //  value = (1 + mantissa/2^52) * 2^52 * 2 ^ (exponent-1023-52)
                    //  value = (2^52 + mantissa) * 2 ^ (exponent-1023-52)
                    // Because of the implied 1 in front of the mantissa we have 53 bits of precision.
                    //   m = (2^52 + mantissa)
                    //   e = (exponent-1023+1-53)
                    mantissa = (1UL << 52) | floatMantissa;
                    exponent = (int)(floatExponent - 1023 - 52);
                    mantissaHighBitIdx = 52;
                    hasUnequalMargins = (floatExponent != 1) && (floatMantissa == 0);
                }
                else {
                    // subnormal
                    // The floating point equation is:
                    //  value = (mantissa/2^52) * 2 ^ (1-1023)
                    // We convert the integer equation by factoring a 2^52 out of the exponent
                    //  value = (mantissa/2^52) * 2^52 * 2 ^ (1-1023-52)
                    //  value = mantissa * 2 ^ (1-1023-52)
                    // We have up to 52 bits of precision.
                    //   m = (mantissa)
                    //   e = (1-1023-52)
                    mantissa = floatMantissa;
                    exponent = 1 - 1023 - 52;
                    mantissaHighBitIdx = mantissa.LogBase2();
                    hasUnequalMargins = false;
                }

                // format the value
                switch (format) {
                    case PrintFloatFormat.PositionalFormat:
                        return FormatPositional(outputBuffer,
                                                    mantissa,
                                                    exponent,
                                                    mantissaHighBitIdx,
                                                    hasUnequalMargins,
                                                    precision) + prefixLength;

                    case PrintFloatFormat.ScientificFormat:
                        return FormatScientific(outputBuffer,
                                                    mantissa,
                                                    exponent,
                                                    mantissaHighBitIdx,
                                                    hasUnequalMargins,
                                                    precision) + prefixLength;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(format));
                }
            }
        }


        /// <summary>
        ///     Print a 80-bit floating-point number as a decimal string
        /// </summary>
        /// <param name="outputBuffer">output buffer</param>
        /// <param name="value">value to print</param>
        /// <param name="format">print format</param>
        /// <param name="precision">
        ///     If negative, the minimum number of digits to represent a
        ///     unique 80-bit floating point value is output. Otherwise,
        ///     this is the number of digits to print past the decimal point.
        /// </param>
        /// <returns></returns>
        public static uint PrintFloat80(StringBuilder outputBuffer, in ExtF80 value, PrintFloatFormat format, int precision) {
            var floatExponent = value.UnsignedExponent;
            var floatMantissa = value.signif;
            var prefixLength = 0U;

            // output the sign
            if (value.IsNegative) {
                outputBuffer.Append('-');
                ++prefixLength;
            }

            if (value.IsNaN || value.IsSpecialOperand) {
                return PrintInfNan(outputBuffer, floatMantissa, 13) + prefixLength;
            }
            else {
                // factor the value into its parts
                ulong mantissa;
                int exponent;
                uint mantissaHighBitIdx;
                bool hasUnequalMargins;

                if (floatExponent != 0) {
                    mantissa = (1UL << 63) | floatMantissa;
                    exponent = (int)(floatExponent - 16383 - 63);
                    mantissaHighBitIdx = 63;
                    hasUnequalMargins = (floatExponent != 1) && (floatMantissa == 0);
                }
                else {
                    mantissa = floatMantissa;
                    exponent = 1 - 16383 - 63;
                    mantissaHighBitIdx = mantissa.LogBase2();
                    hasUnequalMargins = false;
                }

                // format the value
                switch (format) {
                    case PrintFloatFormat.PositionalFormat:
                        return FormatPositional(outputBuffer,
                                                    mantissa,
                                                    exponent,
                                                    mantissaHighBitIdx,
                                                    hasUnequalMargins,
                                                    precision) + prefixLength;

                    case PrintFloatFormat.ScientificFormat:
                        return FormatScientific(outputBuffer,
                                                    mantissa,
                                                    exponent,
                                                    mantissaHighBitIdx,
                                                    hasUnequalMargins,
                                                    precision) + prefixLength;

                    default:
                        throw new ArgumentOutOfRangeException(nameof(format));
                }
            }
        }

    }
}
