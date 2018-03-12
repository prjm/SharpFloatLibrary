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

using System.Runtime.InteropServices;
using SharpFloat.Globals;

namespace SharpFloat.Helpers {

    public static partial class FloatHelpers {

        public static float RoundPackToF32(bool sign, short exp, uint sig, RoundingMode roundingMode) {
            uint uiZ;
            var roundNearEven = (roundingMode == RoundingMode.NearEven);
            short roundIncrement = 0x40;
            if (!roundNearEven && (roundingMode != RoundingMode.NearMaximumMagnitude)) {
                roundIncrement =
                    (roundingMode
                         == (sign ? RoundingMode.Minimum : RoundingMode.Maximum))
                        ? (short)0x7F
                        : (short)0;
            }
            var roundBits = (short)(sig & 0x7F);

            if (0xFD <= (uint)exp) {
                if (exp < 0) {

                    var isTiny =
                        (Settings.DetectTininess == DetectTininess.BeforeRounding)
                            || (exp < -1) || (sig + roundIncrement < 0x80000000);
                    sig = sig.ShiftRightJam32((ushort)-exp);
                    exp = 0;
                    roundBits = (short)(sig & 0x7F);
                    if (isTiny && roundBits != 0) {
                        Settings.Raise(ExceptionFlags.Underflow);
                    }
                }
                else if ((0xFD < exp) || (0x80000000 <= sig + roundIncrement)) {
                    Settings.Raise(ExceptionFlags.Overflow);
                    Settings.Raise(ExceptionFlags.Inexact);
                    uiZ = (uint)(PackToF32UI(sign, (short)0xFF, 0U) - (roundIncrement == 0 ? (short)1 : (short)0));
                    goto uiZ;
                }
            }
            /*------------------------------------------------------------------------
            *------------------------------------------------------------------------*/
            sig = (uint)(sig + roundIncrement) >> 7;
            if (roundBits != 0) {
                Settings.Raise(ExceptionFlags.Inexact);
                if (roundingMode == RoundingMode.Odd) {
                    sig |= 1;
                    goto packReturn;
                }
            }
            sig &= ~(uint)(((roundBits ^ 0x40) == 0 ? 1 : 0) & (roundNearEven ? 1 : 0));
            if (sig == 0)
                exp = 0;

            packReturn:
            uiZ = FloatHelpers.PackToF32UI(sign, exp, sig);
        uiZ:
            return FloatHelpers.Int32BitsToSingle(uiZ);
        }

    }
}
