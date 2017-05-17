using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using pkm.EventManager;
using UnityEngine.UI;

public class AchievementDisplay : MonoBehaviour {
    public GameObject achievementPanel;
    Text panelText;

	void Start () {
        panelText = achievementPanel.transform.GetChild(0).GetComponent<Text>();
    }
	
    void OnEnable() {
        EventManager.StartListening("playerMove", OnPlayerMove);
    }

    void OnDisable() {
        EventManager.StopListening("playerMove", OnPlayerMove);
    }

    void OnPlayerMove() {
        Debug.Log("A player moved !");
        UnlockAchievement("You made it !");
    }

    void UnlockAchievement(string text) {
        achievementPanel.SetActive(true);
        panelText.text = text;
        StartCoroutine("hidePanel");
    }

    IEnumerator hidePanel() {
        yield return new WaitForSeconds(2.0f);
        achievementPanel.SetActive(false);
    }
}
