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

        private static ExtF80 AddMagsExtF80(in ExtF80 a, in ExtF80 b, bool signZ) {

            var exponentDifference = a.UnsignedExponent - b.UnsignedExponent;

            if (exponentDifference == 0)
                return AddExtF80WithSameExponents(a, b, signZ);

            if (exponentDifference < 0)
                return AddExtF80SmallerAndLargerExponents(a, b, signZ, exponentDifference);

            return AddExtF80LargerAndSmallerExponents(a, b, signZ, exponentDifference);
        }


        private static ExtF80 AddExtF80WithSameExponents(in ExtF80 a, in ExtF80 b, bool signZ) {

            if (a.UnsignedExponent == MaxExponent) {
                if (((a.signif | b.signif) & MaskAll63Bits) != 0)
                    return PropagateNaN(a, b);
                else
                    return a;
            }

            var sigZ = a.signif + b.signif;

            if (a.UnsignedExponent == 0) {
                var normExpSig = NormSubnormalSig(sigZ);
                return RoundPackToExtF80(signZ, normExpSig.exp + 1, normExpSig.sig, 0, Settings.ExtF80RoundingPrecision);
            }

            var sig64Extra = UInt64Extra.ShortShiftRightJam64Extra(sigZ, 0, 1);
            sigZ = sig64Extra.v | MaskBit64;
            var sigZExtra = sig64Extra.extra;
            return RoundPackToExtF80(signZ, a.UnsignedExponent + 1, sigZ, sigZExtra, Settings.ExtF80RoundingPrecision);
        }

        private static ExtF80 AddExtF80SmallerAndLargerExponents(in ExtF80 a, in ExtF80 b, bool signZ, int expDiff) {
            if (b.UnsignedExponent == MaxExponent) {
                if ((b.signif & MaskAll63Bits) != 0)
                    return PropagateNaN(a, b);
                else
                    return new ExtF80(MaxExponent.PackToExtF80UI64(signZ), b.signif);
            }

            if (a.UnsignedExponent == 0)
                ++expDiff;

            UInt64Extra sig64Extra;
            ulong sigZExtra;
            ulong sigZ;

            if (a.UnsignedExponent != 0 || expDiff != 0) {
                sig64Extra = UInt64Extra.ShiftRightJam64Extra(a.signif, 0, -expDiff);
                sigZExtra = sig64Extra.extra;
                sigZ = sig64Extra.v + b.signif;
            }
            else {
                sigZExtra = 0UL;
                sigZ = a.signif + b.signif;
            }

            if ((sigZ & MaskBit64) != 0)
                return RoundPackToExtF80(signZ, b.UnsignedExponent, sigZ, sigZExtra, Settings.ExtF80RoundingPrecision);

            sig64Extra = UInt64Extra.ShortShiftRightJam64Extra(sigZ, sigZExtra, 1);
            sigZ = sig64Extra.v | MaskBit64;
            return RoundPackToExtF80(signZ, b.UnsignedExponent + 1, sigZ, sig64Extra.extra, Settings.ExtF80RoundingPrecision);
        }

        private static ExtF80 AddExtF80LargerAndSmallerExponents(in ExtF80 a, in ExtF80 b, bool signZ, int expDiff) {

            if (a.UnsignedExponent == MaxExponent) {
                if ((a.signif & MaskAll63Bits) != 0UL)
                    return PropagateNaN(a, b);
                else
                    return a;
            }

            var expZ = a.UnsignedExponent;
            if (b.UnsignedExponent == 0)
                --expDiff;

            ulong sigZ;
            ulong sigZExtra;
            UInt64Extra sig64Extra;

            if (b.UnsignedExponent != 0 || expDiff != 0) {
                sig64Extra = UInt64Extra.ShiftRightJam64Extra(b.signif, 0, expDiff);
                sigZ = a.signif + sig64Extra.v;
                sigZExtra = sig64Extra.extra;
            }
            else {
                sigZ = a.signif + b.signif;
                sigZExtra = 0UL;
            }

            if ((sigZ & MaskBit64) != 0)
                return RoundPackToExtF80(signZ, a.UnsignedExponent, sigZ, sigZExtra, Settings.ExtF80RoundingPrecision);

            sig64Extra = UInt64Extra.ShortShiftRightJam64Extra(sigZ, sigZExtra, 1);
            sigZ = sig64Extra.v | MaskBit64;
            return RoundPackToExtF80(signZ, a.UnsignedExponent + 1, sigZ, sig64Extra.extra, Settings.ExtF80RoundingPrecision);
        }
    }
}
