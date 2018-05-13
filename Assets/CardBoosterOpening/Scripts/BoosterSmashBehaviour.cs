using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoosterSmashBehaviour : MonoBehaviour {

    public GameObject boosterHead;
    public GameObject boosterBody;
    public GameObject card;
    public GameObject openEffect;
    public GameObject progressBar;
    public GameObject progressBarBackground;

    private bool _boosterOpened = false;
    private float _maxScale;
    private int _progress;

    void Start ()
    {
        progressBar.SetActive(false);
        _maxScale = progressBarBackground.transform.localScale.x;
        _progress = 0;

        progressBar.transform.localScale = new Vector3(0f, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
    }
	
	void Update ()
    {
	    if(Input.GetKeyDown(KeyCode.Space) && !_boosterOpened)
        {
            if(!progressBar.activeSelf)
            {
                progressBar.SetActive(true);
            }

            _progress += 10;
            _progress = Mathf.Clamp(_progress, 0, 100);

            progressBar.transform.localScale = new Vector3((_maxScale * _progress) / 100f,
                                                            progressBar.transform.localScale.y,
                                                            progressBar.transform.localScale.z
            );

            GetComponent<Shaker>().ShakeRotation(0f, 0.25f);

            if (_progress == 100)
            {
                Open();
            }
        }
	}

    private void Open()
    {
        _boosterOpened = true;
        Destroy(boosterHead);
        Destroy(progressBar);
        Destroy(progressBarBackground);
        StartCoroutine(ShowCards());
        Instantiate(openEffect, boosterHead.transform.position, Quaternion.identity);
    }

    IEnumerator ShowCards()
    {
        yield return new WaitForSecondsRealtime(0.25f);

        card.transform.DOMoveY(2f, 1f).SetEase(Ease.OutCubic);
        boosterBody.transform.DOMoveY(-6.5f, 1f).SetEase(Ease.OutQuart);
    }

    public bool IsOpened()
    {
        return _boosterOpened;
    }
}
