using System;
using System.Collections.Generic;
using System.Linq;

namespace Warface.XMPP
{
    public class WakeCipher
    {
        int[]         _iv;
        long[]        _key;
        volatile bool _isInitialized;

        public void SetKey(long[] wakeKey)
        {
            _key = new long[32];
            wakeKey?.CopyTo(_key, 0);
        }

        public void SetIV(byte k)
        {
            if (_key == null)
                throw new InvalidOperationException("Set encryption key first");

            _iv            = new[] {31 ^ k, 120 ^ k, 212 ^ k, 244 ^ k, 34 ^ k, 86 ^ k, 249 ^ k, 25 ^ k};
            _isInitialized = true;
        }


        public void Decrypt(byte[] data)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Decryptor is not yet initialized.");
            new Wake(_key, _iv).Decrypt(data);
        }

        public void Encrypt(byte[] data)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("Decryptor is not yet initialized.");
            new Wake(_key, _iv).Encrypt(data);
        }


        class Wake
        {
            readonly IReadOnlyList<uint> _key;
            readonly IReadOnlyList<uint> _iv;
            readonly WakeKey             _wakeKey;

            public Wake(long[] key, int[] iv)
            {
                _key     = key.Select(k => (uint) k).ToArray();
                _iv      = iv.Select(i => (uint) i).ToArray();
                _wakeKey = new WakeKey();
                SetKey();
            }

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

            int SetKey()
            {
                uint x, p;
                var  k = new uint[4];
                /* the key must be exactly 256 bits */
                if (_key.Count != 32)
                    return -1;

                //# ifdef WORDS_BIGENDIAN
                //            k[0] = byteswap32(key[0]);
                //            k[1] = byteswap32(key[1]);
                //            k[2] = byteswap32(key[2]);
                //            k[3] = byteswap32(key[3]);
                //#else
                k[0] = _key[0];
                k[1] = _key[1];
                k[2] = _key[2];
                k[3] = _key[3];

                for (p = 0; p < 4; p++)
                {
                    _wakeKey.T[p] = k[p];
                }

                for (p = 4; p < 256; p++)
                {
                    x             = _wakeKey.T[p - 4] + _wakeKey.T[p - 1];
                    _wakeKey.T[p] = x >> 3 ^ Tt[x & 7];
                }

                for (p = 0; p < 23; p++)
                    _wakeKey.T[p] += _wakeKey.T[p + 89];

                x = _wakeKey.T[33];
                var z = _wakeKey.T[59] | 0x01000001;
                z &= 0xff7fffff;

                for (p = 0; p < 256; p++)
                {
                    x             = (x             & 0xff7fffff) + z;
                    _wakeKey.T[p] = (_wakeKey.T[p] & 0x00ffffff) ^ x;
                }

                _wakeKey.T[256] =  _wakeKey.T[0];
                x               &= 0xff;

                for (p = 0; p < 256; p++)
                {
                    _wakeKey.T[p] = _wakeKey.T[x =
                        (_wakeKey.T[p ^ x] ^ x) &
                        0xff];
                    _wakeKey.T[x] = _wakeKey.T[p + 1];
                }

                _wakeKey.Counter = 0;
                _wakeKey.R[0]    = k[0];
                _wakeKey.R[1]    = k[1];
                _wakeKey.R[2]    = k[2];
                //# ifdef WORDS_BIGENDIAN
                //            _wakeKey.r[3] = byteswap32(k[3]);
                //#else
                _wakeKey.R[3] = k[3];
                //#endif
                _wakeKey.Started = 0;
                _wakeKey.Ivsize  = _iv.Count > 32 ? 32 : _iv.Count;

                if (_iv == null)
                    _wakeKey.Ivsize = 0;
                if (_wakeKey.Ivsize > 0 && _iv != null)
                    _wakeKey.Iv = _iv.ToArray();

                return 0;
            }

            uint M(uint x, uint y)
            {
                var tmp = x + y;
                return ((((tmp) >> 8) & 0x00ffffff) ^ _wakeKey.T[(tmp) & 0xff]);
            }

            void Encrypt(uint[] input)
            {
                var bytesInput = input.SelectMany(BitConverter.GetBytes).ToArray();
                Encrypt(bytesInput);
                const int blockLength = 4;
                for (var blockIndex = 0; blockIndex < bytesInput.Length / blockLength; blockIndex++)
                {
                    var block = bytesInput.Skip(blockIndex * blockLength).Take(blockLength).ToArray();
                    input[blockIndex] = BitConverter.ToUInt32(block, 0);
                }
            }

            public void Encrypt(byte[] input)
            {
                int i;
                if (input.Length == 0)
                    return;

                var r3 = _wakeKey.R[0];
                var r4 = _wakeKey.R[1];
                var r5 = _wakeKey.R[2];
                var r6 = _wakeKey.R[3];

                if (_wakeKey.Started == 0)
                {
                    _wakeKey.Started = 1;
                    Encrypt(_wakeKey.Iv);
                }
                for (i = 0; i < input.Length; i++)
                {
                    /* R1 = V[n] = V[n] XOR R6 - here we do it per byte --sloooow */
                    /* R1 is ignored */
                    var r6Bytes = BitConverter.GetBytes(r6);
                    input[i] ^= r6Bytes[_wakeKey.Counter];

                    /* _wakeKey.tmp = V[n] = R1 - per byte also */
                    var tmpBytes = BitConverter.GetBytes(_wakeKey.Tmp);
                    tmpBytes[_wakeKey.Counter] = input[i];
                    _wakeKey.Tmp               = BitConverter.ToUInt32(tmpBytes, 0);
                    _wakeKey.Counter++;

                    if (_wakeKey.Counter == 4)
                    {
                        /* r6 was used - update it! */
                        _wakeKey.Counter = 0;

                        //# ifdef WORDS_BIGENDIAN
                        //                    /* these swaps are because we do operations per byte */
                        //                    _wakeKey.tmp = byteswap32(_wakeKey.tmp);
                        //                    r6 = byteswap32(r6);
                        //#endif
                        r3 = M(r3, _wakeKey.Tmp);
                        r4 = M(r4, r3);
                        r5 = M(r5, r4);
                        r6 = M(r6, r5);

                        //# ifdef WORDS_BIGENDIAN
                        //                    r6 = byteswap32(r6);
                        //#endif
                    }
                }

                _wakeKey.R[0] = r3;
                _wakeKey.R[1] = r4;
                _wakeKey.R[2] = r5;
                _wakeKey.R[3] = r6;
            }

            void Decrypt(ref uint[] input)
            {
                var bytesInput = input.SelectMany(BitConverter.GetBytes).ToArray();
                Decrypt(bytesInput);
                const int blockLength = 4;
                for (var blockIndex = 0; blockIndex < bytesInput.Length / blockLength; blockIndex++)
                {
                    var block = bytesInput.Skip(blockIndex * blockLength).Take(blockLength).ToArray();
                    input[blockIndex] = BitConverter.ToUInt32(block, 0);
                }
            }

            public void Decrypt(byte[] input)
            {
                int i;

                if (input.Length == 0)
                    return;

                var r3 = _wakeKey.R[0];
                var r4 = _wakeKey.R[1];
                var r5 = _wakeKey.R[2];
                var r6 = _wakeKey.R[3];

                if (_wakeKey.Started == 0)
                {
                    _wakeKey.Started = 1;
                    Encrypt(_wakeKey.Iv);
                    _wakeKey.R[0] = r3;
                    _wakeKey.R[1] = r4;
                    _wakeKey.R[2] = r5;
                    _wakeKey.R[3] = r6;
                    Decrypt(ref _wakeKey.Iv);
                }

                for (i = 0; i < input.Length; i++)
                {
                    var tmpBytes = BitConverter.GetBytes(_wakeKey.Tmp);
                    /* _wakeKey.tmp = V[n] */
                    tmpBytes[_wakeKey.Counter] = input[i];
                    _wakeKey.Tmp               = BitConverter.ToUInt32(tmpBytes, 0);
                    /* R2 = V[n] = V[n] ^ R6 */
                    /* R2 is ignored */
                    var r6Bytes = BitConverter.GetBytes(r6);
                    input[i] ^= r6Bytes[_wakeKey.Counter];
                    _wakeKey.Counter++;

                    if (_wakeKey.Counter == 4)
                    {
                        _wakeKey.Counter = 0;

                        //# ifdef WORDS_BIGENDIAN
                        //                    _wakeKey.tmp = byteswap32(_wakeKey.tmp);
                        //                    r6 = byteswap32(r6);
                        //#endif
                        r3 = M(r3, _wakeKey.Tmp);
                        r4 = M(r4, r3);
                        r5 = M(r5, r4);
                        r6 = M(r6, r5);

                        //# ifdef WORDS_BIGENDIAN
                        //                    r6 = byteswap32(r6);
                        //#endif
                    }
                }

                _wakeKey.R[0] = r3;
                _wakeKey.R[1] = r4;
                _wakeKey.R[2] = r5;
                _wakeKey.R[3] = r6;
            }

            class WakeKey
            {
                public readonly uint[] T = new uint[257];
                public readonly uint[] R = new uint[4];
                public          int    Counter;
                public          uint   Tmp; /* used as r1 or r2 */
                public          int    Started;
                public          uint[] Iv = new uint[8];
                public          int    Ivsize;
            }
        }
    }
}