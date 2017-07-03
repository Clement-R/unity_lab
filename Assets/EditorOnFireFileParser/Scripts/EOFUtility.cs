using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace EditorOnFireFileParser {
    public class EOFUtility {
        public static int bytesToInt32(byte[] bytes) {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return BitConverter.ToInt32(bytes, 0);
        }

        public static int bytesToInt16(byte[] bytes) {
            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return BitConverter.ToInt16(bytes, 0);
        }
    }
}
