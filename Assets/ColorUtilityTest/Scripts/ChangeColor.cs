using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColorUtilityTest {
    public class ChangeColor : MonoBehaviour {
        public Renderer re;
        public string htmlColor = "#2ecc71";

        Color col;

        void Start() {
            UpdateColor("#2ecc71");
        }

        void UpdateColor(string htmlColor) {
            ColorUtility.TryParseHtmlString(htmlColor, out col);
            re.sharedMaterial.color = col;
        }

        void OnValidate() {
            UpdateColor(htmlColor);
        }
    }
}