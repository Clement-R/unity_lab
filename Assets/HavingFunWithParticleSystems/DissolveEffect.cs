using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{
    public class DissolveEffect : MonoBehaviour
    {

        public float effectDuration = 0.5f;

        void Start()
        {
            StartCoroutine(Effect());
        }

        private IEnumerator Effect()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            float t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / effectDuration;
                sr.material.SetFloat("_Level", t);
                yield return null;
            }
        }
    }

}
