﻿// This code is based on the roslyn file RealParser.cs
// Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.
// See License_Roslyn.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Numerics;
using System.Text;
using SharpFloat.Helpers;

namespace SharpFloat.FloatingPoint {

    public partial struct ExtF80 {

        /// <summary>
        /// Try parsing a correctly-formatted double floating-point literal into the nearest representable double
        /// using IEEE round-to-nearest-ties-to-even rounding mode. Behavior is not defined for inputs that are
        /// not valid C# floating-point literals.
        /// </summary>
        /// <param name="s">The decimal floating-point constant's string</param>
        /// <param name="d">The nearest extended value, if conversion succeeds</param>
        /// <returns>True if the input was converted; false if there was an overflow</returns>
        public static bool TryParse(string s, out ExtF80 d) {
            var str = DecimalFloatingPointString.FromSource(s);
            var status = ConvertDecimalToFloatingPoint(str, out d);
            return status != Status.Overflow;
        }

        /// <summary>
        ///     conversion status
        /// </summary>
        private enum Status {
            OK,
            NoDigits,
            Underflow,
            Overflow
        }

        private const ushort DenormalMantissaBits = 63;
        private const ushort NormalMantissaBits = (DenormalMantissaBits + 1);
        private const int MaxBinaryExponent = 16383;
        private const int ExponentBias = 16383;
        private const int OverflowDecimalExponent = (MaxBinaryExponent + 2 * NormalMantissaBits) / 3;
        private const int MinBinaryExponent = 1 - MaxBinaryExponent;

        /// <summary>
        /// Converts the floating point value 0.mantissa * 2^exponent into the
        /// correct form for the FloatingPointType and stores the bits of the resulting value
        /// into the result object.
        /// The caller must ensure that the mantissa and exponent are correctly computed
        /// such that either [1] the most significant bit of the mantissa is in the
        /// correct position for the FloatingType, or [2] the exponent has been correctly
        /// adjusted to account for the shift of the mantissa that will be required.
        ///
        /// This function correctly handles range errors and stores a zero or infinity in
        /// the result object on underflow and overflow errors, respectively.  This
        /// function correctly forms denormal numbers when required.
        ///
        /// If the provided mantissa has more bits of precision than can be stored in the
        /// result object, the mantissa is rounded to the available precision.  Thus, if
        /// possible, the caller should provide a mantissa with at least one more bit of
        /// precision than is required, to ensure that the mantissa is correctly rounded.
        /// (The caller should not round the mantissa before calling this function.)
        /// </summary>
        /// <param name="initialMantissa">The bits of the mantissa</param>
        /// <param name="initialExponent">The exponent</param>
        /// <param name="hasZeroTail">Whether there are any nonzero bits past the supplied mantissa</param>
        /// <param name="result">Where the bits of the floating-point number are stored</param>
        /// <param name="isNegative"><c>true</c> if this value is negative</param>
        /// <returns>A status indicating whether the conversion succeeded and why</returns>
        private static Status AssembleFloatingPointValue(BigInteger initialMantissa, int initialExponent, bool hasZeroTail, bool isNegative, out ExtF80 result) {

            // number of bits by which we must adjust the mantissa to shift it into the
            // correct position, and compute the resulting base two exponent for the
            // normalized mantissa:
            var initialMantissaBits = CountSignificantBits(initialMantissa);
            var normalMantissaShift = NormalMantissaBits - (int)initialMantissaBits;
            var normalExponent = initialExponent - normalMantissaShift;

            var mantissa = initialMantissa;
            var exponent = normalExponent;

            if (normalExponent > MaxBinaryExponent) {
                // The exponent is too large to be represented by the floating point
                // type; report the overflow condition:
                result = Infinity;
                return Status.Overflow;
            }
            else if (normalExponent < MinBinaryExponent) {
                // The exponent is too small to be represented by the floating point
                // type as a normal value, but it may be representable as a denormal
                // value.  Compute the number of bits by which we need to shift the
                // mantissa in order to form a denormal number.
                var denormalMantissaShift = normalMantissaShift + normalExponent + ExponentBias - 1;

                // Denormal values have an exponent of zero, so the debiased exponent is
                // the negation of the exponent bias:
                exponent = -ExponentBias;

                if (denormalMantissaShift < 0) {

                    // Use two steps for right shifts:  for a shift of N bits, we first
                    // shift by N-1 bits, then shift the last bit and use its value to
                    // round the mantissa.
                    //RightShiftWithRounding(ref mantissa, -denormalMantissaShift, hasZeroTail);
                    ShiftRight(ref mantissa, (uint)-denormalMantissaShift);

                    // If the mantissa is now zero, we have underflowed:
                    if (mantissa == 0) {
                        result = Zero;
                        return Status.Underflow;
                    }

                    // When we round the mantissa, the result may be so large that the
                    // number becomes a normal value.  For example, consider the single
                    // precision case where the mantissa is 0x01ffffff and a right shift
                    // of 2 is required to shift the value into position. We perform the
                    // shift in two steps:  we shift by one bit, then we shift again and
                    // round using the dropped bit.  The initial shift yields 0x00ffffff.
                    // The rounding shift then yields 0x007fffff and because the least
                    // significant bit was 1, we add 1 to this number to round it.  The
                    // final result is 0x00800000.
                    //
                    // 0x00800000 is 24 bits, which is more than the 23 bits available
                    // in the mantissa.  Thus, we have rounded our denormal number into
                    // a normal number.
                    //
                    // We detect this case here and re-adjust the mantissa and exponent
                    // appropriately, to form a normal number:
                    //if (mantissa > DenormalMantissaMask) {
                    if (mantissa > new BigInteger(0xFFFFFFFFFFFFFFFF)) {
                        exponent = initialExponent - denormalMantissaShift - normalMantissaShift;
                    }
                }
                else {
                    mantissa <<= denormalMantissaShift;
                }
            }
            else {
                if (normalMantissaShift < 0) {
                    // Use two steps for right shifts:  for a shift of N bits, we first
                    // shift by N-1 bits, then shift the last bit and use its value to
                    // round the mantissa.
                    RightShiftWithRounding(ref mantissa, -normalMantissaShift, hasZeroTail);

                    // When we round the mantissa, it may produce a result that is too
                    // large.  In this case, we divide the mantissa by two and increment
                    // the exponent (this does not change the value).
                    if (CountSignificantBits(mantissa) > NormalMantissaBits) {
                        mantissa >>= 1;
                        ++exponent;

                        // The increment of the exponent may have generated a value too
                        // large to be represented.  In this case, report the overflow:
                        if (exponent > MaxBinaryExponent) {
                            result = Infinity;
                            return Status.Overflow;
                        }
                    }
                }
                else if (normalMantissaShift > 0) {
                    mantissa <<= normalMantissaShift;
                }
            }

            var finalExponent = (ushort)(exponent + ExponentBias);
            var finalMantissa = (finalExponent == 0 ? 0 : 0x8000000000000000) | (ulong)mantissa;
            result = new ExtF80(finalExponent.PackToExtF80UI64(isNegative), finalMantissa);
            return Status.OK;
        }

