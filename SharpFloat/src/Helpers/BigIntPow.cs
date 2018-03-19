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
        ///     lookup table: smaller powers of 10
        /// </summary>
        private static readonly uint[] SmallPowerOf10 = {
            1,          // 10 ^ 0
            10,         // 10 ^ 1
            100,        // 10 ^ 2
            1000,       // 10 ^ 3
            10000,      // 10 ^ 4
            100000,     // 10 ^ 5
            1000000,    // 10 ^ 6
            10000000,   // 10 ^ 7
        };

        /// <summary>
        ///     lookup table: larger powers of ten
        /// </summary>
        private static readonly BigInt[] BigPowerOf10 = {

            // 10 ^ 8
            new BigInt(100000000),

            // 10 ^ 16
            new BigInt(new uint[]  { 0x6fc10000, 0x002386f2 } ),

            // 10 ^ 32
            new BigInt(new uint[] { 0x00000000, 0x85acef81, 0x2d6d415b, 0x000004ee, } ),

            // 10 ^ 64
            new BigInt(new uint[] { 0x00000000, 0x00000000, 0xbf6a1f01, 0x6e38ed64, 0xdaa797ed, 0xe93ff9f4, 0x00184f03, } ),

            // 10 ^ 128
            new BigInt(new uint[] { 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x2e953e01, 0x03df9909, 0x0f1538fd,
                0x2374e42f, 0xd3cff5ec, 0xc404dc08, 0xbccdb0da, 0xa6337f19, 0xe91f2603, 0x0000024e, } ),

            // 10 ^ 256
            new BigInt(new uint[] { 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000, 0x00000000,
                0x00000000, 0x982e7c01, 0xbed3875b, 0xd8d99f72, 0x12152f87, 0x6bde50c6, 0xcf4a6e70,
                0xd595d80f, 0x26b2716e, 0xadc666b0, 0x1d153624, 0x3c42d35a, 0x63ff540e, 0xcc5573c0,
                0x65f9ef17, 0x55bc28f2, 0x80dcc7f7, 0xf46eeddc, 0x5fdcefce, 0x000553f7, } )

        };

        /// <summary>
        ///     compute the power of ten
        /// </summary>
        /// <param name="result">result value</param>
        /// <param name="exponent">exponent</param>
        public static void Pow10(BigInt result, uint exponent) {

            if (exponent >= 512)
                throw new System.ArgumentOutOfRangeException();

            // create two temporary values to reduce large integer copy operations
            var pCurTemp = new BigInt();
            var pNextTemp = new BigInt();

            // initialize the result by looking up a 32-bit power of 10 corresponding to the first 3 bits
            var smallExponent = exponent & 0x7U;
            pCurTemp.AsUInt = SmallPowerOf10[smallExponent];

            // remove the low bits that we used for the 32-bit lookup table
            exponent >>= 3;
            uint tableIdx = 0;

            // while there are remaining bits in the exponent to be processed
            while (exponent != 0) {
                // if the current bit is set, multiply it with the corresponding power of 10
                if (0 != (exponent & 1)) {

                    // multiply into the next temporary
                    Multiply(pNextTemp, pCurTemp, BigPowerOf10[tableIdx]);

                    // swap to the next temporary
                    var pSwap = pCurTemp;
                    pCurTemp = pNextTemp;
                    pNextTemp = pSwap;
                }

                // advance to the next bit
                ++tableIdx;
                exponent >>= 1;
            }

            // output the result
            result.Assign(pCurTemp);
        }

        /// <summary>
        ///     multiply by a power of 10
        /// </summary>
        /// <param name="result">result vector</param>
        /// <param name="input">input</param>
        /// <param name="exponent">exponent</param>
        public static void MultiplyPow10(BigInt result, BigInt input, uint exponent) {

            if (exponent >= 512)
                throw new System.ArgumentOutOfRangeException();

            // create two temporary values to reduce large integer copy operations
            var pCurTemp = new BigInt();
            var pNextTemp = new BigInt();

            // initialize the result by looking up a 32-bit power of 10 corresponding to the first 3 bits
            var smallExponent = exponent & 0x7U;
            if (smallExponent != 0) {
                Multiply(pCurTemp, input, SmallPowerOf10[smallExponent]);
            }
            else {
                pCurTemp = input;
            }

            // remove the low bits that we used for the 32-bit lookup table
            exponent >>= 3;
            var tableIdx = 0U;

            // while there are remaining bits in the exponent to be processed
            while (exponent != 0) {
                // if the current bit is set, multiply it with the corresponding power of 10
                if (0 != (exponent & 1)) {
                    // multiply into the next temporary
                    Multiply(pNextTemp, pCurTemp, BigPowerOf10[tableIdx]);

                    // swap to the next temporary
                    var pSwap = pCurTemp;
                    pCurTemp = pNextTemp;
                    pNextTemp = pSwap;
                }

                // advance to the next bit
                ++tableIdx;
                exponent >>= 1;
            }

            // output the result
            result.Assign(pCurTemp);
        }

        /// <summary>
        ///     compute a power of two
        /// </summary>
        /// <param name="result">result number</param>
        /// <param name="exponent">exponent value</param>
        public static void Pow2(BigInt result, uint exponent) {
            var blockIdx = exponent / 32;
            result.Zero = true;
            result.Length = 1 + blockIdx;

            var bitIdx = (int)(exponent % 32);
            result[blockIdx] |= (1U << bitIdx);
        }


    }
}
