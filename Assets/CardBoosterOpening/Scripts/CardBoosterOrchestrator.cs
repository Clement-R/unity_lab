using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardBoosterOrchestrator : MonoBehaviour {

    public GameObject previous;
    public GameObject next;

    private int _index = 0;
    private int _numberOfEffects;
    private Grid _grid;

    void Start ()
    {
        _numberOfEffects = transform.childCount;

        _grid = GetComponent<Grid>();

        previous.GetComponent<ButtonBehaviour>().onClick.AddListener(Previous);
        next.GetComponent<ButtonBehaviour>().onClick.AddListener(Next);

        previous.SetActive(false);

        for (int ii = 0; ii < _numberOfEffects; ii++)
        {
            transform.GetChild(ii).position = new Vector2( (_grid.cellSize.x * 1.5f) * ii, 0f);
        }
    }
	
	private void Next()
    {
        _index++;
        if(_index == _numberOfEffects - 1)
        {
            next.SetActive(false);
        }

        previous.SetActive(true);

        Move(-1f);
    }

    private void Previous()
    {
        _index--;
        if(_index == 0)
        {
            previous.SetActive(false);
        }

        next.SetActive(true);

        Move(1f);
    }

    private void Move(float direction = 1f)
    {
        for (int ii = 0; ii < _numberOfEffects; ii++)
        {
            Transform child = transform.GetChild(ii);
            child.DOMoveX(child.position.x + direction * _grid.cellSize.x * 1.5f, 0.75f).SetEase(Ease.InOutElastic);
            child.GetComponent<Shaker>().ShakeRotation(0.35f, 0.15f);
        }
    }
}