        /// <summary>
        ///     Return the number of significant bits set.
        /// </summary>
        private static uint CountSignificantBits(ulong data) {
            var result = 0U;
            while (data != 0) {
                data >>= 1;
                result++;
            }
            return result;
        }

        /// <summary>
        /// Computes value / 2^shift, then rounds the result according to the current
        /// rounding mode.  By the time we call this function, we will already have
        /// discarded most digits.  The caller must pass true for has_zero_tail if
        /// all discarded bits were zeroes.
        /// </summary>
        /// <param name="value">The value to shift</param>
        /// <param name="shift">The amount of shift</param>
        /// <param name="hasZeroTail">Whether there are any less significant nonzero bits in the value</param>
        /// <returns></returns>
        private static void RightShiftWithRounding(ref BigInteger value, int shift, bool hasZeroTail) {
            var extraBitsMask = (1UL << (shift - 1)) - 1;
            var roundBitMask = (1UL << (shift - 1));
            var lsbBitMask = 1UL << shift;

            var lsbBit = (value & lsbBitMask) != 0;
            var roundBit = (value & roundBitMask) != 0;
            var hasTailBits = !hasZeroTail || (value & extraBitsMask) != 0;

            ShiftRight(ref value, (uint)shift);
            value += (ShouldRoundUp(lsbBit: lsbBit, roundBit: roundBit, hasTailBits: hasTailBits) ? 1UL : 0);
        }

