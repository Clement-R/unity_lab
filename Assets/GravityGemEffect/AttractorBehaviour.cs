using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GravityGemEffect
{
    public class AttractorBehaviour : MonoBehaviour {

        public GameObject particleEffect;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Instantiate(particleEffect, collision.transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }   
}