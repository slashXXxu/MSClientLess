using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MSCore.Tools
{

    /*
     *  Directly taken from MapleShark
     *  Credits: Diamondo25
     */
    static class MapleKeys
    {
        private static Dictionary<byte, Dictionary<KeyValuePair<ushort, byte>, byte[]>> MapleStoryKeys;

        private static void InitByContents(string pContents)
        {
            string[] lines = pContents.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i += 2)
            {
                var firstLine = lines[i];
                var semicolonPos = firstLine.IndexOf(':');
                var dotPos = firstLine.IndexOf('.');

                byte locale = byte.Parse(firstLine.Substring(0, semicolonPos));
                ushort version = ushort.Parse(firstLine.Substring(semicolonPos + 1, dotPos - (semicolonPos + 1)));
                byte subversion = byte.Parse(firstLine.Substring(dotPos + 1));

                string tmpkey = lines[i + 1];
                byte[] realkey = new byte[8];
                int tmp = 0;
                for (int j = 0; j < 4 * 8 * 2; j += 4 * 2)
                    realkey[tmp++] = byte.Parse(tmpkey[j] + "" + tmpkey[j + 1], System.Globalization.NumberStyles.HexNumber);

                AddKey(locale, version, subversion, realkey);
            }

        }

        private static void AddKey(byte locale, ushort version, byte subversion, byte[] key)
        {
            if (!MapleStoryKeys.ContainsKey(locale))
                MapleStoryKeys.Add(locale, new Dictionary<KeyValuePair<ushort, byte>, byte[]>());
            MapleStoryKeys[locale].Add(new KeyValuePair<ushort, byte>(version, subversion), key);
        }

        public static void Initialize()
        {
            MapleStoryKeys = new Dictionary<byte, Dictionary<KeyValuePair<ushort, byte>, byte[]>>();
            AddKey(6, 113, 1, new byte[] {
                0x13, // Full key's lost
                0x08,
                0x06,
                0xB4,
                0x1B,
                0x0F,
                0x33,
                0x52,
            });

            // Quickly count amount of keys
            int keyCount = 0;
            foreach (var kvp in MapleStoryKeys)
                keyCount += kvp.Value.Count;
            Logger.Info("初始化客戶端金鑰 , 已讀取 {0} 個客戶端金鑰", keyCount);
        }

        public static byte[] GetKeyForVersion(byte locale, ushort version, byte subversion)
        {

            if (MapleStoryKeys == null) Initialize();
            if (!MapleStoryKeys.ContainsKey(locale))
            {
                Logger.Warning("找不到合適的解密金鑰");
                return null;
            }

            // Get first version known
            for (ushort v = version; v > 0; v--)
            {
                for (byte sv = subversion; sv >= 0; sv--)
                {
                    var tuple = new KeyValuePair<ushort, byte>(v, sv);
                    if (MapleStoryKeys[locale].ContainsKey(tuple))
                    {
                        byte[] key = MapleStoryKeys[locale][tuple];
                        byte[] ret = new byte[32];
                        for (int i = 0; i < 8; i++)
                            ret[i * 4] = key[i];

                        Logger.Info("使用 {0}.{1} 版本的金鑰", v, sv);
                        return ret;
                    }
                    if (sv == 0) break;
                }
            }
            return null;
        }
    }
}
