using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using JetBrains.Annotations;

namespace Warface.XMPP
{
    public class WakeCipher
    {
        readonly uint[] _key;
        readonly uint[] _t        = new uint[257];
        readonly uint[] _r = new uint[4];

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

        public WakeCipher(IEnumerable<long> key)
        {
            _key = key.Select(k => (uint) k).ToArray();
            SetArrays();
        }

        public void Encrypt(byte[] input)
        {
            CryptOperation(input,  false);
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

        void SetArrays()
        {
            uint x, p;
            var  k = new uint[4];

            if (_key.Length != 4)
                throw new ArgumentException("Invalid key length");

            k[0] = _key[0];
            k[1] = _key[1];
            k[2] = _key[2];
            k[3] = _key[3];

            for (p = 0; p < 4; p++)
            {
                _t[p] = k[p];
            }

            for (p = 4; p < 256; p++)
            {
                x     = _t[p - 4] + _t[p - 1];
                _t[p] = x >> 3 ^ Tt[x & 7];
            }

            for (p = 0; p < 23; p++)
                _t[p] += _t[p + 89];

            x = _t[33];
            uint z = _t[59] | 0x01000001;
            z &= 0xff7fffff;

            for (p = 0; p < 256; p++)
            {
                x     = (x     & 0xff7fffff) + z;
                _t[p] = (_t[p] & 0x00ffffff) ^ x;
            }

            _t[256] =  _t[0];
            x       &= 0xff;

            for (p = 0; p < 256; p++)
            {
                _t[p] = _t[x =
                    (_t[p ^ x] ^ x) &
                    0xff];
                _t[x] = _t[p + 1];
            }

            _r[0] = k[0];
            _r[1] = k[1];
            _r[2] = k[2];
            _r[3] = k[3];
        }

        uint M(uint x, uint y)
        {
            uint tmp = x + y;
            return ((((tmp) >> 8) & 0x00ffffff) ^ _t[(tmp) & 0xff]);
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