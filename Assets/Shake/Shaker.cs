using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour {

    [Range(0, 1)]
    public float power = 1f;

    [Range(0, 90)]
    public float rotationPower = 45f;

    public void ShakePosition(float delay = 0f, float duration = 0.5f)
    {
        if (delay > 0f)
        {
            StartCoroutine(ShakePositionWithDelay(delay, duration));
        }
        else
        {
            StartCoroutine(ShakePos(duration));
        }
    }

    public void ShakeRotation(float delay=0f, float duration = 0.5f)
    {
        if(delay > 0f)
        {
            StartCoroutine(ShakeRotationWithDelay(delay, duration));
        }
        else
        {
            StartCoroutine(ShakeRot(duration));
        }
    }

    public IEnumerator ShakePositionWithDelay(float delay, float duration)
    {
        yield return new WaitForSecondsRealtime(delay);
        StartCoroutine(ShakePos());
    }

    public IEnumerator ShakeRotationWithDelay(float delay, float duration)
    {
        yield return new WaitForSecondsRealtime(delay);
        StartCoroutine(ShakeRot());
    }

    private IEnumerator ShakeRot(float duration = 0.5f)
    {
        Quaternion baseRotation = transform.rotation;

        float counter = 0f;

        float frequency = 10f;

        while (counter < duration)
        {
            transform.rotation = baseRotation;

            transform.rotation = Quaternion.Euler(0f, 0f, (Mathf.PerlinNoise(Time.time * frequency, 0f) - 0.5f) * rotationPower);

            counter += Time.deltaTime;
            yield return null;
        }

        transform.rotation = baseRotation;
    }

    private IEnumerator ShakePos(float duration = 0.5f)
    {
        Vector2 basePosition = transform.position;
        Vector2 nextPosition = new Vector2();

        float counter = 0f;

        float frequency = 10f;

        while (counter < duration)
        {
            transform.position = basePosition;
            transform.rotation = Quaternion.identity;

            nextPosition.x = Mathf.Clamp01(Mathf.PerlinNoise(Time.time * frequency, 0f)) - 0.5f;
            nextPosition.y = Mathf.Clamp01(Mathf.PerlinNoise(0f, Time.time * frequency)) - 0.5f;

            transform.position = basePosition + (nextPosition * power);

            counter += Time.deltaTime;
            yield return null;
        }

        transform.position = basePosition;
        transform.rotation = Quaternion.identity;
    }
}
