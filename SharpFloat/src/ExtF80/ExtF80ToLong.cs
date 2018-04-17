/*  This library is a port of the standard softfloat library, Release 3e, from John Hauser to C#.
 *
 *    Copyright 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018 The Regents of the
 *    University of California.  All rights reserved.
 *
 *    Copyright 2018 Bastian Turcs. All rights reserved.
 *
 *    Redistribution and use in source and binary forms, with or without
 *    modification, are permitted provided that the following conditions are met:
 *
 *     1. Redistributions of source code must retain the above copyright notice,
 *        this list of conditions, and the following disclaimer.
 *
 *     2. Redistributions in binary form must reproduce the above copyright
 *        notice, this list of conditions, and the following disclaimer in the
 *        documentation and/or other materials provided with the distribution.
 *
 *     3. Neither the name of the University nor the names of its contributors
 *        may be used to endorse or promote products derived from this software
 *        without specific prior written permission.
 *
 *    THIS SOFTWARE IS PROVIDED BY THE REGENTS AND CONTRIBUTORS "AS IS", AND ANY
 *    EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 *    WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE, ARE
 *    DISCLAIMED.  IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR ANY
 *    DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 *    (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 *    LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 *    ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 *    (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
 *    THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using SharpFloat.Globals;
using SharpFloat.Helpers;

namespace SharpFloat.FloatingPoint {

    public partial struct ExtF80 {

        private const long i64_fromPosOverflow = -0x7FFFFFFFFFFFFFFF - 1;
        private const long i64_fromNegOverflow = -0x7FFFFFFFFFFFFFFF - 1;
        private const long i64_fromNaN = -0x7FFFFFFFFFFFFFFF - 1;
        private const ulong ui64_fromPosOverflow = 0xFFFFFFFFFFFFFFFF;
        private const ulong ui64_fromNegOverflow = 0xFFFFFFFFFFFFFFFF;
        private const ulong ui64_fromNaN = 0xFFFFFFFFFFFFFFFF;

        /// <summary>
        ///     convert this number to a long value
        /// </summary>
        /// <returns></returns>
        public long ToInt64()
            => (long)this;

        /// <summary>
        ///     truncate a 80-bit floating point number to long
        /// </summary>
        /// <param name="a">number</param>
        public static explicit operator long(in ExtF80 a)
            => a.ToLong(RoundingMode.MinimumMagnitude, true);

        /// <summary>
        ///     convert this value to an unsigned long
        /// </summary>
        /// <returns></returns>
        public ulong ToUInt64()
            => (ulong)this;

        /// <summary>
        ///     truncate a 80-bit floating point number to unsigned long
        /// </summary>
        /// <param name="a">number</param>
        public static explicit operator ulong(in ExtF80 a)
            => a.ToULong(RoundingMode.MinimumMagnitude, true);

        /// <summary>
        ///     round a 80-bit floating point number to long
        /// </summary>
        /// <param name="exact">if <c>true</c> the inexact flag is raised</param>
        /// <param name="roundingMode">explicit rounding mode</param>
        public long ToLong(RoundingMode roundingMode, bool exact = false) {
            ushort uiA64;
            bool sign;
            int exp;
            ulong sig;
            int shiftDist;
            ulong sigExtra;
            UInt64Extra sig64Extra;

            uiA64 = signExp;
            sign = IsNegative;
            exp = UnsignedExponent;
            sig = signif;

            shiftDist = 0x403E - exp;
            if (shiftDist <= 0) {

                if (0 != shiftDist) {
                    Settings.Raise(ExceptionFlags.Invalid);
                    return
                        (exp == MaxExponent) && (0 != (sig & (MaskAll63Bits)))
                            ? i64_fromNaN
                            : sign ? i64_fromNegOverflow : i64_fromPosOverflow;
                }

                sigExtra = 0;
            }
            else {
                sig64Extra = UInt64Extra.ShiftRightJam64Extra(sig, 0, shiftDist);
                sig = sig64Extra.v;
                sigExtra = sig64Extra.extra;
            }
            return RoundToI64(sign, sig, sigExtra, roundingMode, exact);
        }

        /// <summary>
        ///     round a 80-bit floating point number to long
        /// </summary>
        /// <param name="exact">if <c>true</c> the inexact flag is raised</param>
        /// <param name="roundingMode">explicit rounding mode</param>
        public ulong ToULong(RoundingMode roundingMode, bool exact = false) {

            var uiA64 = signExp;
            var sign = IsNegative;
            var exp = UnsignedExponent;
            var sig = signif;
            var shiftDist = 0x403E - exp;

            if (shiftDist < 0) {
                Settings.Raise(ExceptionFlags.Invalid);
                return
                    (exp == 0x7FFF) && ((sig & MaskAll63Bits) != 0)
                        ? ui64_fromNaN
                        : sign ? ui64_fromNegOverflow : ui64_fromPosOverflow;
            }

            var sigExtra = 0UL;
            if (shiftDist != 0) {
                var sig64Extra = UInt64Extra.ShiftRightJam64Extra(sig, 0, shiftDist);
                sig = sig64Extra.v;
                sigExtra = sig64Extra.extra;
            }

            return RoundToUI64(sign, sig, sigExtra, roundingMode, exact);
        }

        private static long RoundToI64(bool sign, ulong sig, ulong sigExtra, RoundingMode roundingMode, bool exact) {
            long z;

            if ((roundingMode == RoundingMode.NearMaximumMagnitude) || (roundingMode == RoundingMode.NearEven)) {
                if (MaskBit64 <= sigExtra) {
                    ++sig;
                    if (0 == sig) {
                        Settings.Raise(ExceptionFlags.Invalid);
                        return sign ? i64_fromNegOverflow : i64_fromPosOverflow;
                    }
                    if ((sigExtra == MaskBit64) && (roundingMode == RoundingMode.NearEven)) {
                        sig &= ~(ulong)1;
                    }
                }
            }
            else {
                if (sigExtra != 0 && (sign ? (roundingMode == RoundingMode.Minimum) || (roundingMode == RoundingMode.Odd) : (roundingMode == RoundingMode.Maximum))) {
                    ++sig;
                    if (0 == sig) {
                        Settings.Raise(ExceptionFlags.Invalid);
                        return sign ? i64_fromNegOverflow : i64_fromPosOverflow;
                    }
                    if ((sigExtra == MaskBit64) && (roundingMode == RoundingMode.NearEven)) {
                        sig &= ~(ulong)1;
                    }
                }
            }
            var ui = sign ? (~sig + 1) : sig;
            z = (long)ui;
            if (z != 0 && ((z < 0) ^ sign)) {
                Settings.Raise(ExceptionFlags.Invalid);
                return sign ? i64_fromNegOverflow : i64_fromPosOverflow;
            }
            if (sigExtra != 0) {
                if (roundingMode == RoundingMode.Odd)
                    z |= 1;
                if (exact)
                    Settings.Raise(ExceptionFlags.Inexact);
            }
            return z;
        }


        private static ulong RoundToUI64(bool sign, ulong sig, ulong sigExtra, RoundingMode roundingMode, bool exact) {

            if ((roundingMode == RoundingMode.NearMaximumMagnitude) || (roundingMode == RoundingMode.NearEven)) {
                if (MaskBit64 <= sigExtra) {
                    ++sig;
                    if (sig == 0) {
                        Settings.Raise(ExceptionFlags.Invalid);
                        return sign ? ui64_fromNegOverflow : ui64_fromPosOverflow;
                    }
                    if ((sigExtra == MaskBit64) && (roundingMode == RoundingMode.NearEven)) {
                        sig &= ~(ulong)1;
                    }
                }
            }
            else {
                if (sign) {
                    if (0 == (sig | sigExtra))
                        return 0;
                    if (roundingMode == RoundingMode.Minimum || roundingMode == RoundingMode.Odd) {
                        Settings.Raise(ExceptionFlags.Invalid);
                        return sign ? ui64_fromNegOverflow : ui64_fromPosOverflow;
                    }
                }
                else {
                    if ((roundingMode == RoundingMode.Maximum) && (0 != sigExtra)) {
                        ++sig;
                        if (sig == 0) {
                            Settings.Raise(ExceptionFlags.Invalid);
                            return sign ? ui64_fromNegOverflow : ui64_fromPosOverflow;
                        }
                        if ((sigExtra == MaskBit64) && (roundingMode == RoundingMode.NearEven)) {
                            sig &= ~(ulong)1;
                        }
                    }
                }
            }
            if (sign && sig != 0) {
                Settings.Raise(ExceptionFlags.Invalid);
                return sign ? ui64_fromNegOverflow : ui64_fromPosOverflow;
            }
            if (sigExtra != 0) {
                if (roundingMode == RoundingMode.Odd)
                    sig |= 1;
                if (exact)
                    Settings.Raise(ExceptionFlags.Inexact);
            }
            return sig;
        }
    }
}
