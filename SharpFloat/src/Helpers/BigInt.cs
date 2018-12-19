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
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace SharpFloat.Helpers {

    /// <summary>
    ///     helper class for bit integers
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public partial class BigInt {

        /// <summary>
        ///     create a new big integer
        /// </summary>
        public BigInt() {
            length = 0;
            blocks = Array.Empty<uint>();
        }

        /// <summary>
        ///     create a new big integer
        /// </summary>
        /// <param name="value">initial value</param>
        public BigInt(uint value) {
            AsUInt = value;
        }

        /// <summary>
        ///     create a new big integer
        /// </summary>
        /// <param name="value">value</param>
        public BigInt(ulong value) {
            AsULong = value;
        }

        /// <summary>
        ///     copy a big int value
        /// </summary>
        /// <param name="value"></param>
        public BigInt(BigInt value) {
            SetLength(value.length);
            if (length > 0)
                Array.Copy(value.blocks, blocks, value.blocks.Length);
        }

        /// <summary>
        ///     create a new big integer
        /// </summary>
        /// <param name="value">value</param>
        public BigInt(uint[] value) {
            SetLength((uint)value.Length);
            if (length > 0)
                Array.Copy(value, blocks, value.Length);
        }

        /// <summary>
        ///     length
        /// </summary>
        private uint length;

        /// <summary>
        ///     data blocks
        /// </summary>
        private uint[] blocks;

        /// <summary>
        ///     length
        /// </summary>
        public uint Length {
            get => length;
            set => SetLength(value);
        }

        /// <summary>
        ///     access a specific block
        /// </summary>
        /// <param name="index">block index</param>
        /// <returns>block value</returns>
        public uint this[uint index] {
            get => blocks[index];
            set => blocks[index] = value;
        }

        private void SetLength(uint newLength) {

            if (newLength == length)
                return;

            if (blocks != null && blocks.Length > length && newLength > length)
                Array.Clear(blocks, (int)length, Math.Min(blocks.Length, (int)newLength) - (int)length);

            length = newLength;

            if (blocks == null) {
                blocks = new uint[newLength];
                return;
            }

            if (blocks.Length >= newLength)
                return;

            var arrayLength = Math.Max(1, blocks.Length);
            while (arrayLength < newLength)
                arrayLength = 2 * arrayLength;

            var newBlocks = new uint[arrayLength];
            Array.Copy(blocks, newBlocks, blocks.Length);
            blocks = newBlocks;
        }

        /// <summary>
        ///     Zero value
        /// </summary>
        public bool Zero {

            get {
                if (length == 0)
                    return true;
                for (var i = 0; i < length; i++)
                    if (blocks[i] != 0)
                        return false;
                return true;
            }
        }

        /// <summary>
        ///     set this value to zero
        /// </summary>
        public void Clear() {
            SetLength(0);
        }

        /// <summary>
        ///     get the value as unsigned long
        /// </summary>
        public ulong AsULong {
            get {
                if (length == 0)
                    return 0;
                else if (length == 1)
                    return blocks[0];
                return ((ulong)blocks[1]) << 32 | blocks[0];
            }
            set {
                if (value == 0) {
                    SetLength(0);
                }
                else if (value <= 0xFFFFFFFF) {
                    SetLength(1);
                    blocks[0] = (uint)(value & 0xFFFFFFFF);

                }
                else {
                    SetLength(2);
                    blocks[0] = (uint)(value & 0xFFFFFFFF);
                    blocks[1] = (uint)((value >> 32) & 0xFFFFFFFF);
                }
            }
        }

        /// <summary>
        ///     get the value as unsigned integer
        /// </summary>
        public uint AsUInt {
            get {
                if (length == 0)
                    return 0;
                return blocks[0];
            }
            set {
                if (value == 0) {
                    SetLength(0);
                }
                else {
                    SetLength(1);
                    blocks[0] = value & 0xFFFFFFFF;
                }
            }
        }

        /// <summary>
        ///     assign the value of another big integer to this big integer
        /// </summary>
        /// <param name="another">another big integer</param>
        public void Assign(BigInt another) {
            SetLength(another.Length);
            for (var i = 0U; i < another.Length; i++)
                blocks[i] = another[i];
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string DebuggerDisplay {
            get {
                if (length == 0)
                    return "0x0";

                var sb = new StringBuilder();
                sb.Append("0x");
                for (var i = (int)(length - 1); i >= 0; i--)
                    sb.Append(string.Format(CultureInfo.InvariantCulture, "{0}", blocks[i].ToString("X8", CultureInfo.InvariantCulture)));
                return sb.ToString();
            }
        }
    }
}

