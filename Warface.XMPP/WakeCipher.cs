using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace Warface.XMPP
{
    public class WakeCipher
    {
        readonly uint[]              _key;
        readonly IReadOnlyList<uint> _r;
        readonly IReadOnlyList<uint> _t;

        static readonly uint[] Tt =
        {
            0x726a8f3b,
            0xe69a3b5c,
            0xd3c71fe5,
            0xab3c73d2,
            0x4d3a8eb3,
            0x0396d6e8,
            0x3d4c2f7a,
            0x9ee27cf3
        };

        public WakeCipher(IEnumerable<uint> key)
        {
            _key     = key.ToArray();
            (_r, _t) = SetArrays();
        }

        public void Encrypt(byte[] input)
        {
            CryptOperation(input, false);
        }


        public void Decrypt(byte[] input)
        {
            CryptOperation(input, true);
        }


        void CryptOperation(byte[] input, bool decrypt)
        {
            if (input.Length == 0)
                return;

            uint r3 = _r[0];
            uint r4 = _r[1];
            uint r5 = _r[2];
            uint r6 = _r[3];

            var tmp     = 0u;
            var counter = 0;
            for (var i = 0; i < input.Length; i++)
            {
                if (decrypt)
                {
                    ReplaceByte(ref tmp, input[i], counter);
                    input[i] ^= GetByte(r6, counter);
                }
                else
                {
                    input[i] ^= GetByte(r6, counter);
                    ReplaceByte(ref tmp, input[i], counter);
                }
                counter++;

                if (counter == 4)
                {
                    counter = 0;

                    r3 = M(r3, tmp);
                    r4 = M(r4, r3);
                    r5 = M(r5, r4);
                    r6 = M(r6, r5);
                }
            }
        }

        (uint[] r, uint[] t) SetArrays()
        {
            var t = new uint[257];
            var r = new uint[4];

            uint x, p;

            if (_key.Length != 4)
                throw new ArgumentException("Invalid key length");

            for (p = 0; p < 4; p++)
            {
                t[p] = _key[p];
                r[p] = _key[p];
            }

            for (p = 4; p < 256; p++)
            {
                x    = t[p - 4] + t[p - 1];
                t[p] = x >> 3 ^ Tt[x & 7];
            }

            for (p = 0; p < 23; p++)
                t[p] += t[p + 89];

            x = t[33];
            uint z = t[59] | 0x01000001;
            z &= 0xff7fffff;

            for (p = 0; p < 256; p++)
            {
                x    = (x    & 0xff7fffff) + z;
                t[p] = (t[p] & 0x00ffffff) ^ x;
            }

            t[256] =  t[0];
            x      &= 0xff;

            for (p = 0; p < 256; p++)
            {
                t[p] = t[x =
                    (t[p ^ x] ^ x) &
                    0xff];
                t[x] = t[p + 1];
            }

            return (r, t);
        }

        uint M(uint x, uint y)
        {
            uint tmp = x + y;
            return ((((tmp) >> 8) & 0x00ffffff) ^ _t[(int) (tmp & 0xff)]);
        }

        /// <summary>
        /// Get byte from uint at a given index
        /// </summary>
        /// <param name="val"></param>
        /// <param name="byteIndex"></param>
        /// <returns></returns>
        [Pure]
        static byte GetByte(uint val, int byteIndex)
        {
            return (byte) ((val >> (8 * byteIndex)) & 0xff);
        }

        static void ReplaceByte(ref uint val, byte replacementByte, int replaceIndex)
        {
            val = (uint) ((val & ~(0xff << (8 * replaceIndex))) | (uint) (replacementByte << (8 * replaceIndex)));
        }
    }
}