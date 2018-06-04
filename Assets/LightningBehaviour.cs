using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBehaviour : MonoBehaviour {

    private LineRenderer _line;

	void Awake () {
        _line = GetComponent<LineRenderer>();
        _line.SetPosition(0, Vector2.zero);
        _line.SetPosition(1, Vector2.zero);
    }
	
	public void FireTo(Vector2 end)
    {
        StartCoroutine(Fire(end));
    }

    private IEnumerator Fire(Vector2 end)
    {
        float t = 0f;
        while(_line.GetPosition(1).x != end.x && _line.GetPosition(1).y != end.y) {
            _line.SetPosition(1, Vector2.Lerp(Vector2.zero, end, t));
            t += Time.deltaTime;

            Vector2 start = new Vector2(_line.GetPosition(0).x, _line.GetPosition(0).y);
            Vector2 lineEnd = new Vector2(_line.GetPosition(1).x, _line.GetPosition(1).y); 
            Vector2 direction = (end - start).normalized;
            float distance = Vector2.Distance(start, lineEnd);

            RaycastHit2D[] hits = Physics2D.RaycastAll(start, direction, distance);

            foreach (var hit in hits)
            {
                if(hit.transform.CompareTag("Enemy"))
                {
                    Debug.Log("HIT AN ENEMY");
                    Debug.DebugBreak();
                }
            }

            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        _line.SetPosition(1, Vector2.zero);
    }
}
