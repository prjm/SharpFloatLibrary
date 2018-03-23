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
        ///     division: compute a quotient of two 80-bit floating point numbers
        /// </summary>
        /// <param name="a">dividend (first operand)</param>
        /// <param name="b">divisor (second operand)</param>
        /// <returns>quotient</returns>
        public static ExtF80 operator /(in ExtF80 a, in ExtF80 b) {
            var isNegative = a.IsNegative ^ b.IsNegative;

            if (a.IsSpecialOperand || b.IsSpecialOperand) {
                return ZeroOrNaNInDivision(a, b, isNegative);
            }

            var sigA = a.signif;
            var sigB = b.signif;
            int expA = a.UnsignedExponent;
            int expB = b.UnsignedExponent;
            Exp32Sig64 normExpSig;

            if (expB == 0)
                expB = 1;

            if (0 == (sigB & MaskBit64)) {
                if (sigB == 0) {
                    if (sigA == 0) {
                        Settings.Raise(ExceptionFlags.Invalid);
                        return DefaultNaN;
                    }

                    Settings.Raise(ExceptionFlags.Infinite);

                    if (!isNegative)
                        return Infinity;
                    else
                        return NegativeInfinity;

                }
                normExpSig = NormalizeSubnormalSignificand(sigB);
                expB += normExpSig.exp;
                sigB = normExpSig.sig;
            }

            if (expA == 0)
                expA = 1;

            if (0 == (sigA & MaskBit64)) {
                if (sigA == 0)
                    return isNegative ? NegativeZero : Zero;
                normExpSig = NormalizeSubnormalSignificand(sigA);
                expA += normExpSig.exp;
                sigA = normExpSig.sig;
            }

            var expZ = expA - expB + 0x3FFF;
            UInt128 rem;
            UInt128 term;

            if (sigA < sigB) {
                --expZ;
                rem = UInt128.ShortShiftLeft(0, sigA, 32);
            }
            else {
                rem = UInt128.ShortShiftLeft(0, sigA, 31);
            }

            var recip32 = ((uint)(sigB >> 32)).ApproxRecip32_1();
            var sigZ = 0UL;
            var ix = 2;
            var q = 0U;

            for (; ; ) {
                var q64 = ((ulong)(uint)(rem.v64 >> 2)) * recip32;
                q = (uint)((q64 + 0x80000000) >> 32);
                --ix;
                if (ix < 0)
                    break;
                rem = UInt128.ShortShiftLeft(rem.v64, rem.v0, 29);
                term = UInt128.Mul64ByShifted32To128(sigB, q);
                rem = rem - term;
                if (0 != (rem.v64 & MaskBit64)) {
                    --q;
                    rem = rem + new UInt128(sigB >> 32, sigB << 32);
                }
                sigZ = (sigZ << 29) + q;
            }

            if (((q + 1) & 0x3FFFFF) < 2) {
                rem = UInt128.ShortShiftLeft(rem.v64, rem.v0, 29);
                term = UInt128.Mul64ByShifted32To128(sigB, q);
                rem = rem - term;
                term = UInt128.ShortShiftLeft(0, sigB, 32);
                if (0 != (rem.v64 & MaskBit64)) {
                    --q;
                    rem = rem + term;
                }
                else if (term <= rem) {
                    ++q;
                    rem = rem - term;
                }
                if (0 != (rem.v64 | rem.v0))
                    q |= 1;
            }

            sigZ = (sigZ << 6) + (q >> 23);
            return RoundPack(isNegative, expZ, sigZ, ((ulong)q) << 41, Settings.ExtF80RoundingPrecision);
        }

        /// <summary>
        ///     return constant on division
        /// </summary>
        /// <param name="a">dividend</param>
        /// <param name="b">divisor</param>
        /// <param name="isNegative"><c>true</c> if the result is negative</param>
        /// <returns>constant result</returns>
        private static ExtF80 ZeroOrNaNInDivision(in ExtF80 a, in ExtF80 b, bool isNegative) {
            if (a.IsSpecialOperand) {
                if ((a.signif & MaskAll63Bits) != 0)
                    return PropagateNaN(a, b);

                if (b.IsSpecialOperand) {
                    if ((b.signif & MaskAll63Bits) != 0)
                        return PropagateNaN(a, b);

                    Settings.Raise(ExceptionFlags.Invalid);
                    return DefaultNaN;
                }

                if (!isNegative)
                    return Infinity;
                else
                    return NegativeInfinity;

            }

            if ((b.signif & MaskAll63Bits) != 0)
                return PropagateNaN(a, b);

            if (isNegative)
                return NegativeZero;

            return Zero;
        }
    }
}
