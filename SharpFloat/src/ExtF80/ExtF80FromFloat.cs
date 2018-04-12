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
using SharpFloat.Helpers;

namespace SharpFloat.FloatingPoint {

    public partial struct ExtF80 {

        private static Exp16Sig32 NormSubnormalF32Sig(uint sig) {
            var shiftDist = sig.CountLeadingZeros() - 8;
            return new Exp16Sig32((ushort)(1 - shiftDist), sig << shiftDist);
        }

        /// <summary>
        ///     convert a single precision floating point value to an 80-bit precisision value
        /// </summary>
        /// <param name="a"></param>
        public static implicit operator ExtF80(float a) {
            var bits = FloatHelpers.SingleToInt32Bits(a);
            var sign = bits >> 31 != 0;
            var exp = (ushort)((bits >> 23) & 0xFF);
            var frac = bits & 0x007FFFFF;

            if (exp == 0xFF) {
                var e = ((ushort)0x7FFF).PackToExtF80UI64(sign);
                if (frac != 0) {
                    return new ExtF80(e, 0xC000000000000000 | (((ulong)bits << 41) >> 1));
                }
                return new ExtF80(e, 0x8000000000000000);
            }

            if (exp == 0) {
                if (frac == 0) {
                    if (sign)
                        return NegativeZero;
                    else
                        return Zero;
                }
                var normExpSig = NormSubnormalF32Sig(frac);
                exp = normExpSig.exp;
                frac = normExpSig.sig;
            }

            return new ExtF80(((ushort)(exp + 0x3F80)).PackToExtF80UI64(sign), (frac | 0x00800000UL) << 40);
        }

    }
}
