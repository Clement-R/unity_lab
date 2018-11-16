using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningLine : MonoBehaviour {

    public Sprite lineSprite;

    private Vector2 start;
    private Vector2 end;

    void Start () {
		
	}
	
	void Update () {
        Debug.Log(AngleBetweenPoints(Vector2.zero, new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y)));
	}

    float AngleBetweenPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(b.y - a.y, b.x - a.x) * 180f / Mathf.PI;
    }
}
