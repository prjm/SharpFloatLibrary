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

        /// <summary>
        ///     calculate a multiplication: return the product of two 80-bit floating point values
        /// </summary>
        /// <param name="a">multiplicand</param>
        /// <param name="b">multiplier</param>
        /// <returns>product of the two number</returns>
        public static ExtF80 operator *(in ExtF80 a, in ExtF80 b) {
            var sigA = a.signif;
            var sigB = b.signif;
            var expA = a.UnsignedExponent;
            var expB = b.UnsignedExponent;
            var signZ = a.IsNegative ^ b.IsNegative;

            if (expA == MaxExponent || expB == MaxExponent) {
                if (((sigA & MaskAll63Bits) != 0 && expA == MaxExponent) || (0 != (b.signif & MaskAll63Bits)) && expB == MaxExponent) {
                    return PropagateNaN(a, b);
                }

                var isInvalid = false;
                if (expB != MaxExponent)
                    isInvalid = 0 == ((uint)expB | sigB);
                else
                    isInvalid = 0 == ((uint)expA | sigA);

                if (isInvalid) {
                    Settings.Raise(ExceptionFlags.Invalid);
                    return DefaultNaN;
                }

                return new ExtF80(MaxExponent.PackToExtF80UI64(signZ), MaskBit64);
            }

            if (expA == 0)
                expA = 1;

            if (0 == (sigA & MaskBit64)) {
                if (0 == sigA)
                    return signZ ? NegativeZero : Zero;
                var normExpSig = NormSubnormalSig(sigA);
                expA += normExpSig.exp;
                sigA = normExpSig.sig;
            }

            if (expB == 0)
                expB = 1;

            if (0 == (sigB & MaskBit64)) {
                if (0 == sigB)
                    return signZ ? NegativeZero : Zero;
                var normExpSig = NormSubnormalSig(sigB);
                expB += normExpSig.exp;
                sigB = normExpSig.sig;
            }

            var expZ = expA + expB - 0x3FFE;
            var sig128Z = UInt128.Mul64To128(sigA, sigB);

            if (sig128Z.v64 < MaskBit64) {
                --expZ;
                sig128Z = sig128Z + sig128Z;
            }
            return NormRoundPackToExtF80(signZ, expZ, sig128Z.v64, sig128Z.v0, Settings.ExtF80RoundingPrecision);
        }
    }
}
