using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterHeadScrapPiece : MonoBehaviour {

    public bool isActive = true;

    private Rigidbody2D _rb2d;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _rb2d.bodyType = RigidbodyType2D.Static;
    }

    private void OnMouseDown()
    {
        _rb2d.bodyType = RigidbodyType2D.Dynamic;
        float rand = Random.Range(0f, 1f) > 0.5f ? 1f : -1f;
        _rb2d.AddForce(new Vector2(rand * 15f, 10f), ForceMode2D.Impulse);

        isActive = false;
    }
}
