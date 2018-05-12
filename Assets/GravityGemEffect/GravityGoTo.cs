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
            Vector2 explodeForce = Vector2.up + Vector2.left * Random.value + Vector2.right * Random.value;
            explodeForce.Normalize();
            explodeForce = transform.TransformDirection(explodeForce);
            explodeForce += new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f));
            _rb2d.AddForce(explodeForce * 400.0f, ForceMode2D.Impulse);
        }

        private void Update()
        {
            _targetPosition = target.transform.position;
        }

        void LateUpdate()
        {
            SteeringGoTo();
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

        private void SteeringGoTo()
        {
            Vector2 desiredVelocity = (_targetPosition - transform.position).normalized * 4800f;
            Vector2 steering = desiredVelocity - _rb2d.velocity;
            steering = Vector2.ClampMagnitude(steering, 4800f);
            
            //steering = truncate(steering, max_force)
            //steering = steering / mass
            //velocity = truncate(velocity + steering, max_speed)
            //position = position + velocity
            
            _rb2d.AddForce(steering);
        }
    }
}
