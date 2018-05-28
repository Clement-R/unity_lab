using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GrabbableUIElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.DOMove(new Vector2(mousePosition.x, mousePosition.y), 0.15f).SetEase(Ease.InOutExpo);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag : " + gameObject.name);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.DOMove(new Vector2(mousePosition.x, mousePosition.y), 0.05f).SetEase(Ease.InOutExpo);
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