        /// <summary>
        /// Determines whether a mantissa should be rounded up in the
        /// round-to-nearest-ties-to-even mode given [1] the value of the least
        /// significant bit of the mantissa, [2] the value of the next bit after
        /// the least significant bit (the "round" bit) and [3] whether any
        /// trailing bits after the round bit are set.
        ///
        /// The mantissa is treated as an unsigned integer magnitude.
        ///
        /// For this function, "round up" is defined as "increase the magnitude" of the
        /// mantissa.
        /// </summary>
        /// <param name="lsbBit">the least-significant bit of the representable value</param>
        /// <param name="roundBit">the bit following the least-significant bit</param>
        /// <param name="hasTailBits">true if there are any (less significant) bits set following roundBit</param>
        /// <returns></returns>
        private static bool ShouldRoundUp(bool lsbBit, bool roundBit, bool hasTailBits) {
            // If there are insignificant set bits, we need to round to the
            // nearest; there are two cases:
            // we round up if either [1] the value is slightly greater than the midpoint
            // between two exactly representable values or [2] the value is exactly the
            // midpoint between two exactly representable values and the greater of the
            // two is even (this is "round-to-even").
            return roundBit && (hasTailBits || lsbBit);
        }

        /// <summary>
        /// Convert a DecimalFloatingPointString to the bits of the given floating-point type.
        /// </summary>
        private static Status ConvertDecimalToFloatingPoint(DecimalFloatingPointString data, out ExtF80 result) {
            if (data.Mantissa.Length == 0) {
                result = Zero;
                return Status.NoDigits;
            }

            // To generate an N bit mantissa we require N + 1 bits of precision.  The
            // extra bit is used to correctly round the mantissa (if there are fewer bits
            // than this available, then that's totally okay; in that case we use what we
            // have and we don't need to round).
            var requiredBitsOfPrecision = (uint)NormalMantissaBits + 1;

            // The input is of the form 0.Mantissa x 10^Exponent, where 'Mantissa' are
            // the decimal digits of the mantissa and 'Exponent' is the decimal exponent.
            // We decompose the mantissa into two parts: an integer part and a fractional
            // part.  If the exponent is positive, then the integer part consists of the
            // first 'exponent' digits, or all present digits if there are fewer digits.
            // If the exponent is zero or negative, then the integer part is empty.  In
            // either case, the remaining digits form the fractional part of the mantissa.
            var positiveExponent = (uint)Math.Max(0, data.Exponent);
            var integerDigitsPresent = Math.Min(positiveExponent, data.MantissaCount);
            var integerDigitsMissing = positiveExponent - integerDigitsPresent;
            var integerFirstIndex = 0U;
            var integerLastIndex = integerDigitsPresent;

            var fractionalFirstIndex = integerLastIndex;
            var fractionalLastIndex = data.MantissaCount;
            var fractionalDigitsPresent = fractionalLastIndex - fractionalFirstIndex;

            // First, we accumulate the integer part of the mantissa into a big_integer:
            var integerValue = AccumulateDecimalDigitsIntoBigInteger(data, integerFirstIndex, integerLastIndex);

            if (integerDigitsMissing > 0) {
                if (integerDigitsMissing > OverflowDecimalExponent) {
                    result = Infinity;
                    return Status.Overflow;
                }

                MultiplyByPowerOfTen(ref integerValue, integerDigitsMissing);
            }

            // At this point, the integer_value contains the value of the integer part
            // of the mantissa.  If either [1] this number has more than the required
            // number of bits of precision or [2] the mantissa has no fractional part,
            // then we can assemble the result immediately:
            var integerBitsOfPrecision = CountSignificantBits(integerValue, out var integerValueAsBytes);
            if (integerBitsOfPrecision >= requiredBitsOfPrecision ||
                fractionalDigitsPresent == 0) {
                return ConvertBigIntegerToFloatingPointBits(
                    integerValueAsBytes,
                    integerBitsOfPrecision,
                    fractionalDigitsPresent != 0,
                    data.IsNegative,
                    out result);
            }

            // Otherwise, we did not get enough bits of precision from the integer part,
            // and the mantissa has a fractional part.  We parse the fractional part of
            // the mantissa to obtain more bits of precision.  To do this, we convert
            // the fractional part into an actual fraction N/M, where the numerator N is
            // computed from the digits of the fractional part, and the denominator M is
            // computed as the power of 10 such that N/M is equal to the value of the
            // fractional part of the mantissa.

            var fractionalDenominatorExponent = data.Exponent < 0
                ? fractionalDigitsPresent + (uint)-data.Exponent
                : fractionalDigitsPresent;

            if (integerBitsOfPrecision == 0 && (fractionalDenominatorExponent - (int)data.MantissaCount) > OverflowDecimalExponent) {
                // If there were any digits in the integer part, it is impossible to
                // underflow (because the exponent cannot possibly be small enough),
                // so if we underflow here it is a true underflow and we return zero.
                result = Zero;
                return Status.Underflow;
            }

            var fractionalNumerator = AccumulateDecimalDigitsIntoBigInteger(data, fractionalFirstIndex, fractionalLastIndex);

            var fractionalDenominator = new BigInteger(1);
            MultiplyByPowerOfTen(ref fractionalDenominator, fractionalDenominatorExponent);

            // Because we are using only the fractional part of the mantissa here, the
            // numerator is guaranteed to be smaller than the denominator.  We normalize
            // the fraction such that the most significant bit of the numerator is in
            // the same position as the most significant bit in the denominator.  This
            // ensures that when we later shift the numerator N bits to the left, we
            // will produce N bits of precision.
            var fractionalNumeratorBits = CountSignificantBits(fractionalNumerator);
            var fractionalDenominatorBits = CountSignificantBits(fractionalDenominator);

            var fractionalShift = fractionalDenominatorBits > fractionalNumeratorBits
                ? fractionalDenominatorBits - fractionalNumeratorBits
                : 0;

            if (fractionalShift > 0) {
                ShiftLeft(ref fractionalNumerator, fractionalShift);
            }

            var requiredFractionalBitsOfPrecision =
                requiredBitsOfPrecision -
                integerBitsOfPrecision;

            var remainingBitsOfPrecisionRequired = requiredFractionalBitsOfPrecision;

            if (integerBitsOfPrecision > 0) {
                // If the fractional part of the mantissa provides no bits of precision
                // and cannot affect rounding, we can just take whatever bits we got from
                // the integer part of the mantissa.  This is the case for numbers like
                // 5.0000000000000000000001, where the significant digits of the fractional
                // part start so far to the right that they do not affect the floating
                // point representation.
                //
                // If the fractional shift is exactly equal to the number of bits of
                // precision that we require, then no fractional bits will be part of the
                // result, but the result may affect rounding.  This is e.g. the case for
                // large, odd integers with a fractional part greater than or equal to .5.
                // Thus, we need to do the division to correctly round the result.
                if (fractionalShift > remainingBitsOfPrecisionRequired) {
                    return ConvertBigIntegerToFloatingPointBits(
                        integerValueAsBytes,
                        integerBitsOfPrecision,
                        fractionalDigitsPresent != 0,
                        data.IsNegative,
                        out result);
                }

                remainingBitsOfPrecisionRequired -= fractionalShift;
            }

            // If there was no integer part of the mantissa, we will need to compute the
            // exponent from the fractional part.  The fractional exponent is the power
            // of two by which we must multiply the fractional part to move it into the
            // range [1.0, 2.0).  This will either be the same as the shift we computed
            // earlier, or one greater than that shift:
            var fractionalExponent = fractionalNumerator < fractionalDenominator
                ? fractionalShift + 1
                : fractionalShift;

            ShiftLeft(ref fractionalNumerator, remainingBitsOfPrecisionRequired);
            var bigFractionalMantissa = BigInteger.DivRem(fractionalNumerator, fractionalDenominator, out var fractionalRemainder);
            var hasZeroTail = fractionalRemainder.IsZero;

            // We may have produced more bits of precision than were required.  Check,
            // and remove any "extra" bits:
            var fractionalMantissaBits = CountSignificantBits(bigFractionalMantissa);
            if (fractionalMantissaBits > requiredFractionalBitsOfPrecision) {
                var shift = (int)(fractionalMantissaBits - requiredFractionalBitsOfPrecision);
                var tailMask = new BigInteger(1);
                ShiftLeft(ref tailMask, (uint)shift);
                tailMask -= 1;
                hasZeroTail = hasZeroTail && (bigFractionalMantissa & tailMask) == 0;
                ShiftRight(ref bigFractionalMantissa, (uint)shift);
            }

            // Compose the mantissa from the integer and fractional parts:
            ShiftLeft(ref integerValue, requiredFractionalBitsOfPrecision);
            var completeMantissa = integerValue + bigFractionalMantissa;

            // Compute the final exponent:
            // * If the mantissa had an integer part, then the exponent is one less than
            //   the number of bits we obtained from the integer part.  (It's one less
            //   because we are converting to the form 1.11111, with one 1 to the left
            //   of the decimal point.)
            // * If the mantissa had no integer part, then the exponent is the fractional
            //   exponent that we computed.
            // Then, in both cases, we subtract an additional one from the exponent, to
            // account for the fact that we've generated an extra bit of precision, for
            // use in rounding.
            var finalExponent = integerBitsOfPrecision > 0
                ? (int)integerBitsOfPrecision - 2
                : -(int)(fractionalExponent) - 1;

            return AssembleFloatingPointValue(completeMantissa, finalExponent, hasZeroTail, data.IsNegative, out result);
        }


