using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIParticleEffects : MonoBehaviour {

    public float boxLength = 2f;
    public float boxHeight = 1f;
    public float effectDuration = 2f;

    void Start ()
    {
        StartCoroutine(Effect());
    }

    private LineRenderer AddLine()
    {
        GameObject go = new GameObject();
        go.transform.parent = transform;
        go.transform.localPosition = Vector3.zero;

        LineRenderer line = go.AddComponent<LineRenderer>();
        line.material = new Material(Shader.Find("Particles/Additive"));
        line.widthMultiplier = 0.05f;

        return line;
    }

    IEnumerator Effect()
    {
        Vector2[] positions = new Vector2[4];
        Vector2 position = Vector2.zero;

        position += new Vector2(boxLength, 0);
        positions[0] = position;
            
        position -= new Vector2(0, boxHeight);
        positions[1] = position;
            
        position -= new Vector2(boxLength, 0);
        positions[2] = position;
            
        position += new Vector2(0, boxHeight);
        positions[3] = position;

        position = Vector2.zero;

        for (int i = 0; i < 4; i++)
        {
            LineRenderer line = AddLine();
            line.positionCount = 2;
            line.useWorldSpace = false;

            line.SetPosition(0, position);

            float t = 0f;
            while (t < 1)
            {
                Vector2 nextPosition = positions[i];

                t += Time.deltaTime / (effectDuration / 4f);
                position = Vector2.Lerp(position, nextPosition, t);
                line.SetPosition(1, position);
                yield return null;
            }
        }

        /*
        LineRenderer line = AddLine();
        line.positionCount = 1;
        line.SetPosition(0, position);

        line.positionCount = counter;
        counter++;
        Vector2 nextPosition = new Vector2(line.transform.localPosition.x + boxLength, line.transform.localPosition.y);
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / (effectDuration / 4f);
            position = Vector2.Lerp(position, nextPosition, t);
            line.SetPosition(0, position);
            yield return null;
        }

        line.positionCount = counter;
        counter++;
        nextPosition = new Vector2(position.x, position.y - boxHeight);
        t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / (effectDuration / 4f);
            position = Vector2.Lerp(position, nextPosition, t);
            line.SetPosition(1, position);
            yield return null;
        }

        line.positionCount = counter;
        counter++;
        nextPosition = new Vector2(position.x - boxLength, position.y);
        t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / (effectDuration / 4f);
            position = Vector2.Lerp(position, nextPosition, t);
            line.SetPosition(2, position);
            yield return null;
        }

        line.SetPosition(2, nextPosition);

        line.positionCount = counter;
        counter++;
        nextPosition = new Vector2(position.x, position.y + boxHeight);
        t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / (effectDuration / 4f);
            position = Vector2.Lerp(position, nextPosition, t);
            line.SetPosition(3, position);
            yield return null;
        }

        line.positionCount = counter;
        counter++;
        nextPosition = new Vector2(position.x + boxLength + (0.5f * line.widthMultiplier), position.y);
        t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / (effectDuration / 4f);
            position = Vector2.Lerp(position, nextPosition, t);
            line.SetPosition(4, position);
            yield return null;
        }
        */
    }
}
