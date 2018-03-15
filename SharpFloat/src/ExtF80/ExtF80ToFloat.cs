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

        /// <summary>
        ///     convert this number to a float value
        /// </summary>
        /// <returns></returns>
        public static explicit operator float(in ExtF80 a)
            => a.ToFloat(RoundingMode.MinimumMagnitude);

        /// <summary>
        ///     convert this number to a float value
        /// </summary>
        /// <returns></returns>
        public float ToFloat(RoundingMode roundingMode) {
            var uiZ = 0U;
            var exp = (short)UnsignedExponent;

            if (exp == MaxExponent) {
                if ((signif & MaskAll63Bits) != 0) {
                    var sign = signExp >> 15;
                    var v64 = signif << 1;
                    uiZ = (((uint)sign) << 31) | (0x7FC00000U) | ((uint)(v64 >> 41));
                }
                else {
                    uiZ = FloatHelpers.PackToF32UI(IsNegative, 0xFF, 0);
                }
                return FloatHelpers.Int32BitsToSingle(uiZ);
            }

            var sig32 = signif.ShortShiftRightJam64(33);
            if (0 == (((ushort)exp) | sig32)) {
                uiZ = FloatHelpers.PackToF32UI(IsNegative, 0, 0);
                return FloatHelpers.Int32BitsToSingle(uiZ);
            }

            exp -= 0x3F81;
            return FloatHelpers.RoundPackToF32(IsNegative, exp, (uint)sig32, roundingMode);
        }

    }
}
