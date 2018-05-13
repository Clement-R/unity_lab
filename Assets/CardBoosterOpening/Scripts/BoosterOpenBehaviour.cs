using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterOpenBehaviour : MonoBehaviour {

    public GameObject topSpriteMask;

    private BoosterBehaviour _booster;
    private bool _startOpening = false;
    private Vector3 _startMousePosition;

    private float maxScale;
    private Vector3 startPos;

    private void Start()
    {
        _booster = GetComponentInParent<BoosterBehaviour>();
        maxScale = GameObject.Find("booster_top").GetComponent<Renderer>().bounds.size.x;
        startPos = new Vector3( -(GameObject.Find("booster_top").GetComponent<Renderer>().bounds.size.x / 2f), 0f, 0f);
    }

    void Update()
    {
        if(_startOpening && !_booster.IsOpened())
        {
            if((Input.mousePosition - _startMousePosition).x > (Screen.width / 5f))
            {
                _booster.Open();
                Destroy(topSpriteMask);
            }

            float distance = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPos).x;

            print(distance);
            print(topSpriteMask.transform.localScale.x);

            if(topSpriteMask.transform.localScale.x <= distance * 2f)
            {
                topSpriteMask.transform.localScale = new Vector3(distance * 2f, 1f, 1f);
            }
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("Start booster opening");

        if(!_startOpening)
        {
            _startOpening = true;
            _startMousePosition = Input.mousePosition;
        }
    }
}
