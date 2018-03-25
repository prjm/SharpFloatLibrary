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
    ///     cutoff-mode for formatting
    /// </summary>
    public enum FormatCutoffMode : byte {

        /// <summary>
        ///     all digits required for a unique number
        /// </summary>
        Unique = 0,

        /// <summary>
        ///     cutoff after a fixed number of digits
        /// </summary>
        TotalLength = 1,

        /// <summary>
        ///     cutoff after a fixed number of fractional digits
        /// </summary>
        FractionLength = 2

    }

    public partial struct ExtF80 {


        //******************************************************************************
        // This is an implementation the Dragon4 algorithm to convert a binary number
        // in floating point format to a decimal number in string format. The function
        // returns the number of digits written to the output buffer and the output is
        // not NUL terminated.
        //
        // The floating point input value is (mantissa * 2^exponent).
        //
        // See the following papers for more information on the algorithm:
        //  "How to Print Floating-Point Numbers Accurately"
        //    Steele and White
        //    http://kurtstephens.com/files/p372-steele.pdf
        //  "Printing Floating-Point Numbers Quickly and Accurately"
        //    Burger and Dybvig
        //    http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.72.4656&rep=rep1&type=pdf
        //******************************************************************************
        /// <summary>
        ///     format a floating point number using the dragon4 algorithm
        /// </summary>
        /// <param name="mantissa">mantissa / significand</param>
        /// <param name="exponent">exponent</param>
        /// <param name="mantissaHighBitIdx">index of the highest set mantissa bit</param>
        /// <param name="hasUnequalMargins">is the high margin twice as large as the low margin</param>
        /// <param name="cutoffMode">cutoff mode: how to determine output length</param>
        /// <param name="cutoffNumber">number of digits</param>
        /// <param name="pOutBuffer">output buffer</param>
        /// <param name="pOutExponent">calculated exponent</param>
        /// <returns>number of digits</returns>
        public static uint Dragon4(ulong mantissa, int exponent, uint mantissaHighBitIdx, bool hasUnequalMargins, FormatCutoffMode cutoffMode, uint cutoffNumber, StringBuilder pOutBuffer, out int pOutExponent) {
            var bufferOffset = pOutBuffer.Length;

            // if the mantissa is zero, the value is zero regardless of the exponent
            if (mantissa == 0) {
                pOutBuffer.Append('0');
                pOutExponent = 0;
                return 1;
            }

            // compute the initial state in integral form such that
            //  value     = scaledValue / scale
            //  marginLow = scaledMarginLow / scale
            BigInt scale; // positive scale applied to value and margin such that they can be, represented as whole numbers
            BigInt scaledValue; // scale * mantissa
            BigInt scaledMarginLow;  // scale * 0.5 * (distance between this floating-point number and its immediate lower value)

            // For normalized IEEE floating point values, each time the exponent is incremented the margin also
            // doubles. That creates a subset of transition numbers where the high margin is twice the size of
            // the low margin.
            BigInt pScaledMarginHigh;
            var optionalMarginHigh = new BigInt();

            if (hasUnequalMargins) {
                // if we have no fractional component
                if (exponent > 0) {
                    // 1) Expand the input value by multiplying out the mantissa and exponent. This represents
                    //    the input value in its whole number representation.
                    // 2) Apply an additional scale of 2 such that later comparisons against the margin values
                    //    are simplified.
                    // 3) Set the margin value to the lowest mantissa bit's scale.

                    // scaledValue      = 2 * 2 * mantissa*2^exponent
                    scaledValue = new BigInt(4 * mantissa);
                    BigInt.ShiftLeft(scaledValue, (uint)exponent);

                    // scale            = 2 * 2 * 1
                    scale = new BigInt(4);

                    // scaledMarginLow  = 2 * 2^(exponent-1)
                    scaledMarginLow = new BigInt();
                    BigInt.Pow2(scaledMarginLow, (uint)exponent);

                    // scaledMarginHigh = 2 * 2 * 2^(exponent-1)
                    BigInt.Pow2(optionalMarginHigh, (uint)exponent + 1);
                }
                // else we have a fractional exponent
                else {
                    // In order to track the mantissa data as an integer, we store it as is with a large scale

                    // scaledValue      = 2 * 2 * mantissa
                    scaledValue = new BigInt(4 * mantissa);

                    // scale            = 2 * 2 * 2^(-exponent)
                    scale = new BigInt();
                    BigInt.Pow2(scale, (uint)(-exponent) + 2);

                    // scaledMarginLow  = 2 * 2^(-1)
                    scaledMarginLow = new BigInt(1);

                    // scaledMarginHigh = 2 * 2 * 2^(-1)
                    optionalMarginHigh = new BigInt(2);
                }

                // the high and low margins are different
                pScaledMarginHigh = optionalMarginHigh;
            }
            else {
                // if we have no fractional component
                if (exponent > 0) {
                    // 1) Expand the input value by multiplying out the mantissa and exponent. This represents
                    //    the input value in its whole number representation.
                    // 2) Apply an additional scale of 2 such that later comparisons against the margin values
                    //    are simplified.
                    // 3) Set the margin value to the lowest mantissa bit's scale.

                    // scaledValue     = 2 * mantissa*2^exponent
                    scaledValue = new BigInt(2 * mantissa);
                    BigInt.ShiftLeft(scaledValue, (uint)exponent);

                    // scale           = 2 * 1
                    scale = new BigInt(2);

                    // scaledMarginLow = 2 * 2^(exponent-1)
                    scaledMarginLow = new BigInt();
                    BigInt.Pow2(scaledMarginLow, (uint)exponent);
                }
                // else we have a fractional exponent
                else {
                    // In order to track the mantissa data as an integer, we store it as is with a large scale

                    // scaledValue     = 2 * mantissa
                    scaledValue = new BigInt(2 * mantissa);

                    // scale           = 2 * 2^(-exponent)
                    scale = new BigInt();
                    BigInt.Pow2(scale, (uint)(-exponent + 1));

                    // scaledMarginLow = 2 * 2^(-1)
                    scaledMarginLow = new BigInt(1);
                }

                // the high and low margins are equal
                pScaledMarginHigh = scaledMarginLow;
            }

            // Compute an estimate for digitExponent that will be correct or undershoot by one.
            // This optimization is based on the paper "Printing Floating-Point Numbers Quickly and Accurately"
            // by Burger and Dybvig http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.72.4656&rep=rep1&type=pdf
            // We perform an additional subtraction of 0.69 to increase the frequency of a failed estimate
            // because that lets us take a faster branch in the code. 0.69 is chosen because 0.69 + log10(2) is
            // less than one by a reasonable epsilon that will account for any floating point error.
            //
            // We want to set digitExponent to floor(log10(v)) + 1
            //  v = mantissa*2^exponent
            //  log2(v) = log2(mantissa) + exponent;
            //  log10(v) = log2(v) * log10(2)
            //  floor(log2(v)) = mantissaHighBitIdx + exponent;
            //  log10(v) - log10(2) < (mantissaHighBitIdx + exponent) * log10(2) <= log10(v)
            //  log10(v) < (mantissaHighBitIdx + exponent) * log10(2) + log10(2) <= log10(v) + log10(2)
            //  floor( log10(v) ) < ceil( (mantissaHighBitIdx + exponent) * log10(2) ) <= floor( log10(v) ) + 1
            var log10_2 = 0.30102999566398119521373889472449;
            var digitExponent = (int)(Math.Ceiling(((int)mantissaHighBitIdx + exponent) * log10_2 - 0.69));

            // if the digit exponent is smaller than the smallest desired digit for fractional cutoff,
            // pull the digit back into legal range at which point we will round to the appropriate value.
            // Note that while our value for digitExponent is still an estimate, this is safe because it
            // only increases the number. This will either correct digitExponent to an accurate value or it
            // will clamp it above the accurate value.
            if (cutoffMode == FormatCutoffMode.FractionLength && digitExponent <= -(int)cutoffNumber) {
                digitExponent = -(int)cutoffNumber + 1;
            }

            // Divide value by 10^digitExponent.
            if (digitExponent > 0) {
                // The exponent is positive creating a division so we multiply up the scale.
                var temp = new BigInt();
                BigInt.MultiplyPow10(temp, scale, (uint)digitExponent);
                scale = temp;
            }
            else if (digitExponent < 0) {
                // The exponent is negative creating a multiplication so we multiply up the scaledValue,
                // scaledMarginLow and scaledMarginHigh.
                var pow10 = new BigInt();
                BigInt.Pow10(pow10, (uint)(-digitExponent));

                var temp = new BigInt();
                BigInt.Multiply(temp, scaledValue, pow10);
                scaledValue = temp;

                BigInt.Multiply(temp, scaledMarginLow, pow10);
                scaledMarginLow = temp;

                if (pScaledMarginHigh != scaledMarginLow)
                    BigInt.Multiply2(pScaledMarginHigh, scaledMarginLow);
            }

            // If (value >= 1), our estimate for digitExponent was too low
            if (BigInt.Compare(scaledValue, scale) >= 0) {
                // The exponent estimate was incorrect.
                // Increment the exponent and don't perform the premultiply needed
                // for the first loop iteration.
                digitExponent = digitExponent + 1;
            }
            else {
                // The exponent estimate was correct.
                // Multiply larger by the output base to prepare for the first loop iteration.
                BigInt.Multiply10(scaledValue);
                BigInt.Multiply10(scaledMarginLow);
                if (pScaledMarginHigh != scaledMarginLow)
                    BigInt.Multiply2(pScaledMarginHigh, scaledMarginLow);
            }

            // Compute the cutoff exponent (the exponent of the final digit to print).
            var cutoffExponent = digitExponent - 1000;
            switch (cutoffMode) {

                // print digits until we pass the accuracy margin limits or buffer size
                case FormatCutoffMode.Unique:
                    break;

                // print cutoffNumber of digits or until we reach the buffer size
                case FormatCutoffMode.TotalLength: {
                        var desiredCutoffExponent = digitExponent - (int)cutoffNumber;
                        if (desiredCutoffExponent > cutoffExponent)
                            cutoffExponent = desiredCutoffExponent;
                    }
                    break;

                // print cutoffNumber digits past the decimal point or until we reach the buffer size
                case FormatCutoffMode.FractionLength: {
                        var desiredCutoffExponent = -(int)cutoffNumber;
                        if (desiredCutoffExponent > cutoffExponent)
                            cutoffExponent = desiredCutoffExponent;
                    }
                    break;
            }

            // Output the exponent of the first digit we will print
            pOutExponent = digitExponent - 1;

            // In preparation for calling BigInt_DivideWithRemainder_MaxQuotient9(),
            // we need to scale up our values such that the highest block of the denominator
            // is greater than or equal to 8. We also need to guarantee that the numerator
            // can never have a length greater than the denominator after each loop iteration.
            // This requires the highest block of the denominator to be less than or equal to
            // 429496729 which is the highest number that can be multiplied by 10 without
            // overflowing to a new block.
            if (scale.Length < 1)
                throw new InvalidOperationException();

            var hiBlock = scale[scale.Length - 1];
            if (hiBlock < 8 || hiBlock > 429496729) {
                // Perform a bit shift on all values to get the highest block of the denominator into
                // the range [8,429496729]. We are more likely to make accurate quotient estimations
                // in BigInt_DivideWithRemainder_MaxQuotient9() with higher denominator values so
                // we shift the denominator to place the highest bit at index 27 of the highest block.
                // This is safe because (2^28 - 1) = 268435455 which is less than 429496729. This means
                // that all values with a highest bit at index 27 are within range.
                var hiBlockLog2 = hiBlock.LogBase2();

                if (hiBlockLog2 >= 3 && hiBlockLog2 <= 27)
                    throw new InvalidOperationException();

                var shift = (32 + 27 - hiBlockLog2) % 32;

                BigInt.ShiftLeft(scale, shift);
                BigInt.ShiftLeft(scaledValue, shift);
                BigInt.ShiftLeft(scaledMarginLow, shift);
                if (pScaledMarginHigh != scaledMarginLow)
                    BigInt.Multiply2(pScaledMarginHigh, scaledMarginLow);
            }

            // These values are used to inspect why the print loop terminated so we can properly
            // round the final digit.
            bool low;            // did the value get within marginLow distance from zero
            bool high;           // did the value get within marginHigh distance from one
            uint outputDigit;    // current digit being output

            if (cutoffMode == FormatCutoffMode.Unique) {
                // For the unique cutoff mode, we will try to print until we have reached a level of
                // precision that uniquely distinguishes this value from its neighbors. If we run
                // out of space in the output buffer, we terminate early.
                for (; ; )
                {
                    digitExponent = digitExponent - 1;

                    // divide out the scale to extract the digit
                    outputDigit = BigInt.DivideWithRemainder_MaxQuotient9(scaledValue, scale);

                    if (outputDigit > 9)
                        throw new InvalidOperationException();

                    // update the high end of the value
                    var scaledValueHigh = new BigInt();
                    BigInt.Add(scaledValueHigh, scaledValue, pScaledMarginHigh);

                    // stop looping if we are far enough away from our neighboring values
                    // or if we have reached the cutoff digit
                    low = BigInt.Compare(scaledValue, scaledMarginLow) < 0;
                    high = BigInt.Compare(scaledValueHigh, scale) > 0;
                    if (low | high | (digitExponent == cutoffExponent))
                        break;

                    // store the output digit
                    pOutBuffer.Append((char)('0' + outputDigit));

                    // multiply larger by the output base
                    BigInt.Multiply10(scaledValue);
                    BigInt.Multiply10(scaledMarginLow);
                    if (pScaledMarginHigh != scaledMarginLow)
                        BigInt.Multiply2(pScaledMarginHigh, scaledMarginLow);
                }
            }
            else {
                // For length based cutoff modes, we will try to print until we
                // have exhausted all precision (i.e. all remaining digits are zeros) or
                // until we reach the desired cutoff digit.
                low = false;
                high = false;

                for (; ; )
                {
                    digitExponent = digitExponent - 1;

                    // divide out the scale to extract the digit
                    outputDigit = BigInt.DivideWithRemainder_MaxQuotient9(scaledValue, scale);

                    if (outputDigit > 9)
                        throw new InvalidOperationException();

                    if (scaledValue.Zero | (digitExponent == cutoffExponent))
                        break;

                    // store the output digit
                    pOutBuffer.Append((char)('0' + outputDigit));

                    // multiply larger by the output base
                    BigInt.Multiply10(scaledValue);
                }
            }

            // round off the final digit
            // default to rounding down if value got too close to 0
            var roundDown = low;

            // if it is legal to round up and down
            if (low == high) {
                // round to the closest digit by comparing value with 0.5. To do this we need to convert
                // the inequality to large integer values.
                //  compare( value, 0.5 )
                //  compare( scale * value, scale * 0.5 )
                //  compare( 2 * scale * value, scale )
                BigInt.Multiply2(scaledValue);
                var compare = BigInt.Compare(scaledValue, scale);
                roundDown = compare < 0;

                // if we are directly in the middle, round towards the even digit (i.e. IEEE rouding rules)
                if (compare == 0)
                    roundDown = (outputDigit & 1) == 0;
            }

            // print the rounded digit
            if (roundDown) {
                pOutBuffer.Append((char)('0' + outputDigit));
            }
            else {
                // handle rounding up
                if (outputDigit == 9) {
                    // find the first non-nine prior digit
                    for (; ; )
                    {
                        // if we are at the first digit
                        if (pOutBuffer.Length - bufferOffset == 0) {
                            // output 1 at the next highest exponent
                            pOutBuffer.Append('1');
                            pOutExponent += 1;
                            break;
                        }

                        if (pOutBuffer[pOutBuffer.Length - 1] != '9') {
                            pOutBuffer[pOutBuffer.Length - 1] = (char)(pOutBuffer[pOutBuffer.Length - 1] + 1);
                            break;
                        }

                        pOutBuffer.Remove(pOutBuffer.Length - 1, 1);
                    }
                }
                else {
                    // values in the range [0,8] can perform a simple round up
                    pOutBuffer.Append((char)('0' + outputDigit + 1));
                }
            }

            // return the number of digits output
            return (uint)(pOutBuffer.Length - bufferOffset);
        }

    }
}
