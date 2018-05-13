using UnityEngine;
using UnityEngine.Events;

public class ButtonBehaviour : MonoBehaviour {

    public UnityEvent onClick;

	void Start ()
    {
        onClick = new UnityEvent();
    }

    private void OnMouseDown()
    {
        onClick.Invoke();
    }
}
