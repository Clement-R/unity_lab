using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameSystem : MonoBehaviour {
    public GameObject[] playersNameUI;

    public GameObject namePlayer1;
    public GameObject namePlayer2;

    // 0-A ; 1-B ; 2-X ; 3-Y
    public Sprite[] buttons;
    public Sprite blankButton;

    private List<List<Image>> playersNameImages = new List<List<Image>>();
    private int[] playersCharacterIndex = new int[0];

    private int maxCharacterNumber = 0;

    void Start () {
        playersCharacterIndex = new int[playersNameUI.Length];

        for (int i = 0; i < playersNameUI.Length; i++) {
            playersNameImages.Add(new List<Image>());
            playersCharacterIndex[i] = 0;

            foreach (Transform child in playersNameUI[i].transform) {
                playersNameImages[i].Add(child.GetComponent<Image>());
                maxCharacterNumber++;
            }
        }

        maxCharacterNumber = playersNameUI[0].transform.childCount;
    }

	void Update () {
        for (int playerIndex = 0; playerIndex < playersNameUI.Length; playerIndex++) {

            if (Input.GetButtonDown("A_" + (playerIndex + 1))) {
                playersNameImages[playerIndex][playersCharacterIndex[playerIndex]].sprite = buttons[0];
                IncreasePlayerCharacterIndex(playerIndex + 1);
            }
            else if (Input.GetButtonDown("B_" + (playerIndex + 1))) {
                playersNameImages[playerIndex][playersCharacterIndex[playerIndex]].sprite = buttons[1];
                IncreasePlayerCharacterIndex(playerIndex + 1);
            }
            else if (Input.GetButtonDown("X_" + (playerIndex + 1))) {
                playersNameImages[playerIndex][playersCharacterIndex[playerIndex]].sprite = buttons[2];
                IncreasePlayerCharacterIndex(playerIndex + 1);
            }
            else if (Input.GetButtonDown("Y_" + (playerIndex + 1))) {
                playersNameImages[playerIndex][playersCharacterIndex[playerIndex]].sprite = buttons[3];
                IncreasePlayerCharacterIndex(playerIndex + 1);
            }
            else if (Input.GetButtonDown("RB_" + (playerIndex + 1))) {
                if (playersCharacterIndex[playerIndex] != 3 && playersCharacterIndex[playerIndex] > 0) {
                    playersNameImages[playerIndex][playersCharacterIndex[playerIndex] - 1].sprite = blankButton;
                    DecreasePlayerCharacterIndex(playerIndex + 1);
                }
                else {
                    if (playersNameImages[playerIndex][playersCharacterIndex[playerIndex]].sprite == blankButton) {
                        DecreasePlayerCharacterIndex(playerIndex + 1);
                    }
                    playersNameImages[playerIndex][playersCharacterIndex[playerIndex]].sprite = blankButton;
                }
            }
        }
    }

    void IncreasePlayerCharacterIndex(int playerIndex) {
        if (playersCharacterIndex[playerIndex - 1] + 1 < maxCharacterNumber) {
            playersCharacterIndex[playerIndex - 1]++;
        }
    }

    void DecreasePlayerCharacterIndex(int playerIndex) {
        if (playersCharacterIndex[playerIndex - 1] != 0) {
            playersCharacterIndex[playerIndex - 1]--;
        }
    }
}
