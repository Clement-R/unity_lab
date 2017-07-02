using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EditorOnFireFileParser {
    public class EOFFileReader : MonoBehaviour {

        void Start() {
            // Read file
            byte[] buffer = File.ReadAllBytes("Assets/EditorOnFireFileParser/Resources/notes_2.eof");
            
            char[] rheader = new char[16];

            for (int i = 0; i < 16; i++) {
                rheader[i] = Convert.ToChar(buffer[i]);

                Debug.Log(rheader[i]);
            }

            for (int i = 0; i < 4; i++) {
                rheader[i] = Convert.ToChar(buffer[i]);

                Debug.Log(rheader[i]);
            }
        }
    }
}
