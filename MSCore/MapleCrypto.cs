/*  MapleLib - A general-purpose MapleStory library
 * Copyright (C) 2009, 2010 Snow and haha01haha01
   
 * This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

 * This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

 * You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.*/

    /*
     * This file is heavily modified by Novak
     * Credits to Diamondo25's MapleShark for sexy morph function.
    */

using System;
using System.Security.Cryptography;

namespace MapleLib.MapleCryptoLib
{
	/// <summary>
	/// Class to manage Encryption and IV generation
	/// </summary>
	public class MapleCrypto
	{
		#region Properties
		/// <summary>
		/// Version of MapleStory used in encryption
		/// </summary>
		private short mapleVersion;

        /// <summary>
        /// (public) IV used in the packet encryption
        /// </summary>
        public byte[] mIV { get; private set; }

        private RijndaelManaged mAES = new RijndaelManaged();
        private ICryptoTransform mTransformer = null;

		#endregion

		#region Methods
		/// <summary>
		/// Creates a new MapleCrypto class
		/// </summary>
		/// <param name="IV">Intializing Vector</param>
		/// <param name="mapleVersion">Version of MapleStory</param>
		public MapleCrypto(byte[] pIV, short mapleVersion)
		{
			this.mapleVersion = mapleVersion;

            mAES.Key = CryptoConstants.UserKey;
            mAES.Mode = CipherMode.ECB;
            mAES.Padding = PaddingMode.PKCS7;
            mTransformer = mAES.CreateEncryptor();
            mIV = pIV;
        }

        public byte[] MulBytes(byte[] ary, int count, int mul)
        {
            byte[] ret = new byte[count * mul];
            for (int x = 0; x < count * mul; x++)
            {
                ret[x] = ary[x % count];
            }
            return ret;
        }

        public void Transform(byte[] pBuffer)
        {
            int remainLen = pBuffer.Length;
            int maxLength = 0x5B4;
            int start = 0;

            while(remainLen > 0)
            {
                byte[] cIv = MulBytes(mIV, 4, 4);
                if(remainLen < maxLength)
                {
                    maxLength = remainLen;
                }
                for(int i = start; i < (start+ maxLength); ++i)
                {
                    if((i-start) % cIv.Length == 0)
                    {
                        byte[] nIv = mTransformer.TransformFinalBlock(cIv, 0, cIv.Length);
                        Buffer.BlockCopy(nIv, 0, cIv, 0, cIv.Length);
                    }
                    pBuffer[i] ^= cIv[(i - start) % cIv.Length];
                }
                start += maxLength;
                remainLen -= maxLength;
                maxLength = 0x5B4;
            }
            
            ShiftIV(mIV);
        }

        public bool checkPacket(byte[] packet)
        {
            return ((((packet[0] ^ mIV[2]) & 0xFF) == ((mapleVersion >> 8) & 0xFF)) 
                && (((packet[1] ^ mIV[3]) & 0xFF) == (mapleVersion & 0xFF)));
        }

        public void ShiftIV(byte[] pOldIV = null)
        {
            if (pOldIV == null) pOldIV = mIV;

            byte[] newIV = new byte[] { 0xF2, 0x53, 0x50, 0xC6 };
            for (int i = 0; i < 4; ++i)
                Morph(pOldIV[i], newIV);

            Buffer.BlockCopy(newIV, 0, mIV, 0, mIV.Length);
        }

        public static void Morph(byte pValue, byte[] pIV)
        {
            byte input = pValue;
            byte tableInput = CryptoConstants.sShiftKey[input];
            pIV[0] += (byte)(CryptoConstants.sShiftKey[pIV[1]] - input);
            pIV[1] -= (byte)(pIV[2] ^ tableInput);
            pIV[2] ^= (byte)(CryptoConstants.sShiftKey[pIV[3]] + input);
            pIV[3] -= (byte)(pIV[0] - tableInput);

            uint val = (uint)(pIV[0] | pIV[1] << 8 | pIV[2] << 16 | pIV[3] << 24);
            val = (val >> 0x1D | val << 0x03);
            pIV[0] = (byte)(val & 0xFF);
            pIV[1] = (byte)((val >> 8) & 0xFF);
            pIV[2] = (byte)((val >> 16) & 0xFF);
            pIV[3] = (byte)((val >> 24) & 0xFF);
        }

