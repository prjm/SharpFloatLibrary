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

using System;
using SharpFloat.Globals;
using SharpFloat.Helpers;

namespace SharpFloat.FloatingPoint {

    public partial struct ExtF80 {

        public static explicit operator int(in ExtF80 a)
            => a.ToInt(RoundingMode.MinimumMagnitude, true);

        private const int i32_fromNegOverflow = (-0x7FFFFFFF - 1);
        private const int i32_fromPosOverflow = (-0x7FFFFFFF - 1);
        private const int i32_fromNaN = (-0x7FFFFFFF - 1);

        public int ToInt(RoundingMode roundingMode, bool exact) {
            var shiftDist = 0x4032 - UnsignedExponent;
            if (shiftDist <= 0)
                shiftDist = 1;
            var sig = signif.ShiftRightJam64((uint)shiftDist);
            return RoundToI32(IsNegative, sig, roundingMode, exact);
        }

        private int RoundToI32(bool sign, ulong sig, RoundingMode roundingMode, bool exact) {
            ushort roundIncrement, roundBits;
            uint sig32;
            int z;
            uint ui;
            int i;

            roundIncrement = 0x800;
            if ((roundingMode != RoundingMode.NearMaximumMagnitude) && (roundingMode != RoundingMode.NearEven)) {
                roundIncrement = 0;
                if (sign ? (roundingMode == RoundingMode.Minimum) || (roundingMode == RoundingMode.Odd) : (roundingMode == RoundingMode.Maximum)) {
                    roundIncrement = 0xFFF;
                }
            }

            roundBits = (ushort)(sig & 0xFFF);
            sig += roundIncrement;
            if (0 != (sig & 0xFFFFF00000000000))
                goto invalid;
            sig32 = (uint)(sig >> 12);
            if (
                (roundBits == 0x800) && (roundingMode == RoundingMode.NearEven)
            ) {
                sig32 &= ~(uint)1;
            }
            ui = (uint)(sign ? -sig32 : sig32);
            z = (int)ui;
            if (z != 0 && ((z < 0) ^ sign))
                goto invalid;
            if (0 != roundBits) {
                if (roundingMode == RoundingMode.Odd)
                    z |= 1;
                if (exact)
                    Settings.Raise(ExceptionFlags.Inexact);
            }
            return z;
        /*------------------------------------------------------------------------
        *------------------------------------------------------------------------*/
        invalid:
            Settings.Raise(ExceptionFlags.Invalid);
            return sign ? i32_fromNegOverflow : i32_fromPosOverflow;


        }
    }
}