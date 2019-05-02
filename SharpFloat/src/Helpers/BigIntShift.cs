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
        ///     shift the integer to left
        /// </summary>
        /// <param name="pResult"></param>
        /// <param name="shift"></param>
        public static void ShiftLeft(BigInt pResult, uint shift) {

            if (shift == 0)
                throw new System.ArgumentOutOfRangeException(nameof(shift));

            var shiftBlocks = shift / 32;
            var shiftBits = (int)(shift % 32);

            // process blocks high to low so that we can safely process in place
            var pInBlocks = 0;
            var inLength = pResult.Length;

            // check if the shift is block aligned
            if (shiftBits == 0) {
                pResult.Length += shiftBlocks;

                // copy blocks from high to low
                for (uint pInCur = inLength - 1, pOutCur = pInCur + shiftBlocks; pInCur >= pInBlocks && pInCur < inLength; --pInCur, --pOutCur) {
                    pResult[pOutCur] = pResult[pInCur];
                }

                // zero the remaining low blocks
                for (var i = 0U; i < shiftBlocks; ++i)
                    pResult[i] = 0;

            }
            // else we need to shift partial blocks
            else {
                var inBlockIdx = inLength - 1;
                var outBlockIdx = inLength + shiftBlocks;

                // set the length to hold the shifted blocks
                pResult.Length = outBlockIdx + 1;

                // output the initial blocks
                var lowBitsShift = (int)(32 - shiftBits);
                var highBits = 0U;
                var block = pResult[inBlockIdx];
                var lowBits = block >> lowBitsShift;
                while (inBlockIdx > 0) {
                    pResult[outBlockIdx] = highBits | lowBits;
                    highBits = block << shiftBits;

                    --inBlockIdx;
                    --outBlockIdx;

                    block = pResult[inBlockIdx];
                    lowBits = block >> lowBitsShift;
                }

                // output the final blocks
                if (outBlockIdx != shiftBlocks + 1)
                    throw new System.InvalidOperationException();

                pResult[outBlockIdx] = highBits | lowBits;
                pResult[outBlockIdx - 1] = block << shiftBits;

                // zero the remaining low blocks
                for (var i = 0U; i < shiftBlocks; ++i)
                    pResult[i] = 0;

                // check if the terminating block has no set bits
                if (pResult[pResult.Length - 1] == 0)
                    pResult.Length -= 1;
            }
        }

    }
}
