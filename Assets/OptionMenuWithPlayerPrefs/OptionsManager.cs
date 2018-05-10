using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsManager : MonoBehaviour {

    private static OptionsManager _optionsManager;

    public static OptionsManager instance
    {
        get
        {
            if (!_optionsManager)
            {
                _optionsManager = FindObjectOfType(typeof(OptionsManager)) as OptionsManager;

                if (!_optionsManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
            }

            return _optionsManager;
        }
    }

    void Start ()
    {
        if(!PlayerPrefs.HasKey("MasterVolume"))
        {
            PlayerPrefs.SetInt("MasterVolume", 75);
        }
    }
	
	void Update ()
    {
		
	}
}
