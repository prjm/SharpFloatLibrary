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

using SharpFloat.Helpers;

namespace SharpFloat.FloatingPoint {

    public partial struct ExtF80 {

        /// <summary>
        ///     convert an unsigned long to an extended floating point value
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ExtF80(ulong value) {
            if (value == 0)
                return Zero;

            var shiftDist = value.CountLeadingZeros();
            var exponent = 0x403E - shiftDist;
            var mantissa = value << shiftDist;
            return new ExtF80((ushort)exponent, mantissa);
        }

        /// <summary>
        ///     convert an signed long to an extended floating point value
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator ExtF80(long value) {
            if (value == 0)
                return Zero;

            var sign = value < 0;
            var absoluteValue = value < 0 ? (ulong)-value : (ulong)value;
            var shiftDist = absoluteValue.CountLeadingZeros();
            var exponent = (0x403E - shiftDist).PackToExtF80(sign);
            var mantissa = absoluteValue << shiftDist;
            return new ExtF80(exponent, mantissa);
        }


    }
}