        /// <summary>
        /// Get a packet header for a packet being sent to the server
        /// </summary>
        /// <param name="size">Size of the packet</param>
        /// <returns>The packet header</returns>
        public byte[] getHeaderToClient(int size)
		{
			byte[] header = new byte[4];
			int a = mIV[3] * 0x100 + mIV[2];
			a ^= -(mapleVersion + 1);
			int b = a ^ size;
			header[0] = (byte)(a % 0x100);
			header[1] = (byte)((a - header[0]) / 0x100);
			header[2] = (byte)(b ^ 0x100);
			header[3] = (byte)((b - header[2]) / 0x100);
			return header;
		}

		/// <summary>
		/// Get a packet header for a packet being sent to the client
		/// </summary>
		/// <param name="size">Size of the packet</param>
		/// <returns>The packet header</returns>
		public byte[] getHeaderToServer(int size)
		{
			byte[] header = new byte[4];
			int a = mIV[3] * 0x100 + mIV[2];
			a = a ^ (mapleVersion);
			int b = a ^ size;
			header[0] = Convert.ToByte(a % 0x100);
			header[1] = Convert.ToByte(a / 0x100);
			header[2] = Convert.ToByte(b % 0x100);
			header[3] = Convert.ToByte(b / 0x100);
			return header;
		}

		/// <summary>
		/// Gets the length of a packet from the header
		/// </summary>
		/// <param name="packetHeader">Header of the packet</param>
		/// <returns>The length of the packet</returns>
		public static ushort getPacketLength(byte[] pBuffer, int pStart)
		{
            int length = (int)pBuffer[pStart] |
                        (int)(pBuffer[pStart + 1] << 8) |
                        (int)(pBuffer[pStart + 2] << 16) |
                        (int)(pBuffer[pStart + 3] << 24);
            length = (length >> 16) ^ (length & 0xFFFF);
            return (ushort)length;
		}

		/// <summary>
		/// Checks to make sure the packet is a valid MapleStory packet
		/// </summary>
		/// <param name="packetHeader">The header of the packet received</param>
		/// <returns>The packet is valid</returns>
		public bool checkPacketToServer(byte[] packet, int offset)
		{
			int a = packet[offset] ^ mIV[2];
			int b = mapleVersion;
			int c = packet[offset + 1] ^ mIV[3];
			int d = mapleVersion >> 8;
			return (a == b && c == d);
		}

		/// <summary>
		/// Multiplies bytes
		/// </summary>
		/// <param name="input">Bytes to multiply</param>
		/// <param name="count">Amount of bytes to repeat</param>
		/// <param name="mult">Times to repeat the packet</param>
		/// <returns>The multiplied bytes</returns>
		public static byte[] multiplyBytes(byte[] input, int count, int mult)
		{
			byte[] ret = new byte[count * mult];
			for (int x = 0; x < ret.Length; x++)
			{
				ret[x] = input[x % count];
			}
			return ret;
		}
        #endregion

        #region Encrypt/Decrypt
        public void Decrypt(byte[] pBuffer)
        {
            Transform(pBuffer); //AES + IV shift
        }

        public void Encrypt(byte[] data)
        {
            Transform(data); //crypt
        }
        #endregion
    }

    public static class Extensions
    {
        public static byte RollLeft(this byte pThis, int pCount)
        {
            uint overflow = ((uint)pThis) << (pCount % 8);
            return (byte)((overflow & 0xFF) | (overflow >> 8));
        }

        public static byte RollRight(this byte pThis, int pCount)
        {
            uint overflow = (((uint)pThis) << 8) >> (pCount % 8);
            return (byte)((overflow & 0xFF) | (overflow >> 8));
        }
    }
}