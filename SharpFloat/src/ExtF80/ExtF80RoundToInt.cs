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

        /// <summary>
        ///     round a floating point number to an integer
        /// </summary>
        /// <param name="roundingMode"></param>
        /// <param name="exact"></param>
        /// <returns></returns>
        public ExtF80 RoundToInt(RoundingMode roundingMode, bool exact) {
            ulong lastBitMask, roundBitsMask;

            var signUI64 = (ushort)(signExp & 0.PackToExtF80(true));
            var exp = (int)UnsignedExponent;
            var sigA = signif;

            if (0 == (sigA & MaskBit64) && (exp != MaxExponent)) {
                if (0 == sigA) {
                    return new ExtF80(signUI64, 0);
                }
                var normExpSig = NormalizeSubnormalSignificand(sigA);
                exp += normExpSig.exp;
                sigA = normExpSig.sig;
            }

            if (0x403E <= exp) {
                if (exp == 0x7FFF) {
                    if (0 != (sigA & MaskAll63Bits)) {
                        return PropagateNaN(this, Zero);
                    }
                    return new ExtF80((ushort)(signUI64 | exp), MaskBit64);
                }
                else {
                    return new ExtF80((ushort)(signUI64 | exp), sigA);
                }
            }
            if (exp <= 0x3FFE) {
                if (exact)
                    Settings.Raise(ExceptionFlags.Inexact);

                switch (roundingMode) {
                    case RoundingMode.NearEven:
                        if (0 == (sigA & MaskAll63Bits))
                            break;
                        if (exp == 0x3FFE)
                            return new ExtF80((ushort)(signUI64 | 0x3FFF), MaskBit64);
                        break;
                    case RoundingMode.NearMaximumMagnitude:
                        if (exp == 0x3FFE)
                            return new ExtF80((ushort)(signUI64 | 0x3FFF), MaskBit64);
                        break;
                    case RoundingMode.Minimum:
                        if (0 != signUI64)
                            return new ExtF80((ushort)(signUI64 | 0x3FFF), MaskBit64);
                        break;
                    case RoundingMode.Maximum:
                        if (0 == signUI64)
                            return new ExtF80((ushort)(signUI64 | 0x3FFF), MaskBit64);
                        break;
                    case RoundingMode.Odd:
                        return new ExtF80((ushort)(signUI64 | 0x3FFF), MaskBit64);
                }
                return new ExtF80(signUI64, 0);
            }

            var uiZ64 = (ushort)(signUI64 | exp);
            lastBitMask = (ulong)1 << (0x403E - exp);
            roundBitsMask = lastBitMask - 1;
            var sigZ = sigA;
            if (roundingMode == RoundingMode.NearMaximumMagnitude) {
                sigZ += lastBitMask >> 1;
            }
            else if (roundingMode == RoundingMode.NearEven) {
                sigZ += lastBitMask >> 1;
                if (0 == (sigZ & roundBitsMask))
                    sigZ &= ~lastBitMask;
            }
            else if (
              roundingMode == (signUI64 != 0 ? RoundingMode.Minimum : RoundingMode.Maximum)
          ) {
                sigZ += roundBitsMask;
            }
            sigZ &= ~roundBitsMask;
            if (0 == sigZ) {
                ++uiZ64;
                sigZ = MaskBit64;
            }
            if (sigZ != sigA) {
                if (roundingMode == RoundingMode.Odd)
                    sigZ |= lastBitMask;
                if (exact)
                    Settings.Raise(ExceptionFlags.Inexact);
            }
            return new ExtF80(uiZ64, sigZ);
        }
    }
}
