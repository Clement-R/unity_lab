using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterOpenBehaviour : MonoBehaviour {

    private BoosterBehaviour _booster;
    private bool _startOpening = false;
    private Vector3 _startMousePosition;

    private void Start()
    {
        _booster = GetComponentInParent<BoosterBehaviour>();
    }

    void Update()
    {
        if(_startOpening && !_booster.IsOpened())
        {
            if((Input.mousePosition - _startMousePosition).x > (Screen.width / 5f))
            {
                _booster.Open();
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
