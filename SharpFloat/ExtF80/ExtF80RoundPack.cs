/*============================================================================

This C# source file is part of the SharpFloat IEEE Floating-Point Arithmetic
Package, Release 1.

This library ports the standard softfloat library , Release 3e, from John Hauser to C#.

Copyright 2011, 2012, 2013, 2014, 2015, 2017 The Regents of the University of
California.  All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met:

 1. Redistributions of source code must retain the above copyright notice,
    this list of conditions, and the following disclaimer.

 2. Redistributions in binary form must reproduce the above copyright notice,
    this list of conditions, and the following disclaimer in the documentation
    and/or other materials provided with the distribution.

 3. Neither the name of the University nor the names of its contributors may
    be used to endorse or promote products derived from this software without
    specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE REGENTS AND CONTRIBUTORS "AS IS", AND ANY
EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE, ARE
DISCLAIMED.  IN NO EVENT SHALL THE REGENTS OR CONTRIBUTORS BE LIABLE FOR ANY
DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

=============================================================================*/

using SharpFloat.Globals;
using SharpFloat.Helpers;

namespace SharpFloat.ExtF80 {
    public partial struct ExtF80 {


        private static ExtF80 RoundPackToExtF80(
            bool sign,
            int exp,
            ulong sig,
            ulong sigExtra,
            byte roundingPrecision
        ) {
            RoundingMode roundingMode;
            bool roundNearEven;
            ulong roundIncrement, roundMask, roundBits;
            bool isTiny, doIncrement;
            UInt64Extra sig64Extra;
            ExtF80 uZ;

            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            roundingMode = Settings.roundingMode.Value;
            roundNearEven = (roundingMode == RoundingMode.NearEven);
            if (roundingPrecision == 80)
                goto precision80;
            if (roundingPrecision == 64) {
                roundIncrement = 0x0000000000000400UL;
                roundMask = 0x00000000000007FFUL;
            }
            else if (roundingPrecision == 32) {
                roundIncrement = 0x0000008000000000UL;
                roundMask = 0x000000FFFFFFFFFFUL;
            }
            else {
                goto precision80;
            }
            sig |= (sigExtra != 0) ? 1UL : 0UL;
            if (!roundNearEven && (roundingMode != RoundingMode.NearMaximumMagnitude)) {
                roundIncrement =
                    (roundingMode
                         == (sign ? RoundingMode.Minimum : RoundingMode.Maximum))
                        ? roundMask
                        : 0;
            }
            roundBits = sig & roundMask;
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            if (0x7FFD <= (uint)(exp - 1)) {
                if (exp <= 0) {
                    /*----------------------------------------------------------------
                    *----------------------------------------------------------------*/
                    isTiny = (Settings.detectTininess.Value == DetectTininess.BeforeRounding)
                        || (exp < 0)
                        || (sig <= (ulong)(sig + roundIncrement));
                    sig = ShiftRightJam64(sig, (byte)(1 - exp));
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
                    if (roundNearEven && (roundBits << 1 == roundIncrement)) {
                        roundMask |= roundIncrement;
                    }
                    sig &= ~roundMask;
                    goto packReturn;
                }
                if (
                       (0x7FFE < exp)
                    || ((exp == 0x7FFE) && ((ulong)(sig + roundIncrement) < sig))
                ) {
                    Settings.Raise(ExceptionFlags.Overflow | ExceptionFlags.Inexact);
                    if (
                           roundNearEven
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
                    goto packReturn;
                }
            }
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            if (roundBits != 0) {
                Settings.Raise(ExceptionFlags.Inexact);
                if (roundingMode == RoundingMode.Odd) {
                    sig = (sig & ~roundMask) | (roundMask + 1);
                    goto packReturn;
                }
            }
            sig = (ulong)(sig + roundIncrement);
            if (sig < roundIncrement) {
                ++exp;
                sig = 0x8000000000000000UL;
            }
            roundIncrement = roundMask + 1;
            if (roundNearEven && (roundBits << 1 == roundIncrement)) {
                roundMask |= roundIncrement;
            }
            sig &= ~roundMask;
            goto packReturn;
        /*------------------------------------------------------------------------
        *------------------------------------------------------------------------*/
        precision80:
            doIncrement = (0x8000000000000000UL <= sigExtra);
            if (!roundNearEven && (roundingMode != RoundingMode.NearMaximumMagnitude)) {
                doIncrement = (roundingMode == (sign ? RoundingMode.Minimum : RoundingMode.Maximum)) && sigExtra != 0;
            }
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            if (0x7FFD <= (uint)(exp - 1)) {
                if (exp <= 0) {
                    /*----------------------------------------------------------------
                    *----------------------------------------------------------------*/
                    isTiny = (Settings.detectTininess.Value == DetectTininess.BeforeRounding)
                        || (exp < 0)
                        || !doIncrement
                        || (sig < 0xFFFFFFFFFFFFFFFFUL);
                    sig64Extra = ShiftRightJam64Extra(sig, sigExtra, 1 - exp);
                    exp = 0;
                    sig = sig64Extra.v;
                    sigExtra = sig64Extra.extra;
                    if (sigExtra != 0) {
                        if (isTiny)
                            Settings.Raise(ExceptionFlags.Underflow);
                        Settings.Raise(ExceptionFlags.Inexact);
                        if (roundingMode == RoundingMode.Odd) {
                            sig |= 1;
                            goto packReturn;
                        }
                    }
                    doIncrement = (0x8000000000000000UL <= sigExtra);
                    if (
                        !roundNearEven
                            && (roundingMode != RoundingMode.NearMaximumMagnitude)
                    ) {
                        doIncrement =
                            (((roundingMode
                                 == (sign ? RoundingMode.Minimum : RoundingMode.Maximum)) ? 1UL : 0UL)
                                & sigExtra) != 0;
                    }
                    if (doIncrement) {
                        ++sig;
                        sig &=
                            ~(ulong)(((sigExtra & 0x7FFFFFFFFFFFFFFFUL) == 0 ? 1 : 0UL) & (roundNearEven ? 1 : 0UL));
                        exp = (ushort)(((sig & 0x8000000000000000UL) != 0) ? 1 : 0);
                    }
                    goto packReturn;
                }
                if (
                       (0x7FFE < exp)
                    || ((exp == 0x7FFE) && (sig == 0xFFFFFFFFFFFFFFFFUL)
                            && doIncrement)
                ) {
                    /*----------------------------------------------------------------
                    *----------------------------------------------------------------*/
                    roundMask = 0;
                    Settings.Raise(ExceptionFlags.Overflow | ExceptionFlags.Inexact);
                    if (
                           roundNearEven
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
                    goto packReturn;
                }
            }
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            if (sigExtra != 0) {
                Settings.Raise(ExceptionFlags.Inexact);
                if (roundingMode == RoundingMode.Odd) {
                    sig |= 1;
                    goto packReturn;
                }
            }
            if (doIncrement) {
                ++sig;
                if (sig == 0) {
                    ++exp;
                    sig = 0x8000000000000000UL;
                }
                else {
                    sig &=
                        ~(ulong)
                             (((sigExtra & 0x7FFFFFFFFFFFFFFFUL) == 0 ? 1 : 0)
                                  & (roundNearEven ? 1 : 0));
                }
            }
        /*------------------------------------------------------------------------
        *------------------------------------------------------------------------*/
        packReturn:
            uZ.signExp = exp.PackToExtF80UI64(sign);
            uZ.signif = sig;
            return uZ;

        }



        private static ExtF80
         NormRoundPackToExtF80(
             bool sign,
             int exp,
             ulong sig,
             ulong sigExtra,
             byte roundingPrecision
         ) {
            byte shiftDist;
            UInt128 sig128;

            if (sig != 0) {
                exp -= 64;
                sig = sigExtra;
                sigExtra = 0;
            }
            shiftDist = sig.CountLeadingZeroes();
            exp -= shiftDist;
            if (shiftDist != 0) {
                sig128 = ShortShiftLeft128(sig, sigExtra, shiftDist);
                sig = sig128.v64;
                sigExtra = sig128.v0;
            }
            return
                RoundPackToExtF80(
                    sign, exp, sig, sigExtra, roundingPrecision);

        }

    }
}
