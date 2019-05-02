/*  This library is a port of the standard softfloat library, Release 3e, from John Hauser to C#.
 *
 *    Copyright 2011, 2012, 2013, 2014, 2015, 2016, 2017, 2018 The Regents of the
 *    University of California.  All rights reserved.
 *
 *    Copyright 2018, 2019 Bastian Turcs. All rights reserved.
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

namespace SharpFloat.FloatingPoint {

    public partial struct ExtF80 : IEquatable<ExtF80> {

        /// <summary>
        ///     compare two floating point numbers
        /// </summary>
        /// <param name="l">left side</param>
        /// <param name="r">ride side</param>
        /// <returns><c>true</c> if the numbers are equal</returns>
        /// <remarks>comparisons with <c>NaN</c> return false</remarks>
        public static bool operator ==(in ExtF80 l, in ExtF80 r) {

            if (l.IsNaN || r.IsNaN) {
                if (l.IsSignalingNaN || r.IsSignalingNaN)
                    Settings.Raise(ExceptionFlags.Invalid);
                return false;
            }

            return //
                (l.signif == r.signif)
                && ((l.signExp == r.signExp) || (l.signif == 0 && 0 == ((l.signExp | r.signExp) & 0x7FFF)));
        }

        /// <summary>
        ///     compare this value to another object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is ExtF80 e)
                return e == this;
            return false;
        }

        /// <summary>
        ///     compare this value to another floating point value
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(in ExtF80 other)
            => this == other;

        /// <summary>
        ///     compare this value to another floating point value
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ExtF80 other)
            => this == other;

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns>computed hash code</returns>
        public override int GetHashCode()
            => unchecked((signExp.GetHashCode() * 397) ^ signif.GetHashCode());

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns>computed hash code</returns>
        public static int GetHashCode(in ExtF80 f)
            => unchecked((f.signExp.GetHashCode() * 397) ^ f.signif.GetHashCode());
    }

}
