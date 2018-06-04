using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBehaviour : MonoBehaviour {

    public float speed = 10f;
    public Transform target;

    private Rigidbody2D _rb2d;

    void Start () {
        _rb2d = GetComponent<Rigidbody2D>();
	}
	
    private void FixedUpdate()
    {
        if(transform.position != target.position)
        {
            _rb2d.velocity = Vector2.zero;

            Vector2 vectorDiff = (target.position - transform.position).normalized;
            Vector2 force = (target.position - transform.position).normalized * speed;

            float rotZ = Mathf.Atan2(vectorDiff.y, vectorDiff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ - 90);

            _rb2d.AddForce(force);
        }
    }
}
