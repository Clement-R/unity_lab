using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkEffect : MonoBehaviour {

    private SpriteRenderer sr;

    void Start () {
        sr = GetComponent<SpriteRenderer>();
	}

	void Update () {
        if(Input.GetKeyDown(KeyCode.Alpha1)) {
            StartCoroutine(blink(0.25f, 3, Color.white));
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) {
            Debug.Log("Smooth");
            StartCoroutine(blinkSmooth(1f, 0.5f, Color.white));
        }
    }

    IEnumerator blink(float delayBetweenBlinks, int numberOfBlinks, Color blinkColor) {
        var material = sr.material;
        var counter = 0;
        while (counter <= numberOfBlinks) {
            material.SetColor("_BlinkColor", blinkColor);
            counter++;
            blinkColor.a = blinkColor.a == 1f ? 0f : 1f;
            yield return new WaitForSeconds(delayBetweenBlinks);
        }

        // revert to our standard sprite color
        blinkColor.a = 0f;
        material.SetColor("_BlinkColor", blinkColor);
    }

    IEnumerator blinkSmooth(float timeScale, float duration, Color blinkColor) {
        var material = sr.material;
        var elapsedTime = 0f;

        while (elapsedTime <= duration) {
            material.SetColor("_BlinkColor", blinkColor);

            blinkColor.a = Mathf.Lerp(1, 0, elapsedTime * timeScale);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // revert to our standard sprite color
        blinkColor.a = 0f;
        material.SetColor("_BlinkColor", blinkColor);
    }
}
