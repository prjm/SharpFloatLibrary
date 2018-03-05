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
        ///     compute the square root from this 80-bit number
        /// </summary>
        /// <param name="a">number to compute root from</param>
        /// <returns>square root</returns>
        public ExtF80 Sqrt() {
            ushort uiA64;
            ulong uiA0;
            bool signA;
            int expA;
            ulong sigA;
            UInt128 uiZ;
            ushort uiZ64;
            ulong uiZ0;
            Exp32Sig64 normExpSig;
            int expZ;
            uint sig32A, recipSqrt32, sig32Z;
            UInt128 rem;
            ulong q, x64, sigZ;
            UInt128 y, term;
            ulong sigZExtra;

            uiA64 = signExp;
            uiA0 = signif;
            signA = IsNegative;
            expA = UnsignedExponent;
            sigA = uiA0;

            if (expA == MaxExponent) {
                if (0 != (sigA & MaskAll63Bits)) {
                    return PropagateNaN(this, Zero);
                }
                if (!signA)
                    return this;
                goto invalid;
            }

            if (signA) {
                if (0 == sigA)
                    goto zero;
                goto invalid;
            }
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            if (0 == expA)
                expA = 1;
            if (0 == (sigA & MaskBit64)) {
                if (0 == sigA)
                    goto zero;
                normExpSig = NormSubnormalSig(sigA);
                expA += normExpSig.exp;
                sigA = normExpSig.sig;
            }

            expZ = ((expA - 0x3FFF) >> 1) + 0x3FFF;
            expA &= 1;
            sig32A = (uint)(sigA >> 32);
            recipSqrt32 = sig32A.ApproxRecipSqrt32_1((uint)expA);
            sig32Z = (uint)((ulong)sig32A * recipSqrt32) >> 32;
            if (0 != expA) {
                sig32Z >>= 1;
                rem = UInt128.ShortShiftLeft128(0, sigA, 61);
            }
            else {
                rem = UInt128.ShortShiftLeft128(0, sigA, 62);
            }
            rem = new UInt128(rem.v64 - (ulong)sig32Z * sig32Z, rem.v0);

            q = ((uint)(rem.v64 >> 2) * (ulong)recipSqrt32) >> 32;
            x64 = (ulong)sig32Z << 32;
            sigZ = x64 + (q << 3);
            y = UInt128.ShortShiftLeft128(rem.v64, rem.v0, 29);

            for (; ; ) {
                term = UInt128.Mul64ByShifted32To128(x64 + sigZ, (uint)q);
                rem = y - term;
                if (0 == (rem.v64 & MaskBit64))
                    break;
                --q;
                sigZ -= 1 << 3;
            }

            q = (((rem.v64 >> 2) * recipSqrt32) >> 32) + 2;
            x64 = sigZ;
            sigZ = (sigZ << 1) + (q >> 25);
            sigZExtra = q << 39;

            if ((q & 0xFFFFFF) <= 2) {
                q &= ~(ulong)0xFFFF;
                sigZExtra = q << 39;
                term = UInt128.Mul64ByShifted32To128(x64 + (q >> 27), (uint)q);
                x64 = (uint)(q << 5) * (ulong)(uint)q;
                term = term + new UInt128(0, x64);
                rem = UInt128.ShortShiftLeft128(rem.v64, rem.v0, 28);
                rem = rem - term;
                if (0 != (rem.v64 & MaskBit64)) {
                    if (0 == sigZExtra)
                        --sigZ;
                    --sigZExtra;
                }
                else {
                    if (0 != (rem.v64 | rem.v0))
                        sigZExtra |= 1;
                }
            }
            return RoundPackToExtF80(false, expZ, sigZ, sigZExtra, Settings.ExtF80RoundingPrecision);

        invalid:
            Settings.Raise(ExceptionFlags.Invalid);
            return DefaultNaN;

        zero:
            return signA ? NegativeZero : Zero;

        }

    }
}
