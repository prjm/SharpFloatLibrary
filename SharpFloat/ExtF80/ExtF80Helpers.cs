﻿/*  This library is a port of the standard softfloat library, Release 3e, from John Hauser to C#.
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


        private static UInt64Extra ShiftRightJam64Extra(ulong a, ulong extra, int dist) {
            UInt64Extra z;
            if (dist < 64) {
                z.v = a >> dist;
                z.extra = a << (-dist & 63);
            }
            else {
                z.v = 0;
                z.extra = (dist == 64) ? a : ((a != 0) ? 1UL : 0UL);
            }
            z.extra |= (extra != 0) ? 1UL : 0UL;
            return z;
        }

        private static UInt64Extra ShortShiftRightJam64Extra(ulong a, ulong extra, byte dist) {
            UInt64Extra z;
            z.v = a >> dist;
            z.extra = a << (-dist & 63) | (extra != 0 ? 1UL : 0UL);
            return z;
        }

        private static UInt128 PropagateNaNExtF80UI(ushort uiA64, ulong uiA0, ushort uiB64, ulong uiB0) {
            bool isSigNaNA, isSigNaNB;
            ulong uiNonsigA0, uiNonsigB0;
            ushort uiMagA64, uiMagB64;
            UInt128 uiZ;

            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            isSigNaNA = IsSigNaNExtF80UI(uiA64, uiA0);
            isSigNaNB = IsSigNaNExtF80UI(uiB64, uiB0);
            /*------------------------------------------------------------------------
            | Make NaNs non-signaling.
            *------------------------------------------------------------------------*/
            uiNonsigA0 = uiA0 | 0xC000000000000000UL;
            uiNonsigB0 = uiB0 | 0xC000000000000000UL;
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            if (isSigNaNA | isSigNaNB) {
                Settings.Raise(ExceptionFlags.Invalid);
                if (isSigNaNA) {
                    if (isSigNaNB)
                        goto returnLargerMag;
                    if (IsNaNExtF80UI(uiB64, uiB0))
                        goto returnB;
                    goto returnA;
                }
                else {
                    if (IsNaNExtF80UI(uiA64, uiA0))
                        goto returnA;
                    goto returnB;
                }
            }
        returnLargerMag:
            uiMagA64 = (ushort)(uiA64 & 0x7FFF);
            uiMagB64 = (ushort)(uiB64 & 0x7FFF);
            if (uiMagA64 < uiMagB64)
                goto returnB;
            if (uiMagB64 < uiMagA64)
                goto returnA;
            if (uiA0 < uiB0)
                goto returnB;
            if (uiB0 < uiA0)
                goto returnA;
            if (uiA64 < uiB64)
                goto returnA;
            returnB:
            uiZ.v64 = uiB64;
            uiZ.v0 = uiNonsigB0;
            return uiZ;
        returnA:
            uiZ.v64 = uiA64;
            uiZ.v0 = uiNonsigA0;
            return uiZ;

        }


    }
}