        /// <summary>
        /// This type is used to hold a partially-parsed string representation of a
        /// floating point number.  The number is stored in the following form:
        ///  <pre>
        ///     0.Mantissa * 10^Exponent
        ///  </pre>
        /// The Mantissa buffer stores the mantissa digits as characters in a string.
        /// The MantissaCount gives the number of digits present in the Mantissa buffer.
        /// There shall be neither leading nor trailing zero digits in the Mantissa.
        /// Note that this represents only nonnegative floating-point literals; the
        /// negative sign in C# and VB is actually a separate unary negation operator.
        /// </summary>
        [DebuggerDisplay("{Sign}0.{Mantissa}e{Exponent}")]
        private struct DecimalFloatingPointString {
            public int Exponent;
            public string Mantissa;
            public bool IsNegative;
            public uint MantissaCount => (uint)Mantissa.Length;
            public string Sign => IsNegative ? "-" : string.Empty;

            /// <summary>
            /// Create a DecimalFloatingPointString from a string representing a floating-point literal.
            /// </summary>
            /// <param name="source">The text of the floating-point literal</param>
            public static DecimalFloatingPointString FromSource(string source) {
                var mantissaBuilder = new StringBuilder();
                var exponent = 0;
                var i = 0;
                var result = default(DecimalFloatingPointString);

                if (source[i] == '-') {
                    result.IsNegative = true;
                    i++;
                }

                while (i < source.Length && source[i] == '0')
                    i++;
                var skippedDecimals = 0;
                while (i < source.Length && source[i] >= '0' && source[i] <= '9') {
                    if (source[i] == '0') {
                        skippedDecimals++;
                    }
                    else {
                        mantissaBuilder.Append('0', skippedDecimals);
                        skippedDecimals = 0;
                        mantissaBuilder.Append(source[i]);
                    }
                    exponent++;
                    i++;
                }
                if (i < source.Length && source[i] == '.') {
                    i++;
                    while (i < source.Length && source[i] >= '0' && source[i] <= '9') {
                        if (source[i] == '0') {
                            skippedDecimals++;
                        }
                        else {
                            mantissaBuilder.Append('0', skippedDecimals);
                            skippedDecimals = 0;
                            mantissaBuilder.Append(source[i]);
                        }
                        i++;
                    }
                }

                result.Mantissa = mantissaBuilder.ToString();
                if (i < source.Length && (source[i] == 'e' || source[i] == 'E')) {
                    const int MAX_EXP = (1 << 30); // even playing ground
                    var exponentSign = '\0';
                    i++;
                    if (i < source.Length && (source[i] == '-' || source[i] == '+')) {
                        exponentSign = source[i];
                        i++;
                    }
                    var firstExponent = i;
                    var lastExponent = i;
                    while (i < source.Length && source[i] >= '0' && source[i] <= '9')
                        lastExponent = ++i;

                    if (int.TryParse(source.Substring(firstExponent, lastExponent - firstExponent), out var exponentMagnitude) &&
                        exponentMagnitude <= MAX_EXP) {
                        if (exponentSign == '-') {
                            exponent -= exponentMagnitude;
                        }
                        else {
                            exponent += exponentMagnitude;
                        }
                    }
                    else {
                        exponent = exponentSign == '-' ? -MAX_EXP : MAX_EXP;
                    }
                }
                result.Exponent = exponent;
                return result;
            }
        }


