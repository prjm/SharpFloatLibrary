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

namespace SharpFloat.Helpers {

    public readonly partial struct UInt128 : IComparable<UInt128> {

        /// <summary>
        ///     compare this value to another 128bit integer
        /// </summary>
        /// <param name="other">other value</param>
        /// <returns><c>-1</c> if this one is smaller, <c>1</c> if the other value is smaller, <c>0</c> if the values are equal</returns>
        public int CompareTo(UInt128 other)
            => this <= other ? this >= other ? 0 : -1 : 1;

        /// <summary>
        ///     test if the first operand is less than the second operand
        /// </summary>
        /// <param name="a">first operand</param>
        /// <param name="b">second operand</param>
        /// <returns><c>true</c> if a is less than b</returns>
        public static bool operator <(UInt128 a, UInt128 b)
            => (a.v64 < b.v64) || ((a.v64 == b.v64) && (a.v0 < b.v0));

        /// <summary>
        ///     test if the first operand is less than or equal the second operand
        /// </summary>
        /// <param name="a">first operand</param>
        /// <param name="b">second operand</param>
        /// <returns><c>true</c> if a is less than or equals b</returns>
        public static bool operator <=(UInt128 a, UInt128 b)
            => (a.v64 < b.v64) || ((a.v64 == b.v64) && (a.v0 <= b.v0));

        /// <summary>
        ///     test if the first operand is greater than or equal the second operand
        /// </summary>
        /// <param name="a">first operand</param>
        /// <param name="b">second operand</param>
        /// <returns><c>true</c> if a is greater than or equals b</returns>
        public static bool operator >=(UInt128 a, UInt128 b)
            => (a.v64 > b.v64) || ((a.v64 == b.v64) && (a.v0 >= b.v0));

        /// <summary>
        ///     test if the first operand is greater than the second operand
        /// </summary>
        /// <param name="a">first operand</param>
        /// <param name="b">second operand</param>
        /// <returns><c>true</c> if a is greater than b</returns>
        public static bool operator >(UInt128 a, UInt128 b)
            => (a.v64 > b.v64) || ((a.v64 == b.v64) && (a.v0 > b.v0));

    }
}
