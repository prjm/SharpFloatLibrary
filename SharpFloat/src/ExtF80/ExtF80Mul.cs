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

        public static ExtF80 operator *(ExtF80 a, ExtF80 b) {
            bool signA;
            bool signB;
            int expA;
            int expB;
            bool signZ;
            ulong magBits;
            Exp32Sig64 normExpSig;
            int expZ;
            UInt128 sig128Z, uiZ;
            ushort uiZ64;
            ulong uiZ0;
            ulong sigA;
            ulong sigB;

            sigA = a.signif;
            sigB = b.signif;
            signA = a.IsNegative;
            signB = b.IsNegative;
            expA = a.UnsignedExponent;
            expB = b.UnsignedExponent;
            signZ = signA ^ signB;

            if (expA == MaxExponent) {
                if (((sigA & MaskAll63Bits) != 0) || ((expB == MaxExponent) && (0 != (b.signif & MaskAll63Bits)))) {
                    goto propagateNaN;
                }
                magBits = (ulong)expB | sigB;
                goto infArg;
            }

            if (expB == 0x7FFF) {
                if ((b.signif & MaskAll63Bits) != 0)
                    goto propagateNaN;

                magBits = a.UnsignedExponent | a.signif;
                goto infArg;
            }

            if (expA == 0)
                expA = 1;

            if (0 == (sigA & MaskBit64)) {
                if (0 == sigA)
                    goto zero;
                normExpSig = NormSubnormalSig(sigA);
                expA += normExpSig.exp;
                sigA = normExpSig.sig;
            }

            if (expB == 0)
                expB = 1;

            if (0 == (sigB & MaskBit64)) {
                if (0 == sigB)
                    goto zero;
                normExpSig = NormSubnormalSig(sigB);
                expB += normExpSig.exp;
                sigB = normExpSig.sig;
            }

            expZ = expA + expB - 0x3FFE;
            sig128Z = UInt128.Mul64To128(sigA, sigB);

            if (sig128Z.v64 < MaskBit64) {
                --expZ;
                sig128Z = sig128Z + sig128Z;
            }
            return NormRoundPackToExtF80(signZ, expZ, sig128Z.v64, sig128Z.v0, Settings.ExtF80RoundingPrecision);

        propagateNaN:
            return UInt128.PropagateNaNExtF80UI(a, b);

        infArg:
            if (0 == magBits) {
                Settings.Raise(ExceptionFlags.Invalid);
                return DefaultNaN;
            }
            else {
                uiZ64 = MaxExponent.PackToExtF80UI64(signZ);
                uiZ0 = MaskBit64;
            }
            goto uiZ;

        zero:
            uiZ64 = 0.PackToExtF80UI64(signZ);
            uiZ0 = 0;

        uiZ:
            return new ExtF80(uiZ64, uiZ0);
        }
    }
}