        /// <summary>
        /// Multiply a BigInteger by the given power of ten.
        /// </summary>
        /// <param name="number">The BigInteger to multiply by a power of ten and replace with the product</param>
        /// <param name="power">The power of ten to multiply it by</param>
        private static void MultiplyByPowerOfTen(ref BigInteger number, uint power) {
            var powerOfTen = BigInteger.Pow(new BigInteger(10), (int)power);
            number *= powerOfTen;
        }

        /// <summary>
        /// This function is part of the fast track for integer floating point strings.
        /// It takes an integer stored as an array of bytes (lsb first) and converts the value into its FloatingType
        /// representation, storing the bits into "result".  If the value is not
        /// representable, +/-infinity is stored and overflow is reported (since this
        /// function only deals with integers, underflow is impossible).
        /// </summary>
        /// <param name="integerValueAsBytes">the bits of the integer, least significant bits first</param>
        /// <param name="integerBitsOfPrecision">the number of bits of precision in integerValueAsBytes</param>
        /// <param name="hasNonzeroFractionalPart">whether there are nonzero digits after the decimal</param>
        /// <param name="isNegative"><c>true</c> if this is a negative number</param>
        /// <param name="result">the result</param>
        /// <returns>An indicator of the kind of result</returns>
        private static Status ConvertBigIntegerToFloatingPointBits(byte[] integerValueAsBytes, uint integerBitsOfPrecision, bool hasNonzeroFractionalPart, bool isNegative, out ExtF80 result) {
            int baseExponent = DenormalMantissaBits;
            var has_zero_tail = !hasNonzeroFractionalPart;
            var topElementIndex = ((int)integerBitsOfPrecision - 1) / 8;
            var bottomElementIndex = Math.Max(0, topElementIndex - (72 / 8) + 1);
            var exponent = baseExponent + bottomElementIndex * 8;
            var mantissa = new BigInteger();
            for (var i = topElementIndex; i >= bottomElementIndex; i--) {
                ShiftLeft(ref mantissa, 8);
                mantissa |= integerValueAsBytes[i];
            }
            for (var i = bottomElementIndex - 1; has_zero_tail && i >= 0; i--) {
                if (integerValueAsBytes[i] != 0)
                    has_zero_tail = false;
            }

            return AssembleFloatingPointValue(mantissa, exponent, has_zero_tail, isNegative, out result);
        }

