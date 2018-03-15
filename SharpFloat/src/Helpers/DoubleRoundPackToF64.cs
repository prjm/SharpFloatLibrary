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

namespace SharpFloat.Helpers {

    public static partial class DoubleHelpers {

        /// <summary>
        ///     round a value and convert it to 64-bit precision
        /// </summary>
        /// <param name="sign">sing</param>
        /// <param name="unsignedExponent">exponent</param>
        /// <param name="significant">significant</param>
        /// <param name="roundingMode">rounding mode to use</param>
        /// <returns></returns>
        public static double RoundPackToF64(bool sign, short unsignedExponent, ulong significant, RoundingMode roundingMode) {
            bool roundNearEven;
            ushort roundIncrement, roundBits;
            bool isTiny;
            ulong uiZ;

            roundNearEven = (roundingMode == RoundingMode.NearEven);
            roundIncrement = 0x200;
            if (!roundNearEven && (roundingMode != RoundingMode.NearMaximumMagnitude)) {
                roundIncrement = (ushort)((roundingMode == (sign ? RoundingMode.Minimum : RoundingMode.Maximum))
                        ? 0x3FF
                        : 0);
            }
            roundBits = (ushort)(significant & 0x3FF);

            if (0x7FD <= (ushort)unsignedExponent) {
                if (unsignedExponent < 0) {
                    isTiny = (Settings.DetectTininess == DetectTininess.BeforeRounding)
                            || (unsignedExponent < -1)
                            || (significant + roundIncrement < 0x8000000000000000);
                    significant = significant.ShiftRightJam64((uint)-unsignedExponent);
                    unsignedExponent = 0;
                    roundBits = (ushort)(significant & 0x3FF);
                    if (isTiny && roundBits != 0) {
                        Settings.Raise(ExceptionFlags.Underflow);
                    }
                }
                else if ((0x7FD < unsignedExponent) || (0x8000000000000000 <= significant + roundIncrement)) {
                    Settings.Raise(ExceptionFlags.Overflow);
                    Settings.Raise(ExceptionFlags.Inexact);
                    uiZ = PackToF64(sign, 0x7FF, 0) - (roundIncrement == 0 ? 1UL : 0UL);
                    return BitConverter.Int64BitsToDouble((long)uiZ);
                }
            }

            significant = (significant + roundIncrement) >> 10;
            if (roundBits != 0) {
                Settings.Raise(ExceptionFlags.Inexact);
                if (roundingMode == RoundingMode.Odd) {
                    significant |= 1;
                    uiZ = PackToF64(sign, unsignedExponent, significant);
                    return BitConverter.Int64BitsToDouble((long)uiZ);
                }
            }
            significant &= ~(ulong)(((roundBits ^ 0x200) == 0 ? 1 : 0) & (roundNearEven ? 1 : 0));
            if (significant == 0)
                unsignedExponent = 0;

            uiZ = PackToF64(sign, unsignedExponent, significant);
            return BitConverter.Int64BitsToDouble((long)uiZ);
        }
    }
}
