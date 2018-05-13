using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class BoosterBehaviour : MonoBehaviour
{
    public GameObject boosterHead;
    public GameObject boosterBody;
    public GameObject card;
    public GameObject openEffect;

    private bool _boosterOpened = false;

    public void Open()
    {
        _boosterOpened = true;
        Destroy(boosterHead);
        Debug.Log("Booster opened !");
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
