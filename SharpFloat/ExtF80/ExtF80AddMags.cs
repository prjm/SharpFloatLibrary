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

        private static ExtF80 AddMagsExtF80(
             ushort uiA64,
             ulong uiA0,
             ushort uiB64,
             ulong uiB0,
             bool signZ) {

            ushort expA;
            ulong sigA;
            ushort expB;
            ulong sigB;
            int expDiff;
            ushort uiZ64;
            ulong uiZ0, sigZ, sigZExtra;
            Exp32Sig64 normExpSig;
            ushort expZ;
            UInt64Extra sig64Extra;
            UInt128 uiZ;
            ExtF80 uZ;

            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            expA = uiA64.ExpExtF80UI64();
            sigA = uiA0;
            expB = uiB64.ExpExtF80UI64();
            sigB = uiB0;
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            expDiff = expA - expB;
            if (expDiff == 0) {
                if (expA == 0x7FFF) {
                    if (((sigA | sigB) & 0x7FFFFFFFFFFFFFFFUL) != 0) {
                        goto propagateNaN;
                    }
                    uiZ64 = uiA64;
                    uiZ0 = uiA0;
                    goto uiZ;
                }
                sigZ = sigA + sigB;
                sigZExtra = 0;
                if (expA == 0) {
                    normExpSig = NormSubnormalSig(sigZ);
                    expZ = (ushort)(normExpSig.exp + 1);
                    sigZ = normExpSig.sig;
                    goto roundAndPack;
                }
                expZ = expA;
                goto shiftRight1;
            }
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            if (expDiff < 0) {
                if (expB == 0x7FFF) {
                    if ((sigB & 0x7FFFFFFFFFFFFFFFUL) != 0)
                        goto propagateNaN;
                    uiZ64 = 0x7FFF.PackToExtF80UI64(signZ);
                    uiZ0 = uiB0;
                    goto uiZ;
                }
                expZ = expB;
                if (expA == 0) {
                    ++expDiff;
                    sigZExtra = 0;
                    if (expDiff == 0)
                        goto newlyAligned;
                }
                sig64Extra = UInt64Extra.ShiftRightJam64Extra(sigA, 0, -expDiff);
                sigA = sig64Extra.v;
                sigZExtra = sig64Extra.extra;
            }
            else {
                if (expA == 0x7FFF) {
                    if ((sigA & 0x7FFFFFFFFFFFFFFFUL) != 0UL)
                        goto propagateNaN;
                    uiZ64 = uiA64;
                    uiZ0 = uiA0;
                    goto uiZ;
                }
                expZ = expA;
                if (expB == 0) {
                    --expDiff;
                    sigZExtra = 0;
                    if (expDiff == 0)
                        goto newlyAligned;
                }
                sig64Extra = UInt64Extra.ShiftRightJam64Extra(sigB, 0, expDiff);
                sigB = sig64Extra.v;
                sigZExtra = sig64Extra.extra;
            }
        newlyAligned:
            sigZ = sigA + sigB;
            if ((sigZ & 0x8000000000000000UL) != 0)
                goto roundAndPack;
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            shiftRight1:
            sig64Extra = UInt64Extra.ShortShiftRightJam64Extra(sigZ, sigZExtra, 1);
            sigZ = sig64Extra.v | 0x8000000000000000UL;
            sigZExtra = sig64Extra.extra;
            ++expZ;
        roundAndPack:
            return
                RoundPackToExtF80(
                    signZ, expZ, sigZ, sigZExtra, Settings.extF80_roundingPrecision.Value);
        /*------------------------------------------------------------------------
        *------------------------------------------------------------------------*/
        propagateNaN:
            uiZ = UInt128.PropagateNaNExtF80UI(uiA64, uiA0, uiB64, uiB0);
            uiZ64 = (ushort)uiZ.v64;
            uiZ0 = uiZ.v0;
        uiZ:
            uZ.signExp = uiZ64;
            uZ.signif = uiZ0;
            return uZ;
        }
    }
}
