using UnityEngine;
using UnityEngine.Events;

public class ButtonBehaviour : MonoBehaviour {

    [HideInInspector]
    public UnityEvent onClick;

    private void OnMouseDown()
    {
        onClick.Invoke();
    }
}
