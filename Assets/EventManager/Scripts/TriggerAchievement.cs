using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;

public class TriggerAchievement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)) {
            EventManager.TriggerEvent("playerMove");
        }
	}
}
