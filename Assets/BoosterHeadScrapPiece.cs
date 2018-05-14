using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterHeadScrapPiece : MonoBehaviour {

    public bool isActive = true;

    private Rigidbody2D _rb2d;

    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _rb2d.simulated = false;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(pos.x, pos.y), Vector2.zero);

            if (hit && hit.collider.CompareTag("Wall"))
            {
                print(hit.collider.gameObject);
                if (hit.collider.gameObject == gameObject)
                {
                    Debug.Log("This is me");
                }
            }
        }
    }

    private void OnMouseDown()
    {
        print("Clicked");

        _rb2d.simulated = true;
        _rb2d.AddForce(new Vector2(2f, 10f), ForceMode2D.Impulse);

        isActive = false;
    }
}
