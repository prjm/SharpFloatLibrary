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

        public static ExtF80 Remainder(in ExtF80 a, in ExtF80 b) {
            ushort uiA64;
            ulong uiA0;
            bool signA;
            int expA;
            ulong sigA;
            ushort uiB64;
            ulong uiB0;
            int expB;
            ulong sigB;
            Exp32Sig64 normExpSig;
            int expDiff;
            UInt128 rem, shiftedSigB;
            uint q, recip32;
            ulong q64;
            UInt128 term, altRem, meanRem;
            bool signRem;
            ExtF80 uiZ;
            ushort uiZ64;
            ulong uiZ0;

            uiA64 = a.signExp;
            uiA0 = a.signif;
            signA = a.IsNegative;
            expA = a.UnsignedExponent;
            sigA = uiA0;
            uiB64 = b.signExp;
            uiB0 = b.signif;
            expB = b.UnsignedExponent;
            sigB = uiB0;

            if (expA == MaxExponent) {
                if ((0 != (sigA & MaskAll63Bits)) || ((expB == MaxExponent) && (0 != (sigB & MaxExponent)))) {
                    goto propagateNaN;
                }
                goto invalid;
            }

            if (expB == MaxExponent) {
                if (0 != (sigB & MaskAll63Bits))
                    goto propagateNaN;
                /*--------------------------------------------------------------------
                | Argument b is an infinity.  Doubling `expB' is an easy way to ensure
                | that `expDiff' later is less than -1, which will result in returning
                | a canonicalized version of argument a.
                *--------------------------------------------------------------------*/
                expB += expB;
            }

            if (0 == expB)
                expB = 1;

            if (0 == (sigB & MaskBit64)) {
                if (sigB == 0)
                    goto invalid;
                normExpSig = NormSubnormalSig(sigB);
                expB += normExpSig.exp;
                sigB = normExpSig.sig;
            }

            if (expA == 0)
                expA = 1;

            if (0 == (sigA & MaskBit64)) {
                if (0 == sigA) {
                    expA = 0;
                    goto copyA;
                }
                normExpSig = NormSubnormalSig(sigA);
                expA += normExpSig.exp;
                sigA = normExpSig.sig;
            }

            expDiff = expA - expB;
            if (expDiff < -1)
                goto copyA;

            rem = UInt128.ShortShiftLeft128(0, sigA, 32);
            shiftedSigB = UInt128.ShortShiftLeft128(0, sigB, 32);

            if (expDiff < 1) {
                if (expDiff != 0) {
                    --expB;
                    shiftedSigB = UInt128.ShortShiftLeft128(0, sigB, 33);
                    q = 0;
                }
                else {
                    q = (sigB <= sigA) ? 1U : 0U;
                    if (q != 0) {
                        rem = rem - shiftedSigB;
                    }
                }
            }
            else {
                recip32 = ((uint)(sigB >> 32)).ApproxRecip32_1();
                expDiff -= 30;
                for (; ; ) {
                    q64 = (ulong)(uint)(rem.v64 >> 2) * recip32;
                    if (expDiff < 0)
                        break;
                    q = (uint)((q64 + 0x80000000) >> 32);
                    rem = UInt128.ShortShiftLeft128(rem.v64, rem.v0, 29);
                    term = UInt128.Mul64ByShifted32To128(sigB, q);
                    rem = rem - term;
                    if (0 != (rem.v64 & MaskBit64)) {
                        rem = rem + shiftedSigB;
                    }
                    expDiff -= 29;
                }
                /*--------------------------------------------------------------------
                | (`expDiff' cannot be less than -29 here.)
                *--------------------------------------------------------------------*/
                q = (uint)(q64 >> 32) >> (~expDiff & 31);
                rem = UInt128.ShortShiftLeft128(rem.v64, rem.v0, (byte)(expDiff + 30));
                term = UInt128.Mul64ByShifted32To128(sigB, q);
                rem = rem - term;
                if (0 != (rem.v64 & MaskBit64)) {
                    altRem = rem + shiftedSigB;
                    goto selectRem;
                }
            }

            do {
                altRem = rem;
                ++q;
                rem = rem - shiftedSigB;
            } while (0 == (rem.v64 & MaskBit64));

        selectRem:
            meanRem = rem + altRem;
            if (0 != (meanRem.v64 & MaskBit64)
                    || ((0 == (meanRem.v64 | meanRem.v0)) && (0 != (q & 1)))
            ) {
                rem = altRem;
            }
            signRem = signA;
            if (0 != (rem.v64 & MaskBit64)) {
                signRem = !signRem;
                rem = new UInt128(0, 0) - rem;
            }

            return
                NormRoundPackToExtF80(
                    signRem, (rem.v64 | rem.v0) != 0 ? expB + 32 : 0, rem.v64, rem.v0, 80);

        propagateNaN:
            uiZ = PropagateNaN(a, b);
            uiZ64 = uiZ.signExp;
            uiZ0 = uiZ.signif;
            goto uiZ;

        invalid:
            Settings.Raise(ExceptionFlags.Invalid);
            uiZ64 = DefaultNaNExponent;
            uiZ0 = DefaultNaNSignificant;
            goto uiZ;

        copyA:
            if (expA < 1) {
                sigA >>= 1 - expA;
                expA = 0;
            }
            uiZ64 = expA.PackToExtF80UI64(signA);
            uiZ0 = sigA;
        uiZ:
            return new ExtF80(uiZ64, uiZ0);

        }

    }
}
