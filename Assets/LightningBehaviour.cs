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

    private void ResetLine(Vector2 start)
    {
        _line.SetPosition(0, start);
        _line.SetPosition(1, start);
    }

    private void DestroyLine()
    {
        Destroy(gameObject);
    }

    private IEnumerator CastNextLightning(GameObject enemy)
    {
        Debug.Log("FIRE");
        Debug.Log("Target : " + enemy.name);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.transform.position, 500f);

        GameObject nearest = null;
        float distance = 9999f;
        foreach (var collider in colliders)
        {
            if(collider.CompareTag("Enemy") && collider.gameObject != enemy.gameObject)
            {
                float dist = Vector2.Distance(collider.gameObject.transform.position, enemy.transform.position);
                if (dist <= distance)
                {
                    nearest = collider.gameObject;
                    distance = dist;
                }
            }
        }

        // TODO : Change for kill behaviour
        ResetLine(enemy.transform.position);
        Destroy(enemy.gameObject);

        if(nearest != null)
        {
            Debug.Log("FIRE NEXT ONE");
            Debug.Log("Nearest : " + nearest.name);
            
            // Cast line to next enemy
            _line.SetPosition(1, nearest.transform.position);

            yield return new WaitForSeconds(0.15f);

            StartCoroutine(CastNextLightning(nearest.transform.gameObject));
        }

        yield return null;
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

            bool hasHit = false;
            foreach (var hit in hits)
            {
                if(hit.transform.CompareTag("Enemy"))
                {
                    hasHit = true;
                    StartCoroutine(CastNextLightning(hit.transform.gameObject));
                    break;
                }
            }

            if(hasHit)
            {
                yield break;
            }

            yield return null;
        }
        
        yield return new WaitForSeconds(0.25f);
        DestroyLine();
    }
}