        /// <summary>
        /// Multiply a BigInteger by the given power of two.
        /// </summary>
        /// <param name="number">The BigInteger to multiply by a power of two and replace with the product</param>
        /// <param name="shift">The power of two to multiply it by</param>
        private static void ShiftLeft(ref BigInteger number, uint shift) {
            var powerOfTwo = BigInteger.Pow(new BigInteger(2), (int)shift);
            number *= powerOfTwo;
        }

        private static void ShiftRight(ref BigInteger number, uint shift) {
            var powerOfTwo = BigInteger.Pow(new BigInteger(2), (int)shift);
            number /= powerOfTwo;
        }

        /// <summary>
        /// Parse a sequence of digits into a BigInteger.
        /// </summary>
        /// <param name="data">The DecimalFloatingPointString containing the digits in its Mantissa</param>
        /// <param name="integer_first_index">The index of the first digit to convert</param>
        /// <param name="integer_last_index">The index just past the last digit to convert</param>
        /// <returns>The BigInteger result</returns>
        private static BigInteger AccumulateDecimalDigitsIntoBigInteger(DecimalFloatingPointString data, uint integer_first_index, uint integer_last_index) {
            if (integer_first_index == integer_last_index)
                return new BigInteger(0);
            var valueString = data.Mantissa.Substring((int)integer_first_index, (int)(integer_last_index - integer_first_index));
            return BigInteger.Parse(valueString, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Return the number of significant bits set.
        /// </summary>
        private static uint CountSignificantBits(BigInteger data, out byte[] dataBytes) {
            if (data.IsZero) {
                dataBytes = new byte[1];
                return 0;
            }

            dataBytes = data.ToByteArray(); // the bits of the BigInteger, least significant bits first
            for (var i = dataBytes.Length - 1; i >= 0; i--) {
                var v = dataBytes[i];
                if (v != 0)
                    return 8 * (uint)i + CountSignificantBits(v);
            }

            return 0;
        }

        /// <summary>
        /// Return the number of significant bits set.
        /// </summary>
        private static uint CountSignificantBits(BigInteger data) {
            return CountSignificantBits(data, out _);
        }

    }
}
