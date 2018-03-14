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

using SharpFloat.Globals;
using SharpFloat.Helpers;

namespace SharpFloat.FloatingPoint {

    public partial struct ExtF80 {

        public static ExtF80 RoundPack(bool sign, int exp, ulong sig, ulong sigExtra, byte roundingPrecision) {
            if (roundingPrecision == 64 || roundingPrecision == 32) {
                return RoundPackToExtF80WithReducedPrecision(sign, exp, sig, sigExtra, roundingPrecision);
            }
            else {
                return RoundPackToExtF80WithStandardPrecision(sign, exp, sig, sigExtra, roundingPrecision);
            }
        }

        private static ExtF80 RoundPackToExtF80WithStandardPrecision(bool sign, int exp, ulong sig, ulong sigExtra, byte roundingPrecision) {
            var roundingMode = Settings.RoundingMode;
            var doIncrement = MaskBit64 <= sigExtra;

            if (roundingMode != RoundingMode.NearEven && roundingMode != RoundingMode.NearMaximumMagnitude) {
                doIncrement = (roundingMode == (sign ? RoundingMode.Minimum : RoundingMode.Maximum)) && sigExtra != 0;
            }

            if (0x7FFD <= (uint)(exp - 1)) {
                if (exp <= 0) {
                    var isTiny = (Settings.DetectTininess == DetectTininess.BeforeRounding)
                        || (exp < 0)
                        || !doIncrement
                        || (sig < 0xFFFFFFFFFFFFFFFFUL);

                    var sig64Extra = UInt64Extra.ShiftRightJam64Extra(sig, sigExtra, 1 - exp);
                    exp = 0;
                    sig = sig64Extra.v;
                    sigExtra = sig64Extra.extra;
                    if (sigExtra != 0) {
                        if (isTiny)
                            Settings.Raise(ExceptionFlags.Underflow);
                        Settings.Raise(ExceptionFlags.Inexact);
                        if (roundingMode == RoundingMode.Odd) {
                            return new ExtF80(exp.PackToExtF80UI64(sign), sig | 1);
                        }
                    }
                    doIncrement = (0x8000000000000000UL <= sigExtra);
                    if (roundingMode != RoundingMode.NearEven && roundingMode != RoundingMode.NearMaximumMagnitude) {
                        doIncrement =
                            (((roundingMode == (sign ? RoundingMode.Minimum : RoundingMode.Maximum)) ? true : false)
                                && sigExtra != 0);
                    }

                    if (doIncrement) {
                        ++sig;
                        sig &=
                            ~(ulong)(((sigExtra & 0x7FFFFFFFFFFFFFFFUL) == 0 ? 1 : 0UL) & (roundingMode == RoundingMode.NearEven ? 1 : 0UL));
                        exp = (ushort)(((sig & 0x8000000000000000UL) != 0) ? 1 : 0);
                    }
                    return new ExtF80(exp.PackToExtF80UI64(sign), sig);
                }

                if ((0x7FFE < exp) || ((exp == 0x7FFE) && (sig == 0xFFFFFFFFFFFFFFFFUL) && doIncrement)) {
                    Settings.Raise(ExceptionFlags.Overflow | ExceptionFlags.Inexact);
                    if (roundingMode == RoundingMode.NearEven || roundingMode == RoundingMode.NearMaximumMagnitude || roundingMode == (sign ? RoundingMode.Minimum : RoundingMode.Maximum)) {
                        exp = 0x7FFF;
                        sig = 0x8000000000000000UL;
                    }
                    else {
                        exp = 0x7FFE;
                        sig = ~0UL;
                    }
                    return new ExtF80(exp.PackToExtF80UI64(sign), sig);
                }
            }

            if (sigExtra != 0) {
                Settings.Raise(ExceptionFlags.Inexact);
                if (roundingMode == RoundingMode.Odd) {
                    return new ExtF80(exp.PackToExtF80UI64(sign), sig | 1);
                }
            }

            if (doIncrement) {
                ++sig;
                if (sig == 0) {
                    ++exp;
                    sig = MaskBit64;
                }
                else {
                    sig &= ~(ulong)
                             (((sigExtra & MaskAll63Bits) == 0 ? 1 : 0)
                                  & (roundingMode == RoundingMode.NearEven ? 1 : 0));
                }
            }
            return new ExtF80(exp.PackToExtF80UI64(sign), sig);
        }

