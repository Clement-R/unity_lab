using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningGenerator : MonoBehaviour {

    private LineRenderer lightning;
	
	void Start () {
        lightning = GetComponent<LineRenderer>();
        lightning.startWidth = 10f;
        lightning.endWidth = 10f;
        
        lightning.positionCount = 2;
        
        lightning.SetPosition(0, new Vector2(0f, 0f));
        
        //for (int i = 0; i < 7; i++)
        //{
        //    float height = Random.Range(15f, 125f);

        //    if (i != 0 && i % 2 != 0)
        //    {
        //        lightning.SetPosition(i, new Vector2(75f * i, height));
        //    } else
        //    {
        //        lightning.SetPosition(i, new Vector2(75f * i, -height));
        //    }
        //}
    }
	
	void Update () {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        lightning.SetPosition(1, pos);
    }
}
