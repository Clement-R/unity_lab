using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMotionMecanim : MonoBehaviour {

    public Vector2 startPosition;
    public Vector2 endPosition;
    public float effectDuration = 1f;
    public float delay = 0f;

    private LineRenderer _line;
    private Vector2 _position;

	void Start () {
        _line = GetComponent<LineRenderer>();
        StartCoroutine(Effect());
    }

    IEnumerator Effect()
    {
        yield return new WaitForSeconds(delay);

        _line.SetPosition(0, startPosition);

        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / effectDuration;
            _position = Vector2.Lerp(startPosition, endPosition, t);
            _line.SetPosition(1, _position);
            yield return null;
        }
    }
}
