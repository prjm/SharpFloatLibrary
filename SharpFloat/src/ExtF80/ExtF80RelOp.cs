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

    public partial struct ExtF80 : IComparable, IComparable<ExtF80> {

        /// <summary>
        ///     check if one number is smaller than the other number
        /// </summary>
        /// <param name="a">first number</param>
        /// <param name="b">second number</param>
        /// <returns><c>true</c> if the first number is smaller than the second number</returns>
        public static bool operator <(in ExtF80 a, in ExtF80 b) {

            if (a.IsNaN || b.IsNaN) {
                if (a.IsSignalingNaN || b.IsSignalingNaN)
                    Settings.Raise(ExceptionFlags.Invalid);
                return false;
            }

            var signA = a.IsNegative;
            var signB = b.IsNegative;

            if (signA != signB)
                return signA && (0 != ((a.signExp | (ulong)b.signExp) & 0x7FFFUL | a.signif | b.signif));
            else
                return ((a.signExp != b.signExp) || (a.signif != b.signif)) && (signA ^ (new UInt128(a) < new UInt128(b)));
        }

        /// <summary>
        ///     check if one number is smaller or equal than the other number
        /// </summary>
        /// <param name="a">first number</param>
        /// <param name="b">second number</param>
        /// <returns><c>true</c> if the first number is smaller or equal than the second number</returns>
        public static bool operator <=(in ExtF80 a, in ExtF80 b) {

            if (a.IsNaN || b.IsNaN) {
                if (a.IsSignalingNaN || b.IsSignalingNaN)
                    Settings.Raise(ExceptionFlags.Invalid);
                return false;
            }

            var signA = a.IsNegative;
            var signB = b.IsNegative;

            if (signA != signB)
                return signA || (0 == ((a.signExp | (ulong)b.signExp) & 0x7FFFUL | a.signif | b.signif));
            else
                return ((a.signExp == b.signExp) && (a.signif == b.signif)) || (signA ^ (new UInt128(a) < new UInt128(b)));
        }

        /// <summary>
        ///     check if one number is larger than the other number
        /// </summary>
        /// <param name="a">first number</param>
        /// <param name="b">second number</param>
        /// <returns><c>true</c> if the first number is larger than the second number</returns>
        public static bool operator >(in ExtF80 a, in ExtF80 b)
            => b < a;

        /// <summary>
        ///     check if one number is larger or equal than the other number
        /// </summary>
        /// <param name="a">first number</param>
        /// <param name="b">second number</param>
        /// <returns><c>true</c> if the first number is larger or equal than the second number</returns>
        public static bool operator >=(in ExtF80 a, in ExtF80 b)
            => b <= a;

        /// <summary>
        ///     compare to another floating point value
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(ExtF80 other) {
            if (IsNaN) {
                if (other.IsNaN)
                    return 0;
                return -1;
            }

            if (other.IsNaN)
                return 1;

            if (this < other)
                return -1;

            if (other < this)
                return 1;

            return 0;
        }

        /// <summary>
        ///     compare to another object
        /// </summary>
        /// <param name="obj">object to compare with</param>
        /// <returns>comparison result</returns>
        /// <remarks>the other object has to be an ExtF80 value</remarks>
        public int CompareTo(object obj) {
            if (obj == null)
                return 1;
            else if (obj is ExtF80 value)
                return CompareTo(value);

            throw new ArgumentException("Invalid comparison", nameof(obj));
        }
    }
}
