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

namespace SharpFloat.Helpers {

    public partial class BigInt {

        /// <summary>
        ///     multiply two big integer values
        /// </summary>
        /// <param name="result"></param>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static void Multiply(BigInt result, BigInt lhs, BigInt rhs) {

            BigInt large, small;
            if (lhs.length < rhs.length) {
                small = lhs;
                large = rhs;
            }
            else {
                small = rhs;
                large = lhs;
            }

            var maxResultLen = large.length + small.length;
            result.Zero = true;
            result.SetLength(maxResultLen);

            var pLargeBeg = 0U;
            var pLargeEnd = large.length;
            var pResultStart = 0U;
            for (uint pSmallCur = 0U, pSmallEnd = small.length; pSmallCur != pSmallEnd; ++pSmallCur, ++pResultStart) {

                var multiplier = small[pSmallCur];
                if (multiplier != 0) {
                    var pLargeCur = pLargeBeg;
                    var pResultCur = pResultStart;
                    var carry = 0UL;

                    do {
                        var product = result[pResultCur] + large[pLargeCur] * (ulong)multiplier + carry;
                        carry = product >> 32;

                        result[pResultCur] = (uint)(product & 0xFFFFFFFF);
                        ++pLargeCur;
                        ++pResultCur;
                    } while (pLargeCur != pLargeEnd);
                    result[pResultCur] = (uint)(carry & 0xFFFFFFFF);
                }
            }

            if (maxResultLen > 0 && result[maxResultLen - 1] == 0)
                result.Length = maxResultLen - 1;
            else
                result.Length = maxResultLen;
        }

        /// <summary>
        ///     multiply a big integer value with an unsigned inter
        /// </summary>
        /// <param name="result">result value</param>
        /// <param name="lhs">left hand side of the product</param>
        /// <param name="rhs">right hand side of the product</param>
        public static void Multiply(BigInt result, BigInt lhs, uint rhs) {
            var carry = 0U;
            var pResultCur = 0U;
            var pLhsCur = 0U;
            var pLhsEnd = lhs.Length;
            result.Length = lhs.length;

            for (; pLhsCur != pLhsEnd; ++pLhsCur, ++pResultCur) {
                var product = (ulong)(lhs[pLhsCur]) * rhs + carry;
                result[pResultCur] = (uint)(product & 0xFFFFFFFF);
                carry = (uint)(product >> 32);
            }

            if (carry != 0) {
                result.Length = result.Length + 1;
                result[pResultCur] = (uint)carry;
            }
        }

        /// <summary>
        ///     multiply a big number by 2
        /// </summary>
        /// <param name="result">result value</param>
        /// <param name="number">number to multiply</param>
        public static void Multiply2(BigInt result, BigInt number) {
            var carry = 0U;
            var pResultCur = 0U;
            var pLhsCur = 0U;
            var pLhsEnd = number.Length;
            result.Length = number.length;

            for (; pLhsCur != pLhsEnd; ++pLhsCur, ++pResultCur) {
                var cur = number[pLhsCur];
                result[pResultCur] = (cur << 1) | carry;
                carry = cur >> 31;
            }

            if (carry != 0) {
                result.Length = result.Length + 1;
                result[pResultCur] = carry;
            }
        }

        /// <summary>
        ///     multiply a big number by two
        /// </summary>
        /// <param name="number">number to be multiplied</param>
        public static void Multiply2(BigInt number) {
            var carry = 0U;
            var cur = 0U;
            var end = number.Length;

            for (; cur != end; ++cur) {
                var v = number[cur];
                number[cur] = (v << 1) | carry;
                carry = v >> 31;
            }

            if (carry != 0) {
                number.Length = number.Length + 1;
                number[cur] = carry;
            }
        }

        /// <summary>
        ///     multiply a big number by 10
        /// </summary>
        /// <param name="number">number to multiply</param>
        public static void Multiply10(BigInt number) {
            var carry = 0UL;
            var cur = 0U;
            var end = number.Length;

            for (; cur != end; ++cur) {
                var v = number[cur] * 10UL + carry;
                number[cur] = (uint)(v & 0xFFFF_FFFF);
                carry = v >> 32;
            }

            if (carry != 0) {
                number.Length = number.Length + 1;
                number[cur] = (uint)carry;
            }
        }


    }
}
