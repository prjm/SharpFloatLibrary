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

        const ushort defaultNaNExtF80UI64 = 0xFFFF;
        const ulong defaultNaNExtF80UI0 = 0xC000000000000000UL;


        private static ExtF80
         SubMagsExtF80(
             ushort uiA64,
             ulong uiA0,
             ushort uiB64,
             ulong uiB0,
             bool signZ
         ) {
            int expA;
            ulong sigA;
            int expB;
            ulong sigB;
            int expDiff;
            ushort uiZ64;
            ulong uiZ0;
            int expZ;
            ulong sigExtra;
            UInt128 sig128, uiZ;

            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            expA = uiA64.ExpExtF80UI64();
            sigA = uiA0;
            expB = uiB64.ExpExtF80UI64();
            sigB = uiB0;
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            expDiff = expA - expB;
            if (0 < expDiff)
                goto expABigger;
            if (expDiff < 0)
                goto expBBigger;
            if (expA == 0x7FFF) {
                if (0 != ((sigA | sigB) & 0x7FFFFFFFFFFFFFFFUL)) {
                    goto propagateNaN;
                }
                Settings.Raise(ExceptionFlags.Invalid);
                uiZ64 = defaultNaNExtF80UI64;
                uiZ0 = defaultNaNExtF80UI0;
                goto uiZ;
            }
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            expZ = expA;
            if (expZ == 0)
                expZ = 1;
            sigExtra = 0;
            if (sigB < sigA)
                goto aBigger;
            if (sigA < sigB)
                goto bBigger;
            uiZ64 = 0.PackToExtF80UI64(Settings.RoundingMode == RoundingMode.Minimum);
            uiZ0 = 0;
            goto uiZ;
        /*------------------------------------------------------------------------
        *------------------------------------------------------------------------*/
        expBBigger:
            if (expB == 0x7FFF) {
                if ((sigB & 0x7FFFFFFFFFFFFFFFUL) != 0)
                    goto propagateNaN;
                uiZ64 = 0x7FFF.PackToExtF80UI64(((signZ ? 1 : 0) ^ 1) != 0);
                uiZ0 = 0x8000000000000000UL;
                goto uiZ;
            }
            if (expA == 0) {
                ++expDiff;
                sigExtra = 0;
                if (expDiff == 0)
                    goto newlyAlignedBBigger;
            }
            sig128 = UInt128.ShiftRightJam128(sigA, 0, -expDiff);
            sigA = sig128.v64;
            sigExtra = sig128.v0;
        newlyAlignedBBigger:
            expZ = expB;
        bBigger:
            signZ = !signZ;
            sig128 = UInt128.Sub128(sigB, 0, sigA, sigExtra);
            goto normRoundPack;
        /*------------------------------------------------------------------------
        *------------------------------------------------------------------------*/
        expABigger:
            if (expA == 0x7FFF) {
                if ((sigA & 0x7FFFFFFFFFFFFFFFUL) != 0)
                    goto propagateNaN;
                uiZ64 = uiA64;
                uiZ0 = uiA0;
                goto uiZ;
            }
            if (expB == 0) {
                --expDiff;
                sigExtra = 0;
                if (expDiff == 0)
                    goto newlyAlignedABigger;
            }
            sig128 = UInt128.ShiftRightJam128(sigB, 0, expDiff);
            sigB = sig128.v64;
            sigExtra = sig128.v0;
        newlyAlignedABigger:
            expZ = expA;
        aBigger:
            sig128 = UInt128.Sub128(sigA, 0, sigB, sigExtra);
        /*------------------------------------------------------------------------
        *------------------------------------------------------------------------*/
        normRoundPack:
            return
                NormRoundPackToExtF80(
                    signZ, expZ, sig128.v64, sig128.v0, Settings.extF80_roundingPrecision.Value);
        /*------------------------------------------------------------------------
        *------------------------------------------------------------------------*/
        propagateNaN:
            uiZ = UInt128.PropagateNaNExtF80UI(uiA64, uiA0, uiB64, uiB0);
            uiZ64 = (ushort)uiZ.v64;
            uiZ0 = uiZ.v0;
        uiZ:
            return new ExtF80(uiZ64, uiZ0);

        }


    }
}
