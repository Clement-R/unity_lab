using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GrabbableUIElement : MonoBehaviour, IDragHandler, IEndDragHandler {

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag : " + gameObject.name);
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Stop Drag : " + gameObject.name);
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    
}
