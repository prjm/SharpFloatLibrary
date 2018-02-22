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

        public static ExtF80 operator /(ExtF80 a, ExtF80 b) {
            ushort uiA64;
            ulong uiA0;
            bool signA;
            int expA;
            ulong sigA;
            ushort uiB64;
            ulong uiB0;
            bool signB;
            int expB;
            ulong sigB;
            bool signZ;
            Exp32Sig64 normExpSig;
            int expZ;
            UInt128 rem;
            uint recip32;
            ulong sigZ;
            int ix;
            ulong q64;
            uint q;
            UInt128 term;
            ulong sigZExtra;
            ushort uiZ64;
            ulong uiZ0;

            uiA64 = a.signExp;
            uiA0 = a.signif;
            signA = uiA64.SignExtF80UI64();
            expA = uiA64.ExpExtF80UI64();
            sigA = uiA0;
            uiB64 = b.signExp;
            uiB0 = b.signif;
            signB = uiB64.SignExtF80UI64();
            expB = uiB64.ExpExtF80UI64();
            sigB = uiB0;
            signZ = signA ^ signB;

            if (expA == 0x7FFF) {
                if ((sigA & 0x7FFFFFFFFFFFFFFFUL) != 0)
                    goto propagateNaN;
                if (expB == 0x7FFF) {
                    if ((sigB & 0x7FFFFFFFFFFFFFFFUL) != 0)
                        goto propagateNaN;
                    goto invalid;
                }
                goto infinity;
            }
            if (expB == 0x7FFF) {
                if ((sigB & 0x7FFFFFFFFFFFFFFFUL) != 0)
                    goto propagateNaN;
                goto zero;
            }
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            if (expB == 0)
                expB = 1;
            if (0 == (sigB & 0x8000000000000000UL)) {
                if (sigB == 0) {
                    if (sigA == 0)
                        goto invalid;
                    Settings.Raise(ExceptionFlags.Infinite);
                    goto infinity;
                }
                normExpSig = NormSubnormalSig(sigB);
                expB += normExpSig.exp;
                sigB = normExpSig.sig;
            }
            if (expA == 0)
                expA = 1;
            if (0 == (sigA & 0x8000000000000000UL)) {
                if (sigA == 0)
                    goto zero;
                normExpSig = NormSubnormalSig(sigA);
                expA += normExpSig.exp;
                sigA = normExpSig.sig;
            }
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            expZ = expA - expB + 0x3FFF;
            if (sigA < sigB) {
                --expZ;
                rem = UInt128.ShortShiftLeft128(0, sigA, 32);
            }
            else {
                rem = UInt128.ShortShiftLeft128(0, sigA, 31);
            }
            recip32 = ((uint)(sigB >> 32)).ApproxRecip32_1();
            sigZ = 0;
            ix = 2;
            for (; ; ) {
                q64 = (uint)(rem.v64 >> 2) * recip32;
                q = (uint)((q64 + 0x80000000) >> 32);
                --ix;
                if (ix < 0)
                    break;
                rem = UInt128.ShortShiftLeft128(rem.v64, rem.v0, 29);
                term = UInt128.Mul64ByShifted32To128(sigB, q);
                rem = UInt128.Sub128(rem.v64, rem.v0, term.v64, term.v0);
                if (0 != (rem.v64 & 0x8000000000000000UL)) {
                    --q;
                    rem = rem + new UInt128(sigB >> 32, sigB << 32);
                }
                sigZ = (sigZ << 29) + q;
            }
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            if (((q + 1) & 0x3FFFFF) < 2) {
                rem = UInt128.ShortShiftLeft128(rem.v64, rem.v0, 29);
                term = UInt128.Mul64ByShifted32To128(sigB, q);
                rem = UInt128.Sub128(rem.v64, rem.v0, term.v64, term.v0);
                term = UInt128.ShortShiftLeft128(0, sigB, 32);
                if (0 != (rem.v64 & 0x8000000000000000UL)) {
                    --q;
                    rem = rem + term;
                }
                else if (term <= rem) {
                    ++q;
                    rem = UInt128.Sub128(rem.v64, rem.v0, term.v64, term.v0);
                }
                if (0 != (rem.v64 | rem.v0))
                    q |= 1;
            }
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            sigZ = (sigZ << 6) + (q >> 23);
            sigZExtra = ((ulong)q) << 41;
            return RoundPackToExtF80(signZ, expZ, sigZ, sigZExtra, Settings.ExtF80RoundingPrecision);
        /*------------------------------------------------------------------------
        *------------------------------------------------------------------------*/
        propagateNaN:
            return UInt128.PropagateNaNExtF80UI(a, b);
        /*------------------------------------------------------------------------
        *------------------------------------------------------------------------*/
        invalid:
            Settings.Raise(ExceptionFlags.Invalid);
            uiZ64 = DefaultNaNExponent;
            uiZ0 = DefaultNaNSignificant;
            goto uiZ;
        /*------------------------------------------------------------------------
        *------------------------------------------------------------------------*/
        infinity:
            uiZ64 = 0x7FFF.PackToExtF80UI64(signZ);
            uiZ0 = 0x8000000000000000;
            goto uiZ;
        /*------------------------------------------------------------------------
        *------------------------------------------------------------------------*/
        zero:
            uiZ64 = 0.PackToExtF80UI64(signZ);
            uiZ0 = 0;
        uiZ:
            return new ExtF80(uiZ64, uiZ0);
        }

    }
}
