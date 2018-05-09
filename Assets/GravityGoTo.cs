using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GravityGemEffect
{
    public class GravityGoTo : MonoBehaviour
    {
        public GameObject target;

        private Vector3 _targetPosition;
        private Rigidbody2D _rb2d;

        void Awake()
        {
            _targetPosition = target.transform.position;
            _rb2d = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _rb2d.WakeUp();
            Vector2 explodeForce = Vector2.left + Vector2.up;
            explodeForce.Normalize();
            explodeForce = transform.TransformDirection(explodeForce);
            explodeForce += new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            _rb2d.AddForce(explodeForce * 250.0f, ForceMode2D.Impulse);
        }

        void LateUpdate()
        {
            GoTo();
        }

        private void GoTo()
        {
            Vector2 force = _targetPosition - transform.position;
            float distance = force.magnitude;
            force.Normalize();

            float strength = distance * distance * 10.0f;
            strength = Mathf.Clamp(strength, 1.0f, 80000.0f);

            force *= strength;
            //force *= 0.5f;
            _rb2d.AddForce(force * Time.fixedDeltaTime);
        }
    }
}
