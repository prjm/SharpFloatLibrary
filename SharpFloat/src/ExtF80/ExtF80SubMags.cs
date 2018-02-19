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

namespace SharpFloat.ExtF80 {

    public partial struct ExtF80 {

        private static ExtF80 SubMagsExtF80(ushort uiA64, ulong uiA0, ushort uiB64, ulong uiB0, bool signZ) {
            var expA = uiA64.ExpExtF80UI64();
            var sigA = uiA0;
            var expB = uiB64.ExpExtF80UI64();
            var sigB = uiB0;
            var expDiff = expA - expB;

            if (0 < expDiff) {
                return SubLargerAndSmallerExponent(uiA64, uiA0, uiB64, uiB0, expA, expB, signZ, expDiff);
            }
            else if (expDiff < 0) {
                return SubSmallAndLargerExponent(uiA64, uiA0, uiB64, uiB0, expA, expB, signZ, expDiff);
            }
            else {
                return SumSameExponents(uiA64, uiA0, uiB64, uiB0, expA, expB, signZ, expDiff);
            }
        }

        private static ExtF80 SumSameExponents(ushort uiA64, ulong uiA0, ushort uiB64, ulong uiB0, ushort expA, ushort expB, bool signZ, int expDiff) {
            var sigA = uiA0;
            var sigB = uiB0;
            var sigExtra = 0UL;
            UInt128 sig128;

            if (expA == 0x7FFF) {
                if (0 != ((sigA | sigB) & 0x7FFFFFFFFFFFFFFFUL)) {
                    var uiZ = UInt128.PropagateNaNExtF80UI(uiA64, uiA0, uiB64, uiB0);
                    return new ExtF80((ushort)uiZ.v64, uiZ.v0);
                }
                Settings.Raise(ExceptionFlags.Invalid);
                return new ExtF80(DefaultNaNExponent, DefaultNaNSignificant);
            }

            var expZ = expA;
            if (expZ == 0)
                expZ = 1;
            sigExtra = 0UL;

            if (sigB < sigA) {
                sig128 = UInt128.Sub128(sigA, 0, sigB, sigExtra);
                return NormRoundPackToExtF80(signZ, expZ, sig128.v64, sig128.v0, Settings.ExtF80RoundingPrecision);
            }

            if (sigA < sigB) {
                signZ = !signZ;
                sig128 = UInt128.Sub128(sigB, 0, sigA, sigExtra);
                return NormRoundPackToExtF80(signZ, expZ, sig128.v64, sig128.v0, Settings.ExtF80RoundingPrecision);
            }

            return new ExtF80(0.PackToExtF80UI64(Settings.RoundingMode == RoundingMode.Minimum), 0);
        }

        private static ExtF80 SubSmallAndLargerExponent(ushort uiA64, ulong uiA0, ushort uiB64, ulong uiB0, ushort expA, ushort expB, bool signZ, int expDiff) {
            var sigA = uiA0;
            var sigB = uiB0;
            var sigExtra = 0UL;
            UInt128 sig128;

            if (expB == 0x7FFF) {
                if ((sigB & 0x7FFFFFFFFFFFFFFFUL) != 0) {
                    var uiZ = UInt128.PropagateNaNExtF80UI(uiA64, uiA0, uiB64, uiB0);
                    return new ExtF80((ushort)uiZ.v64, uiZ.v0);
                }
                return new ExtF80(0x7FFF.PackToExtF80UI64(((signZ ? 1 : 0) ^ 1) != 0), 0x8000000000000000UL);
            }

            if (expA == 0) {
                ++expDiff;
                sigExtra = 0;
            }

            if (expA != 0 || expDiff != 0) {
                sig128 = UInt128.ShiftRightJam128(sigA, 0, -expDiff);
                sigA = sig128.v64;
                sigExtra = sig128.v0;
            }

            var expZ = expB;
            signZ = !signZ;
            sig128 = UInt128.Sub128(sigB, 0, sigA, sigExtra);
            return NormRoundPackToExtF80(signZ, expZ, sig128.v64, sig128.v0, Settings.ExtF80RoundingPrecision);
        }

        private static ExtF80 SubLargerAndSmallerExponent(ushort uiA64, ulong uiA0, ushort uiB64, ulong uiB0, ushort expA, ushort expB, bool signZ, int expDiff) {
            var sigA = uiA0;
            var sigB = uiB0;
            var sigExtra = 0UL;
            UInt128 sig128;

            if (expA == 0x7FFF) {
                if ((sigA & 0x7FFFFFFFFFFFFFFFUL) != 0) {
                    var uiZ = UInt128.PropagateNaNExtF80UI(uiA64, uiA0, uiB64, uiB0);
                    return new ExtF80((ushort)uiZ.v64, uiZ.v0);
                }
                return new ExtF80(uiA64, uiA0);
            }

            if (expB == 0) {
                --expDiff;
                sigExtra = 0;
            }

            if (expB != 0 || expDiff != 0) {
                sig128 = UInt128.ShiftRightJam128(sigB, 0, expDiff);
                sigB = sig128.v64;
                sigExtra = sig128.v0;
            }

            var expZ = expA;
            sig128 = UInt128.Sub128(sigA, 0, sigB, sigExtra);
            return NormRoundPackToExtF80(signZ, expZ, sig128.v64, sig128.v0, Settings.ExtF80RoundingPrecision);
        }
    }
}
