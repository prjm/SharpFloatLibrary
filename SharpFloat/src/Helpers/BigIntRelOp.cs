/*
 *    This library is a port of the Dragon4 algorithm of Ryan Juckett.
 *    This is not the original software library -- it is an adapted version for C#.
 *
 *    Copyright 2018 Bastian Turcs. All rights reserved.
 *
 */
/******************************************************************************
 Copyright (c) 2014 Ryan Juckett
 http://www.ryanjuckett.com/

 This software is provided 'as-is', without any express or implied
 warranty. In no event will the authors be held liable for any damages
 arising from the use of this software.

 Permission is granted to anyone to use this software for any purpose,
 including commercial applications, and to alter it and redistribute it
 freely, subject to the following restrictions:

 1. The origin of this software must not be misrepresented; you must not
    claim that you wrote the original software. If you use this software
    in a product, an acknowledgment in the product documentation would be
    appreciated but is not required.

 2. Altered source versions must be plainly marked as such, and must not be
    misrepresented as being the original software.

 3. This notice may not be removed or altered from any source
    distribution.
******************************************************************************/

using System;

namespace SharpFloat.Helpers {

    public partial class BigInt : IComparable<BigInt>, IComparable, IEquatable<BigInt> {

        /// <summary>
        ///     compare two big integers
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static int Compare(BigInt left, BigInt right) {
            var lengthDiff = (int)left.length - (int)right.length;
            if (lengthDiff != 0)
                return lengthDiff;

            for (var i = (int)left.length - 1; i >= 0; --i) {
                if (left.blocks[i] == right.blocks[i])
                    continue;
                else if (left.blocks[i] > right.blocks[i])
                    return 1;
                else
                    return -1;
            }

            return 0;
        }

        /// <summary>
        ///     compare this integer to another
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(BigInt other)
            => Compare(this, other);

        /// <summary>
        ///     compare this integer to another
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int CompareTo(object obj)
            => obj is BigInt b ? CompareTo(b) : throw new ArgumentException("Invalid argument.", nameof(obj));

        /// <summary>
        ///     compare this value to another value
        /// </summary>
        /// <param name="obj">other value</param>
        /// <returns><c>true</c> if the other value is a <c>BigInt</c> with the same value</returns>
        public override bool Equals(object obj)
            => obj is BigInt bigInt && Equals(bigInt);

        /// <summary>
        ///     compare two big integer values
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(BigInt other)
            => CompareTo(other) == 0;

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 13;
            for (var index = 0; index < length; index++)
                result += 17 * blocks[index].GetHashCode();
            return result;
        }

        /// <summary>
        ///     compare for equality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator ==(BigInt left, BigInt right)
            => left.Equals(right);

        /// <summary>
        ///     compare for inequality
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(BigInt left, BigInt right)
            => !(left == right);

    }
}
