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
using System.Diagnostics;

namespace SharpFloat.Helpers {


    /// <summary>
    ///     helper structure: 16-bit exponent and 64-bit significant
    /// </summary>
    [DebuggerDisplay("exp = {exp}, sig = {sig}")]
    public readonly partial struct Exp16Sig64 : IEquatable<Exp16Sig64> {

        /// <summary>
        ///     exponent
        /// </summary>
        public readonly ushort exp;

        /// <summary>
        ///     significant
        /// </summary>
        public readonly ulong sig;

        /// <summary>
        ///     create a new helper structure
        /// </summary>
        /// <param name="aExp">exponent</param>
        /// <param name="aSig">significant</param>
        public Exp16Sig64(ushort aExp, ulong aSig) {
            exp = aExp;
            sig = aSig;
        }

        /// <summary>
        ///     compare to another value
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Exp16Sig64 other)
            => (exp == other.exp) && (sig == other.sig);

        /// <summary>
        ///     compare to another value
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
            => obj is Exp16Sig64 value && Equals(value);

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
            => (exp.GetHashCode() * 397) ^ sig.GetHashCode();

        /// <summary>
        ///     compare for equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(Exp16Sig64 left, Exp16Sig64 right)
            => left.Equals(right);

        /// <summary>
        ///     compare for inequality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Exp16Sig64 left, Exp16Sig64 right)
            => !(left == right);

    }
}