        private static ExtF80 RoundPackToExtF80WithReducedPrecision(bool sign, int exp, ulong sig, ulong sigExtra, byte roundingPrecision) {
            ulong roundIncrement, roundMask, roundBits;
            bool isTiny;
            var roundingMode = Settings.RoundingMode;

            if (roundingPrecision == 64) {
                roundIncrement = 0x0000000000000400UL;
                roundMask = 0x00000000000007FFUL;
            }
            else {
                roundIncrement = 0x0000008000000000UL;
                roundMask = 0x000000FFFFFFFFFFUL;
            }

            sig |= (sigExtra != 0) ? 1UL : 0UL;
            if (roundingMode != RoundingMode.NearEven && (roundingMode != RoundingMode.NearMaximumMagnitude)) {
                roundIncrement = (roundingMode == (sign ? RoundingMode.Minimum : RoundingMode.Maximum)) ? roundMask : 0;
            }
            roundBits = sig & roundMask;

            if (0x7FFD <= (uint)(exp - 1)) {
                if (exp <= 0) {
                    isTiny = (Settings.DetectTininess == DetectTininess.BeforeRounding)
                        || (exp < 0)
                        || (sig <= (ulong)(sig + roundIncrement));

                    sig = sig.ShiftRightJam64((uint)(1 - exp));
                    roundBits = sig & roundMask;
                    if (roundBits != 0) {
                        if (isTiny)
                            Settings.Raise(ExceptionFlags.Underflow);
                        Settings.Raise(ExceptionFlags.Inexact);
                        if (roundingMode == RoundingMode.Odd) {
                            sig |= roundMask + 1;
                        }
                    }
                    sig += roundIncrement;
                    exp = (ushort)(((sig & 0x8000000000000000UL) != 0UL) ? 1 : 0);
                    roundIncrement = roundMask + 1;
                    if (roundingMode == RoundingMode.NearEven && (roundBits << 1 == roundIncrement)) {
                        roundMask |= roundIncrement;
                    }
                    sig &= ~roundMask;
                    return new ExtF80(exp.PackToExtF80UI64(sign), sig);
                }

                if ((0x7FFE < exp) || ((exp == 0x7FFE) && sig + roundIncrement < sig)) {
                    Settings.Raise(ExceptionFlags.Overflow | ExceptionFlags.Inexact);
                    if (
                           roundingMode == RoundingMode.NearEven
                        || (roundingMode == RoundingMode.NearMaximumMagnitude)
                        || (roundingMode
                                == (sign ? RoundingMode.Minimum : RoundingMode.Maximum))
                    ) {
                        exp = 0x7FFF;
                        sig = 0x8000000000000000UL;
                    }
                    else {
                        exp = 0x7FFE;
                        sig = ~roundMask;
                    }
                    return new ExtF80(exp.PackToExtF80UI64(sign), sig);
                }
            }

            if (roundBits != 0) {
                Settings.Raise(ExceptionFlags.Inexact);
                if (roundingMode == RoundingMode.Odd) {
                    sig = (sig & ~roundMask) | (roundMask + 1);
                    return new ExtF80(exp.PackToExtF80UI64(sign), sig);
                }
            }

            sig = (ulong)(sig + roundIncrement);
            if (sig < roundIncrement) {
                ++exp;
                sig = 0x8000000000000000UL;
            }

            roundIncrement = roundMask + 1;
            if (roundingMode == RoundingMode.NearEven && (roundBits << 1 == roundIncrement)) {
                roundMask |= roundIncrement;
            }

            sig &= ~roundMask;
            return new ExtF80(exp.PackToExtF80UI64(sign), sig);
        }

    }
}
