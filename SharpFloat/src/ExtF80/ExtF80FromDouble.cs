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

        private static Exp16Sig64 NormSubnormalF64Sig(ulong sig) {
            var shiftDist = sig.CountLeadingZeros() - 11;
            return new Exp16Sig64(
                (ushort)(1 - shiftDist),
                sig << shiftDist
            );
        }

        /// <summary>
        ///     convert a double value to an ExtF80 value
        /// </summary>
        /// <param name="value">double value to convert</param>
        public static implicit operator ExtF80(double value) {
            var bits = BitConverter.DoubleToInt64Bits(value);
            var sign = bits >> 63 != 0;
            var exp = (ushort)((bits >> 52) & 0x7FF);
            var frac = (ulong)bits & 0x000FFFFFFFFFFFFFUL;

            if (exp == 0x7FF) {
                var e = ((ushort)0x7FFF).PackToExtF80UI64(sign);
                if (frac != 0) {
                    return new ExtF80(e, 0xC000000000000000UL | (((ulong)bits << 12) >> 1));
                }
                return new ExtF80(e, 0x8000000000000000UL);
            }

            if (exp == 0) {
                if (frac == 0) {
                    if (sign)
                        return NegativeZero;
                    else
                        return Zero;
                }
                var normExpSig = NormSubnormalF64Sig(frac);
                exp = normExpSig.exp;
                frac = normExpSig.sig;
            }

            return new ExtF80(((ushort)exp + 0x3C00).PackToExtF80(sign), (frac | 0x0010000000000000) << 11);
        }
    }
}