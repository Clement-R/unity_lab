using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMotionBox : MonoBehaviour {

    public enum Effects
    {
        Full,
        TwoHalf
    };

    public Effects effect;
    public float boxLength = 2f;
    public float boxHeight = 1f;
    public float effectDuration = 2f;

    void Start ()
    {
        switch (effect)
        {
            case Effects.Full:
                StartCoroutine(Full());
                break;

            case Effects.TwoHalf:
                StartCoroutine(TwoHalf());
                break;
            default:
                break;
        }
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

    IEnumerator TwoHalf()
    {
        Vector2[] positions = new Vector2[4];
        
        Vector2 position1 = Vector2.zero;
        Vector2 position2 = Vector2.zero;

        position1 += new Vector2(boxLength, 0);
        positions[0] = position1;

        position1 -= new Vector2(0, boxHeight);
        positions[1] = position1;

        position1 -= new Vector2(boxLength, 0);
        positions[2] = position1;

        position1 += new Vector2(0, boxHeight);
        positions[3] = position1;

        for (int i = 0; i < 2; i++)
        {
            LineRenderer line1 = AddLine();
            line1.positionCount = 2;
            line1.useWorldSpace = false;
            
            if (i - 1 < 0)
            {
                position1 = Vector2.zero;
            }
            else
            {
                position1 = positions[i - 1];
                
            }
            line1.SetPosition(0, position1);

            LineRenderer line2 = AddLine();
            line2.positionCount = 2;
            line2.useWorldSpace = false;

            position2 = positions[i + 1];
            line2.SetPosition(0, position2);

            float t = 0f;
            while (t < 1)
            {
                Vector2 nextPosition1 = positions[i];
                Vector2 nextPosition2 = positions[i + 1];

                t += Time.deltaTime / (effectDuration / 2f);

                position1 = Vector2.Lerp(position1, nextPosition1, t);
                line1.SetPosition(1, position1);

                position2 = Vector2.Lerp(position2, nextPosition2, t);
                line2.SetPosition(1, position2);

                yield return null;
            }
        }
    }

    IEnumerator Full()
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
    }
}
