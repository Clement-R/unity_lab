using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class BoosterHeadScrap : BoosterBehaviour
{
    public BoosterHeadScrapPiece[] headScraps;

    private void Update()
    {
        if(headScraps.All(e => e.isActive == false) && !IsOpened())
        {
            Open();
        }
    }
}
