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

namespace SharpFloat.ExtF80 {

    public partial struct ExtF80 {

        private static ExtF80 AddMagsExtF80(in ExtF80 a, in ExtF80 b, bool signZ) {
            var expA = a.signExp.ExpExtF80UI64();
            var expB = b.signExp.ExpExtF80UI64();
            var expDiff = expA - expB;

            if (expDiff == 0) {
                return AddExtF80WithSameExponents(a, b, expA, expB, signZ);
            }
            else if (expDiff < 0) {
                return AddExtF80SmallerAndLargerExponents(a, b, expA, expB, signZ, expDiff);
            }
            else {
                return AddExtF80LargerAndSmallerExponents(a, b, expA, expB, signZ, expDiff);
            }
        }

        private static ExtF80 AddExtF80WithSameExponents(in ExtF80 a, in ExtF80 b, ushort expA, ushort expB, bool signZ) {
            var sigA = a.signif;
            var sigB = b.signif;

            if (expA == 0x7FFF) {
                if (((sigA | sigB) & 0x7FFFFFFFFFFFFFFFUL) != 0) {
                    return UInt128.PropagateNaNExtF80UI(a, b);
                }
                return a;
            }

            var expZ = 0;
            var sigZ = sigA + sigB;
            var sigZExtra = 0UL;
            if (expA == 0) {
                var normExpSig = NormSubnormalSig(sigZ);
                expZ = normExpSig.exp + 1;
                sigZ = normExpSig.sig;
                return RoundPackToExtF80(signZ, expZ, sigZ, sigZExtra, Settings.ExtF80RoundingPrecision);
            }

            expZ = expA;
            var sig64Extra = UInt64Extra.ShortShiftRightJam64Extra(sigZ, sigZExtra, 1);
            sigZ = sig64Extra.v | 0x8000000000000000UL;
            sigZExtra = sig64Extra.extra;
            ++expZ;
            return RoundPackToExtF80(signZ, expZ, sigZ, sigZExtra, Settings.ExtF80RoundingPrecision);
        }

        private static ExtF80 AddExtF80SmallerAndLargerExponents(in ExtF80 a, in ExtF80 b, ushort expA, ushort expB, bool signZ, int expDiff) {
            var sigA = a.signif;
            var sigB = b.signif;
            ulong sigZ, sigZExtra = 0;
            UInt64Extra sig64Extra;

            if (expB == 0x7FFF) {
                if ((sigB & 0x7FFFFFFFFFFFFFFFUL) != 0) {
                    return UInt128.PropagateNaNExtF80UI(a, b);
                }
                return new ExtF80(0x7FFF.PackToExtF80UI64(signZ), b.signif);
            }

            var expZ = expB;
            if (expA == 0) {
                ++expDiff;
                sigZExtra = 0;
            }

            if (expA != 0 || expDiff != 0) {
                sig64Extra = UInt64Extra.ShiftRightJam64Extra(sigA, 0, -expDiff);
                sigA = sig64Extra.v;
                sigZExtra = sig64Extra.extra;
            }

            sigZ = sigA + sigB;
            if ((sigZ & 0x8000000000000000UL) != 0)
                return RoundPackToExtF80(signZ, expZ, sigZ, sigZExtra, Settings.ExtF80RoundingPrecision);

            sig64Extra = UInt64Extra.ShortShiftRightJam64Extra(sigZ, sigZExtra, 1);
            sigZ = sig64Extra.v | 0x8000000000000000UL;
            sigZExtra = sig64Extra.extra;
            ++expZ;
            return RoundPackToExtF80(signZ, expZ, sigZ, sigZExtra, Settings.ExtF80RoundingPrecision);
        }

        private static ExtF80 AddExtF80LargerAndSmallerExponents(in ExtF80 a, in ExtF80 b, ushort expA, ushort expB, bool signZ, int expDiff) {
            var sigA = a.signif;
            var sigB = b.signif;
            ulong sigZ, sigZExtra = 0;
            UInt64Extra sig64Extra;

            if (expA == 0x7FFF) {
                if ((sigA & 0x7FFFFFFFFFFFFFFFUL) != 0UL) {
                    return UInt128.PropagateNaNExtF80UI(a, b);
                }
                return a;
            }

            var expZ = expA;
            if (expB == 0) {
                --expDiff;
                sigZExtra = 0;
            }

            if (expB != 0 || expDiff != 0) {
                sig64Extra = UInt64Extra.ShiftRightJam64Extra(sigB, 0, expDiff);
                sigB = sig64Extra.v;
                sigZExtra = sig64Extra.extra;
            }

            sigZ = sigA + sigB;
            if ((sigZ & 0x8000000000000000UL) != 0)
                return RoundPackToExtF80(signZ, expZ, sigZ, sigZExtra, Settings.ExtF80RoundingPrecision);

            sig64Extra = UInt64Extra.ShortShiftRightJam64Extra(sigZ, sigZExtra, 1);
            sigZ = sig64Extra.v | 0x8000000000000000UL;
            sigZExtra = sig64Extra.extra;
            ++expZ;
            return RoundPackToExtF80(signZ, expZ, sigZ, sigZExtra, Settings.ExtF80RoundingPrecision);
        }
    }
}
