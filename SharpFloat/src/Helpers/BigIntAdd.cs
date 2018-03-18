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
        ///         add two big integers
        /// </summary>
        /// <param name="result"></param>
        /// <param name="lhs"></param>
        /// <param name="rhs"></param>
        public static void Add(BigInt result, BigInt lhs, BigInt rhs) {

            BigInt large, small;
            if (lhs.length < rhs.length) {
                small = lhs;
                large = rhs;
            }
            else {
                small = rhs;
                large = lhs;
            }

            var largeLen = large.length;
            var smallLen = small.length;

            // The output will be at least as long as the largest input
            result.Zero = true;
            result.SetLength(largeLen);

            // Add each block and add carry the overflow to the next block
            var carry = 0UL;
            var LargeCur = 0U;
            var LargeEnd = largeLen;
            var SmallCur = 0U;
            var SmallEnd = smallLen;
            var ResultCur = 0U;
            while (SmallCur != SmallEnd) {
                var sum = carry + large[LargeCur] + small[SmallCur];
                carry = sum >> 32;
                result[ResultCur] = (uint)(sum & 0xFFFFFFFFU);
                ++LargeCur;
                ++SmallCur;
                ++ResultCur;
            }

            // Add the carry to any blocks that only exist in the large operand
            while (LargeCur != LargeEnd) {
                var sum = carry + large[LargeCur];
                carry = sum >> 32;
                result[ResultCur] = (uint)(sum & 0xFFFFFFFFU);
                ++LargeCur;
                ++ResultCur;
            }

            // If there's still a carry, append a new block
            if (carry != 0) {
                result.SetLength(result.Length + 1);
                result[ResultCur] = 1;
            }
        }
    }
}
